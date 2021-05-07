using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using semenarna_id2.Areas.Cart.ViewModels;
using semenarna_id2.Data;
using semenarna_id2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace semenarna_id2.Areas.Cart.Controllers {
    [Area("Cart")]
    public class OrderController : Controller {
        private readonly ApplicationDbContext _ctx;
        private readonly UserManager<ApplicationUser> _userManager;
        public OrderController(ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager) {
            _ctx = applicationDbContext;
            _userManager = userManager;
        }
        //get a list of ids of validated products
        private List<CartProduct> ValidateProducts(CartViewModel cartViewModel) {

            //get item ids from client cart
            List<int> itemIds = new List<int>();
            foreach (var item in cartViewModel.Items) {
                itemIds.Add(item.Id);
            }

            //get equivalent client side products
            List<Product> clientSideProducts = _ctx.Products
                .Where(p => itemIds.Contains(p.ProductId))
                .ToList();

            //return CartProducts generated via client products ids
            List<CartProduct> validatedProducts = new List<CartProduct>();
            foreach (var product in cartViewModel.Items) {
                foreach (var p in clientSideProducts) {

                    if (p.ProductId == product.Id) {
                        var new_product = new CartProduct {
                            Product = p,
                            Price = int.Parse(p.Price),
                            Quantity = product.Quantity,
                            Variations = product.Variations
                        };
                        validatedProducts.Add(new_product);

                    }
                }

            }

            return validatedProducts;
        }

        [HttpPost]
        public async Task<IActionResult> Process([FromBody] OrderViewModel orderViewModel) {

            ApplicationUser user = null;

            if (User.Identity.IsAuthenticated) {
                var id = _userManager.GetUserId(User);
                user = await _ctx.Users
                                .Include(u => u.Cart)
                                .ThenInclude(u => u.CartProducts)
                                .Where(u => u.Id == id).FirstOrDefaultAsync();
            }

            var validatedProducts = ValidateProducts(orderViewModel.Cart);
            /*var cartProducts = await _ctx.CartProducts
                                .Where(p => validatedProductsIds.Contains(p.Id))
                                .ToListAsync();*/

            if (ModelState.IsValid) {
                var order = new Order {
                    FirstName = orderViewModel.FirstName,
                    LastName = orderViewModel.LastName,
                    CompanyName = orderViewModel.CompanyName,
                    Country = orderViewModel.Country,
                    Address = orderViewModel.Address,
                    ZipCode = orderViewModel.ZipCode,
                    City = orderViewModel.City,
                    Email = orderViewModel.Email,
                    Phone = orderViewModel.Phone,
                    PaymentMethod = orderViewModel.PaymentMethod,
                    DeliveryMethod = orderViewModel.DeliveryMethod,
                    DateTime = DateTime.UtcNow,
                    CartProducts = validatedProducts

                };
                string order_id;
                //if user logged add order with cartproducts and clear user cart
                if (user != null) {
                    user.Cart.CartProducts = new List<CartProduct>();
                    if(user.Orders == null) {
                        user.Orders = new List<Order>();
                    }
                    user.Orders.Add(order);

                    await _ctx.SaveChangesAsync();
                    order_id = order.OrderId.ToString();

                }
                else {

                    await _ctx.Orders.AddAsync(order);
                    await _ctx.SaveChangesAsync();
                    order_id = order.OrderId.ToString();
                }



                var success_message = $"Your order has been placed successfully. An email containing your order's details has been sent to";

                return RedirectToAction("Result", new ResultViewModel { OrderId = order_id, EmailSent = order.Email, Message = success_message, OrderStatus = "true" });
            }
            else {
                var error_message = $"Your order has not been been placed. Please review your contact information and try again.";
                return BadRequest();
            }

        }
        [HttpGet]
        public IActionResult Result(string OrderId, string EmailSent, string Message, string OrderStatus) {

            var result = new ResultViewModel {
                OrderId = OrderId,
                EmailSent = EmailSent,
                Message = Message,
                OrderStatus = OrderStatus
            };


            return View(result);
        }

    }
}

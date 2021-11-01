using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using application.Areas.Cart.ViewModels;
using application.Data;
using application.Models;
using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RazorLight;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace application.Areas.Cart.Controllers {
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
            List<Product> serverSideProducts = _ctx.Products
                .Where(p => itemIds.Contains(p.ProductId))
                .ToList();

            //return CartProducts generated via client products ids
            List<CartProduct> validatedProducts = new List<CartProduct>();
            foreach (var product in cartViewModel.Items) {
                foreach (var p in serverSideProducts) {

                    if (p.ProductId == product.Id) {
                        var new_product = new CartProduct {
                            Product = p,
                            Price = p.Price,
                            Name = p.Name,
                            Quantity = product.Quantity,
                        };
                        if (product.Variations != null) {
                            new_product.Variations = product.Variations;
                        }

                        validatedProducts.Add(new_product);

                    }
                }

            }

            return validatedProducts;
        }


        [HttpPost]
        public async Task<IActionResult> Process([FromBody] OrderViewModel orderViewModel) {
            
            
            

            try {

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
                        DateTime = DateTime.Now,
                        CartProducts = validatedProducts

                    };
                    string order_id;
                    //if user logged add order with cartproducts and clear user cart
                    if (user != null) {
                        user.Cart.CartProducts = new List<CartProduct>();
                        if (user.Orders == null) {
                            user.Orders = new List<Order>();
                        }
                        user.Orders.Add(order);
                        order_id = order.OrderId.ToString();

                    }
                    else {

                        await _ctx.Orders.AddAsync(order);
                        order_id = order.OrderId.ToString();
                    }

                    double totalAll = 0;
                    foreach(var item in validatedProducts) {
                        var totalProduct = item.Quantity * item.Price;
                        totalAll += totalProduct;
                    }

                    //viewmodel for email order confirmation
                    var viewModel = new OrderEmailViewModel {
                        OrderId = order_id,
                        FirstName = order.FirstName,
                        LastName = order.LastName,
                        CompanyName = order.CompanyName,
                        Country = order.Country,
                        Address = order.Address,
                        ZipCode = order.ZipCode,
                        City = order.City,
                        Email = order.Email,
                        Phone = order.Phone,
                        PaymentMethod = order.PaymentMethod,
                        DeliveryMethod = order.DeliveryMethod,
                        Date = order.DateTime.ToString(new CultureInfo("en-GB")),
                        Total = totalAll.ToString(),
                        OrderUrl = $"https://semenarna.mk/Cart/Order/Single/{order_id}"
                    };

                    var engine = new RazorLightEngineBuilder()
                    // required to have a default RazorLightProject type,
                    // but not required to create a template from string.
                    .UseEmbeddedResourcesProject(typeof(OrderViewModel))
                    .SetOperatingAssembly(typeof(OrderViewModel).Assembly)
                    .UseMemoryCachingProvider()
                    .Build();


                    string cshtml = System.IO.File.ReadAllText("/home/ubuntu/projects/semenarna/Utils/Views/OrderConfirmationEmail.cshtml");


                    string result = await engine.CompileRenderStringAsync("templateKey", cshtml, viewModel);

                    if (user != null) {
                        var new_email = new Mailer("billing@semenarna.mk", user.Email, result);
                        new_email.Send();
                    }
                    else {
                        var new_email = new Mailer("billing@semenarna.mk", order.Email, result);
                        new_email.Send();
                    }



                    var success_message = $"Your order has been placed successfully. An email containing your order's details has been sent to";


                    await _ctx.SaveChangesAsync();
                    return RedirectToAction("Result", new ResultViewModel { OrderId = order_id, EmailSent = order.Email, Message = success_message, OrderStatus = "true" });
                }
                else {
                    var error_message = $"Your order has not been been placed. Please review your contact information and try again.";
                    return BadRequest();
                }
            }
            catch (Exception e) {
                return BadRequest();
            }

        }
        [HttpGet]
        public async Task<IActionResult> Result(string OrderId, string EmailSent, string Message, string OrderStatus) {
            ViewBag.Categories = await _ctx.Categories.ToListAsync();

            var result = new ResultViewModel {
                OrderId = OrderId,
                EmailSent = EmailSent,
                Message = Message,
                OrderStatus = OrderStatus
            };


            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Single(int id)
        {
            ViewBag.Categories = await _ctx.Categories.ToListAsync();

            var order = await _ctx.Orders
                .Include(o => o.CartProducts)
                .Where(o => o.OrderId == id)
                .FirstAsync();


            return View(order);
        }

    }
}

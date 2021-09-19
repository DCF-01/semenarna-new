using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using application.Areas.Cart.ViewModels;
using application.Data;
using application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace application.Areas.Cart.Controllers {
    [Area("Cart")]
    public class BaseController : Controller {
        private readonly ApplicationDbContext _ctx;
        private readonly UserManager<ApplicationUser> _userManager;
        public BaseController(ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager) {
            _ctx = applicationDbContext;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index() {
            ViewBag.Categories = await _ctx.Categories.ToListAsync();
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user != null) {
                var res = _ctx.Carts.Single(cart => cart.UserId == user.Id);

                return View(res);
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Get() {

            var user_id = _userManager.GetUserId(User);


            var user = await _ctx.Users.Include(user => user.Cart).ThenInclude(cart => cart.CartProducts).ThenInclude(product => product.Product)
                                    .Where(user => user.Id == user_id).FirstOrDefaultAsync();

            if (user.Cart.CartProducts != null) {

                var json_cart = new CartViewModel();

                var list = new List<CartProductViewModel>();
                foreach (var item in user.Cart.CartProducts) {
                    var product = new CartProductViewModel {
                        Id = item.Product.ProductId,
                        Price = item.Product.Price,
                        Img = "data:image/jpeg;base64, " + Convert.ToBase64String(item.Product.Img),
                        Name = item.Product.Name,
                        Quantity = item.Quantity,
                        Variations = item.Variations
                    };
                    list.Add(product);
                }
                json_cart.Items = list.ToArray();

                return Ok(json_cart);
            }
            else return Ok(null);
        }
        [HttpPost]
        public async Task<IActionResult> Update([FromBody] CartViewModel cartViewModel) {

            if (cartViewModel.Items != null) {
                //check client side no id doubles
                var checked_items = new List<CartProductViewModel>();
                foreach (var item in cartViewModel.Items) {
                    if (!checked_items.Contains(item)) {
                        checked_items.Add(item);
                    }
                }


                var user_id = _userManager.GetUserId(User);

                var user = _ctx.Users.Include(user => user.Cart).ThenInclude(cart => cart.CartProducts)
                                    .Where(user => user.Id == user_id).FirstOrDefault();

                


                var cart_product_list = new List<CartProduct>();
                foreach (var item in checked_items) {
                    var product_entity = _ctx.Products.Find(item.Id);

                    var cart_product = new CartProduct {
                        Product = product_entity,
                        Quantity = item.Quantity,
                    };
                    if(item.Variations != null) {
                        cart_product.Variations = item.Variations;
                    }

                    /*_ctx.CartProducts.Add(cart_product);
                    _ctx.SaveChanges();*/
                    cart_product_list.Add(cart_product);
                }

                user.Cart.CartProducts = cart_product_list;
                await _ctx.SaveChangesAsync();

                return Ok();
            }
            else {
                return Ok();
            }



        }
        public async Task<IActionResult> Checkout() {
            ViewBag.Categories = await _ctx.Categories.ToListAsync();

            return View();
        }
    }
}

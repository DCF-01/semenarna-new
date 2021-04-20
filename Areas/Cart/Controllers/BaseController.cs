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
using System.Text.Json;
using System.Text.Json.Serialization;

namespace semenarna_id2.Areas.Cart.Controllers {
    [Area("Cart")]
    public class BaseController : Controller {
        private readonly ApplicationDbContext _ctx;
        private readonly UserManager<ApplicationUser> _userManager;
        public BaseController(ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager) {
            _ctx = applicationDbContext;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index() {
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


            var user = _ctx.Users.Include(user => user.Cart).ThenInclude(cart => cart.CartProducts).ThenInclude(product => product.Product)
                                    .Where(user => user.Id == user_id).FirstOrDefault();

                            /*.Where(user => user.Id == user_id).FirstOrDefaultAsync();*/

            /*let product = new Product(data.id, data.name, data.price, 1, data.img);*/

            if (user.Cart.CartProducts != null) {


                var json_cart = new CartViewModel();/* {
                    items = new CartProductViewModel[user.Cart.CartProducts.Count()]
                };*/

                var list = new List<CartProductViewModel>();
                foreach (var item in user.Cart.CartProducts) {
                    var product = new CartProductViewModel {
                        Id = item.Product.ProductId,
                        Price = item.Product.Price,
                        Img = Convert.ToBase64String(item.Product.Img),
                        Name = item.Product.Name,
                        Quantity = item.Quantity
                    };
                    list.Add(product);
                }
                json_cart.items = list.ToArray();

                return Ok(json_cart);
                /*var a = new { CartProducts = new[] { 1, 2, 3 } };
                return Ok(a);*/
            }
            else return Ok(null);
        }
        [HttpPost]
        public async Task<IActionResult> Update([FromBody] CartViewModel cartViewModel) {

            if (cartViewModel.items != null) {



                var user_id = _userManager.GetUserId(User);

                var user = _ctx.Users.Include(user => user.Cart).ThenInclude(cart => cart.CartProducts)
                                    .Where(user => user.Id == user_id).FirstOrDefault();

                var cart_product_list = new List<CartProduct>();
                foreach (var item in cartViewModel.items) {
                    var product_entity = _ctx.Products.Find(item.Id);

                    var cart_product = new CartProduct {
                        Product = product_entity,
                        Quantity = item.Quantity
                    };
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
    }
}

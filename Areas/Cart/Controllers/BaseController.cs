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
        public IActionResult Get() {

            var user_id = _userManager.GetUserId(User);

            var cart = _ctx.Carts
                        .Where(cart => cart.UserId == user_id)
                        .Select(cart => cart).FirstOrDefault();

            return Ok(cart);
        }
        [HttpPost]
        public async Task<IActionResult> Update([FromBody] CartViewModel cartViewModel) {
            var user_id = _userManager.GetUserId(User);

            var user = await _ctx.Users.Include("Cart")
                        .Where(user => user.Id == user_id).FirstOrDefaultAsync();

            var new_products = _ctx.Products
                                    .Where(product => cartViewModel.CartItemIds
                                    .Contains(product.ProductId.ToString()))
                                    .Select(product => product).ToList();

            user.Cart.Products = new_products;
                        


            return Ok();
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using semenarna_id2.Data;
using semenarna_id2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace semenarna_id2.Areas.Cart.Controllers {
    [Area("Cart")]
    public class BaseController : Controller {
        private ApplicationDbContext _ctx;
        private UserManager<ApplicationUser> _userManager;
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
    }
}

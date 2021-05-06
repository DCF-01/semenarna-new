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
    public class OrderController : Controller {
        private readonly ApplicationDbContext _ctx;
        private readonly UserManager<ApplicationUser> _userManager;
        public OrderController(ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager) {
            _ctx = applicationDbContext;
            _userManager = userManager;
        }

        public IActionResult Process() {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using semenarna_id2.Data;
using semenarna_id2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace semenarna_id2.Areas.Store.Controllers {
    [Area("Store")]
    public class HomeController : Controller {
        private ApplicationDbContext _ctx;
        public HomeController(ApplicationDbContext ctx) {
            _ctx = ctx;
        }
        [HttpGet]
        public IActionResult Index() {

            var all = _ctx.TestProduct.ToList();

            return View(all);
        }

       /* public IActionResult Index() {

        }*/
    }
}

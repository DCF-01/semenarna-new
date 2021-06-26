using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using semenarna_id2.Areas.Panel.ViewModels;
using semenarna_id2.Data;
using semenarna_id2.Models;
using semenarna_id2.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace semenarna_id2.Controllers {
    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _ctx;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext applicationDbContext) {
            _ctx = applicationDbContext;
            _logger = logger;
        }

        public async Task<IActionResult> Index() {
            ViewBag.Categories = await _ctx.Categories.ToListAsync();

            return View();
        }

        public async Task<IActionResult> Privacy() {
            ViewBag.Categories = await _ctx.Categories.ToListAsync();
            return View();
        }

        public async Task<IActionResult> Contact() {
            ViewBag.Categories = await _ctx.Categories.ToListAsync();
            return View();
        }

        public async Task<IActionResult> Faq() {
            ViewBag.Categories = await _ctx.Categories.ToListAsync();
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetPromotion() {
            var active_promotion = await _ctx.Promotions
                                             .Where(p => p.Active == true)
                                             .Select(p => new PromotionViewModel {
                                                 DateToMil = p.DateTo.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds,
                                                 GetImg = Convert.ToBase64String(p.Img),
                                                 Text = p.Text,
                                                 Price = p.Price
                                             })
                                             .FirstOrDefaultAsync();


            return Ok(active_promotion);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

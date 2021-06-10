using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

        public IActionResult Index() {

            return View();
        }

        public IActionResult Privacy() {

            return View();
        }

        public IActionResult Contact() {

            return View();
        }

        public IActionResult Faq() {

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

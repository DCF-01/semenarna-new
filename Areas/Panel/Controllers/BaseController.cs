using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using semenarna_id2.Areas.Panel.ViewModels;
using semenarna_id2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace semenarna_id2.Areas.Panel.Controllers {
    [Authorize(Roles = "Admin")]
    [Area("Panel")]
    public class BaseController : Controller {
        [HttpGet]
        public IActionResult Index() {

            return View();
        }

        [HttpPost]
        public IActionResult Create(VariationViewModel variationViewModel) {
            
            /*var variation = new Variation {
                na
            }*/



            return null;
        }
    }
}

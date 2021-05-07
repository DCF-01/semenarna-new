using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using semenarna_id2.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace semenarna_id2.Areas.Panel.Controllers {
    [Authorize(Roles = "Admin")]
    [Area("Panel")]
    public class OrdersController : Controller {
        readonly ApplicationDbContext _ctx;
        public OrdersController(ApplicationDbContext applicationDbContext) {
            _ctx = applicationDbContext;
        }
        
        public IActionResult Index() {
            return View();
        }
    }
}

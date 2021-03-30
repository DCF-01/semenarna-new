using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace semenarna_id2.Controllers {
    [Authorize(Roles = "Admin")]
    public class PanelController : Controller {
        public IActionResult Index() {
            return PartialView();
        }
    }
}

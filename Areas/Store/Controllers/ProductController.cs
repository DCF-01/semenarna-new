using Microsoft.AspNetCore.Mvc;
using semenarna_id2.Areas.Store.ViewModels;
using semenarna_id2.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace semenarna_id2.Areas.Store.Controllers {
    [Area("Store")]
    public class ProductController : Controller {
        private ApplicationDbContext _ctx;
        public ProductController(ApplicationDbContext applicationDbContext) {
            _ctx = applicationDbContext;
        }
        public async Task<IActionResult> Single(int id) {

            var result = await _ctx.TestProduct.FindAsync(id);

            if (result != null) {
                var product = new ProductViewModel {
                    Id = result.Id,
                    Name = result.Name,
                    Description = result.Description,
                    Img = Convert.ToBase64String(result.Img)
                };
                return View(product);
            }

            return NotFound();

        }
    }
}

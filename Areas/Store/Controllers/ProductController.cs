using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

            var item = await _ctx.Products.Include(item => item.Categories)
                                    .Where(item => item.ProductId == id)
                                    .Select(item => item)
                                    .FirstOrDefaultAsync();

            if (item != null) {
                var product = new ProductViewModel {
                    ProductId = item.ProductId,
                    Name = item.Name,
                    Description = item.Description,
                    Price = item.Price,
                    SalePrice = item.SalePrice,
                    OnSale = item.OnSale,
                    InStock = item.InStock,
                    Categories = item.Categories,
                    Spec = null,
                    Img = Convert.ToBase64String(item.Img)
                };
                var a = 0;

                return View(product);
            }

            return NotFound();

        }
    }
}

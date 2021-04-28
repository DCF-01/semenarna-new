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
        [HttpGet]
        public async Task<IActionResult> Single(int id) {

            var item = await _ctx.Products.Include(item => item.Categories)
                                    .Include(item => item.Spec)
                                    .Where(item => item.ProductId == id)
                                    .Select(item => item)
                                    .FirstOrDefaultAsync();

            

            //finish spec

            if (item != null) {
                var spec = _ctx.Specs.Include(s => s.First)
                .Include(s => s.Second)
                .Include(s => s.Third)
                .Include(s => s.Fourth)
                .Where(s => s.Name == item.Spec.Name)
                .Select(s => s).FirstOrDefault();


                var product = new ProductViewModel {
                    ProductId = item.ProductId,
                    Name = item.Name,
                    Description = item.Description,
                    Price = item.Price,
                    SalePrice = item.SalePrice,
                    OnSale = item.OnSale,
                    InStock = item.InStock,
                    Categories = item.Categories,
                    Img = Convert.ToBase64String(item.Img)
                };

/*
                product.SpecFirst = new List<string>();
                foreach(var i in spec.First) {
                    product.SpecFirst.Add(i.First.);
                }*/

                return View(product);
            }

            return NotFound();

        }
    }
}

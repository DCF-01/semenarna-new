using Microsoft.AspNetCore.Mvc;
using semenarna_id2.Data;
using semenarna_id2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using semenarna_id2.Areas.Store.Controllers;
using Microsoft.EntityFrameworkCore;
using semenarna_id2.Areas.Store.ViewModels;

namespace semenarna_id2.Areas.Store.Controllers {
    [Area("Store")]
    public class QueryController : Controller {
        private ApplicationDbContext _ctx;
        public QueryController(ApplicationDbContext applicationDbContext) {
            _ctx = applicationDbContext;
        }

        /*private TestProductModel[] StartsWith() {
            return
        }*/

        public IActionResult Index() {
            return View();
        }


        public async Task<IActionResult> Single(int id) {
            var product = await _ctx.Products.FindAsync(id);

            return Ok(product);
        }

        public IActionResult Find([FromQuery] string name = "", [FromQuery] string product_id = "") {

            try {
                if (Request.Query.ContainsKey("name")) {
                    var products = _ctx.Products
                                    .Where(p => EF.Functions.ILike(p.Name, $"%{name}%"))
                                    .Select(p => p)
                                    .ToList();


                    return Ok(products);
                }
                else if (Request.Query.ContainsKey("product_id")) {
                    var products = _ctx.Products
                                    .Where(p => EF.Functions.ILike(p.ProductId.ToString(), $"%{product_id}%"))
                                    .Select(p => p)
                                    .ToList();


                    return Ok(products);
                }
                else {
                    return NotFound();
                }
            }
            catch (Exception e) {
                Console.WriteLine(e);

                return NotFound();
            }


        }
        [HttpGet]
        public async Task<IActionResult> GetCarousel([FromQuery] string category = "") {

            if (category == "") {
                return NotFound();
            }
            else {
                var products = await _ctx.Categories.Include(c => c.Products)
                                                .Where(c => c.Name == category)
                                                .Select(c => c.Products)
                                                .FirstOrDefaultAsync();
                if(products == null) {
                    return NotFound();
                }

                List<CarouselProductViewModel> product_list = new List<CarouselProductViewModel>();

                foreach (var item in products) {
                    var p = new CarouselProductViewModel {
                        Id = item.ProductId,
                        Name = item.Name,
                        Price = item.Price,
                        Img = Convert.ToBase64String(item.Img),
                        Category = category,
                        OnSale = item.OnSale
                    };
                    if (item.OnSale) {
                        p.SalePrice = item.SalePrice;
                    }

                    product_list.Add(p);
                }
                return Ok(product_list);
            }
        }
    }
}

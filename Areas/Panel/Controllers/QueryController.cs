using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using semenarna_id2.Areas.Panel.ViewModels;
using semenarna_id2.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace semenarna_id2.Areas.Panel.Controllers {
    [Authorize(Roles = "Admin")]
    [Area("Panel")]
    public class QueryController : Controller {
        readonly ApplicationDbContext _ctx;
        public QueryController(ApplicationDbContext applicationDbContext) {
            _ctx = applicationDbContext;
        }

        public static List<QueryProductViewModel> RemoveDuplicates(List<QueryProductViewModel> queryProducts) {

            List<QueryProductViewModel> list = new List<QueryProductViewModel>();

            list = queryProducts.GroupBy(p => p.Id)
                                .Select(p => p.First())
                                .ToList();

            return list;
        }

        [HttpGet]
        public async Task<IActionResult> Categories([FromQuery] string name) {

            var categories = await _ctx.Categories
                                 .Where(c => c.Name == name)
                                 .ToListAsync();

            return Ok(categories);
        }
        [HttpGet]
        public async Task<IActionResult> Products([FromQuery] string name, [FromQuery] string sku, [FromQuery] string category) {

            List<QueryProductViewModel> products;
            if (name != null) {
                products = await _ctx.Products
                                   .Where(p => p.Name.StartsWith(name))
                                   .Select(p => new QueryProductViewModel {
                                       Id = p.ProductId,
                                       Name = p.Name,
                                       Description = p.Description
                                   })
                                   .ToListAsync();
            }
            else if(sku != null) {
                products = await _ctx.Products
                                   .Where(p => p.SKU.StartsWith(sku))
                                   .Select(p => new QueryProductViewModel {
                                       Id = p.ProductId,
                                       Name = p.Name,
                                       Description = p.Description
                                   })
                                   .ToListAsync();
            }
            else if(category != null) {

                var all_products = await _ctx.Categories
                                           .Include(c => c.Products)
                                           .Where(c => c.Name.StartsWith(category))
                                           .Select(c => c.Products)
                                           .ToListAsync();


                products = new List<QueryProductViewModel>();

                foreach(var item in all_products) {
                    foreach(var product in item) {
                        var new_product = new QueryProductViewModel {
                            Id = product.ProductId,
                            Name = product.Name,
                            Description = product.Description
                        };
                        products.Add(new_product);
                    }
                }

                return Ok(RemoveDuplicates(products));

               
            }
            else {
                products = await _ctx.Products
                                   .Select(p => new QueryProductViewModel {
                                       Id = p.ProductId,
                                       Name = p.Name,
                                       Description = p.Description
                                   })
                                   .ToListAsync();
            }

            return Ok(products);

        }


    }
}

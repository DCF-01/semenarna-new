using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using application.Areas.Panel.ViewModels;
using application.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace application.Areas.Panel.Controllers {
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
                                 .Where(c => c.Name.Replace(" ", "").ToLower() == name.Replace(" ", "").ToLower())
                                 .ToListAsync();

            return Ok(categories);
        }

        [HttpGet]
        public async Task<IActionResult> Orders([FromQuery] string orderId = "") {
            if (orderId != null) {
                var orders = await _ctx.Orders
                                        .Where(o => o.OrderId.ToString().StartsWith(orderId))
                                        .ToListAsync();
                return Ok(orders);
            }
            else {
                var orders = await _ctx.Orders
                                        .ToListAsync();
                return Ok(orders);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Products([FromQuery] string name, [FromQuery] string sku, [FromQuery] string category) {

            List<QueryProductViewModel> products;
            if (name != null) {
                products = await _ctx.Products
                                   .Where(p => p.Name.Replace(" ", "").ToLower().StartsWith(name.Replace(" ", "").ToLower()))
                                   .Select(p => new QueryProductViewModel {
                                       Id = p.ProductId,
                                       Name = p.Name,
                                       Description = p.Description
                                   })
                                   .ToListAsync();
            }
            else if(sku != null) {
                products = await _ctx.Products
                                   .Where(p => p.SKU.Replace(" ", "").ToLower().StartsWith(sku.Replace(" ", "").ToLower()))
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
                                           .Where(c => c.Name.Replace(" ", "").ToLower().StartsWith(category.Replace(" ", "").ToLower()))
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

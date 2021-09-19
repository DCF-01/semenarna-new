﻿using Microsoft.AspNetCore.Mvc;
using application.Data;
using application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using application.Areas.Store.Controllers;
using Microsoft.EntityFrameworkCore;
using application.Areas.Store.ViewModels;
using ImageMagick;
using System.IO;

namespace application.Areas.Store.Controllers {
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

        public async Task<IActionResult> Find([FromQuery] string name = "", [FromQuery] string SKU = "") {

            try {
                if (Request.Query.ContainsKey("name")) {
                    var products = await _ctx.Products
                                    .Where(p => p.Name.ToLower().StartsWith(name.ToLower()))
                                    .Select(p => new QueryProductViewModel {
                                        ProductId = p.ProductId,
                                        Name = p.Name,
                                        UncompressedImg = p.Img,
                                        Price = p.Price
                                    })
                                    .Take(5).ToListAsync();

                    foreach (var item in products) {
                        using (var stream = new MemoryStream(item.UncompressedImg)) {
                            var optimizer = new ImageOptimizer();
                            optimizer.Compress(stream);
                            item.Img = Convert.ToBase64String(stream.ToArray());
                            item.UncompressedImg = null;
                        }
                    }


                    return Ok(products);
                }
                else if (Request.Query.ContainsKey("SKU")) {
                    var products = await _ctx.Products
                                    .Where(p => p.Name.ToLower().StartsWith(name.ToLower()))
                                    .Select(p => new QueryProductViewModel {
                                        ProductId = p.ProductId,
                                        Name = p.Name,
                                        UncompressedImg = p.Img,
                                        Price = p.Price
                                    })
                                    .Take(5).ToListAsync();

                    foreach (var item in products) {
                        using (var stream = new MemoryStream(item.UncompressedImg)) {
                            var optimizer = new ImageOptimizer();
                            optimizer.Compress(stream);
                            item.Img = Convert.ToBase64String(stream.ToArray());
                            item.UncompressedImg = null;
                        }
                    }



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
                if (products == null) {
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

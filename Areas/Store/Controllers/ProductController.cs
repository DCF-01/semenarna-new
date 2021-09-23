﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using application.Areas.Store.ViewModels;
using application.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace application.Areas.Store.Controllers {
    [Area("Store")]
    public class ProductController : Controller {
        private ApplicationDbContext _ctx;
        public ProductController(ApplicationDbContext applicationDbContext) {
            _ctx = applicationDbContext;
        }
        [HttpGet]
        public async Task<IActionResult> Single(int id) {
            ViewBag.Categories = await _ctx.Categories.ToListAsync();
            try {


                var item = await _ctx.Products.Include(item => item.Categories)
                                        .Include(item => item.Spec)
                                        .Include(item => item.GalleryImages)
                                        .Include(item => item.Variations)
                                        .Where(item => item.ProductId == id)
                                        .Select(item => item)
                                        .FirstOrDefaultAsync();


                var related_category = item.Categories.FirstOrDefault();

                var related_products = await _ctx.Products
                                            .Include(p => p.Categories)
                                            .Where(p => p.Categories.Contains(item.Categories.FirstOrDefault()))
                                            .Select(p => p).ToArrayAsync();



                //finish spec

                if (item != null) {
                    var spec = await _ctx.Specs
                    .Where(s => s.Name == item.Spec.Name)
                    .Select(s => s).FirstOrDefaultAsync();



                    var product = new ProductViewModel {
                        ProductId = item.ProductId,
                        SKU = item.SKU,
                        Name = item.Name,
                        Description = item.Description,
                        ShortDescription = item.ShortDescription,
                        Price = item.Price,
                        SalePrice = item.SalePrice,
                        OnSale = item.OnSale,
                        InStock = item.InStock,
                        Categories = item.Categories,
                        Variations = item.Variations.ToList(),
                        Spec = new StoreSpecViewModel {
                            ItemsPerRow = spec.ItemsPerRow,
                            First = spec.First,
                            Rest = spec.Rest,
                            Name = spec.Name
                        },
                        Img = Convert.ToBase64String(item.Img),
                        RelatedProducts = related_products
                    };
                    if (item.GalleryImages != null) {
                        List<string> img_gallery = new List<string>();
                        foreach (var image in item.GalleryImages) {
                            img_gallery.Add(Convert.ToBase64String(image.Img));
                        }
                        product.GalleryImages = img_gallery;
                    }
                    return View(product);
                }

                return NotFound();

            }
            catch(Exception e) {
                Console.WriteLine(e.Message);
                return BadRequest();
            }
        }
    }
}

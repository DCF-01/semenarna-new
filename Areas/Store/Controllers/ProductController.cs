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
                        Name = item.Name,
                        Description = item.Description,
                        Price = int.Parse(item.Price),
                        SalePrice = int.Parse(item.SalePrice),
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

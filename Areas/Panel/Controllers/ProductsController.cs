using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using semenarna_id2.Areas.Panel.ViewModels;
using semenarna_id2.Data;
using semenarna_id2.Models;
using semenarna_id2.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace semenarna_id2.Controllers {
    [Authorize(Roles = "Admin")]
    [Area("Panel")]
    public class ProductsController : Controller {
        readonly ApplicationDbContext _ctx;
        readonly UserManager<ApplicationUser> _userManager;

        public ProductsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager) {
            _ctx = context;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> Index() {
            var result = await _ctx.Products.ToListAsync();

            return View(result);
        }
        [HttpGet]
        public async Task<IActionResult> Create() {


            var categories = await _ctx.Categories
                .Select(c => c.Name).ToListAsync();
            var specs = await _ctx.Specs
                .Select(spec => spec.Name).ToListAsync();
            var variations = await _ctx.Variations
                            .Select(v => v.Name).ToListAsync();


            var productViewModel = new ProductViewModel {
                GetCategories = categories,
                GetSpecs = specs,
                CurrentSpec = "",
                GetVariations = variations
            };

            return View(productViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel product_data) {
            try {
                bool sale_state = (product_data.OnSale != null) || false;
                bool stock_state = (product_data.InStock != null) || false;

                var categories = from c in _ctx.Categories
                                 where product_data.Categories.Contains(c.Name)
                                 select c;

                var variations = await _ctx.Variations
                                .Where(item => product_data.Variations.Contains(item.Name))
                                .Select(item => item).ToListAsync();


                var spec = _ctx.Specs
                .Where(s => s.Name == product_data.CurrentSpec)
                .Select(s => s).FirstOrDefault();

                var product = new Product {
                    Name = product_data.Name,
                    SKU = product_data.SKU,
                    Description = product_data.Description,
                    ShortDescription = product_data.ShortDescription,
                    Price = product_data.Price,
                    OnSale = sale_state,
                    InStock = stock_state,
                    Spec = spec,
                    Variations = variations
                };
                if(product_data.SalePrice == null) {
                    product.SalePrice = "0";
                }
                else {
                    product.SalePrice = product_data.SalePrice;
                }


                product.Categories = new List<Category>();

                foreach (var item in categories) {
                    product.Categories.Add(item);
                };



                IFormFile Image = product_data.Img;

                if (Image != null && Image.Length > 0) {
                    byte[] ba = null;
                    using (var fs = Image.OpenReadStream())
                    using (var ms = new MemoryStream()) {
                        fs.CopyTo(ms);
                        ba = ms.ToArray();
                    }
                    product.Img = ba;
                }
                else {
                    product.Img = null;
                    throw new Exception("No Image file was provided");
                }

                if (product_data.GalleryImages != null) {
                    product.GalleryImages = new List<Image>();

                    foreach (IFormFile image in product_data.GalleryImages) {
                        byte[] ba = null;
                        using (var fs = image.OpenReadStream())
                        using (var ms = new MemoryStream()) {
                            fs.CopyTo(ms);
                            ba = ms.ToArray();
                        }
                        var gallery_image = new Image {
                            Img = ba
                        };
                        product.GalleryImages.Add(gallery_image);
                    }
                }

                await _ctx.Products.AddAsync(product);

                await _ctx.SaveChangesAsync();

                return RedirectToAction("Create", "Products");
            }
            catch (Exception e) {
                Console.WriteLine(e);
                return NotFound($"Error: {e}");
            }
        }


        [HttpDelete]
        public IActionResult Delete([FromRoute] int id) {
            try {
                //delete product
                var product = _ctx.Products.Find(id);

                if (product != null) {
                    _ctx.Products.Remove(product);
                    _ctx.SaveChanges();
                    return Ok();
                }
                else {
                    return StatusCode(500);
                }
            }
            catch (Exception e) {
                return BadRequest(e.Message);
            }
        }
        //Display edited product
        [HttpGet]
        public async Task<IActionResult> Manage(int id) {
            try {
                //get product with related categories
                var product = await _ctx.Products.Include(product => product.Categories)
                    .Include(product => product.Spec)
                    .Include(product => product.Variations)
                    .Where(product => product.ProductId == id)
                    .FirstOrDefaultAsync();

                var all_specs = await _ctx.Specs
                    .Select(s => s.Name).ToListAsync();

                var all_variations = await _ctx.Variations
                    .Select(item => item.Name).ToListAsync();

                var current_variations = await _ctx.Variations
                                        .Where(item => product.Variations.Contains(item))
                                        .Select(item => item.Name).ToListAsync();

                var byte_arr_img = product.Img;

                string img = Convert.ToBase64String(byte_arr_img);

                var item = new ProductViewModel {
                    Name = product.Name,
                    SKU = product.SKU,
                    Description = product.Description,
                    ShortDescription = product.ShortDescription,
                    Price = product.Price,
                    SalePrice = product.SalePrice,
                    OnSale = product.OnSale.ToString(),
                    InStock = product.InStock.ToString(),
                    CurrentSpec = product.Spec.Name,
                    GetSpecs = all_specs,
                    GetImg = img,
                    GetVariations = all_variations,
                    CurrentVariations = current_variations
                };

                //current active categories
                item.GetCategories = new List<string>();

                //all available categories
                item.Categories = _ctx.Categories.Select(item => item.Name).ToArray();

                if (product.Categories != null) {
                    foreach (var i in product.Categories) {
                        item.GetCategories.Add(i.Name);
                    }
                }

                return View(item);
            }
            catch (Exception e) {
                return BadRequest($"Error: {e.Message}");
            }
        }
        //Update product route
        [HttpPost]
        public async Task<IActionResult> Manage(int id, ProductViewModel productViewModel) {
            try {

                if (productViewModel.Categories == null) {
                    throw new Exception("A product must have at least 1 category");
                }
                if (productViewModel.Name == null) {
                    throw new Exception("The product must have a name");
                }
                if (productViewModel.Description == null) {
                    throw new Exception("The product must have a description");
                }
                if (productViewModel.CurrentSpec == null) {
                    throw new Exception("Spec required");
                }

                var spec = _ctx.Specs
                    .Where(s => s.Name == productViewModel.CurrentSpec)
                    .Select(s => s).FirstOrDefault();

                // Product Img
                IFormFile Image;

                if (productViewModel.Img != null) {
                    Image = productViewModel.Img;
                }
                else {
                    Image = null;
                }
                //product to update
                var entity = await _ctx.Products
                    .Include(product => product.Categories)
                    .FirstOrDefaultAsync(item => item.ProductId == id);

                bool sale_state = (productViewModel.OnSale != null) || false;
                bool stock_state = (productViewModel.InStock != null) || false;



                var categories = from c in _ctx.Categories
                                 where productViewModel.Categories.Contains(c.Name)
                                 select c;

                var variations = await _ctx.Variations
                                    .Where(item => productViewModel.Variations.Contains(item.Name))
                                    .Select(item => item).ToListAsync();

                //remove all from product categories field
                entity.Categories = new List<Category>();
                entity.Variations = new List<Variation>();


                foreach (var item in categories) {
                    entity.Categories.Add(item);
                };

                foreach(var item in variations) {
                    entity.Variations.Add(item);
                };

                entity.Name = productViewModel.Name;
                entity.SKU = productViewModel.SKU;
                entity.ShortDescription = productViewModel.ShortDescription;
                entity.Description = productViewModel.Description;
                entity.Price = productViewModel.Price;
                entity.SalePrice = productViewModel.SalePrice;
                entity.OnSale = sale_state;
                entity.InStock = stock_state;
                entity.Spec = spec;


                if (Image != null) {
                    if (Image.Length > 0) {
                        byte[] p1 = null;
                        using (var fs = Image.OpenReadStream())
                        using (var ms = new MemoryStream()) {
                            fs.CopyTo(ms);
                            p1 = ms.ToArray();
                        }
                        entity.Img = p1;
                    }
                }

                await _ctx.SaveChangesAsync();

                return RedirectToAction("Index", "Products");

            }
            catch (Exception e) {
                return BadRequest("Bad request: " + e.Message);
            }
        }

    }
}

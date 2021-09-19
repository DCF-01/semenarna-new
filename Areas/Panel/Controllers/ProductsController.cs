using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using application.Areas.Panel.ViewModels;
using application.Data;
using application.Models;
using application.Utils;
using application.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace application.Controllers {
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
                if (product_data.Name == null) {
                    throw new Exception("The product must have a name");
                }
                if (product_data.Description == null) {
                    throw new Exception("The product must have a description");
                }
                if (product_data.ShortDescription == null) {
                    throw new Exception("The product must have a short description");
                }
                if (product_data.Price == null) {
                    throw new Exception("Price required");
                }

                bool sale_state = (product_data.OnSale != null) || false;
                bool stock_state = (product_data.InStock != null) || false;


                var product = new Product {
                    Name = product_data.Name,
                    SKU = product_data.SKU,
                    Description = product_data.Description,
                    ShortDescription = product_data.ShortDescription,
                    Price = product_data.Price,
                    OnSale = sale_state,
                    InStock = stock_state,
                };
                if (product_data.SalePrice == null) {
                    product.SalePrice = product_data.Price;
                }
                else {
                    product.SalePrice = product_data.SalePrice;
                }

                //set variations if null provided by user
                if (product_data.Variations == null) {
                    var default_variation = await _ctx.Variations
                                        .Where(v => v.Name == "нема")
                                        .Select(v => v)
                                        .ToListAsync();

                    product.Variations = default_variation;
                }
                else {
                    var new_variations = await _ctx.Variations
                                    .Where(item => product_data.Variations.Contains(item.Name))
                                    .Select(item => item)
                                    .ToListAsync();

                    product.Variations = new_variations;
                }
                //set categories if null provided by user
                if (product_data.Categories == null) {
                    var default_category = await _ctx.Categories
                                                .Where(c => c.Name == "uncategorized")
                                                .Select(c => c)
                                                .ToListAsync();
                    product.Categories = default_category;
                }
                else {
                    var new_categories = await _ctx.Categories
                                                    .Where(c => product_data.Categories.Contains(c.Name))
                                                    .Select(c => c)
                                                    .ToListAsync();
                    product.Categories = new_categories;
                }
                //set spec if null provided by user
                if (product_data.CurrentSpec == null) {
                    var default_spec = await _ctx.Specs
                                                 .Where(s => s.Name == "Empty")
                                                 .Select(s => s)
                                                 .FirstOrDefaultAsync();
                    product.Spec = default_spec;
                }
                else {
                    var new_spec = _ctx.Specs
                    .Where(s => s.Name == product_data.CurrentSpec)
                    .Select(s => s).FirstOrDefault();

                    product.Spec = new_spec;
                }


                IFormFile Image = product_data.Img;

                if (Image != null && Image.Length > 0) {
                    var bytes = await Image.GetBytesAsync();
                    product.Img = bytes.CompressBytes();

                }
                else {
                    product.Img = null;
                    throw new Exception("No Image file was provided");
                }

                if (product_data.GalleryImages != null) {
                    product.GalleryImages = new List<Image>();

                    foreach (IFormFile image in product_data.GalleryImages) {
                        var gallery_image = new Image {
                            Img = (await image.GetBytesAsync()).CompressBytes()
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
        public async Task<IActionResult> Delete([FromRoute] int id) {
            try {
                var product = await _ctx.Products.FindAsync(id);

                //remove product and range of related cartproducts
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

                if (productViewModel.Name == null) {
                    throw new Exception("The product must have a name");
                }
                if (productViewModel.Description == null) {
                    throw new Exception("The product must have a description");
                }
                if (productViewModel.ShortDescription == null) {
                    throw new Exception("The product must have a short description");
                }
                if (productViewModel.Price == null) {
                    throw new Exception("Price required");
                }

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
                    .Include(product => product.Variations)
                    .Include(product => product.Spec)
                    .Include(product => product.GalleryImages)
                    .FirstOrDefaultAsync(item => item.ProductId == id);

                bool sale_state = (productViewModel.OnSale != null) || false;
                bool stock_state = (productViewModel.InStock != null) || false;

                entity.Name = productViewModel.Name;
                entity.SKU = productViewModel.SKU;
                entity.ShortDescription = productViewModel.ShortDescription;
                entity.Description = productViewModel.Description;
                entity.Price = productViewModel.Price;
                entity.OnSale = sale_state;
                entity.InStock = stock_state;

                if (productViewModel.SalePrice == null) {
                    entity.SalePrice = productViewModel.Price;
                }
                else {
                    entity.SalePrice = productViewModel.SalePrice;
                }

                //set variations if null provided by user
                if (productViewModel.Variations == null) {
                    var default_variation = await _ctx.Variations
                                        .Where(v => v.Name == "нема")
                                        .Select(v => v)
                                        .ToListAsync();

                    entity.Variations = default_variation;
                }
                else {
                    var new_variations = await _ctx.Variations
                                    .Where(item => productViewModel.Variations.Contains(item.Name))
                                    .Select(item => item)
                                    .ToListAsync();

                    entity.Variations = new_variations;
                }

                //set categories if null provided by user
                if (productViewModel.Categories == null) {
                    var default_category = await _ctx.Categories
                                                .Where(c => c.Name == "uncategorized")
                                                .Select(c => c)
                                                .ToListAsync();
                    entity.Categories = default_category;
                }
                else {
                    var new_categories = await _ctx.Categories
                                                    .Where(c => productViewModel.Categories.Contains(c.Name))
                                                    .Select(c => c)
                                                    .ToListAsync();
                    entity.Categories = new_categories;
                }
                //set spec if null provided by user
                if (productViewModel.CurrentSpec == null) {
                    var default_spec = await _ctx.Specs
                                                 .Where(s => s.Name == "Empty")
                                                 .Select(s => s)
                                                 .FirstOrDefaultAsync();
                    entity.Spec = default_spec;
                }
                else {
                    var new_spec = _ctx.Specs
                    .Where(s => s.Name == productViewModel.CurrentSpec)
                    .Select(s => s).FirstOrDefault();

                    entity.Spec = new_spec;
                }


                if (Image != null) {
                    if (Image.Length > 0) {
                        entity.Img = (await Image.GetBytesAsync()).CompressBytes();
                    }
                }

                if (productViewModel.GalleryImages != null) {
                    entity.GalleryImages = new List<Image>();

                    foreach (IFormFile image in productViewModel.GalleryImages) {
                        var gallery_image = new Image {
                            Img = (await image.GetBytesAsync()).CompressBytes()
                        };
                        entity.GalleryImages.Add(gallery_image);
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

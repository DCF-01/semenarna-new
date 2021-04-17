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
        private ApplicationDbContext _ctx;

        public ProductsController(ApplicationDbContext context) {
            _ctx = context;
        }
        [HttpGet]
        public async Task<IActionResult> Index() {
            var result = await _ctx.Products.ToListAsync();

            /*var data = result.Select(c => new ProductViewModel {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            }).ToList();*/

            return View(result);
        }
        [HttpGet]
        public async Task<IActionResult> Create() {

            var all = from c in _ctx.Categories
                      select c;

            var categories = new ProductViewModel {
                GetCategories = new List<Category>()
            };

            foreach (var item in all) {
                categories.GetCategories.Add(item);
            }

            return View(categories);
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel product_data) {
            try {
                bool sale_state = (product_data.OnSale != null) || false;
                bool stock_state = (product_data.InStock != null) || false;

                var categories = from c in _ctx.Categories
                                 where product_data.Categories.Contains(c.Name)
                                 select c;


                var product = new Product {
                    Name = product_data.Name,
                    Description = product_data.Description,
                    Price = product_data.Price,
                    SalePrice = product_data.SalePrice,
                    OnSale = sale_state,
                    InStock = stock_state,
                    Spec = null
                };
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
        public async Task<IActionResult> Delete(int id) {
            //delete product
            var product = _ctx.Products.Find(id);
            _ctx.Products.Remove(product);

            if (product != null) {
                await _ctx.SaveChangesAsync();
                return NoContent();
            }
            else {
                return BadRequest();
            }
        }
        //Display edited product
        [HttpGet]
        public IActionResult Details(int id) {

            var all_categories = _ctx.Categories.ToArray();
            
            var data = from p in _ctx.Products.Include(product => product.Categories)
                       where p.ProductId == id
                       select p;


            var result = data.Single();

            var byte_arr_img = result.Img;

            string img = Convert.ToBase64String(byte_arr_img);

            var item = new ProductViewModel {
                Name = result.Name,
                Description = result.Description,
                Price = result.Price,
                SalePrice = result.SalePrice,
                OnSale = result.OnSale.ToString(),
                InStock = result.InStock.ToString(),
                Spec = null,
                GetImg = img
            };

            item.GetCategories = new List<Category>();

            //all categories - for comparison checkec/unchecked categories view
            item.Categories = new string[all_categories.Length];
            for(int i = 0; i < all_categories.Length; i++) {
                item.Categories[i] = all_categories[i].Name;
            }

            if (result.Categories != null) {
                foreach (var i in result.Categories) {
                    item.GetCategories.Add(i);
                }
            }



            return View(item);
        }
        //Update product route
        [HttpPost]
        public async Task<IActionResult> Details(int id, ProductViewModel productViewModel) {
            try {

                if (productViewModel.Categories == null) {
                    throw new Exception("A product must have at least 1 category");
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
                var entity = await _ctx.Products.FirstOrDefaultAsync(item => item.ProductId == id);

                bool sale_state = (productViewModel.OnSale != null) || false;
                bool stock_state = (productViewModel.InStock != null) || false;



                var categories = from c in _ctx.Categories
                                 where productViewModel.Categories.Contains(c.Name)
                                 select c;

                var product = new Product {
                    Name = productViewModel.Name,
                    Description = productViewModel.Description,
                    Price = productViewModel.Price,
                    SalePrice = productViewModel.SalePrice,
                    OnSale = sale_state,
                    InStock = stock_state,
                    Spec = null
                };
                product.Categories = new List<Category>();

                foreach (var item in categories) {
                    product.Categories.Add(item);
                };

                entity.Name = productViewModel.Name;
                entity.Description = productViewModel.Description;


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
            catch(Exception e) {
                return BadRequest("Bad request: " + e.Message);
            }
        }

    }
}

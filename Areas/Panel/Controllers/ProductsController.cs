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
            var result = await _ctx.TestProduct.ToListAsync();

            /*var data = result.Select(c => new ProductViewModel {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            }).ToList();*/

            return View(result);
        }
        [HttpGet]
        public IActionResult Create() {
            return View(new ProductViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel product_data) {
            try {

                var product = new TestProductModel {
                    Name = product_data.Name,
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
                    throw new Exception("No Image file was provided");
                }

                await _ctx.TestProduct.AddAsync(product);

                await _ctx.SaveChangesAsync();

                return RedirectToAction("Create", "Products");
            }
            catch (Exception e) {
                Console.WriteLine(e);
                return NotFound("Error: No file provided");
            }
        }


        [HttpDelete]
        public async Task<IActionResult> Delete(int id) {
            //delete product
            var product = _ctx.TestProduct.Find(id);
            _ctx.TestProduct.Remove(product);

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
            var result = _ctx.TestProduct.Find(id);

            var byte_arr_img = result.Img;

            string img = Convert.ToBase64String(byte_arr_img);

            var item = new ProductViewModel {
                Name = result.Name,
                Description = result.Description,
                GetImg = img
            };

            return View(item);
        }
        //Update product route
        [HttpPost]
        public async Task<IActionResult> Details(int id, ProductViewModel productViewModel) {
            IFormFile Image = productViewModel.Img;

            var updated_product = new TestProductModel {
                Id = id,
                Name = productViewModel.Name,
                Description = productViewModel.Description,
            };

            if (Image != null) {
                if (Image.Length > 0) {
                    byte[] p1 = null;
                    using (var fs = Image.OpenReadStream())
                    using (var ms = new MemoryStream()) {
                        fs.CopyTo(ms);
                        p1 = ms.ToArray();
                    }
                    updated_product.Img = p1;
                }
            }



            var result = _ctx.TestProduct.Update(updated_product);

            await _ctx.SaveChangesAsync();

            return RedirectToAction("Index", "Products");
        }

    }
}

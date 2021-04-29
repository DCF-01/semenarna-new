using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using semenarna_id2.Areas.Panel.ViewModels;
using semenarna_id2.Data;
using semenarna_id2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace semenarna_id2.Areas.Panel.Controllers {
    [Authorize(Roles = "Admin")]
    [Area("Panel")]
    public class CategoriesController : Controller {
        private ApplicationDbContext _ctx;
        public CategoriesController(ApplicationDbContext applicationDbContext) {
            _ctx = applicationDbContext;
        }
        [HttpGet]
        public IActionResult Index() {

            var all_categories = _ctx.Categories.Select(item => new CategoryViewModel {
                Id = item.CategoryId,
                Name = item.Name,
                ProductCount = item.Products.Count()
            }).ToList();


            if (all_categories.Count > 0) {

                return View(all_categories);
            }
            else {
                var empty_list = new List<CategoryViewModel>();
                return View(empty_list);
            }

        }
        [HttpGet]
        public IActionResult Create() {

            /*var all = from item in _ctx.Categories
                      select item;*/

            /*var res = all.ToList();*/

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(CategoryViewModel categoryViewModel) {

            var category = new Category {
                Name = categoryViewModel.Name,
                Products = null
            };

            await _ctx.Categories.AddAsync(category);

            await _ctx.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id) {
            try {
                if (id == null) {
                    throw new Exception("Item id not cannot be empty.");
                }
                var category = await _ctx.Categories.FindAsync(id);
                var res = _ctx.Categories.Remove(category);

                await _ctx.SaveChangesAsync();

                return Ok($"Item {category.Name} deleted");
            }
            catch (Exception e) {
                return BadRequest(e.Message);
            }
        }
    }
}

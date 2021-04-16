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

            var all = from x in _ctx.Categories
                      select x;


            var result = all.ToList();
            
            
            return View(result);
        }
        [HttpGet]
        public IActionResult Create() {
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
    }
}

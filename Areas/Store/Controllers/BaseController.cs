using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using semenarna_id2.Areas.Store.ViewModels;
using semenarna_id2.Data;
using semenarna_id2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace semenarna_id2.Areas.Store.Controllers {
    [Area("Store")]
    public class BaseController : Controller {
        private ApplicationDbContext _ctx;
        public BaseController(ApplicationDbContext ctx) {
            _ctx = ctx;
        }
        // returns 24 products (size of page) of page number (id) 
        public StoreViewModel GetProductList(int id, Product[] all, int products_on_page = 24) {

            int page_number;

            if (id < 1) {
                page_number = 1;
            }
            else {
                page_number = id;
            }


            //max page number
            int number_of_pages = (all.Length + products_on_page - 1) / products_on_page;

            //products that will return to view
            List<Product> product_list = new List<Product>();

            //start, end, max at
            int start = (page_number - 1) * products_on_page;
            int end = start + products_on_page;
            int max;
            if (all.Length < end) {
                max = all.Length;
            }
            else {
                max = end;
            }

            //item list to return
            List<ProductViewModel> item_list = new List<ProductViewModel>();
            //array with products --> start to max
            var products_range = all[start..max];

            foreach (var item in products_range) {
                var new_item = new ProductViewModel {
                    ProductId = item.ProductId,
                    Name = item.Name,
                    Description = item.Description,
                    Price = int.Parse(item.Price),
                    OnSale = item.OnSale,
                    InStock = item.InStock,
                    Categories = item.Categories,
                    Img = Convert.ToBase64String(item.Img)
                };
                if (new_item.OnSale) {
                    new_item.SalePrice = int.Parse(item.SalePrice);
                }

                item_list.Add(new_item);
            }

            var view_model = new StoreViewModel {
                Products = item_list,
                Page_number = page_number,
                Number_of_pages = number_of_pages,
            };

            return view_model;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] string products_on_page = "24", int id = 0) {
            // all products
            var all_products = await _ctx.Products
                                        .Include(item => item.Categories)
                                        .ToArrayAsync();
            var categories = await _ctx.Categories.ToListAsync();

            //return # product of selected page(id)
            var available_numbers = new[] { 12, 24, 48 };
            StoreViewModel model;

            if (available_numbers.Contains(int.Parse(products_on_page))) {
                model = GetProductList(id, all_products, int.Parse(products_on_page));
            }
            else {
                model = GetProductList(id, all_products);
            }

            model.Categories = categories;

            return View("Index", model);
        }
        [HttpGet]
        public async Task<IActionResult> Find([FromQuery] string name = "", [FromQuery] string product_id = "") {
            if (Request.Query.ContainsKey("name")) {

                var all_products = _ctx.Products
                                .Where(p => EF.Functions.ILike(p.Name, $"%{name}%"))
                                .Select(p => p)
                                .ToArray();

                var model = GetProductList(0, all_products);
                model.Categories = await _ctx.Categories.ToListAsync();
                model.CurrentCategory = "none";

                return View("Index", model);
            }
            else if (Request.Query.ContainsKey("product_id")) {

                var all_products = _ctx.Products
                                .Where(p => EF.Functions.ILike(p.ProductId.ToString(), $"%{product_id}%"))
                                .Select(p => p)
                                .ToArray();

                var model = GetProductList(0, all_products);
                model.Categories = await _ctx.Categories.ToListAsync();
                model.CurrentCategory = "none";

                return View("Index", model);
            }
            else {
                return NotFound();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Category([FromQuery] string products_on_page = "24", int id = 0, [FromQuery] string name = "", [FromQuery] string product_id = "") {
            
            try {
                var available_numbers = new[] { 12, 24, 48 };



                var category = await _ctx.Categories
                                        .Where(c => c.Name == name)
                                        .FirstOrDefaultAsync();

                var all_products = _ctx.Products
                                .Include(p => p.Categories)
                                .Where(p => p.Categories.Contains(category))
                                .Select(p => p)
                                .ToArray();

                StoreViewModel model;

                if (available_numbers.Contains(int.Parse(products_on_page))) {
                    model = GetProductList(0, all_products, int.Parse(products_on_page));
                }
                else {
                    model = GetProductList(0, all_products);
                }

                 
                model.Categories = await _ctx.Categories.ToListAsync();
                model.CurrentCategory = category.Name;

                return View("Index", model); ;
            }
            catch(Exception e) {
                Console.WriteLine(e.Message);
                return BadRequest();
            }
        }
    }
}



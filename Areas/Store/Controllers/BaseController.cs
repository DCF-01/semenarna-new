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

        private async Task<IEnumerable<ProductViewModel>> GetSortedProducts(string categoryId, string sortOrder = "",  int id = 1, int pageSize = 24) {

            IEnumerable<ProductViewModel> products;
            if (!string.IsNullOrEmpty(categoryId)) {
                var category = await _ctx.Categories.FindAsync(int.Parse(categoryId));

                products = _ctx.Products
                .Include(p => p.Categories)
                .Where(p => p.Categories.Contains(category))
                .Select(p => new ProductViewModel {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Description = p.Description,
                    Price = int.Parse(p.Price),
                    OnSale = p.OnSale,
                    InStock = p.InStock,
                    Categories = p.Categories,
                    Img = Convert.ToBase64String(p.Img)
                })
                .Skip(pageSize * (id - 1))
                .Take(pageSize);
            }
            else {
                products = _ctx.Products
                .Include(p => p.Categories)
                .Select(p => new ProductViewModel {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Description = p.Description,
                    Price = int.Parse(p.Price),
                    OnSale = p.OnSale,
                    InStock = p.InStock,
                    Categories = p.Categories,
                    Img = Convert.ToBase64String(p.Img)
                })
                .Skip(pageSize * (id - 1))
                .Take(pageSize);
            }
            var productsList = SortProducts(products: products, sortOrder);

            return await Task.FromResult(products);
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] int products_on_page = 24, int id = 1) {
            var categories = await _ctx.Categories.ToListAsync();

            ViewBag.Categories = categories;

            StoreViewModel model = new() {
                Categories = categories,
                BaseURL = $"{this.Request.Scheme}://{this.Request.Host}/Store/Base/Index",
                URLParameters = $"products_on_page={products_on_page}",
                PageSize = products_on_page,
                Products = await GetSortedProducts(categoryId: "", id: id, pageSize: products_on_page),
                Total = await _ctx.Products.CountAsync()
            };

            return View("Index", model);
        }
        [HttpGet]
        public async Task<IActionResult> Find([FromQuery] string name = "", [FromQuery] string product_id = "") {
            var categories = await _ctx.Categories.ToListAsync();
            ViewBag.Categories = categories;

            if (Request.Query.ContainsKey("name")) {

                var products = await _ctx.Products
                .Include(p => p.Categories)
                .Where(p => EF.Functions.ILike(p.Name, $"%{name}%"))
                .Select(p => new ProductViewModel {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Description = p.Description,
                    Price = int.Parse(p.Price),
                    OnSale = p.OnSale,
                    InStock = p.InStock,
                    Categories = p.Categories,
                    Img = Convert.ToBase64String(p.Img)
                })
                .Take(5)
                .ToListAsync();

                var model = new StoreViewModel {
                    Products = products,
                    Categories = categories,
                    CurrentCategory = "none"
                };

                return View("Index", model);
            }
            else if (Request.Query.ContainsKey("product_id")) {
                
                var products = await _ctx.Products
                .Include(p => p.Categories)
                .Where(p => EF.Functions.ILike(p.ProductId.ToString(), $"%{product_id}%"))
                .Select(p => new ProductViewModel {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Description = p.Description,
                    Price = int.Parse(p.Price),
                    OnSale = p.OnSale,
                    InStock = p.InStock,
                    Categories = p.Categories,
                    Img = Convert.ToBase64String(p.Img)
                })
                .Take(5)
                .ToListAsync();

                var model = new StoreViewModel {
                    Products = products,
                    Categories = categories,
                    CurrentCategory = "none"
                };
                return View("Index", model);
            }
            else {
                return NotFound();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Category([FromQuery] string category_id, [FromQuery] int products_on_page = 24, int id = 0, [FromQuery] int product_id = 0) {
            ViewBag.Categories = await _ctx.Categories.ToListAsync();
            try {
                var category = await _ctx.Categories
                                        .Where(c => c.CategoryId == int.Parse(category_id))
                                        .FirstOrDefaultAsync();

                StoreViewModel model = new StoreViewModel {
                    Categories = await _ctx.Categories.ToListAsync(),
                    CurrentCategory = category.Name,
                    BaseURL = $"{this.Request.Scheme}://{this.Request.Host}/Store/Base/Category",
                    URLParameters = $"category_id={category_id}&products_on_page={products_on_page}",
                    PageSize = products_on_page,
                    Products = await GetSortedProducts(categoryId: category_id, id: id, pageSize: products_on_page),
                    Total = await _ctx.Products.CountAsync()
                };

                return View("Index", model); ;
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                return BadRequest();
            }
        }

        private IEnumerable<ProductViewModel> SortProducts(IEnumerable<ProductViewModel> products, string sortOrder) {
            switch (sortOrder) {
                case "name_ascending":
                    products = products.OrderBy(p => p.Name);
                    break;
                case "name_descending":
                    products = products.OrderByDescending(p => p.Name);
                    break;
                case "price_ascending":
                    products = products.OrderBy(p => p.Price);
                    break;
                case "price_descending":
                    products = products.OrderByDescending(p => p.Price);
                    break;
                default:
                    products = products.OrderBy(p => p.Price);
                    break;

            }
            return products;

        }
    }
}



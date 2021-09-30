using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using application.Areas.Store.ViewModels;
using application.Data;
using application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using application.Enums;

namespace application.Areas.Store.Controllers {
    [Area("Store")]
    public class BaseController : Controller {
        private ApplicationDbContext _ctx;
        public BaseController(ApplicationDbContext ctx) {
            _ctx = ctx;
        }
        // returns 24 products (size of page) of page number (id) 

        private async Task<IEnumerable<ProductViewModel>> GetSortedProducts(int categoryId = 0, FilterItems sortOrder = FilterItems.NameDescending,  int page = 1, int pageSize = 24) {

            IEnumerable<ProductViewModel> products;
            if (categoryId > 0) {
                var category = await _ctx.Categories.FindAsync(categoryId);

                products = _ctx.Products
                .Include(p => p.Categories)
                .Where(p => p.Categories.Any(c => c == category))
                .Select(p => new ProductViewModel {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    OnSale = p.OnSale,
                    InStock = p.InStock,
                    Categories = p.Categories,
                    Img = Convert.ToBase64String(p.Img)
                })
                .Skip(pageSize * (page - 1))
                .Take(pageSize);
            }
            else {
                products = _ctx.Products
                .Include(p => p.Categories)
                .Select(p => new ProductViewModel {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    OnSale = p.OnSale,
                    InStock = p.InStock,
                    Categories = p.Categories,
                    Img = Convert.ToBase64String(p.Img)
                })
                .Skip(pageSize * (page - 1))
                .Take(pageSize);
            }
            var resultList = SortProducts(products: products, sortOrder);

            return await Task.FromResult(resultList);
        }

        [HttpGet]
        public async Task<IActionResult> Index(StoreViewModel storeViewModel, int id) {
            var categories = await _ctx.Categories.ToListAsync();
            var currentCategory = await _ctx.Categories.FindAsync(storeViewModel.CurrentCategoryId);

            ViewBag.Categories = categories;

            StoreViewModel model = new() {
                Categories = categories,
                CurrentCategoryId = storeViewModel.CurrentCategoryId,
                BaseURL = $"{this.Request.Scheme}://{this.Request.Host}/Store/Base/Index",
                Page = storeViewModel.Page,
                PageSize = storeViewModel.PageSize
            };

            //dont filter for category if CategoryId is less than 1 
            if(storeViewModel.CurrentCategoryId > 0) {
                model.Total = await _ctx.Products
                    .Where(p => p.Categories.Any(c => c == currentCategory))
                    .CountAsync();
            }
            else {
                model.Total = await _ctx.Products.CountAsync();
            }

            //redirect to Index if page > totalPages
            if(model.Page > model.TotalPages) {
                return RedirectToAction("Index", new StoreViewModel());
            }
            model.Products = await GetSortedProducts(categoryId: storeViewModel.CurrentCategoryId, sortOrder: storeViewModel.FilterItems, page: storeViewModel.Page, pageSize: storeViewModel.PageSize);

            return View("Index", model);
        }
        [HttpPost]
        public IActionResult Index([FromForm] StoreViewModel model) {
            return RedirectToAction("Index", model);
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
                    Price = p.Price,
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
                    CurrentCategoryId = -1
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
                    Price = p.Price,
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
                    CurrentCategoryId = -1
                };
                return View("Index", model);
            }
            else {
                return NotFound();
            }
        }

        private IEnumerable<ProductViewModel> SortProducts(IEnumerable<ProductViewModel> products, FilterItems sortOrder) {
            switch (sortOrder) {
                case FilterItems.NameAscending:
                    products = products.OrderBy(p => p.Name);
                    break;
                case FilterItems.NameDescending:
                    products = products.OrderByDescending(p => p.Name);
                    break;
                case FilterItems.PriceAscending:
                    products = products.OrderBy(p => p.OnSale ? p.SalePrice : p.Price);
                    break;
                case FilterItems.PriceDescending:
                    products = products.OrderByDescending(p => p.OnSale ? p.SalePrice : p.Price);
                    break;
                default:
                    products = products.OrderBy(p => p.OnSale ? p.SalePrice : p.Price);
                    break;

            }
            return products.ToList();

        }
    }
}



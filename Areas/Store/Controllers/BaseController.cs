using Microsoft.AspNetCore.Mvc;
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
        public StoreViewModel GetProductList(int id, Product[] all) {

            int page_number;

            if (id < 1) {
                page_number = 1;
            }
            else {
                page_number = id;
            }


            //max page number
            int number_of_pages = (all.Length + 24 - 1) / 24;

            //products that will return to view
            List<Product> product_list = new List<Product>();

            //start, end, max at
            int start = (page_number - 1) * 24;
            int end = start + 24;
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
                    Price = item.Price,
                    SalePrice = item.SalePrice,
                    OnSale = item.OnSale,
                    InStock = item.InStock,
                    Categories = item.Categories,
                    Spec = null,
                    Img = Convert.ToBase64String(item.Img)
                };
                item_list.Add(new_item);
            }

            var view_model = new StoreViewModel {
                Products = item_list,
                Page_number = page_number,
                Number_of_pages = number_of_pages
                /*Number_products_on_page = 24*/
            };

            return view_model;
        }




        /*for (int i = start; i < max; i++) {
            ProductViewModel p = new ProductViewModel {
                Id = all[i].ProductId,
                Name = all[i].Name,
                Description = all[i].Description,
                Img = Convert.ToBase64String(all[i].Img)
            };
            product_list.Add(p);
        }*/




        [HttpGet]
        public IActionResult Index(int id) {
            // all products
            var all = _ctx.Products.ToArray();

            //return 24 product of selected page(id)
            StoreViewModel result = GetProductList(id, all);

            return View(result);
        }
        [HttpGet]
        public IActionResult Find([FromQuery] string name) {

            var products = from b in _ctx.Products
                           where b.Name.StartsWith(name)
                           select b;

            var result = GetProductList(0, products.ToArray());

            return View("Index", result);

        }
    }
}


using Microsoft.AspNetCore.Mvc;
using semenarna_id2.Areas.Panel.ViewModels;
using semenarna_id2.Areas.Store.ViewModels;
using semenarna_id2.Data;
using semenarna_id2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace semenarna_id2.Areas.Store.Controllers {
    [Area("Store")]
    public class HomeController : Controller {
        private ApplicationDbContext _ctx;
        public HomeController(ApplicationDbContext ctx) {
            _ctx = ctx;
        }

        [HttpGet]
        public IActionResult Index(int id) {

            int page_number;

            if (id < 1) {
                page_number = 1;
            }
            else {
                page_number = id;
            }

            // all products
            var all = _ctx.TestProduct.ToArray();


            //max page number
            int number_of_pages = (all.Length + 24 - 1) / 24;

            //products that will return to view
            List<ProductViewModel> product_list = new List<ProductViewModel>();

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
                
                

            for (int i = start; i < max; i++) {
                ProductViewModel p = new ProductViewModel {
                    Name = all[i].Name,
                    Description = all[i].Description,
                    GetImg = Convert.ToBase64String(all[i].Img)
                };
                product_list.Add(p);
            }

            var view_model = new StoreViewModel {
                Product_list = product_list,
                Page_number = page_number,
                Number_of_pages = number_of_pages
                /*Number_products_on_page = 24*/
            };


            return View(view_model);
        }
    }
}


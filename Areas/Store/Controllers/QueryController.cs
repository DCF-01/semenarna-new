using Microsoft.AspNetCore.Mvc;
using semenarna_id2.Data;
using semenarna_id2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using semenarna_id2.Areas.Store.Controllers;

namespace semenarna_id2.Areas.Store.Controllers {
    [Area("Store")]
    public class QueryController : Controller {
        private ApplicationDbContext _ctx;
        public QueryController(ApplicationDbContext applicationDbContext) {
            _ctx = applicationDbContext;
        }

        /*private TestProductModel[] StartsWith() {
            return
        }*/

        public IActionResult Index() {
            return View();
        }


        public async Task<IActionResult> Single(int id) {
            var product = await _ctx.TestProduct.FindAsync(id);

            return Ok(product);
        }

        public IActionResult Find([FromQuery] string name = "", [FromQuery] string id = "") {

            try {
                if (Request.Query.ContainsKey("name")) {
                    var products = from b in _ctx.TestProduct
                                   where b.Name.StartsWith(name)
                                   select b;
                    var result = products.ToArray();

                    return Ok(result);
                }
                if (Request.Query.ContainsKey("id")) {
                    var products = from b in _ctx.TestProduct
                                   where b.Name.StartsWith(id)
                                   select b;
                    var result = products.ToArray();

                    return Ok(result);
                }
                else {
                    return NotFound();
                }
            }
            catch (Exception e) {
                Console.WriteLine(e);

                return NotFound();
            }


        }
    }
}

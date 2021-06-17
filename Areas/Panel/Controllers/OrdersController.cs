using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using semenarna_id2.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace semenarna_id2.Areas.Panel.Controllers {
    [Authorize(Roles = "Admin")]
    [Area("Panel")]
    public class OrdersController : Controller {
        readonly ApplicationDbContext _ctx;
        public OrdersController(ApplicationDbContext applicationDbContext) {
            _ctx = applicationDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index() {
            var result = await _ctx.Orders
                        .Include(order => order.CartProducts)
                        .ThenInclude(order => order.Product)
                        .ToListAsync();

            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Manage(int id) {
            var order = await _ctx.Orders
                                .Include(order => order.CartProducts)
                                .ThenInclude(order => order.Product)
                                .Where(order => order.OrderId == id).FirstOrDefaultAsync();

            return View(order);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromRoute] int id) {
            try {
                //delete product
                var order = _ctx.Orders.Find(id);

                var cart_products = await _ctx.Orders
                                        .Include(o => o.CartProducts)
                                        .Where(o => o.OrderId == id)
                                        .Select(o => o.CartProducts)
                                        .ToListAsync();

                

                if (order != null) {
                    _ctx.RemoveRange(cart_products.SelectMany(x => x));
                    _ctx.Remove(order);
                    _ctx.SaveChanges();
                    return Ok();
                }
                else {
                    return StatusCode(500);
                }
            }
            catch (Exception e) {
                return BadRequest(e.Message);
            }
        }
    }
}

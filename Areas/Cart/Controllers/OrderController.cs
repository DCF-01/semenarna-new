using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using semenarna_id2.Areas.Cart.ViewModels;
using semenarna_id2.Data;
using semenarna_id2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace semenarna_id2.Areas.Cart.Controllers {
    [Area("Cart")]
    public class OrderController : Controller {
        private readonly ApplicationDbContext _ctx;
        private readonly UserManager<ApplicationUser> _userManager;
        public OrderController(ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager) {
            _ctx = applicationDbContext;
            _userManager = userManager;
        }
        [HttpPost]
        public async Task<IActionResult> Process([FromBody] OrderViewModel orderViewModel) {

            ApplicationUser user = null;

            if (User.Identity.IsAuthenticated) {
                var id = _userManager.GetUserId(User);
                user = await _userManager.FindByIdAsync(id);
            }

            if (ModelState.IsValid) {
                var order = new Order {
                    FirstName = orderViewModel.FirstName,
                    LastName = orderViewModel.LastName,
                    CompanyName = orderViewModel.CompanyName,
                    Country = orderViewModel.Country,
                    Address = orderViewModel.Address,
                    ZipCode = orderViewModel.ZipCode,
                    City = orderViewModel.City,
                    Email = orderViewModel.Email,
                    Phone = orderViewModel.Phone,
                    PaymentMethod = orderViewModel.PaymentMethod,
                    DeliveryMethod = orderViewModel.DeliveryMethod,
                    DateTime = DateTime.UtcNow
                    
                };
                await _ctx.Orders.AddAsync(order);

                if(user != null) {
                    user.Orders.Add(order);
                }

                return Ok();
            }
            else {
                return BadRequest(ModelState);
            }
            
        }
    }
}

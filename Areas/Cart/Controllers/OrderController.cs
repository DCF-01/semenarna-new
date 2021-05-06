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
                await _ctx.SaveChangesAsync();
                int id = order.OrderId;

                if (user != null) {
                    user.Orders.Add(order);
                    await _ctx.SaveChangesAsync();
                }

                var success_message = $"Your order has been placed successfully. An email containing your order's details has been sent to {order.Email}.";

                return RedirectToAction("Result", new ResultViewModel { OrderId = id.ToString(), EmailSent = order.Email, Message = success_message, OrderStatus = "true" });
            }
            else {
                var error_message = $"Your order has not been been placed. Please review your contact information and try again.";
                return BadRequest();
            }

        }
        [HttpGet]
        public IActionResult Result([FromRoute] string OrderId, string EmailSent, string Message, string OrderStatus) {

            var result = new ResultViewModel {
                OrderId = OrderId,
                EmailSent = EmailSent,
                Message = Message,
                OrderStatus = OrderStatus
            };


            return View(result);
        }

    }
}

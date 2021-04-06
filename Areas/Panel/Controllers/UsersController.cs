using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using semenarna_id2.Areas.Panel.ViewModels;
using semenarna_id2.Data;
using semenarna_id2.Models;
using semenarna_id2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace semenarna_id2.Areas.Panel.Controllers {
    [Authorize(Roles = "Admin")]
    [Area("Panel")]
    public class UsersController : Controller {
        private ApplicationDbContext _ctx;
        private UserManager<IdentityUser> _userManager;

        public UsersController(UserManager<IdentityUser> userManager, ApplicationDbContext context) {
            _ctx = context;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> Index() {

            var users = await _userManager.Users.ToListAsync();

            var data = new UserViewModel() {
                Users = users
            };
            return View(data);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string Id) {
            //delete user
            var user = await _userManager.FindByIdAsync(Id);
            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded) {
                //get users list
                var users = await _userManager.Users.ToListAsync();

                var data = new UserViewModel() {
                    Users = users
                };

                return Ok();
            }
            return StatusCode(500);
        }
    }
}

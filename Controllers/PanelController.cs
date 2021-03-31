using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using semenarna_id2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace semenarna_id2.Controllers {
    [Authorize(Roles = "Admin")]
    public class PanelController : Controller {
        private UserManager<IdentityUser> _userManager;

        public PanelController(UserManager<IdentityUser> userManager) {
            _userManager= userManager;
        }
        [HttpGet]
        public IActionResult Index() {
            return View();
        }
        [HttpGet]
        public IActionResult Test() {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ManageUsers() {


            var users = await _userManager.Users.ToListAsync();

            var data = new UserViewModel() {
                Users = users
            };

            return View(data);

        }
        [HttpDelete]
        public async Task<IActionResult> ManageUsers(string Id) {
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

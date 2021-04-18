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
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context) {
            _ctx = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        [HttpGet]
        public async Task<IActionResult> Index() {

            var users = await _userManager.Users
                                .Select(user =>
                                new UserViewModel {
                                    UserId = user.Id,
                                    UserName = user.UserName,
                                    Email = user.Email,
                                    EmailConfirmed = user.EmailConfirmed,
                                    CurrentRole = _userManager.GetRolesAsync(user).GetAwaiter().GetResult().FirstOrDefault()
                                }
                                ).ToListAsync();

            return View(users);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id) {
            //delete user
            var user = await _userManager.FindByIdAsync(id);
            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded) {
                return Ok();
            }

            return StatusCode(500);
        }
        [HttpGet]
        public async Task<IActionResult> Manage(string id) {
            //helper var to get roles in usermanager
            var helper_user = await _userManager.FindByIdAsync(id);

            var user = await _ctx.Users
                            .Where(u => u.Id == id)
                            .Select(u =>
                            new UserViewModel {
                                UserName = u.UserName,
                                Email = u.Email,
                                EmailConfirmed = u.EmailConfirmed,
                            })
                            .FirstOrDefaultAsync();

            var role_list = await _roleManager.Roles.Select(role => role.Name).ToListAsync();
            var current_role = await _userManager.GetRolesAsync(helper_user);

            //only 1 role per user 
            user.CurrentRole = current_role.First();

            user.GetRoles = role_list;

            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> Manage(string id, UserViewModel userViewModel) {

            try {
                var user = await _ctx.Users.FindAsync(id);

                if (user != null) {
                    var new_role = userViewModel.RoleSelected;
                    var current_role = await _userManager.GetRolesAsync(user);
                    await _userManager.RemoveFromRolesAsync(user, current_role);
                    await _userManager.AddToRoleAsync(user, new_role);


                    return RedirectToAction("Index");
                }
                else {
                    throw new Exception($"User with id: {id} not found.");
                }
            }
            catch (Exception e) {
                return BadRequest(e.Message);
            }
        }
    }

}

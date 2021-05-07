using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using semenarna_id2.Data;
using semenarna_id2.Models;
using semenarna_id2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace semenarna_id2.Controllers {
    public class AuthController : Controller {

        private SignInManager<ApplicationUser> _signInManager;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private ApplicationDbContext _ctx;
        public AuthController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext applicationDbContext) {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _ctx = applicationDbContext;
        }

        [HttpGet]
        public IActionResult Register() {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel) {
            if (ModelState.IsValid) {
                /* var userRole = new IdentityRole("User");*//**/


                var usr = new ApplicationUser {
                    Email = registerViewModel.Email,
                    UserName = registerViewModel.Email,
                    Cart = new Cart(),
                    Orders = new List<Order>()
                };

                var res = await _userManager.CreateAsync(usr, registerViewModel.Password);
                await _userManager.AddToRoleAsync(usr, "User");

                if (res.Succeeded) {
                  
                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("Index", "Auth");
        }

        [HttpGet]
        public IActionResult Login() {
            


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel) {
            var result = await _signInManager.PasswordSignInAsync(loginViewModel.Username, loginViewModel.Password, false, false);

            var user = _userManager.Users.Where(usr => usr.UserName == loginViewModel.Username)
                .Select(usr => usr);

            /*var f = user.ToArray();*/

           /* var cart = new UserCartModel {
                UserId = f.Id,
                User = (ApplicationUserModel) f,
                TestProducts = new List<TestProductModel> { }
            };*/

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout() {

            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied() {
            return View();
        }

        [HttpGet]
        public IActionResult CheckLogin() {
            if (User.Identity.IsAuthenticated) {
                var result = true;

                return Ok(result);
            }
            else {
                var result = false;
                return Ok(result);
            }
        }
    }
}

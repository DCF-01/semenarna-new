using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using semenarna_id2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace semenarna_id2.Controllers {
    public class AuthController : Controller {

        private SignInManager<IdentityUser> _signInManager;
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        public AuthController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager) {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Register() {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel) {
            if (ModelState.IsValid) {
               /* var userRole = new IdentityRole("User");*//**/
                var usr = new IdentityUser {
                    Email = registerViewModel.Email,
                    UserName = registerViewModel.Email
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
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel) {
            var result = await _signInManager.PasswordSignInAsync(loginViewModel.Username, loginViewModel.Password, false, false);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout() {

            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied() {
            return View();
        }
    }
}

using Google.Rpc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RazorLight;
using semenarna_id2.Data;
using semenarna_id2.Models;
using semenarna_id2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

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

        [HttpGet]
        public IActionResult ResetPassword(string user_id, string token) {

            var model = new ResetPasswordViewModel {
                UserId = user_id,
                ResetToken = token
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel) {


            ResetPasswordViewModel resetPasswordView = new ResetPasswordViewModel();

            var user = await _userManager.FindByIdAsync(resetPasswordViewModel.UserId);


            var result = await _userManager.ResetPasswordAsync(user, resetPasswordViewModel.ResetToken, resetPasswordViewModel.NewPassword);

            if (result.Succeeded) {
                var message_model = new ResetPasswordViewModel {
                    Message = "Your password has been reset successfully"
                };
                return View(message_model);
            }
            else {
                var message_model = new ResetPasswordViewModel {
                    Message = "There was an error, your password has not been reset."
                };

                return View(message_model);
            }
        }


        //request password reset
        [HttpPost]
        public async Task<IActionResult> RequestReset(RequestResetViewModel requestResetViewModel) {

            var user = await _userManager.FindByEmailAsync(requestResetViewModel.Email);
            string token = "";
            if (user != null) {
                token = await _userManager.GeneratePasswordResetTokenAsync(user);

                /* reset_url += "https://localhost:44380/Auth/ResetPassword" + $"?reset_token={reset_token}&user_id={user.Id}";*/
                var callbackUrl = Url.Action("ResetPassword", "Auth",
                new { user_id = user.Id, token = token }, protocol: Request.Scheme);

                var message_model_success = new RequestResetViewModel {
                    Message = "We've sent a reset link to your email address."
                };

                var engine = new RazorLightEngineBuilder()
                // required to have a default RazorLightProject type,
                // but not required to create a template from string.
                .UseEmbeddedResourcesProject(typeof(RequestResetViewModel))
                .SetOperatingAssembly(typeof(RequestResetViewModel).Assembly)
                .UseMemoryCachingProvider()
                .Build();

                var cacheResult = engine.Handler.Cache.RetrieveTemplate("resetPasswordTemplateKey");
                if (cacheResult.Success) {
                    var email_reset_model = new RequestResetViewModel {
                        ResetUrl = callbackUrl
                    };

                    var templatePage = cacheResult.Template.TemplatePageFactory();
                    string processedHtml = await engine.RenderTemplateAsync(templatePage, email_reset_model);

                    var mailer = new Mailer("admin@paralax.mk", user.Email, processedHtml);
                    mailer.Send();
                }
                else {
                    var email_reset_model = new RequestResetViewModel {
                        ResetUrl = callbackUrl
                    };

                    string htmlString = System.IO.File.ReadAllText("/home/ubuntu/projects/semenarna/Utils/Views/ResetPasswordEmail.cshtml");
                    string processedHtml = await engine.CompileRenderStringAsync("resetPasswordTemplateKey", htmlString, email_reset_model);

                    //create and send email
                    var mailer = new Mailer("admin@paralax.mk", user.Email, processedHtml);
                    mailer.Send();
                }

                var message_model_ok = new RequestResetViewModel {
                    Message = "We've sent a reset link to your email address."
                };

                return View(message_model_ok);
            }
            else {
                var message_model_ok = new RequestResetViewModel {
                    Message = "We've sent a reset link to your email address."
                };
                return View(message_model_ok);
            }
        }

        [HttpGet]
        public IActionResult RequestReset() {

            var message_model = new RequestResetViewModel() {
                Message = "If an account is associated with your email, you will receive a password reset link"
            };

            return View(new RequestResetViewModel());
        }

    }
}

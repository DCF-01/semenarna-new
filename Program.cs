using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using application.Data;
using application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RazorLight;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.Extensions.Primitives;

namespace application {
    public class Program {
        public static void Main(string[] args) {
            var host = CreateHostBuilder(args).Build();

            try {

                var scope = host.Services.CreateScope();

                var ctx = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                ctx.Database.EnsureCreated();

                var adminRole = new IdentityRole("Admin");
                var userRole = new IdentityRole("User");

                if (!ctx.Roles.Any()) {
                    //create role
                    roleMgr.CreateAsync(userRole).GetAwaiter().GetResult();
                    roleMgr.CreateAsync(adminRole).GetAwaiter().GetResult();
                }
                if (!ctx.Users.Any(e => e.UserName == "admin")) {

                    //create admin
                    var adminUser = new ApplicationUser {
                        UserName = "admin",
                        Email = "admin@paralax.mk",
                        Cart = new Cart(),
                        Orders = new List<Order>()
                    };

                    var result = userMgr.CreateAsync(adminUser, "radant098").GetAwaiter().GetResult();
                    var res = userMgr.AddToRoleAsync(adminUser, adminRole.Name).GetAwaiter().GetResult();

                    ctx.SaveChangesAsync().GetAwaiter().GetResult();

                }

                if (!ctx.Specs.Any()) {
                    var active_columns = new[] { " " };

                    var spec = new Spec {
                        First = active_columns,
                        Rest = new[] { " " },
                        ItemsPerRow = active_columns.Length,
                        Name = "Empty"
                    };
                    ctx.Specs.AddAsync(spec).GetAwaiter().GetResult();
                    ctx.SaveChangesAsync().GetAwaiter().GetResult();
                }
                if (!ctx.Categories.Any()) {
                    var uncategorized = new Category {
                        Name = "uncategorized"
                    };
                    ctx.Categories.AddAsync(uncategorized).GetAwaiter().GetResult();
                    ctx.SaveChangesAsync().GetAwaiter().GetResult();
                }
                if (!ctx.Variations.Any()) {
                    var none = new Variation {
                        Name = "none"
                    };
                    none.Options = new []{ "optionOne" };
                    ctx.Variations.AddAsync(none).GetAwaiter().GetResult();
                    ctx.SaveChangesAsync().GetAwaiter().GetResult();
                };
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

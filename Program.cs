using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using semenarna_id2.Data;
using semenarna_id2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace semenarna_id2 {
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
                /*roleMgr.CreateAsync(userRole).GetAwaiter().GetResult();*/
                roleMgr.CreateAsync(adminRole).GetAwaiter().GetResult();

                /*if (!ctx.Roles.Any()) {
                    //create role
                    roleMgr.CreateAsync(adminRole).GetAwaiter().GetResult();
                }*/
                if (!ctx.Users.Any(e => e.UserName == "admin"))
                {

                    //create admin
                    var adminUser = new ApplicationUser {
                        UserName = "admin",
                        Email = "admin@paralax.mk",
                    };

                    /*var result = userMgr.CreateAsync(adminUser, "radant098").GetAwaiter().GetResult();*/
                    var res = userMgr.AddToRoleAsync(adminUser, adminRole.Name).GetAwaiter().GetResult();

                    ctx.SaveChangesAsync().GetAwaiter().GetResult();

                }
                /*if (!ctx.Products.Any()) {
                    var seedList = new List<Product>();

                    for(int i = 0; i < 6; i++) {
                        var product = new Product {
                            Name = "Test Product " + i,
                            Description = "Fake description " + i
                        };
                        seedList.Add(product);



                    }
                    seedList.ForEach(n => ctx.Products.Add(n));
                    ctx.SaveChangesAsync();

                }*/
            }
            catch(Exception e) {
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

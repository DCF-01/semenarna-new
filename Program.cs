using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using semenarna_id2.Data;
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
                var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                ctx.Database.EnsureCreated();

                var adminRole = new IdentityRole("Admin");
                var userRole = new IdentityRole("User");

                if (!ctx.Roles.Any()) {
                    //create role
                    roleMgr.CreateAsync(adminRole).GetAwaiter().GetResult();
                }
                /*if(ctx.Users.Any(u => u.UserName == "admin")) {*/
                /*var usr = userMgr.FindByNameAsync("admin").GetAwaiter().GetResult();
                var res = userMgr.DeleteAsync(usr).GetAwaiter().GetResult();*/
                    
                    //create admin
                    /*var adminUser = new IdentityUser {
                        UserName = "admin",
                        Email = "admin@paralax.mk"
                    };
                    var result = userMgr.CreateAsync(adminUser, "radant098").GetAwaiter().GetResult();
                    userMgr.AddToRoleAsync(adminUser, adminRole.Name).GetAwaiter().GetResult();*/

                /*}*/
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

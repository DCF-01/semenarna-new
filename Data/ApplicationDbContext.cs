using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using application.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace application.Data {
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) {
        }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<CartProduct> CartProducts { get; set; }
        public DbSet<Spec> Specs { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Variation> Variations { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
    }
    
}

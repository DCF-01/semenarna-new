using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using semenarna_id2.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace semenarna_id2.Data {
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
    }
    
}

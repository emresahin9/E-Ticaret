using E_Ticaret.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace E_Ticaret.Context
{
    public class ETicaretContext : DbContext
    {
        public ETicaretContext() : base("name=ETicaretDB")
        { }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Cart> Carts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Configure Student & StudentAddress entity
            modelBuilder.Entity<User>()
                        .HasOptional(s => s.cart) // Mark Address property optional in Student entity
                        .WithRequired(ad => ad.cartUser); // mark Student property as required in StudentAddress entity. Cannot save StudentAddress without Student

            modelBuilder.Entity<Cart>()
                        .HasMany<Product>(s => s.Products)
                        .WithMany(c => c.Carts)
                        .Map(cs =>
                        {
                            cs.MapLeftKey("CartRefId");
                            cs.MapRightKey("ProductRefId");
                            cs.ToTable("CartProduct");
                        });
        }
    }
}
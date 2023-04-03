﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SleekClothing.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<SleekClothing.Models.Product> Products { get; set; }
        public DbSet<SleekClothing.Models.Category> Categories { get; set; }
        public DbSet<SleekClothing.Models.UserCart> UserCarts { get; set; }
        public DbSet<Models.UserWishlist> UserWishlists { get; set; }
    }
}
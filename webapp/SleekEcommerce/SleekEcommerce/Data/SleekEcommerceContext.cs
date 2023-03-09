using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SleekEcommerce.Models;

namespace SleekEcommerce.Data
{
    public class SleekEcommerceContext : DbContext
    {
        public SleekEcommerceContext (DbContextOptions<SleekEcommerceContext> options)
            : base(options)
        {
        }
        public DbSet<SleekEcommerce.Models.Product> Products { get; set; }
        public DbSet<SleekEcommerce.Models.Category> Categories { get; set; }

    }
}

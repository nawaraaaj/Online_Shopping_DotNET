using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineShopping_BIT_2025.Models;

namespace OnlineShopping_BIT_2025.Data
{
    public class OnlineShopping_BIT_2025Context : DbContext
    {
        public OnlineShopping_BIT_2025Context (DbContextOptions<OnlineShopping_BIT_2025Context> options)
            : base(options)
        {
        }

        public DbSet<OnlineShopping_BIT_2025.Models.Category> Category { get; set; } = default!;
        public DbSet<OnlineShopping_BIT_2025.Models.Product> Product { get; set; } = default!;
        public DbSet<OnlineShopping_BIT_2025.Models.User> User { get; set; } = default!;
    }
}

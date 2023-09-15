using MaxiShop.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxiShop.InfraStructure.DbContexts
{
    public class MaxiShopDbContext : DbContext
    {
        public MaxiShopDbContext(DbContextOptions<MaxiShopDbContext> options) : base(options)
        {
        }
        public DbSet<Category> Category { get; set; }
    }
}

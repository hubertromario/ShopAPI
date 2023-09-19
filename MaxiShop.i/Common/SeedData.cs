using MaxiShop.Domain.Models;
using MaxiShop.InfraStructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxiShop.InfraStructure.Common
{
    public class SeedData
    {
        public static async Task SeedDataAsync(MaxiShopDbContext _dbContext)
        {
            if (!_dbContext.Brand.Any()) {
                await _dbContext.AddRangeAsync(new Brand
                {
                    Name = "Apple",
                    EstablishedYear = 1956
                },
                new Brand
                {
                    Name = "Samsung",
                    EstablishedYear = 1966
                },
                new Brand
                {
                    Name = "Sony",
                    EstablishedYear = 1977
                },
                new Brand
                {
                    Name = "HP",
                    EstablishedYear = 1983
                },
                new Brand
                {
                    Name = "Lenovo",
                    EstablishedYear = 1956
                },
                new Brand
                {
                    Name = "Acer",
                    EstablishedYear = 1987
                });
                await _dbContext.SaveChangesAsync();  
            }
        }
    }


}

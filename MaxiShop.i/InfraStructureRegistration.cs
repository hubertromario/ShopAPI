using MaxiShop.Domain.Contracts;
using MaxiShop.InfraStructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MaxiShop.InfraStructure
{
    public static class InfraStructureRegistration
    {
        public static IServiceCollection AddInfraStructureServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<IProductRespository, ProductRepository>();
            return services;
        }
    }
}

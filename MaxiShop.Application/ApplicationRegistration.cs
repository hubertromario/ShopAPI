﻿using MaxiShop.Application.Common;
using MaxiShop.Application.Services;
using MaxiShop.Application.Services.Interfaces;
using MaxiShop.Domain.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxiShop.Application
{
    public static class ApplicationRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddScoped(typeof(IPaginationService<,>),typeof(PaginationService<,>));

            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IBrandService, BrandService>();
            services.AddScoped<IProductService, ProductService>();
   
            return services;
        }
    }
}

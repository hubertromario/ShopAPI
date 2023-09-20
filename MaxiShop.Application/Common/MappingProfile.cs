using AutoMapper;
using MaxiShop.Application.DTO.Brand;
using MaxiShop.Application.DTO.Category;
using MaxiShop.Application.DTO.Product;
using MaxiShop.Application.Services;
using MaxiShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxiShop.Application.Common
{
    public class MappingProfile:Profile
    {
        public MappingProfile() { 
            CreateMap<Category,CreateCategoryDTO>().ReverseMap();
            CreateMap<Category,CategoryDTO>().ReverseMap();
            CreateMap<Category,UpdateCategoryDTO>().ReverseMap();

            CreateMap<Brand, CreateBrandDTO>().ReverseMap();
            CreateMap<Brand, BrandDTO>().ReverseMap();
            CreateMap<Brand, UpdateBrandDTO>().ReverseMap();

            CreateMap<Product, CreateProductDTO>().ReverseMap();
            CreateMap<Product, ProductDTO>().ForMember(x => x.Category, opt => opt.MapFrom(source => source.Category.Name)).
                ForMember(x => x.Brand, opt => opt.MapFrom(source => source.Brand.Name));
            CreateMap<Product, UpdateProductDTO>().ReverseMap();

        } 
    }
}

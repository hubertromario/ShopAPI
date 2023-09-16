using AutoMapper;
using MaxiShop.Application.DTO.Category;
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

        } 
    }
}

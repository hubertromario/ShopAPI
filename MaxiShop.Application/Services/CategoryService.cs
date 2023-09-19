using AutoMapper;
using MaxiShop.Application.DTO.Category;
using MaxiShop.Application.Services.Interfaces;
using MaxiShop.Domain.Contracts;
using MaxiShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxiShop.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository,IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
      
        public async Task<CategoryDTO> CreateAsync(CreateCategoryDTO CreateCategoryDto)
        {
            var category = _mapper.Map<Category>(CreateCategoryDto);
            var createdentity = await _categoryRepository.CreateAsync(category);
            var entity = _mapper.Map<CategoryDTO>(createdentity);  // converting category into category dto

            return entity;
        }

        public async Task DeleteAsync(int id)
        {
           var category = await _categoryRepository.GetByIdAsync(x=>x.Id == id);
            await _categoryRepository.DeleteAsync(category);
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllAsync()
        {
           var categories =await _categoryRepository.GetAllAsync();
            return _mapper.Map<List<CategoryDTO>>(categories);
        }

        public async Task<CategoryDTO> GetByIdAsync(int id)
        {
           var category = await _categoryRepository.GetByIdAsync(x=>x.Id==id);
            return _mapper.Map<CategoryDTO>(category);
        }

        public async Task UpdateAsync(UpdateCategoryDTO UpdateCategoryDto)
        {
            var category = _mapper.Map<Category>(UpdateCategoryDto);
            await _categoryRepository.UpdateAsync(category);
        }
    }
    }


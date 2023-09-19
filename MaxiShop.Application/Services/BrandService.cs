using AutoMapper;
using MaxiShop.Application.DTO.Brand;
using MaxiShop.Application.DTO.Brand;
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
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _BrandRepository;
        private readonly IMapper _mapper;

        public BrandService(IBrandRepository BrandRepository, IMapper mapper)
        {
            _BrandRepository = BrandRepository;
            _mapper = mapper;
        }

        public async Task<BrandDTO> CreateAsync(CreateBrandDTO CreateBrandDto)
        {
            var Brand = _mapper.Map<Brand>(CreateBrandDto);
            var createdentity = await _BrandRepository.CreateAsync(Brand);
            var entity = _mapper.Map<BrandDTO>(createdentity);  // converting Brand into Brand dto

            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var Brand = await _BrandRepository.GetByIdAsync(x => x.Id == id);
            await _BrandRepository.DeleteAsync(Brand);
        }

        public async Task<IEnumerable<BrandDTO>> GetAllAsync()
        {
            var categories = await _BrandRepository.GetAllAsync();
            return _mapper.Map<List<BrandDTO>>(categories);
        }

        public async Task<BrandDTO> GetByIdAsync(int id)
        {
            var Brand = await _BrandRepository.GetByIdAsync(x => x.Id == id);
            return _mapper.Map<BrandDTO>(Brand);
        }

        public async Task UpdateAsync(UpdateBrandDTO updateBrandDto)
        {
            var brand = _mapper.Map<Brand>(updateBrandDto);
            await _BrandRepository.UpdateAsync(brand);
        }
    }
}

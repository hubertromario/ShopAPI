using AutoMapper;
using MaxiShop.Application.DTO.Product;
using MaxiShop.Application.DTO.Product;
using MaxiShop.Application.Input_Models;
using MaxiShop.Application.Services.Interfaces;
using MaxiShop.Application.ViewModels;
using MaxiShop.Domain.Contracts;
using MaxiShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxiShop.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRespository _ProductRepository;
        private readonly IPaginationService<ProductDTO,Product> _paginationService;
        private readonly IMapper _mapper;

        public ProductService(IProductRespository ProductRepository, IMapper mapper,IPaginationService<ProductDTO,Product> paginationService)
        {
            _ProductRepository = ProductRepository;
            _mapper = mapper;
            _paginationService = paginationService;
        }

        public async Task<ProductDTO> CreateAsync(CreateProductDTO CreateProductDto)
        {
            var Product = _mapper.Map<Product>(CreateProductDto);
            var createdentity = await _ProductRepository.CreateAsync(Product);
            var entity = _mapper.Map<ProductDTO>(createdentity);  // converting Product into Product dto

            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var Product = await _ProductRepository.GetByIdAsync(x => x.Id == id);
            await _ProductRepository.DeleteAsync(Product);
        }

        public async Task<IEnumerable<ProductDTO>> GetAllAsync()
        {
            var categories = await _ProductRepository.GetAllProductAsync();
            return _mapper.Map<List<ProductDTO>>(categories);
        }

        public async Task<IEnumerable<ProductDTO>> GetAllByFilterAsync(int? categoryId, int? brandId)
        {
            var data = await _ProductRepository.GetAllProductAsync();

            IEnumerable<Product> query = data;
            if (categoryId > 0)
            {
                query = query.Where(x=>x.CategoryId == categoryId);
            }

            if (brandId > 0)
            {
                query = query.Where(x => x.BrandId == brandId);
            }

            var result = _mapper.Map<List<ProductDTO>>(query);

            return result;
        }

        public async Task<ProductDTO> GetByIdAsync(int id)
        {
            var Product = await _ProductRepository.GetProductByIdAsync(id);
            return _mapper.Map<ProductDTO>(Product);
        }

        public async Task<PaginationVM<ProductDTO>> GetPagination(PaginationInputModel pagination)
        {
            var source = await _ProductRepository.GetAllProductAsync();
            var result = _paginationService.GetPagination(source, pagination);  

            return result;
        }

        public async Task UpdateAsync(UpdateProductDTO updateProductDto)
        {
            var Product = _mapper.Map<Product>(updateProductDto);
            await _ProductRepository.UpdateAsync(Product);
        }
    }
}


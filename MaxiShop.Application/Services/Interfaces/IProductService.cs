using MaxiShop.Application.DTO.Category;
using MaxiShop.Application.DTO.Product;
using MaxiShop.Application.Input_Models;
using MaxiShop.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxiShop.Application.Services.Interfaces
{
    public interface IProductService
    {
        Task<PaginationVM<ProductDTO>> GetPagination(PaginationInputModel pagination);
        Task<ProductDTO> GetByIdAsync(int id);

        Task<IEnumerable<ProductDTO>> GetAllAsync();

        Task<IEnumerable<ProductDTO>>GetAllByFilterAsync(int? categoryId,int? brandId);

        Task<ProductDTO> CreateAsync(CreateProductDTO CreateProductDto);

        Task UpdateAsync(UpdateProductDTO UpdateProductDto);

        Task DeleteAsync(int id);
    }
}

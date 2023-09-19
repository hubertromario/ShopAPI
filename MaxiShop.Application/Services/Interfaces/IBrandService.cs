using MaxiShop.Application.DTO.Brand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxiShop.Application.Services.Interfaces
{
    public interface IBrandService
    {
        Task<BrandDTO> GetByIdAsync(int id);

        Task<IEnumerable<BrandDTO>> GetAllAsync();

        Task<BrandDTO> CreateAsync(CreateBrandDTO createBrandDto);

        Task UpdateAsync(UpdateBrandDTO updateBrandDto);

        Task DeleteAsync(int id);
    }
}

using MaxiShop.Application.DTO.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxiShop.Application.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryDTO> GetByIdAsync(int id);

        Task<IEnumerable<CategoryDTO>> GetAllAsync();

        Task<CategoryDTO> CreateAsync(CreateCategoryDTO CreateCategoryDto);

        Task UpdateAsync(UpdateCategoryDTO UpdateCategoryDto);

        Task DeleteAsync(int id);
    }
}

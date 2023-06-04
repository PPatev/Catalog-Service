using Catalog_Service.Models.Crud;
using Catalog_Service.Models.Dtos;

namespace Catalog_Service.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();

        Task<CategoryDetailsDto?> GetCategoryAsync(int id);

        Task<CategoryDetailsDto> CreateCategoryAsync(CreateCategoryModel createCategoryModel);

        Task<UpdateCategoryDto> UpdateCategoryAsync(UpdateCategoryModel updateCategoryModel);

        Task<DeleteCategoryDto> DeleteCategoryAsync(int categoryId);
    }
}

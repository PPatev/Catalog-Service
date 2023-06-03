using Catalog_Service.Models.Crud;
using Catalog_Service.Models.Dtos;

namespace Catalog_Service.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllCategories();

        Task<CategoryDetailsDto?> GetCategory(int id);

        Task<CategoryDetailsDto> CreateCategory(CreateCategoryModel createCategoryModel);

        Task<UpdateCategoryDto> UpdateCategory(UpdateCategoryModel updateCategoryModel);

        Task<DeleteCategoryDto> DeleteCategory(int categoryId);
    }
}

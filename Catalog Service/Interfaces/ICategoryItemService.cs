using Catalog_Service.Models.Crud;
using Catalog_Service.Models.Dtos;

namespace Catalog_Service.Interfaces
{
    public interface ICategoryItemService
    {
        Task<IEnumerable<CategoryItemDetailsDto?>?> GetCategoryItemsAsync(int categoryId, int pageIndex, int pageSize);

        Task<CategoryItemDetailsDto> GetCategoryItemAsync(int categoryId, int itemId);

        Task<CategoryItemDetailsDto?> CreateCategoryItemAsync(CreateItemModel createItemModel);

        Task<UpdateCategoryItemDto> UpdateCategoryItemAsync(UpdateItemModel updateItemModel);

        Task<DeleteCategoryItemDto> DeleteCategoryItemAsync(int categoryId, int itemId);
    }
}

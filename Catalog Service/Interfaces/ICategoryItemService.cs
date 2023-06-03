using Catalog_Service.Models.Crud;
using Catalog_Service.Models.Dtos;

namespace Catalog_Service.Interfaces
{
    public interface ICategoryItemService
    {
        Task<IEnumerable<CategoryItemDetailsDto?>?> GetCategoryItems(int categoryId, int pageIndex, int pageSize);

        Task<CategoryItemDetailsDto?> GetCategoryItem(int categoryId, int itemId);

        Task<CategoryItemDetailsDto?> CreateCategoryItem(CreateItemModel createItemModel);

        Task<UpdateCategoryItemDto> UpdateCategoryItem(UpdateItemModel updateItemModel);

        Task<DeleteCategoryItemDto> DeleteCategoryItem(int categoryId, int itemId);
    }
}

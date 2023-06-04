using Catalog_Service.Data.Entities;
using Catalog_Service.Interfaces;
using Catalog_Service.Models.Crud;
using Catalog_Service.Models.Dtos;

namespace Catalog_Service.Services
{
    public class CategoryItemService : ICategoryItemService
    {
        private readonly ICategoriesRepository _categoriesRepository;

        public CategoryItemService(ICategoriesRepository categoriesRepository)
        {
            _categoriesRepository = categoriesRepository;
        }

        public async Task<IEnumerable<CategoryItemDetailsDto?>?> GetCategoryItemsAsync(int categoryId, int pageIndex, int pageSize)
        {
            var category = await _categoriesRepository.GetByIdAsync(categoryId);
            if (category == null)
            {
                return null;
            }

            var categoryItems = category.CategoryItems.Skip(pageSize * pageIndex).Take(pageSize).Select(x => new CategoryItemDetailsDto
            {
                ItemId = x.Id,
                ItemName = x.Name,
                CategoryId = x.CategoryId,
                CategoryName = category.Name
            }).ToList();

            return categoryItems;
        }

        public async Task<CategoryItemDetailsDto?> GetCategoryItemAsync(int categoryId, int itemId)
        {
            var categoryItemDto = new CategoryItemDetailsDto();

            var category = await _categoriesRepository.GetByIdAsync(categoryId);

            if (category == null)
            {
                return categoryItemDto;
            }

            categoryItemDto.CategoryId = category.Id;
            var categoryItem = category.CategoryItems.SingleOrDefault(x => x.Id == itemId);

            if (categoryItem == null)
            {
                return categoryItemDto;
            }

            categoryItemDto.ItemId = categoryItem.Id;
            categoryItemDto.ItemName = categoryItem.Name;
            categoryItemDto.CategoryName = categoryItem.Category.Name;

            return categoryItemDto;
        }

        public async Task<CategoryItemDetailsDto?> CreateCategoryItemAsync(CreateItemModel createItemModel)
        {
            var category = await _categoriesRepository.GetByIdAsync(createItemModel.CategoryId);
                
            if (category == null)
            {
                return null;
            }

            var categoryItem = new CategoryItem { Name = createItemModel.ItemName };
            category.CategoryItems.Add(categoryItem);

            await _categoriesRepository.SaveAsync();

            var categoryItemDto = new CategoryItemDetailsDto
            {
                ItemId = categoryItem.Id,
                ItemName = categoryItem.Name,
                CategoryId = categoryItem.CategoryId,
                CategoryName = category.Name
            };

            return categoryItemDto;
        }

        public async Task<UpdateCategoryItemDto> UpdateCategoryItemAsync(UpdateItemModel updateItemModel)
        {
            var updateItemDto = new UpdateCategoryItemDto();

            var category = await _categoriesRepository.GetByIdAsync(updateItemModel.CategoryId);

            if (category == null)
            {
                return updateItemDto;
            }

            updateItemDto.CategoryId = category.Id;
            var categoryItem = category.CategoryItems.SingleOrDefault(x => x.Id == updateItemModel.ItemId);

            if (categoryItem == null)
            {
                return updateItemDto;
            }

            categoryItem.Name = updateItemModel.ItemName;
            await _categoriesRepository.SaveAsync();

            updateItemDto.ItemId = categoryItem.Id;
            updateItemDto.ItemName = categoryItem.Name;
            updateItemDto.CategoryName = category.Name;

            return updateItemDto;
        }

        public async Task<DeleteCategoryItemDto> DeleteCategoryItemAsync(int categoryId, int itemId)
        {
            var deleteItemDto = new DeleteCategoryItemDto();

            var category = await _categoriesRepository.GetByIdAsync(categoryId);

            if (category != null)
            {
                deleteItemDto.CategoryId = category.Id;
            }

            var categoryItem = category?.CategoryItems.SingleOrDefault(x => x.Id == itemId);
            if (categoryItem != null)
            {
                deleteItemDto.ItemId = categoryItem.Id;
            }
            else
            {
                return deleteItemDto;
            }

            category?.CategoryItems.Remove(categoryItem);
            await _categoriesRepository.SaveAsync();

            return deleteItemDto;
        }
    }
}

using Catalog_Service.Data;
using Catalog_Service.Data.Entities;
using Catalog_Service.Interfaces;
using Catalog_Service.Models.Crud;
using Catalog_Service.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Catalog_Service.Services
{
    public class CategoryItemService : ICategoryItemService
    {
        private readonly CatalogContext _context;

        public CategoryItemService(CatalogContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoryItemDetailsDto?>?> GetCategoryItems(int categoryId, int pageIndex, int pageSize)
        {
            var categoryData = await _context.Categories.AsNoTracking().Where(x => x.Id == categoryId).Select(x => new
            {
                Items = x.CategoryItems.Select(y => new CategoryItemDetailsDto
                {
                    ItemId = y.Id,
                    ItemName = y.Name,
                    CategoryId = y.CategoryId,
                    CategoryName = x.Name
                }).Skip(pageSize * pageIndex).Take(pageSize).ToList()
            }).SingleOrDefaultAsync();

            if (categoryData == null)
            {
                return null;
            }

            var categoryItems = categoryData.Items;

            return categoryItems;
        }

        public async Task<CategoryItemDetailsDto?> GetCategoryItem(int categoryId, int itemId)
        {
            var categoryItemData = await _context.CategoryItems.AsNoTracking().Where(x => x.Id == itemId).Select(x => new CategoryItemDetailsDto
            {
                ItemId = x.Id,
                ItemName = x.Name,
                CategoryId = x.CategoryId,
                CategoryName = x.Category.Name
            }).SingleOrDefaultAsync();

            if (categoryItemData == null || categoryItemData.CategoryId != categoryId)
            {
                return null;
            }

            return categoryItemData;
        }

        public async Task<CategoryItemDetailsDto?> CreateCategoryItem(CreateItemModel createItemModel)
        {
            var category = await _context.Categories.Include(x => x.CategoryItems).SingleOrDefaultAsync(x => x.Id == createItemModel.CategoryId);
            if (category == null)
            {
                return null;
            }

            var categoryItem = new CategoryItem { Name = createItemModel.ItemName };
            category.CategoryItems.Add(categoryItem);

            await _context.SaveChangesAsync();

            var categoryItemDto = new CategoryItemDetailsDto
            {
                ItemId = categoryItem.Id,
                ItemName = categoryItem.Name,
                CategoryId = categoryItem.CategoryId,
                CategoryName = category.Name
            };

            return categoryItemDto;
        }

        public async Task<UpdateCategoryItemDto> UpdateCategoryItem(UpdateItemModel updateItemModel)
        {
            var updateItemDto = new UpdateCategoryItemDto();

            var category = await _context.Categories.Include(x => x.CategoryItems).SingleOrDefaultAsync(x => x.Id == updateItemModel.CategoryId);

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
            await _context.SaveChangesAsync();

            updateItemDto.ItemId = categoryItem.Id;
            updateItemDto.ItemName = categoryItem.Name;
            updateItemDto.CategoryName = category.Name;

            return updateItemDto;
        }

        public async Task<DeleteCategoryItemDto> DeleteCategoryItem(int categoryId, int itemId)
        {
            var deleteItemDto = new DeleteCategoryItemDto();

            var category = await _context.Categories.Include(x => x.CategoryItems).SingleOrDefaultAsync(x => x.Id == categoryId);

            if (category != null)
            {
                deleteItemDto.CategoryId = category.Id;
            }

            var categoryItem = category?.CategoryItems.SingleOrDefault(x => x.Id == itemId);
            if (categoryItem != null)
            {
                deleteItemDto.ItemId = categoryItem.Id;
            }

            category?.CategoryItems.Remove(categoryItem);
            await _context.SaveChangesAsync();

            return deleteItemDto;
        }
    }
}

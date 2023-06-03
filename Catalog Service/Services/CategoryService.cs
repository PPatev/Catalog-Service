using Catalog_Service.Data;
using Catalog_Service.Data.Entities;
using Catalog_Service.Interfaces;
using Catalog_Service.Models.Crud;
using Catalog_Service.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace Catalog_Service.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly CatalogContext _context;

        public CategoryService(CatalogContext context) 
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategories()
        {
            var categoryDtos = await _context.Categories.AsNoTracking()
                .Select(x => new CategoryDto
                {
                    CategoryId = x.Id,
                    CategoryName = x.Name,
                })
            .ToListAsync();

            return categoryDtos;
        }

        public async Task<CategoryDetailsDto?> GetCategory(int id)
        {
            var categoryDto = await _context.Categories.AsNoTracking().Where(x => x.Id == id)
                   .Select(x => new CategoryDetailsDto
                   {
                       CategoryId = x.Id,
                       CategoryName = x.Name,
                       CategoryItems = x.CategoryItems.Select(y => new CategoryItemDto
                       {
                           ItemId = y.Id,
                           ItemName = y.Name
                       }).ToList()
                   }).SingleOrDefaultAsync();

            return categoryDto;
        }

        public async Task<CategoryDetailsDto> CreateCategory(CreateCategoryModel createCategoryModel)
        {
            var category = new Category { Name = createCategoryModel.CategoryName };
            var categoryEntry = await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            var categoryDto = new CategoryDetailsDto
            {
                CategoryId = categoryEntry.Entity.Id,
                CategoryName = categoryEntry.Entity.Name,
                CategoryItems = new List<CategoryItemDto>()
            };

            return categoryDto;
        }

        public async Task<UpdateCategoryDto> UpdateCategory(UpdateCategoryModel updateCategoryModel)
        {
            var updateCategoryDto = new UpdateCategoryDto();
            var category = await _context.Categories.Include(x => x.CategoryItems).SingleOrDefaultAsync(x => x.Id == updateCategoryModel.Id);
            if (category == null)
            {
                return updateCategoryDto;
            }

            category.Name = updateCategoryModel.CategoryName;
            await _context.SaveChangesAsync();

            updateCategoryDto.CategoryId = category.Id;
            updateCategoryDto.CategoryName = category.Name;
            updateCategoryDto.CategoryItems = category.CategoryItems.Select(x => new CategoryItemDto
            {
                ItemId = x.Id,
                ItemName = x.Name
            }).ToList();

            return updateCategoryDto;
        }

        public async Task<DeleteCategoryDto> DeleteCategory(int categoryId)
        {
            var deleteCategoryDto = new DeleteCategoryDto();
            var category = await _context.Categories.Include(x => x.CategoryItems).SingleOrDefaultAsync(x => x.Id == categoryId);
            if (category == null)
            {
                return deleteCategoryDto;
            }

            deleteCategoryDto.CategoryId = category.Id;

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return deleteCategoryDto;
        }
    }
}

using Catalog_Service.Data.Entities;
using Catalog_Service.Interfaces;
using Catalog_Service.Models.Crud;
using Catalog_Service.Models.Dtos;

namespace Catalog_Service.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoriesRepository _categoriesRepository;

        public CategoryService(ICategoriesRepository categoriesRepository) 
        {
            _categoriesRepository = categoriesRepository;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _categoriesRepository.GetAllAsync();
            var categoryDtos = categories
            .Select(x => new CategoryDto
            {
                CategoryId = x.Id,
                CategoryName = x.Name,
            })
            .ToList();

            return categoryDtos;
        }

        public async Task<CategoryDetailsDto?> GetCategoryAsync(int id)
        {
            var category = await _categoriesRepository.GetByIdAsync(id);
            if (category == null)
            {
                return null;
            }

            var categoryDto = new CategoryDetailsDto
            {
                CategoryId = category.Id,
                CategoryName = category.Name,
                CategoryItems = category.CategoryItems.Select(y => new CategoryItemDto
                {
                    ItemId = y.Id,
                    ItemName = y.Name
                }).ToList()
            };

            return categoryDto;
        }

        public async Task<CategoryDetailsDto> CreateCategoryAsync(CreateCategoryModel createCategoryModel)
        {
            var category = new Category { Name = createCategoryModel.CategoryName };

            await _categoriesRepository.AddAsync(category);
            await _categoriesRepository.SaveAsync();

            var categoryDto = new CategoryDetailsDto
            {
                CategoryId = category.Id,
                CategoryName = category.Name,
                CategoryItems = new List<CategoryItemDto>()
            };

            return categoryDto;
        }

        public async Task<UpdateCategoryDto> UpdateCategoryAsync(UpdateCategoryModel updateCategoryModel)
        {
            var updateCategoryDto = new UpdateCategoryDto();
            var category = await _categoriesRepository.GetByIdAsync(updateCategoryModel.Id);
            if (category == null)
            {
                return updateCategoryDto;
            }

            category.Name = updateCategoryModel.CategoryName;
            await _categoriesRepository.SaveAsync();

            updateCategoryDto.CategoryId = category.Id;
            updateCategoryDto.CategoryName = category.Name;
            updateCategoryDto.CategoryItems = category.CategoryItems.Select(x => new CategoryItemDto
            {
                ItemId = x.Id,
                ItemName = x.Name
            }).ToList();

            return updateCategoryDto;
        }

        public async Task<DeleteCategoryDto> DeleteCategoryAsync(int categoryId)
        {
            var deleteCategoryDto = new DeleteCategoryDto();
            var category = await _categoriesRepository.GetByIdAsync(categoryId);

            if (category == null)
            {
                return deleteCategoryDto;
            }

            deleteCategoryDto.CategoryId = category.Id;

            _categoriesRepository.Delete(category);
            await _categoriesRepository.SaveAsync();

            return deleteCategoryDto;
        }
    }
}

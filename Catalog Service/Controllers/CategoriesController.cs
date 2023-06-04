using Catalog_Service.Constants;
using Catalog_Service.Interfaces;
using Catalog_Service.Models.Crud;
using Catalog_Service.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Catalog_Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    public class CategoriesController : ControllerBase
    {
        
        private readonly ILogger<CategoriesController> _logger;
        private readonly ICategoryService _categoryService;

        public CategoriesController(ILogger<CategoriesController> logger, ICategoryService categoryService)
        {
            _logger = logger;
            _categoryService = categoryService;
        }

        [HttpGet(Name = nameof(GetCategories))]
        [ResponseCache(Duration = 30)]
        [ProducesResponseType(typeof(IEnumerable<CategoryDto>), StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCategories()
        {
            var categoryDtos = await _categoryService.GetAllCategoriesAsync();

            return Ok(categoryDtos);
        }

        [HttpGet("{categoryId}", Name = nameof(GetCategory))]
        [ResponseCache(Duration = 30)]
        [ProducesResponseType(typeof(CategoryDetailsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCategory(int categoryId)
        {
            var categoryDto = await _categoryService.GetCategoryAsync(categoryId);

            if (categoryDto == null) 
            {
                return NotFound(string.Format(ObjectResultMessages.CategoryNotFound, categoryId));
            }

            return Ok(categoryDto);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CategoryDetailsDto), StatusCodes.Status201Created)]
        [SwaggerResponse(StatusCodes.Status201Created)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryModel createCategoryModel)
        {
            var categoryDto = await _categoryService.CreateCategoryAsync(createCategoryModel);

            return CreatedAtRoute(nameof(GetCategory), new { CategoryId = categoryDto.CategoryId }, categoryDto);
        }

        [HttpPut("{categoryId}")]
        [ProducesResponseType(typeof(CategoryDetailsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCategory(int categoryId, [FromBody] UpdateCategoryModel updateCategoryModel)
        {
            updateCategoryModel.Id = categoryId;

            var updateCategoryDto = await _categoryService.UpdateCategoryAsync(updateCategoryModel);

            if (updateCategoryDto.CategoryId == null)
            {
                return NotFound(string.Format(ObjectResultMessages.CategoryNotFound, categoryId));
            }

            return Ok(updateCategoryDto);
        }

        [HttpDelete("{categoryId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            var deleteCategoryDto = await _categoryService.DeleteCategoryAsync(categoryId);

            if (deleteCategoryDto.CategoryId == null)
            {
                return NotFound(string.Format(ObjectResultMessages.CategoryNotFound, categoryId));
            }
            
            return Ok();
        }
    }
}
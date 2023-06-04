using Catalog_Service.Constants;
using Catalog_Service.Data;
using Catalog_Service.Interfaces;
using Catalog_Service.Models.Crud;
using Catalog_Service.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Catalog_Service.Controllers
{
    [ApiController]
    [Route("api/categories/{categoryId}/[controller]")]
    [ApiVersion("1.0")]
    public class ItemsController : ControllerBase
    {
        private readonly ILogger<CategoriesController> _logger;
        private readonly ICategoryItemService _categoryItemService;

        public ItemsController(ILogger<CategoriesController> logger, CatalogDbContext context, ICategoryItemService categoryItemService)
        {
            _logger = logger;
            _categoryItemService = categoryItemService;
        }

        [HttpGet(Name = nameof(GetItems))]
        [ProducesResponseType(typeof(IEnumerable<CategoryItemDetailsDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetItems(int categoryId, [FromQuery] int pageIndex = 0, [FromQuery] int pageSize = 5)
        {
            var categoryItems = await _categoryItemService.GetCategoryItemsAsync(categoryId, pageIndex, pageSize);

            if (categoryItems == null)
            {
                return NotFound(string.Format(ObjectResultMessages.CategoryNotFound, categoryId));
            }

            return Ok(categoryItems);
        }

        [HttpGet("{itemId}", Name = nameof(GetItem))]
        [ResponseCache(Duration = 60)]
        [ProducesResponseType(typeof(CategoryItemDetailsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetItem(int categoryId, int itemId)
        {
            var categoryItemData = await _categoryItemService.GetCategoryItemAsync(categoryId, itemId);

            if (categoryItemData == null)
            {
                return NotFound(string.Format(ObjectResultMessages.CategoryItemNotFoundInCategory, itemId, categoryId));
            }

            return Ok(categoryItemData);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CategoryItemDetailsDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status201Created)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCategoryItem(int categoryId, [FromBody] CreateItemModel createItemModel)
        {
            createItemModel.CategoryId = categoryId;

            var categoryItemDto = await _categoryItemService.CreateCategoryItemAsync(createItemModel);

            if (categoryItemDto == null)
            {
                return NotFound(string.Format(ObjectResultMessages.CategoryNotFound, categoryId));
            }
            
            return CreatedAtRoute(nameof(GetItems), new { CategoryId = categoryItemDto.CategoryId }, categoryItemDto);
        }

        [HttpPut("{itemId}")]
        [ProducesResponseType(typeof(CategoryItemDetailsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCategoryItem(int categoryId, int itemId, [FromBody] UpdateItemModel updateItemModel)
        {
            updateItemModel.CategoryId = categoryId;
            updateItemModel.ItemId = itemId;

            var updateItemDto = await _categoryItemService.UpdateCategoryItemAsync(updateItemModel);

            if (updateItemDto.CategoryId == null)
            {
                return NotFound(string.Format(ObjectResultMessages.CategoryNotFound, categoryId));
            }

            if (updateItemDto.ItemId == null)
            {
                return NotFound(string.Format(ObjectResultMessages.CategoryItemNotFoundInCategory, itemId, categoryId));
            }

            return Ok(updateItemDto);
        }

        [HttpDelete("{itemId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCategoryItem(int categoryId, int itemId)
        {
            var deleteItemDto = await _categoryItemService.DeleteCategoryItemAsync(categoryId, itemId);
            if (deleteItemDto.CategoryId == null)
            {
                return NotFound(string.Format(ObjectResultMessages.CategoryNotFound, categoryId));
            }
            
            if (deleteItemDto.ItemId == null)
            {
                return NotFound(string.Format(ObjectResultMessages.CategoryItemNotFoundInCategory, itemId, categoryId));
            }

            return Ok();
        }
    }
}

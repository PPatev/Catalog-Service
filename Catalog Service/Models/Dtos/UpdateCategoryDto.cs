using System.Text.Json.Serialization;

namespace Catalog_Service.Models.Dtos
{
    public class UpdateCategoryDto
    {
        [JsonPropertyOrder(1)]
        public int? CategoryId { get; set; }

        [JsonPropertyOrder(2)]
        public string CategoryName { get; set; }

        [JsonPropertyOrder(3)]
        public ICollection<CategoryItemDto> CategoryItems { get; set; }
    }
}

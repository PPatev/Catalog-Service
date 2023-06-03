using System.Text.Json.Serialization;

namespace Catalog_Service.Models.Dtos
{
    public class CategoryDetailsDto : CategoryDto
    {
        [JsonPropertyOrder(3)]
        public ICollection<CategoryItemDto> CategoryItems { get; set; }
    }
}

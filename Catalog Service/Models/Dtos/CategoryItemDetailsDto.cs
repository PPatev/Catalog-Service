using System.Text.Json.Serialization;

namespace Catalog_Service.Models.Dtos
{
    public class CategoryItemDetailsDto : CategoryItemDto
    {

        [JsonPropertyOrder(3)]
        public int? CategoryId { get; set; }

        [JsonPropertyOrder(4)]
        public string CategoryName { get; set; }
    }
}

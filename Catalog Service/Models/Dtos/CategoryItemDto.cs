using System.Text.Json.Serialization;

namespace Catalog_Service.Models.Dtos
{
    public class CategoryItemDto
    {
        [JsonPropertyOrder(1)]
        public int? ItemId { get; set; }

        [JsonPropertyOrder(2)]
        public string ItemName { get; set; }
    }
}

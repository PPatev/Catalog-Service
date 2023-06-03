using System.Text.Json.Serialization;

namespace Catalog_Service.Models.Dtos
{
    public class DeleteCategoryItemDto
    {
        public int? CategoryId { get; set; }

        public int? ItemId { get; set; }
    }
}

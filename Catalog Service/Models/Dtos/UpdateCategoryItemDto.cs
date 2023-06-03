using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Catalog_Service.Models.Dtos
{
    public class UpdateCategoryItemDto
    {
        public int? ItemId { get; set; }

        public string ItemName { get; set; } = string.Empty;

        public int? CategoryId { get; set; }

        public string CategoryName { get; set; }

    }
}

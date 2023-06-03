using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Catalog_Service.Models.Crud
{
    public class CreateItemModel
    {
        [JsonIgnore]
        public int CategoryId { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string ItemName { get; set; } = string.Empty;
    }
}

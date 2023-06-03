using System.Text.Json.Serialization;

namespace Catalog_Service.Models.Crud
{
    public class UpdateCategoryModel : CreateCategoryModel
    {
        [JsonIgnore]
        public int Id { get; set; }
    }
}

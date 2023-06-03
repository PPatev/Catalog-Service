using System.Text.Json.Serialization;

namespace Catalog_Service.Models.Crud
{
    public class UpdateItemModel : CreateItemModel
    {
        [JsonIgnore]
        public int CategoryId { get; set; }

        [JsonIgnore]
        public int ItemId { get; set; }
    }
}

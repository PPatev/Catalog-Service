using System.ComponentModel.DataAnnotations;

namespace Catalog_Service.Models.Crud
{
    public class CreateCategoryModel
    {
        [Required(AllowEmptyStrings = false)]
        public string CategoryName { get; set; } = string.Empty;
    }
}

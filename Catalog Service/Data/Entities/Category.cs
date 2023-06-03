namespace Catalog_Service.Data.Entities
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public ICollection<CategoryItem> CategoryItems { get; set; } = new List<CategoryItem>();
    }
}

using AutoFixture;
using Catalog_Service.Data.Entities;

namespace CatalogServiceApiTests
{
    public static class TestHelper
    {
        public static IEnumerable<Category> CreateData(Fixture fixture)
        {
            var categories = new List<Category>();

            for (int i = 1; i <= 20; i++)
            {
                var category = new Category { Id = i, Name = fixture.Create<string>(), CategoryItems = new List<CategoryItem>() };
                for (int j = 1; j <= 20; j++)
                {
                    category.CategoryItems.Add(new CategoryItem { Id = (i * 100 + j), Name = fixture.Create<string>(), CategoryId = category.Id, Category = category });
                }

                categories.Add(category);
            }

            return categories;
        }
    }
}

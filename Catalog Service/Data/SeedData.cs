using Catalog_Service.Data.Entities;

namespace Catalog_Service.Data
{
    public static class SeedData
    {
        public static async Task CreateCategories(IServiceScopeFactory serviceScopeFactory)
        {
            using (IServiceScope scope = serviceScopeFactory.CreateScope())
            {
                CatalogContext context = scope.ServiceProvider.GetRequiredService<CatalogContext>();
                context.Database.EnsureCreated();

                var categories = new List<Category>();
                for (int i = 1; i <= 20; i++)
                {
                    var category = new Category { Name = $"Category{i}", CategoryItems = new List<CategoryItem>() };
                    for (int j = 1; j <= 20; j++)
                    {
                        category.CategoryItems.Add(new CategoryItem { Name = $"CategoryItem{j}" });
                    }

                    categories.Add(category);
                }

                context.Categories.AddRange(categories);
                await context.SaveChangesAsync();
            }
        }
    }
}

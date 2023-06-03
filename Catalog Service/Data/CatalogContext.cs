using Catalog_Service.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Catalog_Service.Data
{
    public class CatalogContext : DbContext
    {
        public CatalogContext(DbContextOptions<CatalogContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<CategoryItem> CategoryItems { get; set; } = null!;
    }
    
}

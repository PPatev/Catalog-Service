using Catalog_Service.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Catalog_Service.Data
{
    public class CatalogDbContext : DbContext
    {
        public CatalogDbContext() {} 
        

        public CatalogDbContext(DbContextOptions<CatalogDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<CategoryItem> CategoryItems { get; set; }
    }
    
}

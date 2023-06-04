using Catalog_Service.Data;
using Catalog_Service.Data.Entities;
using Catalog_Service.Interfaces;
using Catalog_Service.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Catalog_Service.Repositories
{
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly CatalogDbContext _context;

        public CategoriesRepository(CatalogDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.AsNoTracking().ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int categoryId)
        {
            return await _context.Categories.Include(x => x.CategoryItems).SingleOrDefaultAsync(x => x.Id == categoryId);
        }

        public async Task AddAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
        }

        public void Update(Category category)
        {
            _context.Entry(category).State = EntityState.Modified;
        }

        public void Delete(Category category)
        {
            if (category != null)
            {
                _context.Categories.Remove(category);
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

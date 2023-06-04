using Catalog_Service.Data.Entities;

namespace Catalog_Service.Interfaces
{
    public interface IItemsRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int categoryId);
        Task AddAsync(Category category);
        void Update(Category category);
        void Delete(Category category);
        Task SaveAsync();
    }
}

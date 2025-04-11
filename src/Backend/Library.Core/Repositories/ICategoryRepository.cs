using Library.Core.Entities;

namespace Library.Core.Repositories;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetCategoriesAsync();
    Task<Category> AddCategoryAsync(Category category);
    Task<Category?> GetCategoryByIdAsync(Guid id);
    Task<Category?> GetCategoryByNameAsync(string name);
    Task<bool> ExistCategoryAsync(Category category);
    Task Update(Category category);
    Task AddCategoriesAsync(IEnumerable<Category> category);
}
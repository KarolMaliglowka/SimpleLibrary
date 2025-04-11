using Library.Core.Entities;
using Library.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.DAL.Repositories;

public class CategoryRepository(LibraryDbContext context) : ICategoryRepository
{
    public async Task<IEnumerable<Category>> GetCategoriesAsync() =>
        await context.Categories.AsNoTracking().ToListAsync();

    public async Task<Category> AddCategoryAsync(Category category)
    {
        await context.Categories.AddAsync(category);
        await context.SaveChangesAsync();
        return category;
    }

    public async Task<Category?> GetCategoryByIdAsync(Guid id) =>
        await context.Categories
            .AsNoTracking()
            .SingleOrDefaultAsync(c => c.Id == id);

    public async Task<Category?> GetCategoryByNameAsync(string name) =>
        await context.Categories
            .SingleOrDefaultAsync(c => c.Name == name);
    
    public async Task<bool> ExistCategoryAsync(Category category) {
        return await context.Categories
            .AnyAsync(c => c.Name == category.Name);
    }

    public async Task Update(Category category)
    {
        context.Categories.Update(category);
        await context.SaveChangesAsync();
    }
    public async Task AddCategoriesAsync(IEnumerable<Category> category)
    {
        await context.Categories.AddRangeAsync(category);
        await context.SaveChangesAsync();
    }
}
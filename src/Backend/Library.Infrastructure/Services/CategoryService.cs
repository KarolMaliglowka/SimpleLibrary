using Library.Core.Entities;
using Library.Core.Exceptions;
using Library.Core.Repositories;
using Library.Infrastructure.DTO;

namespace Library.Infrastructure.Services;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetCategoriesAsync();
    Task AddCategoryAsync(CategoryDto category);
    Task AddCategoriesAsync(List<CategoryDto> category);
    Task UpdateCategoryAsync(CategoryDto category);
    Task<Category?> GetCategoryByIdAsync(Guid id);
    Task<Category?> GetCategoryByNameAsync(string name);
    Task DeleteCategoryAsync(Guid guid);
}

public class CategoryService(ICategoryRepository categoryRepository, IBookRepository bookRepository) : ICategoryService
{
    public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
    {
        var categoriesList = await categoryRepository.GetCategoriesAsync();
        return categoriesList.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                isDelete = c.IsDeleted
            })
            .OrderBy(x => x.Name)
            .ToList();
    }

    public async Task AddCategoryAsync(CategoryDto categoryDto)
    {
        var existingCategory = await categoryRepository.GetCategoryByNameAsync(categoryDto.Name);
        if (existingCategory != null)
        {
            throw new Exception("Category already exists");
        }

        var category = new Category(categoryDto.Name);
        await categoryRepository.AddCategoryAsync(category);
    }

    public async Task UpdateCategoryAsync(CategoryDto category)
    {
        var existingCategory = await categoryRepository.GetCategoryByIdAsync(category.Id);
        if (existingCategory == null)
        {
            throw new Exception("Category not exists");
        }

        existingCategory.SetCategory(category.Name);
        await categoryRepository.Update(existingCategory);
    }

    public async Task<Category?> GetCategoryByIdAsync(Guid id)
    {
        var categoryExist = await categoryRepository.GetCategoryByIdAsync(id);
        if (categoryExist == null)
        {
            throw new Exception("Category not found");
        }

        return categoryExist;
    }

    public async Task<Category?> GetCategoryByNameAsync(string name)
    {
        var categoryExist = await categoryRepository.GetCategoryByNameAsync(name);
        if (categoryExist == null)
        {
            throw new CategoryNotFoundException(name);
        }

        return categoryExist;
    }

    public async Task AddCategoriesAsync(List<CategoryDto> categoryDto)
    {
        var categoryExistInSystem = await GetCategoriesAsync();
        var categoriesToImport = categoryDto
            .Where(x => !categoryExistInSystem.Any(y =>
                y.Name.Equals(x.Name, StringComparison.OrdinalIgnoreCase)))
            .Select(x => new Category(x.Name)).ToList();

        if (categoriesToImport.Count != 0)
        {
            await categoryRepository.AddCategoriesAsync(categoriesToImport);
        }
    }

    public async Task DeleteCategoryAsync(Guid id)
    {
        var categoryExist = await categoryRepository.GetCategoryByIdAsync(id);
        if (categoryExist == null)
        {
            throw new Exception("Category not found");
        }

        var booksList = await bookRepository.GetAllAsync();
        var isCategoryForSomeBook = booksList.Any(x => x.Category?.Name == categoryExist.Name);
        if (isCategoryForSomeBook)
        {
            
            throw new Exception("Category is in use");
        }

        categoryExist.SetSoftDelete();
        await categoryRepository.Update(categoryExist);
    }
}
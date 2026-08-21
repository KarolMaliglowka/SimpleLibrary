using Library.Application.DTO;
using Library.Core;
using Library.Core.Entities;
using Library.Core.Exceptions;
using Library.Core.Repositories;

namespace Library.Application.Services;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetCategoriesAsync();
    Task AddCategoryAsync(CategoryDto category);
    Task AddCategoriesAsync(List<CategoryDto> category);
    Task UpdateCategoryAsync(CategoryDto category);
    Task<Category?> GetCategoryByIdAsync(Guid id);
    Task<Category?> GetCategoryByNameAsync(string name);
    Task DeleteCategoryAsync(Guid guid);
    Task<List<Dictionary<Guid, string>>> GetCategoriesDictionaryAsync();
}

public class CategoryService(
    ICategoryRepository categoryRepository,
    IBookRepository bookRepository,
    IUnitOfWork unitOfWork) : ICategoryService
{
    public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
    {
        var categoriesList = await categoryRepository.GetCategoriesAsync();
        return categoriesList.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                IsDeleted = c.IsDeleted
            })
            .OrderBy(x => x.Name)
            .ToList();
    }

    public async Task AddCategoryAsync(CategoryDto categoryDto)
    {
        var existingCategory = await categoryRepository.GetCategoryByNameAsync(categoryDto.Name);
        if (existingCategory != null)
        {
            throw new AlreadyExistsException("Category", categoryDto.Name);
        }

        var category = new Category(categoryDto.Name);
        await categoryRepository.AddCategoryAsync(category);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateCategoryAsync(CategoryDto category)
    {
        var existingCategory = await categoryRepository.GetCategoryByIdAsync(category.Id);
        if (existingCategory == null)
        {
            throw new CategoryNotFoundException($" with {category.Id} ");
        }

        existingCategory.SetCategory(category.Name);
        categoryRepository.UpdateCategory(existingCategory);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task<Category?> GetCategoryByIdAsync(Guid id)
    {
        var categoryExist = await categoryRepository.GetCategoryByIdAsync(id);
        return categoryExist ?? throw new CategoryNotFoundException($" with {id} ");
    }

    public async Task<Category?> GetCategoryByNameAsync(string name)
    {
        var categoryExist = await categoryRepository.GetCategoryByNameAsync(name);
        return categoryExist ?? throw new CategoryNotFoundException(name);
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
            await unitOfWork.SaveChangesAsync();
        }
    }

    public async Task DeleteCategoryAsync(Guid id)
    {
        var categoryExist = await categoryRepository.GetCategoryByIdAsync(id);
        if (categoryExist == null)
        {
            throw new CategoryNotFoundException($" with {id} ");
        }

        var booksList = await bookRepository.GetAllBooksAsync();
        var isCategoryForSomeBook = booksList.Any(x => x.Category?.Name == categoryExist.Name);
        if (isCategoryForSomeBook)
        {
            throw new IsInUseException("Category", categoryExist.Name.Value);
        }

        categoryExist.SetSoftDelete();
        categoryRepository.UpdateCategory(categoryExist);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task<List<Dictionary<Guid, string>>> GetCategoriesDictionaryAsync()
    {
        var categoriesList = await categoryRepository.GetCategoriesAsync();
        return
        [
            .. categoriesList
                .OrderBy(x => x.Name.Value)
                .Where(x => !x.IsDeleted)
                .Select(x => new Dictionary<Guid, string>
                {
                    [x.Id] = x.Name.Value
                })
        ];
    }
}
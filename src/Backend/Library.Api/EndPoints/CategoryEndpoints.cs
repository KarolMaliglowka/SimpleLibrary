using System.ComponentModel.DataAnnotations;
using Library.Application.DTO;
using Library.Application.Services;

namespace Library.Api.EndPoints;

public static class CategoryEndpoints
{
    public static void MapCategoriesEndpoints(this WebApplication app)
    {
        app.MapGet("/categories", async (ICategoryService categoryService) =>
            await categoryService.GetCategoriesAsync() is { } category
                ? Results.Ok(category)
                : Results.NotFound("No categories found."));
        
        app.MapGet("/categories/{id:guid}",
            async (Guid id, ICategoryService categoryService) =>
                await categoryService.GetCategoryByIdAsync(id)
                    is { } category
                    ? Results.Ok(category)
                    : Results.NotFound());

        app.MapGet("/categories/{name}",
            async ([Required] string name, ICategoryService categoryService) =>
            await categoryService.GetCategoryByNameAsync(name)
                is { } category
                ? Results.Ok(category)
                : Results.NotFound());

        app.MapGet("/categories/getCategories", async (ICategoryService categoryService) =>
            await categoryService.GetCategoriesDictionaryAsync() is { } category
                ? Results.Ok(category)
                : Results.NotFound("No categories found."));
        
        app.MapPost("/categories",
            async (CategoryDto category, ICategoryService categoryService) =>
            {
                await categoryService.AddCategoryAsync(category);
                return Results.Created();
            });

        app.MapPost("/categories/createMany",
            async (List<CategoryDto> categories, ICategoryService categoryService) =>
            {
                await categoryService.AddCategoriesAsync(categories);
                return Results.Created();
            });

        app.MapPatch("/categories",
            async (CategoryDto category, ICategoryService categoryService) =>
            {
                await categoryService.UpdateCategoryAsync(category);
                return Results.NoContent();
            });

        app.MapDelete("/categories/{id:guid}", async (Guid id, ICategoryService categoryService) =>
            await categoryService.DeleteCategoryAsync(id));
    }
}
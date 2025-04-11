using System.ComponentModel.DataAnnotations;
using Library.Core.Entities;
using Library.Infrastructure.DTO;
using Library.Infrastructure.Services;

namespace Library.Api.EndPoints;

public static class CategoryEndpoints
{
    public static void MapCategoriesEndpoints(this WebApplication app)
    {
        app.MapGet("/category", async (ICategoryService categoryService) =>
            await categoryService.GetCategoriesAsync() is { } category
                ? Results.Ok(category)
                : Results.NotFound("No categories found."));

        app.MapPost("/category/create",
            async (CategoryDto category, ICategoryService categoryService) =>
            {
                await categoryService.AddCategoryAsync(category);
                return Results.Created();
            });

        app.MapPost("/category/createMany",
            async (List<CategoryDto> categories, ICategoryService categoryService) =>
            {
                await categoryService.AddCategoriesAsync(categories);
                return Results.Created();
            });

        app.MapPatch("/category/update",
            async (Category category, ICategoryService categoryService) =>
            {
                await categoryService.UpdateCategoryAsync(category);
                return Results.NoContent();
            });

        app.MapGet("/category/{id:guid}", async (Guid id, ICategoryService categoryService) =>
            await categoryService.GetCategoryByIdAsync(id)
                is { } category
                ? Results.Ok(category)
                : Results.NotFound());

        app.MapGet("/category/{name}", async ([Required] string name, ICategoryService categoryService) =>
        await categoryService.GetCategoryByNameAsync(name)
            is { } category
            ? Results.Ok(category)
            : Results.NotFound());
    }
}
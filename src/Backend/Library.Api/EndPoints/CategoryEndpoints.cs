using System.ComponentModel.DataAnnotations;
using Library.Infrastructure.DTO;
using Library.Infrastructure.Services;

namespace Library.Api.EndPoints;

public static class CategoryEndpoints
{
    public static void MapCategoriesEndpoints(this WebApplication app)
    {
        app.MapGet("/categories", async (ICategoryService categoryService) =>
            await categoryService.GetCategoriesAsync() is { } category
                ? Results.Ok(category)
                : Results.NotFound("No categories found."));

        app.MapPost("/categories/create",
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

        app.MapPatch("/categories/update",
            async (CategoryDto category, ICategoryService categoryService) =>
            {
                await categoryService.UpdateCategoryAsync(category);
                return Results.NoContent();
            });

        app.MapGet("/categories/{id:guid}", async (Guid id, ICategoryService categoryService) =>
            await categoryService.GetCategoryByIdAsync(id)
                is { } category
                ? Results.Ok(category)
                : Results.NotFound());

        app.MapGet("/categories/{name}", async ([Required] string name, ICategoryService categoryService) =>
            await categoryService.GetCategoryByNameAsync(name)
            is { } category
            ? Results.Ok(category)
            : Results.NotFound());
        
        app.MapDelete("/categories/delete/{id:guid}", async (Guid id, ICategoryService categoryService) =>
            await categoryService.GetCategoryByIdAsync(id)
                is { } category
                ? Results.Ok(category)
                : Results.NotFound());
    }
}
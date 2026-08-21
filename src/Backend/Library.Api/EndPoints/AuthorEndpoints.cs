using Library.Application.DTO;
using Library.Application.Services;
using Library.Core.Repositories;

namespace Library.Api.EndPoints;

public static class AuthorEndpoints
{
    public static void MapAuthorEndpoints(this WebApplication app)
    {
        app.MapGet("/authors", async (IAuthorService authorService) =>
            await authorService.GetAuthorsAsync() is { } author
                ? Results.Ok(author)
                : Results.NotFound());
        
        app.MapGet("/authors/{id:guid}", async (Guid id, IAuthorService authorService) =>
        {
            var author = await authorService.GetAuthorsByIdAsync(id);
            return Results.Ok(author);
        });

        app.MapGet("/authors/getAuthors", async (IAuthorService authorService) =>
            await authorService.GetAuthorsDictionaryAsync() is { } author
                ? Results.Ok(author)
                : Results.NotFound());

        app.MapPost("/authors", async (AuthorDto author, IAuthorService authorService) =>
        {
            await authorService.CreateAuthorAsync(author);
            return Results.Created();
        });

        app.MapPost("/authors/createMany", async (List<AuthorDto> authors, IAuthorService authorService) =>
        {
            await authorService.CreateAuthorsAsync(authors);
            return Results.Created();
        });

        app.MapPatch("/authors", async (
            AuthorDto author,
            IAuthorReadRepository authorReadRepository,
            IAuthorService authorService
        ) =>
        {
            await authorService.UpdateAuthorAsync(author);
            return Results.Ok("Author updated");
        });

        app.MapDelete("/authors/{id:guid}", async (Guid id, IAuthorService authorService) =>
            await authorService.DeleteAuthorAsync(id));
    }
}
using Library.Core.Entities;
using Library.Core.Repositories;
using Library.Infrastructure.DTO;
using Library.Infrastructure.Exceptions;
using Library.Infrastructure.Services;

namespace Library.Api.EndPoints;

public static class AuthorEndpoints
{
    public static void MapAuthorEndpoints(this WebApplication app)
    {
        app.MapGet("/authors", async (IAuthorService authorService) =>
            await authorService.GetAuthorsAsync() is { } author
                ? Results.Ok(author)
                : Results.NotFound());

        app.MapPost("/authors/create", async (AuthorDto author, IAuthorService authorService) =>
        {
            try
            {
                var id = await authorService.CreateAuthorAsync(author);
                return Results.Created($"/author/{id}", new { Id = id });
            }
            catch (AuthorAlreadyExistsException ex)
            {
                return Results.Content(ex.Message);
            }
        });

        app.MapPost("/authors/createMany", async (List<Author> authors, IAuthorRepository authorRepository) =>
        {
            await authorRepository.AddAuthorsAsync(authors);
            return Results.Created();
        });

        app.MapPut("/authors/update", async (
            Author author,
            IAuthorReadRepository authorReadRepository,
            IAuthorRepository authorRepository
        ) =>
        {
            var authorInDb = await authorReadRepository.GetAuthorByIdAsync(author.Id);
            if (authorInDb == null)
            {
                return Results.NotFound("Author not found :/");
            }

            await authorRepository.UpdateAuthorAsync(author);
            return Results.Ok("Author updated");
        });

        app.MapDelete("/authors/delete/{id:guid}",
            async (Guid id, IAuthorReadRepository authorReadRepository, IAuthorRepository authorRepository) =>
            {
                var author = await authorReadRepository.GetAuthorByIdAsync(id);
                if (author == null)
                {
                    return Results.NotFound("Author not found");
                }

                await authorRepository.DeleteAuthorAsync(author);
                return Results.Ok("Author deleted");
            });

        app.MapGet("/authors/{id:guid}", async (Guid id, IAuthorReadRepository authorReadRepository) =>
        {
            var author = await authorReadRepository.GetAuthorByIdAsync(id);
            return author != null ? Results.Ok(author) : Results.NotFound("Author not found");
        });
    }
}
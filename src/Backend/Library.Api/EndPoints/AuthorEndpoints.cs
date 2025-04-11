using Library.Core.Entities;
using Library.Core.Repositories;

namespace Library.Api.EndPoints;

public static class AuthorEndpoints
{
    public static void MapAuthorEndpoints(this WebApplication app)
    {
        app.MapGet("/author", async (IAuthorRepository authorRepository) =>
        {
            var authors = await authorRepository.GetAuthorsAsync();
            return Results.Ok(authors);
        });

        app.MapPost("/author/create", async (Author author, IAuthorRepository authorRepository) =>
        {
            await authorRepository.AddAuthorAsync(author);
            return Results.Created($"/author/{author.Id}", author);
        });

        app.MapPost("/author/createMany", async (List<Author> authors, IAuthorRepository authorRepository) =>
        {
            await authorRepository.AddAuthorsAsync(authors);
            return Results.Created();
        });

        app.MapPatch("/author/update", async (Author author, IAuthorRepository authorRepository) =>
        {
            var authorInDb = await authorRepository.GetAuthorByIdAsync(author.Id);
            if (authorInDb == null)
            {
                return Results.NotFound("Author not found :/");
            }

            await authorRepository.UpdateAuthorAsync(author);
            return Results.Ok("Author updated");
        });

        app.MapDelete("/author/delete/{id:guid}", async (Guid id, IAuthorRepository authorRepository) =>
        {
            var author = await authorRepository.GetAuthorByIdAsync(id);
            if (author == null)
            {
                return Results.NotFound("Author not found");
            }

            await authorRepository.DeleteAuthorAsync(author);
            return Results.Ok("Author deleted");
        });

        app.MapGet("/author/{id:guid}", async (Guid id, IAuthorRepository authorRepository) =>
        {
            var author = await authorRepository.GetAuthorByIdAsync(id);
            return author != null ? Results.Ok(author) : Results.NotFound("Author not found....");
        });
    }
}
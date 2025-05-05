using FluentValidation;
using Library.Api.Extensions;
using Library.Infrastructure.DTO;
using Library.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.EndPoints;

public static class BookEndpoints
{
    public static void MapBookEndpoint(this WebApplication app)
    {
        app.MapGet("/book", async (IBookService bookService) =>
        {
            var books = await bookService.GetAllBooksAsync();
            return books.Count == 0 ? Results.NotFound("No books found.") : Results.Ok(books);
        });

        app.MapPost("/book/create",
            async (BookDto book,
                IBookService bookService,
                [FromServices] IValidator<BookDto> bookValidator, HttpContext context) =>
            {
                var validateResult = await bookValidator.ValidateCommandAsync(book, context);
                if (validateResult != Results.Empty) return validateResult;
                await bookService.CreateBookAsync(book);
                return Results.Created();
            });

        app.MapPost("/book/createMany",
            async (List<BookDto> books, IBookService bookService) =>
            {
                await bookService.CreateBooksAsync(books);
                return Results.Created();
            });

        app.MapGet("/book/{id:guid}", async (Guid id, IBookService bookService) =>
        {
            var book = await bookService.GetBookByIdAsync(id);
            return Results.Ok(book);
        });

        app.MapPatch("/book/update",
            async (BookDto book,
                IBookService bookService,
                [FromServices] IValidator<BookDto> bookValidator,
                HttpContext context) =>
            {
                var validateResult = await bookValidator.ValidateCommandAsync(book, context);
                if (validateResult != Results.Empty) return validateResult;
                await bookService.UpdateBook(book);
                return Results.NoContent();
            });

        app.MapGet("/book/author",
            async ([FromQuery] string surname, [FromQuery] string name, IBookService bookService) =>
            {
                var book = await bookService.GetBooksByAuthorAsync(surname, name);
                return Results.Ok(book);
            });

        app.MapGet("/book/category", async ([FromQuery] string name, IBookService bookService) =>
        {
            var book = await bookService.GetBooksByCategoryAsync(name);
            return Results.Ok(book);
        });

        app.MapGet("/book/publisher", async ([FromQuery] string name, IBookService bookService) =>
        {
            var book = await bookService.GetBooksByPublisherAsync(name);
            return Results.Ok(book);
        });

        app.MapGet("/book/{name}", async (string name, IBookService bookService) =>
            await bookService.GetBookByNameAsync(name)
                is { } book
                ? Results.Ok(book)
                : Results.NotFound("Book not found"));
    }
}
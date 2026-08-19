using Library.Application.DTO;
using Library.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.EndPoints;

public static class BorrowEndpoints
{
    public static void MapBorrowEndpoints(this WebApplication app)
    {
        app.MapPost("/borrows", async ([FromBody]BorrowRequestDto borrowDto, IBorrowService borrowService ) =>
        {
            await borrowService.CreateBorrow(borrowDto);
            return Results.Created();
        });
        
        app.MapDelete("/borrows/{id:guid}", async (Guid id, IBorrowService borrowService ) =>
        {
            await borrowService.DeleteBorrow(id);
            return Results.Ok();
        });

        app.MapGet("/borrows", async (IBookService bookService) =>
        {
            var borrowingBooks = await bookService.GetBorrowingBooksWithUsers();
            return Results.Ok(borrowingBooks);
        });
    }
}

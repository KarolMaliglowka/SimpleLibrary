using Library.Infrastructure.DTO;
using Library.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.EndPoints;

public static class BorrowEndpoints
{
    public static void MapBorrowEndpoints(this WebApplication app)
    {
        app.MapPost("/borrow/create", async ([FromBody]BorrowDto borrowDto, IBorrowService borrowService ) =>
        {
            await borrowService.CreateBorrow(borrowDto);
            return Results.Created();
        });
        
        app.MapDelete("/borrow/delete", async ([FromBody]BorrowDto borrowDto, IBorrowService borrowService ) =>
        {
            await borrowService.DeleteBorrow(borrowDto);
            return Results.Ok();
        });
    }
}
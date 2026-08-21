using Library.Core.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace Library.Api.GlobalHandlers;
public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken)
    {
        switch (exception)
        {
            case NotFoundException ex:
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsJsonAsync(new
                {
                    message = ex.Message
                });
                return true;

            case IsInUseException ex:
                context.Response.StatusCode = StatusCodes.Status409Conflict;
                await context.Response.WriteAsJsonAsync(new
                {
                    message = ex.Message
                });
                return true;
            
            case AlreadyExistsException ex:
                context.Response.StatusCode = StatusCodes.Status409Conflict;
                await context.Response.WriteAsJsonAsync(new
                {
                    message = ex.Message
                });
                return true;

            default:
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(new
                {
                    message = "Unexpected error."
                });
                return true;
        }
    }
}
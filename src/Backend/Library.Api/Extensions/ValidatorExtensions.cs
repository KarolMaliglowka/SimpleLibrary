using FluentValidation;

namespace Library.Api.Extensions;

public static class ValidatorExtensions
{
    public static async Task<IResult> ValidateCommandAsync<T>(
        this IValidator<T> validator,
        T command,
        HttpContext httpContext)
    {
        var validationResult = await validator.ValidateAsync(command);
        if (validationResult.IsValid) return Results.Empty;
        var problemDetails = new HttpValidationProblemDetails(validationResult.ToDictionary())
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Validation failed",
            Detail = "One or more validation occured.",
            Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"
        };
        return Results.Problem(problemDetails);
    }
}
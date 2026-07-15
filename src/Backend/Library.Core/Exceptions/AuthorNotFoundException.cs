namespace Library.Core.Exceptions;

public class AuthorNotFoundException: CustomException
{
    public AuthorNotFoundException(string? name = null)
        : base($"Author {name} not found")
    {
    }
}
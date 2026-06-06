namespace Library.Core.Exceptions;

public class AuthorNotFoundException: Exception
{
    public AuthorNotFoundException(string? name = null)
        : base($"Author {name} not found")
    {
    }
}
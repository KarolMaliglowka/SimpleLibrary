namespace Library.Infrastructure.Exceptions;

public class AuthorNotFoundException: Exception
{
    public AuthorNotFoundException()
        : base("Author not found")
    {
    }
}
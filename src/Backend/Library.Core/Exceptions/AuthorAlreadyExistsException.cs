namespace Library.Core.Exceptions;

public class AuthorAlreadyExistsException : CustomException
{
    public AuthorAlreadyExistsException()
        : base("Author already exists")
    {
    }
}
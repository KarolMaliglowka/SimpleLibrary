namespace Library.Core.Exceptions;

public class BookNotFoundException : CustomException
{
    public BookNotFoundException(string? name = null)
        : base($"Book {name} not found")
    {
    }
}
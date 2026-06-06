namespace Library.Core.Exceptions;

public class BookNotFoundException : Exception
{
    public BookNotFoundException(string? name = null)
        : base($"Book {name} not found")
    {
    }
}
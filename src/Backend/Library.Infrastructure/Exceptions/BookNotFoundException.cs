namespace Library.Infrastructure.Exceptions;

public class BookNotFoundException : Exception
{
    public BookNotFoundException()
        : base("Book not found")
    {
    }
}
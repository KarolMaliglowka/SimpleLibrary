namespace Library.Infrastructure.Exceptions;

public class PublisherNotFoundException : Exception
{
    public PublisherNotFoundException(string name)
        : base($"Publisher { name } not found")
    {
    }
}
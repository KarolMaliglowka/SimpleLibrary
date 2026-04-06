namespace Library.Infrastructure.Exceptions;

public class PublisherNotFoundException : Exception
{
    public PublisherNotFoundException()
        : base("Publisher not found")
    {
    }
}
namespace Library.Core.Exceptions;

public class PublisherNotFoundException : Exception
{
    public PublisherNotFoundException(string name)
        : base($"Publisher with id: '{ name }' not found")
    {
    }
}
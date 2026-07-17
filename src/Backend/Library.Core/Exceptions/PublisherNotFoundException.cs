namespace Library.Core.Exceptions;

public class PublisherNotFoundException : CustomException
{
    public PublisherNotFoundException(string name)
        : base($"Publisher with id: '{ name }' not found")
    {
    }
}
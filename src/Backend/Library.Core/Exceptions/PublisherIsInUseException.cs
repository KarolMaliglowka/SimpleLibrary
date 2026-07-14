namespace Library.Core.Exceptions;

public class PublisherIsInUseException : Exception
{
    public PublisherIsInUseException(string name)
        : base($"Publisher '{name}' is in use")
    {
    }
}
namespace Library.Core.Exceptions;

public class UserNotFoundException : Exception
{
    public UserNotFoundException(string? name = null)
        : base($"User {name} not found")
    {
    }
}
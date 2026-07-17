namespace Library.Core.Exceptions;

public class CategoryIsInUseException : Exception
{
    public CategoryIsInUseException(string name)
        : base($"Category '{name}' is in use")
    {
    }
}
namespace Library.Core.Exceptions;

public class InvalidNameException : CustomException
{
    public string Name { get; }

    public InvalidNameException(string name) : base($"Name: '{name}' is invalid.")
    {
        Name = name;
    }
}
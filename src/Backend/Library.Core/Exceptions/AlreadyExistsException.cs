namespace Library.Core.Exceptions;

public class AlreadyExistsException : CustomException
{
    public string Model { get; }
    public string Name { get; }

    public AlreadyExistsException(string model, string name) : base($"{model} '{name}' already exists.")
    {
        
    }
}
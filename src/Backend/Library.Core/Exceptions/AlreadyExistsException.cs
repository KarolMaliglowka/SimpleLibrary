namespace Library.Core.Exceptions;

public class  AlreadyExistsException(string model, string name) : CustomException($"{model} '{name}' already exists.");
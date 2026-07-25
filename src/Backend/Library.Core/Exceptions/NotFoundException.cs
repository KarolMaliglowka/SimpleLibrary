namespace Library.Core.Exceptions;

public class NotFoundException(string position, string name)
    : CustomException($"{position} with id: '{name}' not found");
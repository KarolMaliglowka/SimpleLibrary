namespace Library.Core.Exceptions;

public class IsInUseException(string position, string name) : CustomException($"{position} '{name}' is in use");
namespace Library.Core.Exceptions;

public class IsInUseException(string position, string name) : Exception($"{position} '{name}' is in use");
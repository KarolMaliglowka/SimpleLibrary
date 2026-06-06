namespace Library.Core.Exceptions;

public class BorrowNotFoundException : Exception
{
    public BorrowNotFoundException(Guid? id = null)
        : base($"Borrow with id:  {id} not found")
    {
    }
}
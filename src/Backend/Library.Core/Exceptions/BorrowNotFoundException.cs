namespace Library.Core.Exceptions;

public class BorrowNotFoundException : CustomException
{
    public BorrowNotFoundException(Guid? id = null)
        : base($"Borrow with id:  {id} not found")
    {
    }
}
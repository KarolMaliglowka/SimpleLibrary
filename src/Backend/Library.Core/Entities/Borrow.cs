namespace Library.Core.Entities;

public sealed class Borrow : BaseClass
{
    private Borrow()
    {
    }

    public Borrow(User user, Book book, DateTime borrowDate)
    {
        SetUser(user);
        SetBook(book);
        BorrowDate = borrowDate;
    }

    public Guid UserId { get; private set; }
    public User User { get; private set; }
    public Guid BookId { get; private set; }
    public Book Book { get; private set; }
    public DateTime BorrowDate { get; private set; }

    private void SetUser(User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        User = user;
        UserId = user.Id;
    }

    private void SetBook(Book book)
    {
        ArgumentNullException.ThrowIfNull(book);

        Book = book;
        BookId = book.Id;
    }
}
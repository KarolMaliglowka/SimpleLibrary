namespace Library.Core.Entities;

public class Borrow : BaseClass
{
    public Borrow()
    {
    }

    public Borrow(User user, Book book, DateTime borrowDate)
    {
        Id = Guid.NewGuid();
        User = user;
        Book = book;
        BorrowDate = borrowDate;
    }

    public Guid UserId { get; set; }
    public User User { get; set; }
    public Guid BookId { get; set; }
    public Book Book { get; set; }
    public DateTime BorrowDate { get; set; }
}
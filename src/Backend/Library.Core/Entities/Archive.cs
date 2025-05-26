namespace Library.Core.Entities;

public sealed class Archive
{
    // public Archive()
    // {
    // }
    //
    // public Archive(
    //     Guid bookId,
    //     string bookName,
    //     string bookAuthors,
    //     Guid userId,
    //     string userFullName,
    //     DateTime borrowDate,
    //     DateTime returnDate
    // )
    // {
    //     Id = Guid.NewGuid();
    //     BookId = bookId;
    //     BookName = bookName;
    //     BookAuthors = bookAuthors;
    //     UserId = userId;
    //     UserFullName = userFullName;
    //     BorrowDate = borrowDate;
    //     ReturnDate = returnDate;
    // }

    public Guid Id { get; set; }
    public Guid BookId { get; set; }
    public string BookName { get; set; }
    public string BookAuthors { get; set; }
    public Guid UserId { get; set; }
    public string UserFullName { get; set; }
    public DateTime BorrowDate { get; set; }
    public DateTime ReturnDate { get; set; }
}
namespace Library.Core.Entities;

public sealed class Archive
{
    public Guid Id { get; set; }
    public Guid BookId { get; set; }
    public string BookName { get; set; }
    public string BookAuthors { get; set; }
    public Guid UserId { get; set; }
    public string UserFullName { get; set; }
    public DateTime BorrowDate { get; set; }
    public DateTime ReturnDate { get; set; }
}
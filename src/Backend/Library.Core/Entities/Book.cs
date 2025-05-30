using Library.Core.ValueObjects;

namespace Library.Core.Entities;

public sealed class Book : BaseClass
{
    public Name Name { get; set; }
    public int PagesCount { get; set; }
    public string Description { get; set; }
    public Guid PublisherId { get; set; }
    public Publisher? Publisher { get; set; }
    public string ISBN { get; set; }
    public string YearOfRelease { get; set; }
    public Category? Category { get; set; }
    public Guid? CategoryId { get; set; }
    public ICollection<Author>? Authors { get; set; }
    public List<Borrow> Borrows { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsBorrowed { get; set; }
}
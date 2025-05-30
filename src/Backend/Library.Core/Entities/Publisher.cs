using Library.Core.ValueObjects;

namespace Library.Core.Entities;
public sealed class Publisher : BaseClass
{
    private readonly List<Book> _books = [];

    public Name Name { get; set; }
    public IEnumerable<Book> Books => _books.AsReadOnly();
}
namespace Library.Core.Entities;

public sealed class ArchiveBuilder
{
    private readonly Archive _archive = new()
    {
        Id = Guid.NewGuid()
    };

    public ArchiveBuilder SetBookId(Guid bookId)
    {
        _archive.Id = bookId;
        return this;
    }

    public ArchiveBuilder SetBookName(string bookName)
    {
        _archive.BookName = bookName;
        return this;
    }

    public ArchiveBuilder SetAuthors(string bookAuthors)
    {
        _archive.BookAuthors = bookAuthors;
        return this;
    }

    public ArchiveBuilder SetUserId(Guid userId)
    {
        _archive.UserId = userId;
        return this;
    }

    public ArchiveBuilder SetUserFullName(string userFullName)
    {
        _archive.UserFullName = userFullName;
        return this;
    }

    public ArchiveBuilder SetBorrowDate(DateTime borrowDate)
    {
        _archive.BorrowDate = borrowDate;
        return this;
    }

    public ArchiveBuilder SetReturnDate(DateTime returnDate)
    {
        _archive.ReturnDate = returnDate;
        return this;
    }

    public Archive Build()
    {
        return _archive;
    }
}
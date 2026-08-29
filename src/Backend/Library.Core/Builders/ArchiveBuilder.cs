using Library.Core.Entities;

namespace Library.Core.Builders;

public sealed class ArchiveBuilder
{
    private readonly Archive _archive = new()
    {
        Id = Guid.NewGuid()
    };

    public ArchiveBuilder SetBookId(Guid bookId)
    {
        _archive.BookId = bookId;
        return this;
    }

    /// <summary>
    /// Archive builder
    /// </summary>
    /// <param name="bookName"></param>
    /// <returns>object</returns>
    public ArchiveBuilder SetBookName(string bookName)
    {
        _archive.BookName = bookName;
        return this;
    }

    /// <summary>
    /// Archive builder
    /// </summary>
    /// <param name="bookAuthors"></param>
    /// <returns>object</returns>
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
        if (_archive.BookId == Guid.Empty)
            throw new InvalidOperationException("BookId is required");

        if (_archive.UserId == Guid.Empty)
            throw new InvalidOperationException("UserId is required");

        return _archive;
    }
}
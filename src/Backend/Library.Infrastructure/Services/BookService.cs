using Library.Core.Entities;
using Library.Core.Repositories;
using Library.Infrastructure.DTO;
using Library.Infrastructure.Exceptions;
using Library.Infrastructure.Factories;
using Microsoft.Extensions.Logging;

namespace Library.Infrastructure.Services;

public interface IBookService
{
    Task CreateBookAsync(BookDto book);
    Task<List<BookDto>> GetAllBooksAsync();
    Task<BookDto> GetBookByIdAsync(Guid bookId);
    Task<BookDto> GetBookByNameAsync(string name);
    Task CreateBooksAsync(List<BookDto> book);
    Task UpdateBook(BookDto book);
    Task<List<BookDto>> GetBooksByAuthorAsync(string authorSurname, string? authorName = null);
    Task<List<BookDto>> GetBooksByCategoryAsync(string category);
    Task<List<BookDto>> GetBooksByPublisherAsync(string publisher);
    Task SetBookAsBorrowed(Guid bookId, bool isAvailable);
    Task<List<BorrowDto>> GetBorrowingBooksWithUsers();
}

public class BookService(
    IBookRepository bookRepository,
    IPublisherRepository publisherRepository,
    IAuthorRepository authorRepository,
    IAuthorReadRepository authorReadRepository,
    ICategoryService categoryService,
    ICategoryRepository categoryRepository,
    ILogger<BookService> _logger
) : IBookService
{
    public async Task CreateBookAsync(BookDto book)
    {
        var publisher = await publisherRepository.GetPublisherByNameAsync(book.Publisher.Name);
        if (publisher == null)
        {
            publisher = PublisherFactory.CreatePublisher(book.Publisher);
            await publisherRepository.AddPublisherAsync(publisher);
        }

        var category = await categoryRepository.GetCategoryByNameAsync(book.Category.Name);
        if (category is null)
        {
            category = new Category(book.Category.Name);
            await categoryRepository.AddCategoryAsync(category);
        }

        var authors = new List<Author>();
        var authorsToImport = new List<Author>();
        
        var listOfAllAuthors = await authorReadRepository.GetAuthorsAsync();
        
        foreach (var authorName in book.Authors)
        {
            var author = listOfAllAuthors.FirstOrDefault(a => 
                a.Name == authorName.Name && a.Surname == authorName.Surname);

            if (author is null)
            {
                author = new Author(authorName.Name, authorName.Surname);
                authorsToImport.Add(author);
                listOfAllAuthors.Add(author); // ważne
            }

            authors.Add(author);
        }

        await authorRepository.AddAuthorsAsync(authorsToImport);
        
        var newBook = BookFactory
            .BuildBook(book, authors, publisher, category);
        await bookRepository
            .AddBookAsync(newBook);
    }

    public async Task<List<BookDto>> GetAllBooksAsync()
    {
        var booksList = await bookRepository.GetAllAsync();
        return booksList.Select(x => new BookDto()
            {
                Id = x.Id,
                Name = x.Name,
                PagesCount = x.PagesCount,
                Description = x.Description,
                Publisher = new PublisherDto
                {
                    Name = x.Publisher.Name,
                    Id = x.Publisher.Id
                },
                Isbn = x.ISBN,
                YearOfRelease = x.YearOfRelease,
                Category = new CategoryDto
                {
                    Name = x.Category.Name,
                    Id = x.Category.Id
                },
                Authors = x.Authors?.Select(a => new AuthorDto
                    {
                        Name = a.Name ?? "",
                        Surname = a.Surname ?? "",
                        Id = a.Id
                    }
                ).ToList(),
                IsAvailable = x.IsAvailable
            })
            .OrderBy(x => x.Name)
            .ToList();
    }

    public async Task<BookDto> GetBookByIdAsync(Guid bookId)
    {
        var book = await bookRepository.GetBookByIdAsync(bookId);
        return book is null ? throw new BookNotFoundException(bookId.ToString()) :
            MapBookToDto(book);
    }

    public async Task<BookDto> GetBookByNameAsync(string name)
    {
        var book = await bookRepository.GetBookByNameAsync(name);
        return book is null ? throw new BookNotFoundException(name) :
            MapBookToDto(book);
    }

    public async Task CreateBooksAsync(List<BookDto> books)
    {
        var categoryList = books.Select(x => x.Category.Name)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        var categoryExistInSystem = await categoryService.GetCategoriesAsync();
        var categoriesToImport = categoryList
            .Where(x => !categoryExistInSystem.Any(y =>
                y.Name!.Equals(x, StringComparison.CurrentCultureIgnoreCase)))
            .Select(x => new Category(x!))
            .ToList();
        if (categoriesToImport.Count != 0)
        {
            await categoryRepository.AddCategoriesAsync(categoriesToImport);
        }

        var publishersList = books
            .Select(x => x.Publisher.Name)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        var publishersExistInSystem = await publisherRepository.GetPublishersAsync();
        var publishersToImport = publishersList
            .Where(x => !publishersExistInSystem
                .Any(y =>
                    y.Name.Value.Equals(x, StringComparison.CurrentCultureIgnoreCase)))
            .Select(x => PublisherFactory.CreatePublisher(new PublisherDto
            {
                Name = x
            }))
            .ToList();
        if (publishersToImport.Count != 0)
        {
            await publisherRepository.AddPublishersAsync(publishersToImport);
        }

        var authorsList = books.SelectMany(x => x.Authors)
            .Distinct()
            .ToList();
        var authorsExistInSystem = await authorReadRepository.GetAuthorsAsync();
        var authorsToImport = authorsList
            .Where(x => !authorsExistInSystem.Any(y =>
                y.Name!.Value.Equals(x.Name, StringComparison.CurrentCultureIgnoreCase) &&
                y.Surname!.ToLower() == x.Surname.ToLower()))
            .Select(x => new Author(x.Name, x.Surname)).Distinct().ToList();
        if (authorsToImport.Count != 0)
        {
            await authorRepository.AddAuthorsAsync(authorsToImport);
        }

        var booksListToImport = new List<Book>();
        
        var listOfAllPublishers = await publisherRepository.GetPublishersAsync();
        var listOfAllAuthors = await authorReadRepository.GetAuthorsAsync();
        
        foreach (var book in books)
        {
            var publisher = listOfAllPublishers.FirstOrDefault(x => x.Name == book.Publisher.Name);
            
            var authors = book.Authors
                .Select(bookAuthor =>
                    listOfAllAuthors.FirstOrDefault(x =>
                        x.Surname == bookAuthor.Surname && x.Name == bookAuthor.Name)
                    ?? throw new AuthorNotFoundException($"{bookAuthor.Name} {bookAuthor.Surname}")
                )
                .ToList();

            var category = await categoryRepository.GetCategoryByNameAsync(book.Category?.Name);

            var newBook = BookFactory
                .BuildBook(book, authors, publisher, category);

            booksListToImport.Add(newBook);
        }

        await bookRepository.AddBooksAsync(booksListToImport);
    }

    public async Task UpdateBook(BookDto bookDto)
    {
        var publisher = await publisherRepository.GetPublisherByIdAsync(bookDto.Publisher.Id);
        if (publisher == null)
        {
            publisher = PublisherFactory.CreatePublisher(new PublisherDto
            {
                Name = bookDto.Publisher.Name
            });
            await publisherRepository.AddPublisherAsync(publisher);
        }

        var category = await categoryRepository.GetCategoryByNameAsync(bookDto.Category!.Name);
        if (category is null)
        {
            category = new Category(bookDto.Category.Name);
            await categoryRepository.AddCategoryAsync(category);
        }

        var authors = new List<Author>();
        foreach (var authorName in bookDto.Authors)
        {
            var author = await authorReadRepository.GetAuthorAsync(authorName.Surname, authorName.Name);
            if (author is null)
            {
                author = new Author(authorName.Name, authorName.Surname);
                await authorRepository.AddAuthorAsync(author);
            }

            authors.Add(author);
        }

        var book = await bookRepository.GetBookByIdAsync(bookDto.Id);
        if (book is null)
        {
            _logger.LogError("Book id: {book} not found", bookDto.Id);
            throw new BookNotFoundException(bookDto.Id.ToString());
        }

        var updatedBook = BookFactory.BuildBook(bookDto, authors, publisher, category, book);
        await bookRepository.UpdateBook(updatedBook);
    }

    public async Task<List<BookDto>> GetBooksByAuthorAsync(string authorSurname, string? authorName = null)
    {
        var author = await authorReadRepository.GetAuthorAsync(authorSurname, authorName);
        if (author is null)
        {
            var notExistAuthor = authorName is null
                ? authorSurname
                : $"{authorSurname} {authorName}";
            
            _logger.LogError("Author {author} not found", notExistAuthor);
            throw new AuthorNotFoundException(notExistAuthor);
        }

        var booksList = await bookRepository.GetAllAsync();
        return booksList
            .Where(x => x.Authors != null && x.Authors.Any(a =>
                a.Name == author?.Name && a.Surname == author?.Surname))
            .Select(MapBookToDto)
            .ToList();
    }
    
    public async Task<List<BookDto>> GetBooksByCategoryAsync(string category)
    {
        var categoryInSystem = await categoryRepository.GetCategoryByNameAsync(category);
        if (categoryInSystem is null)
        {
            _logger.LogError("Category {name} not found",  category);
            throw new CategoryNotFoundException(category);
        }

        var booksList = await bookRepository.GetAllAsync();
        return booksList
            .Where(x => string.Equals(
                x.Category?.Name.Value,
                categoryInSystem.Name.Value,
                StringComparison.CurrentCultureIgnoreCase))
            .Select(MapBookToDto)
            .ToList();
    }

    public async Task<List<BookDto>> GetBooksByPublisherAsync(string publisher)
    {
        var publisherInSystem = await publisherRepository.GetPublisherByNameAsync(publisher);
        if (publisherInSystem is null)
        {
            _logger.LogError("Publisher {name} not found", publisher);
            throw new PublisherNotFoundException(publisher);
        }

        var booksList = await bookRepository.GetAllAsync();
        return booksList
            .Where(x => string.Equals(x.Publisher.Name, publisherInSystem.Name,
                StringComparison.CurrentCultureIgnoreCase))
            .Select(MapBookToDto)
            .ToList();
    }

    public async Task SetBookAsBorrowed(Guid bookId, bool isAvailable)
    {
        var book = await bookRepository.GetBookByIdAsync(bookId);
        if (book == null)
        {
            _logger.LogError("Book id: {id} not found", bookId);
            throw new BookNotFoundException(bookId.ToString());
        }

        book.IsAvailable = isAvailable;
        await bookRepository.UpdateBook(book);
    }

    public async Task<List<BorrowDto>> GetBorrowingBooksWithUsers()
    {
        var booksList = await bookRepository.GetBorrowBooksWithUsersAsync();

        return booksList.Select(x => new BorrowDto()
            {
                Id = x.Id,
                BookId = x.Id,
                BookName = x.Book.Name,
                BookAuthors = x.Book.Authors?.Select(a => new AuthorDto
                    {
                        Name = a.Name ?? "",
                        Surname = a.Surname ?? "",
                        Id = a.Id
                    }
                ).ToList(),
                UserId = x.User.Id,
                UserFullName = $"{x.User.Surname} {x.User.Name}",
                BorrowDate = x.BorrowDate
            })
            .OrderBy(x => x.BookName)
            .ToList();
    }
    
    private static BookDto MapBookToDto(Book book)
    {
        return new BookDto
        {
            Id = book.Id,
            Name = book.Name,
            PagesCount = book.PagesCount,
            Description = book.Description,
            Publisher = new PublisherDto { Name = book.Publisher!.Name },
            Isbn = book.ISBN,
            YearOfRelease = book.YearOfRelease,
            Category = new CategoryDto { Name = book.Category!.Name },
            Authors = book.Authors?.Select(a => new AuthorDto
            {
                Name = a.Name ?? "",
                Surname = a.Surname ?? ""
            }).ToList(),
            IsAvailable = book.IsAvailable
        };
    }
}
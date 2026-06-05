using Library.Application.DTO;
using Library.Application.Factories;
using Library.Core;
using Library.Core.Entities;
using Library.Core.Repositories;
using Library.Infrastructure.Exceptions;
using Microsoft.Extensions.Logging;

namespace Library.Application.Services;

/// <summary>
/// Defines operations for managing books in the library system.
/// </summary>
public interface IBookService
{
    /// <summary>
    /// Creates a new book in the system.
    /// </summary>
    /// <param name="book">Data transfer object containing book details.</param>
    Task CreateBookAsync(BookDto book);

    /// <summary>
    /// Retrieves all books available in the system.
    /// </summary>
    /// <returns>A list of books mapped to <see cref="BookDto"/>.</returns>
    Task<List<BookDto>> GetAllBooksAsync();

    /// <summary>
    /// Retrieves a book by its unique identifier.
    /// </summary>
    /// <param name="bookId">Unique identifier of the book.</param>
    /// <returns>The book represented as <see cref="BookDto"/>.</returns>
    /// <exception cref="BookNotFoundException">Thrown when the book is not found.</exception>
    Task<BookDto> GetBookByIdAsync(Guid bookId);

    /// <summary>
    /// Retrieves a book by its name.
    /// </summary>
    /// <param name="name">Name of the book.</param>
    /// <returns>The book represented as <see cref="BookDto"/>.</returns>
    /// <exception cref="BookNotFoundException">Thrown when the book is not found.</exception>
    Task<BookDto> GetBookByNameAsync(string name);

    /// <summary>
    /// Creates multiple books in the system.
    /// </summary>
    /// <param name="book">List of books to be added.</param>
    Task CreateBooksAsync(List<BookDto> book);

    /// <summary>
    /// Updates an existing book in the system.
    /// </summary>
    /// <param name="book">Book data used to update the existing record.</param>
    Task UpdateBook(BookDto book);

    /// <summary>
    /// Retrieves books written by a specific author.
    /// </summary>
    /// <param name="authorSurname">Author's surname.</param>
    /// <param name="authorName">Optional author's first name.</param>
    /// <returns>List of books written by the given author.</returns>
    Task<List<BookDto>> GetBooksByAuthorAsync(string authorSurname, string? authorName = null);

    /// <summary>
    /// Retrieves books belonging to a given category.
    /// </summary>
    /// <param name="category">Category name.</param>
    /// <returns>List of books in the given category.</returns>
    Task<List<BookDto>> GetBooksByCategoryAsync(string category);

    /// <summary>
    /// Retrieves books published by a specific publisher.
    /// </summary>
    /// <param name="publisher">Publisher name.</param>
    /// <returns>List of books published by the given publisher.</returns>
    Task<List<BookDto>> GetBooksByPublisherAsync(string publisher);

    /// <summary>
    /// Sets the availability status of a book (borrowed or returned).
    /// </summary>
    /// <param name="bookId">Unique identifier of the book.</param>
    /// <param name="isAvailable">Availability status of the book.</param>
    Task SetBookAsBorrowed(Guid bookId, bool isAvailable);

    /// <summary>
    /// Retrieves all borrowed books along with information about the users who borrowed them.
    /// </summary>
    /// <returns>List of borrowed books with user details.</returns>
    Task<List<BorrowDto>> GetBorrowingBooksWithUsers();

    Task<List<Dictionary<Guid, string>>> GetBooksDictionaryAsync();
}

public class BookService(
    IBookRepository bookRepository,
    IPublisherRepository publisherRepository,
    IAuthorRepository authorRepository,
    IAuthorReadRepository authorReadRepository,
    ICategoryService categoryService,
    ICategoryRepository categoryRepository,
    ILogger<BookService> logger,
    IUnitOfWork unitOfWork
) : IBookService
{
    /// <summary>
    /// Creates a new book with related entities such as publisher, category and authors.
    /// If related entities do not exist, they are created automatically.
    /// </summary>
    /// <param name="book">Book data transfer object containing book information.</param>
    public async Task CreateBookAsync(BookDto book)
    {
        var publisher = await publisherRepository.GetPublisherByNameAsync(book.Publisher.Name);
        if (publisher == null)
        {
            if (publisher != null)
            {
                var newPublisher = new Publisher(publisher.Name);
                await publisherRepository.AddPublisherAsync(newPublisher);
            }

            await unitOfWork.SaveChangesAsync();
        }

        var category = await categoryRepository.GetCategoryByNameAsync(book.Category.Name);
        if (category is null)
        {
            category = new Category(book.Category.Name);
            await categoryRepository.AddCategoryAsync(category);
            await unitOfWork.SaveChangesAsync();
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
                listOfAllAuthors.Add(author);
            }

            authors.Add(author);
        }

        authorRepository.AddAuthors(authorsToImport);

        var newBook = BookFactory
            .BuildBook(book, authors, publisher, category);
        await bookRepository
            .AddBookAsync(newBook);
        await unitOfWork.SaveChangesAsync();
    }

    /// <summary>
    /// Retrieves all books stored in the system.
    /// </summary>
    /// <returns>A list of books mapped to <see cref="BookDto"/> objects.</returns>
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

    /// <summary>
    /// Retrieves a book by its identifier.
    /// </summary>
    /// <param name="bookId">Unique identifier of the book.</param>
    /// <returns>The book mapped to <see cref="BookDto"/>.</returns>
    /// <exception cref="BookNotFoundException">Thrown when the book cannot be found.</exception>
    public async Task<BookDto> GetBookByIdAsync(Guid bookId)
    {
        var book = await bookRepository.GetBookByIdAsync(bookId);
        return book is null ? throw new BookNotFoundException(bookId.ToString()) : MapBookToDto(book);
    }

    /// <summary>
    /// Retrieves a book by name.
    /// </summary>
    /// <param name="name">Name of the book.</param>
    /// <returns>The book mapped to <see cref="BookDto"/>.</returns>
    /// <exception cref="BookNotFoundException">Thrown when the book cannot be found.</exception>
    public async Task<BookDto> GetBookByNameAsync(string name)
    {
        var book = await bookRepository.GetBookByNameAsync(name);
        return book is null ? throw new BookNotFoundException(name) : MapBookToDto(book);
    }

    /// <summary>
    /// Imports multiple books into the system.
    /// Missing related entities such as categories, publishers and authors
    /// are created automatically before books are inserted.
    /// </summary>
    /// <param name="books">Collection of books to import.</param>
    public async Task CreateBooksAsync(List<BookDto> books)
    {
        var categoryList = books.Select(x => x?.Category.Name)
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
            .Select(x => new Publisher(x ))
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
            authorRepository.AddAuthors(authorsToImport);
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
        await unitOfWork.SaveChangesAsync();
    }

    /// <summary>
    /// Updates an existing book and its related entities.
    /// </summary>
    /// <param name="bookDto">Updated book data.</param>
    /// <exception cref="BookNotFoundException">Thrown when the book does not exist.</exception>
    public async Task UpdateBook(BookDto bookDto)
    {
        var publisher = await publisherRepository.GetPublisherByIdAsync(bookDto.Publisher.Id);
        if (publisher == null)
        {
            publisher = new Publisher(
                bookDto.Publisher.Name
            );
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
            logger.LogError("Book id: {book} not found", bookDto.Id);
            throw new BookNotFoundException(bookDto.Id.ToString());
        }

        var updatedBook = BookFactory.BuildBook(bookDto, authors, publisher, category, book);
        bookRepository.UpdateBook(updatedBook);
        await unitOfWork.SaveChangesAsync();
    }

    /// <summary>
    /// Retrieves all books written by the specified author.
    /// </summary>
    /// <param name="authorSurname">Author's surname.</param>
    /// <param name="authorName">Optional author's first name.</param>
    /// <returns>List of books written by the author.</returns>
    /// <exception cref="AuthorNotFoundException">Thrown when the author does not exist.</exception>
    public async Task<List<BookDto>> GetBooksByAuthorAsync(string authorSurname, string? authorName = null)
    {
        var author = await authorReadRepository.GetAuthorAsync(authorSurname, authorName);
        if (author is null)
        {
            var notExistAuthor = authorName is null
                ? authorSurname
                : $"{authorSurname} {authorName}";

            logger.LogError("Author {author} not found", notExistAuthor);
            throw new AuthorNotFoundException(notExistAuthor);
        }

        var booksList = await bookRepository.GetAllAsync();
        return booksList
            .Where(x => x.Authors != null && x.Authors.Any(a =>
                a.Name == author?.Name && a.Surname == author?.Surname))
            .Select(MapBookToDto)
            .ToList();
    }

    /// <summary>
    /// Retrieves all books belonging to the specified category.
    /// </summary>
    /// <param name="category">Category name.</param>
    /// <returns>List of books in the category.</returns>
    /// <exception cref="CategoryNotFoundException">Thrown when the category does not exist.</exception>
    public async Task<List<BookDto>> GetBooksByCategoryAsync(string category)
    {
        var categoryInSystem = await categoryRepository.GetCategoryByNameAsync(category);
        if (categoryInSystem is null)
        {
            logger.LogError("Category {name} not found", category);
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

    /// <summary>
    /// Retrieves all books published by the specified publisher.
    /// </summary>
    /// <param name="publisher">Publisher name.</param>
    /// <returns>List of books from the publisher.</returns>
    /// <exception cref="PublisherNotFoundException">Thrown when the publisher does not exist.</exception>
    public async Task<List<BookDto>> GetBooksByPublisherAsync(string publisher)
    {
        var publisherInSystem = await publisherRepository.GetPublisherByNameAsync(publisher);
        if (publisherInSystem is null)
        {
            logger.LogError("Publisher {name} not found", publisher);
            throw new PublisherNotFoundException(publisher);
        }

        var booksList = await bookRepository.GetAllAsync();
        return booksList
            .Where(x => string.Equals(x.Publisher.Name, publisherInSystem.Name,
                StringComparison.CurrentCultureIgnoreCase))
            .Select(MapBookToDto)
            .ToList();
    }

    /// <summary>
    /// Updates the availability status of a book.
    /// </summary>
    /// <param name="bookId">Unique identifier of the book.</param>
    /// <param name="isAvailable">Indicates whether the book is available.</param>
    /// <exception cref="BookNotFoundException">Thrown when the book does not exist.</exception>
    public async Task SetBookAsBorrowed(Guid bookId, bool isAvailable)
    {
        var book = await bookRepository.GetBookByIdAsync(bookId);
        if (book == null)
        {
            logger.LogError("Book id: {id} not found", bookId);
            throw new BookNotFoundException(bookId.ToString());
        }

        book.IsAvailable = isAvailable;
        bookRepository.UpdateBook(book);
        await unitOfWork.SaveChangesAsync();
    }

    /// <summary>
    /// Retrieves information about all borrowed books together with the users who borrowed them.
    /// </summary>
    /// <returns>List of borrow records with book and user information.</returns>
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

    /// <summary>
    /// Maps a <see cref="Book"/> entity to a <see cref="BookDto"/>.
    /// </summary>
    /// <param name="book">Book entity.</param>
    /// <returns>Mapped data transfer object.</returns>
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
    
    public async Task<List<Dictionary<Guid, string>>> GetBooksDictionaryAsync()
    {
        var booksList = await bookRepository.GetAllAsync();
        
        return booksList
            .OrderBy(x => x.Name.Value)
            .Select(x => new Dictionary<Guid, string>
            {
                [x.Id] = $"{x.Name.Value} - {string.Join(", ", x.Authors!.Select(a => a.FullName))}"
            })
            .ToList()
            ;
    }
}
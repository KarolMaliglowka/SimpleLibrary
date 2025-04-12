using Library.Core.Entities;
using Library.Core.Repositories;
using Library.Infrastructure.DTO;

namespace Library.Infrastructure.Services;

public interface IBookService
{
    Task CreateBookAsync(BookDto book);
    Task<List<BookDto>> GetAllBooksAsync();
    Task<BookDto> GetBookByIdAsync(Guid bookId);
    Task<BookDto> GetBookByNameAsync(string name);
    Task CreateBooksAsync(List<BookDto> book);
    Task UpdateBook(BookDto book);
    Task<List<BookDto>> GetBooksByAuthorAsync(string authorSurname, string authorName = null);
    Task<List<BookDto>> GetBooksByCategoryAsync(string category);
    Task<List<BookDto>> GetBooksByPublisherAsync(string publisher);
    Task SetBookAsBorrowed(Guid bookId, bool isBorrowed);
}

public class BookService(
    IBookRepository bookRepository,
    IPublisherRepository publisherRepository,
    IAuthorRepository authorRepository,
    ICategoryService categoryService,
    ICategoryRepository categoryRepository
) : IBookService
{
    public async Task CreateBookAsync(BookDto book)
    {
        var publisher = await publisherRepository.GetPublisherByNameAsync(book.Publisher.Name);
        if (publisher == null)
        {
            publisher = new Publisher(book.Publisher.Name);
            await publisherRepository.AddPublisherAsync(publisher);
        }

        var category = await categoryRepository.GetCategoryByNameAsync(book.Category.Name);
        if (category is null)
        {
            category = new Category(book.Category.Name);
            await categoryRepository.AddCategoryAsync(category);
        }

        var authors = new List<Author>();
        foreach (var authorName in book.Authors)
        {
            var author = await authorRepository.GetAuthorAsync(authorName.Surname, authorName.Name);
            if (author is null)
            {
                author = new Author(authorName.Name, authorName.Surname);
                await authorRepository.AddAuthorAsync(author);
            }

            authors.Add(author);
        }

        var newBook = new Book
        (
            book.Name,
            authors,
            book.PagesCount,
            book.Description,
            book.Isbn,
            book.YearOfRelease,
            publisher,
            category
        );
        await bookRepository.AddBookAsync(newBook);
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
                Name = x.Publisher?.Name,
                Id = x.Publisher.Id
            },
            Isbn = x.ISBN,
            YearOfRelease = x.YearOfRelease,
            Category = new CategoryDto
            {
                Name = x.Category?.Name,
                Id = x.Category.Id
            },
            Authors = x.Authors?.Select(a => new AuthorDto
                {
                    Name = a.Name ?? "",
                    Surname = a.Surname ?? "",
                    Id = x.Id
                }
            ).ToList(),
            IsBorrowed = x.IsBorrowed
        }).ToList();
    }

    public async Task<BookDto> GetBookByIdAsync(Guid bookId)
    {
        var book = await bookRepository.GetBookByIdAsync(bookId);
        if (book is null)
        {
            throw new Exception("Book not found.");
        }

        var authorsNames = book.Authors?
            .Select(s => new AuthorDto
            {
                Name = s.Name ?? "",
                Surname = s.Surname ?? "",
            })
            .ToList();
        return new BookDto()
        {
            Id = book.Id,
            Name = book.Name,
            PagesCount = book.PagesCount,
            Description = book.Description,
            Publisher = new PublisherDto { Name = book.Publisher?.Name },
            Isbn = book.ISBN,
            YearOfRelease = book.YearOfRelease,
            Category = new CategoryDto { Name = book.Category?.Name },
            Authors = authorsNames,
            IsBorrowed = book.IsBorrowed
        };
    }

    public async Task<BookDto> GetBookByNameAsync(string name)
    {
        var book = await bookRepository.GetBookByNameAsync(name);
        if (book is null)
        {
            throw new Exception("Book not found.");
        }

        var authorsNames = book.Authors?
            .Select(s => new AuthorDto
            {
                Name = s.Name ?? "",
                Surname = s.Surname ?? "",
            })
            .ToList();
        return new BookDto()
        {
            Id = book.Id,
            Name = book.Name,
            PagesCount = book.PagesCount,
            Description = book.Description,
            Publisher = new PublisherDto { Name = book.Publisher.Name },
            Isbn = book.ISBN,
            YearOfRelease = book.YearOfRelease,
            Category = new CategoryDto { Name = book.Category?.Name },
            Authors = authorsNames,
            IsBorrowed = book.IsBorrowed
        };
    }

    public async Task CreateBooksAsync(List<BookDto> books)
    {
        var categoryList = books.Select(x => x.Category.Name)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        var categoryExistInSystem = await categoryService.GetCategoriesAsync();
        var categoriesToImport = categoryList
            .Where(x => !categoryExistInSystem.Any(y =>
                y.Name.ToLower() == x.ToLower()))
            .Select(x => new Category(x)).ToList();
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
            .Where(x => !publishersExistInSystem.Any(y =>
                y.Name.Value.ToLower() == x.ToLower()))
            .Select(x => new Publisher(x)).ToList();
        if (publishersToImport.Count != 0)
        {
            await publisherRepository.AddPublishersAsync(publishersToImport);
        }

        var authorsList = books.SelectMany(x => x.Authors)
            .Distinct()
            .ToList();
        var authorsExistInSystem = await authorRepository.GetAuthorsAsync();
        var authorsToImport = authorsList
            .Where(x => !authorsExistInSystem.Any(y =>
                y.Name.Value.ToLower() == x.Name.ToLower() &&
                y.Surname.ToLower() == x.Surname.ToLower()))
            .Select(x => new Author(x.Name, x.Surname)).Distinct().ToList();
        if (authorsToImport.Count != 0)
        {
            await authorRepository.AddAuthorsAsync(authorsToImport);
        }

        var booksListToImport = new List<Book>();
        foreach (var book in books)
        {
            var getPublisher = await publisherRepository.GetPublisherByNameAsync(book.Publisher.Name);
            var getAuthors = new List<Author>();
            foreach (var authorName in book.Authors)
            {
                var author = await authorRepository.GetAuthorAsync(authorName.Surname, authorName.Name);
                if (author is null)
                {
                    throw new Exception($"Author {authorName} not found.");
                }

                getAuthors.Add(author);
            }

            var getCategory = await categoryRepository.GetCategoryByNameAsync(book.Category.Name);
            var newBook = new Book
            (
                book.Name,
                getAuthors,
                book.PagesCount,
                book.Description,
                book.Isbn,
                book.YearOfRelease,
                getPublisher,
                getCategory
            );
            booksListToImport.Add(newBook);
        }

        await bookRepository.AddBooksAsync(booksListToImport);
    }

    public async Task UpdateBook(BookDto bookDto)
    {
        var publisher = await publisherRepository.GetPublisherByNameAsync(bookDto.Publisher.Name);
        if (publisher == null)
        {
            publisher = new Publisher(bookDto.Publisher.Name);
            await publisherRepository.AddPublisherAsync(publisher);
        }

        var category = await categoryRepository.GetCategoryByNameAsync(bookDto.Category.Name);
        if (category is null)
        {
            category = new Category(bookDto.Category.Name);
            await categoryRepository.AddCategoryAsync(category);
        }

        var authors = new List<Author>();
        foreach (var authorName in bookDto.Authors)
        {
            var author = await authorRepository.GetAuthorAsync(authorName.Surname, authorName.Name);
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
            throw new Exception("Book not found.");
        }

        book.SetName(bookDto.Name);
        book.PagesCount = bookDto.PagesCount;
        book.Authors = authors;
        book.Description = bookDto.Description;
        book.ISBN = bookDto.Isbn;
        book.YearOfRelease = bookDto.YearOfRelease;
        book.Publisher = publisher;
        book.Category = category;
        book.UpdatedAt = DateTime.UtcNow;
        book.IsBorrowed = bookDto.IsBorrowed;
        await bookRepository.UpdateBook(book);
    }

    public async Task<List<BookDto>> GetBooksByAuthorAsync(string authorSurname, string authorName = null)
    {
        var listOfAuthor = await authorRepository.GetAuthorBySurnameAsync(authorSurname);
        if (listOfAuthor is null)
        {
            throw new Exception("Author not found.");
        }

        var author = listOfAuthor.FirstOrDefault();
        if (listOfAuthor.Count == 1)
        {
            author = listOfAuthor.FirstOrDefault();
        }
        else if (!string.IsNullOrWhiteSpace(authorName))
        {
            author = listOfAuthor.FirstOrDefault(x =>
                string.Equals(x.Name, authorName, StringComparison.CurrentCultureIgnoreCase));
            if (author is null)
            {
                throw new Exception("Author not found.");
            }
        }

        var booksList = await bookRepository.GetAllAsync();
        return booksList
            .Where(x => x.Authors.Any(a =>
                a.Name == author?.Name && a.Surname == author?.Surname))
            .Select(x => new BookDto()
            {
                Id = x.Id,
                Name = x.Name,
                PagesCount = x.PagesCount,
                Description = x.Description,
                Publisher = new PublisherDto { Name = x.Publisher?.Name },
                Isbn = x.ISBN,
                YearOfRelease = x.YearOfRelease,
                Category = new CategoryDto { Name = x.Category?.Name },
                Authors = x.Authors?.Select(a => new AuthorDto
                    {
                        Name = a.Name ?? "",
                        Surname = a.Surname ?? "",
                    }
                ).ToList(),
                IsBorrowed = x.IsBorrowed
            }).ToList();
    }

    public async Task<List<BookDto>> GetBooksByCategoryAsync(string category)
    {
        var categoryInSystem = await categoryRepository.GetCategoryByNameAsync(category);
        if (categoryInSystem is null)
        {
            throw new Exception("Category not found.");
        }

        var booksList = await bookRepository.GetAllAsync();
        return booksList
            .Where(x => 
                x.Category?.Name.Value.ToLower() == categoryInSystem.Name.Value.ToLower())
            .Select(x => new BookDto()
            {
                Id = x.Id,
                Name = x.Name,
                PagesCount = x.PagesCount,
                Description = x.Description,
                Publisher = new PublisherDto { Name = x.Publisher?.Name },
                Isbn = x.ISBN,
                YearOfRelease = x.YearOfRelease,
                Category = new CategoryDto { Name = x.Category?.Name },
                Authors = x.Authors?.Select(a => new AuthorDto
                    {
                        Name = a.Name ?? "",
                        Surname = a.Surname ?? "",
                    }
                ).ToList(),
                IsBorrowed = x.IsBorrowed
            }).ToList();
    }

    public async Task<List<BookDto>> GetBooksByPublisherAsync(string publisher)
    {
        var publisherInSystem = await publisherRepository.GetPublisherByNameAsync(publisher);
        if (publisherInSystem is null)
        {
            throw new Exception("Publisher not found.");
        }

        var booksList = await bookRepository.GetAllAsync();
        return booksList
            .Where(x => string.Equals(x.Publisher?.Name, publisherInSystem.Name,
                StringComparison.CurrentCultureIgnoreCase))
            .Select(x => new BookDto()
            {
                Id = x.Id,
                Name = x.Name,
                PagesCount = x.PagesCount,
                Description = x.Description,
                Publisher = new PublisherDto { Name = x.Publisher?.Name },
                Isbn = x.ISBN,
                YearOfRelease = x.YearOfRelease,
                Category = new CategoryDto { Name = x.Category?.Name },
                Authors = x.Authors?.Select(a => new AuthorDto
                    {
                        Name = a.Name ?? "",
                        Surname = a.Surname ?? "",
                    }
                ).ToList(),
                IsBorrowed = x.IsBorrowed
            }).ToList();
    }

    public async Task SetBookAsBorrowed(Guid bookId, bool isBorrowed)
    {
        var book = await bookRepository.GetBookByIdAsync(bookId);
        if (book == null)
        {
            throw new NullReferenceException("Book not found");
        }

        book.IsBorrowed = isBorrowed;
        await bookRepository.UpdateBook(book);
    }
}
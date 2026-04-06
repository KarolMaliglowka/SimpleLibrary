using Library.Core.Entities;
using Library.Core.Repositories;
using Library.Infrastructure.DTO;
using Library.Infrastructure.Exceptions;
using Library.Infrastructure.Factories;

namespace Library.Infrastructure.Services;

public interface IBookService
{
    Task CreateBookAsync(BookDto book);
    Task<List<BookDto>> GetAllBooksAsync();
    Task<BookDto> GetBookByIdAsync(Guid bookId);
    Task<BookDto> GetBookByNameAsync(string name);
    Task CreateBooksAsync(List<BookDto> book);
    Task UpdateBook(BookDto book);
    Task<List<BookDto>> GetBooksByAuthorAsync(string authorSurname, string authorName = null!);
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
    ICategoryRepository categoryRepository
) : IBookService
{
    public async Task CreateBookAsync(BookDto book)
    {
        var publisher = await publisherRepository.GetPublisherByNameAsync(book.Publisher!.Name);
        if (publisher == null)
        {
            publisher = PublisherFactory.CreatePublisher(book.Publisher);
            await publisherRepository.AddPublisherAsync(publisher);
        }

        var category = await categoryRepository.GetCategoryByNameAsync(book.Category!.Name);
        if (category is null)
        {
            category = new Category(book.Category.Name);
            await categoryRepository.AddCategoryAsync(category);
        }

        var authors = new List<Author>();
        foreach (var authorName in book.Authors!)
        {
            var author = await authorReadRepository
                .GetAuthorAsync(authorName.Surname, authorName.Name);
            if (author is null)
            {
                author = new Author(authorName.Name, authorName.Surname);
                await authorRepository
                    .AddAuthorAsync(author);
            }

            authors.Add(author);
        }

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
                    Name = x.Publisher!.Name,
                    Id = x.Publisher!.Id
                },
                Isbn = x.ISBN,
                YearOfRelease = x.YearOfRelease,
                Category = new CategoryDto
                {
                    Name = x.Category!.Name,
                    Id = x.Category.Id
                },
                Authors = x.Authors?.Select(a => new AuthorDto
                    {
                        Name = a.Name ?? "",
                        Surname = a.Surname ?? "",
                        Id = x.Id
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
            Publisher = new PublisherDto { Name = book.Publisher!.Name },
            Isbn = book.ISBN,
            YearOfRelease = book.YearOfRelease,
            Category = new CategoryDto { Name = book.Category!.Name },
            Authors = authorsNames,
            IsAvailable = book.IsAvailable
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
            Publisher = new PublisherDto { Name = book.Publisher!.Name },
            Isbn = book.ISBN,
            YearOfRelease = book.YearOfRelease,
            Category = new CategoryDto { Name = book.Category!.Name },
            Authors = authorsNames,
            IsAvailable = book.IsAvailable
        };
    }

    public async Task CreateBooksAsync(List<BookDto> books)
    {
        var categoryList = books.Select(x => x.Category!.Name)
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
            .Select(x => x.Publisher!.Name)
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

        var authorsList = books.SelectMany(x => x.Authors!)
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
        foreach (var book in books)
        {
            var publisher = await publisherRepository.GetPublisherByNameAsync(book.Publisher!.Name);
            var authors = new List<Author>();
            foreach (var authorName in book.Authors!)
            {
                var author = await authorReadRepository.GetAuthorAsync(authorName.Surname, authorName.Name);
                if (author is null)
                {
                    throw new Exception($"Author {authorName} not found.");
                }

                authors.Add(author);
            }

            var category = await categoryRepository.GetCategoryByNameAsync(book.Category!.Name);

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
        foreach (var authorName in bookDto.Authors!)
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
            throw new Exception("Book not found.");
        }

        var updatedBook = BookFactory.BuildBook(bookDto, authors, publisher, category, book);
        await bookRepository.UpdateBook(updatedBook);
    }

    
    //zmiana na jednego autora - tylko - i wyszukiwanie książek 
    public async Task<List<BookDto>> GetBooksByAuthorAsync(string authorSurname, string? authorName = null)
    {
        var listOfAuthor = await authorReadRepository.GetAuthorsListBySurnameAndName(authorSurname, authorName);
        if (listOfAuthor is null || listOfAuthor .Count == 0)
        {
            throw new AuthorNotFoundException();
        }

        var author = listOfAuthor.FirstOrDefault();
        if (listOfAuthor.Count == 1)
        {
            author = listOfAuthor.FirstOrDefault();
            //tutaj poprawic pobieranie z listy książek tylko tych z jednym autorem
        }
        else if (!string.IsNullOrWhiteSpace(authorName)) // zmienić
        {
            //tutaj pobieraie z listy ksiązek z autorami z listy 
            
            author = listOfAuthor.FirstOrDefault(x =>
                string.Equals(x.Name!, authorName, StringComparison.CurrentCultureIgnoreCase));
            // if (author is null)
            // {
            //     throw new AuthorNotFoundException();
            // }
        }

        var booksList = await bookRepository.GetAllAsync();
        return booksList
            .Where(x => x.Authors!.Any(a =>
                a.Name == author?.Name && a.Surname == author?.Surname))
            .Select(x => new BookDto()
            {
                Id = x.Id,
                Name = x.Name,
                PagesCount = x.PagesCount,
                Description = x.Description,
                Publisher = new PublisherDto { Name = x.Publisher!.Name },
                Isbn = x.ISBN,
                YearOfRelease = x.YearOfRelease,
                Category = new CategoryDto { Name = x.Category!.Name },
                Authors = x.Authors?.Select(a => new AuthorDto
                    {
                        Name = a.Name ?? "",
                        Surname = a.Surname ?? "",
                    }
                ).ToList(),
                IsAvailable = x.IsAvailable
            }).ToList();
    }
    
    //find books by author
    

    public async Task<List<BookDto>> GetBooksByCategoryAsync(string category)
    {
        var categoryInSystem = await categoryRepository.GetCategoryByNameAsync(category);
        if (categoryInSystem is null)
        {
            throw new CategoryNotFoundException();
        }

        var booksList = await bookRepository.GetAllAsync();
        return booksList
            .Where(x =>
                (x.Category?.Name.Value.ToLower()!).Equals(categoryInSystem.Name.Value,
                    StringComparison.CurrentCultureIgnoreCase))
            .Select(x => new BookDto()
            {
                Id = x.Id,
                Name = x.Name,
                PagesCount = x.PagesCount,
                Description = x.Description,
                Publisher = new PublisherDto { Name = x.Publisher!.Name },
                Isbn = x.ISBN,
                YearOfRelease = x.YearOfRelease,
                Category = new CategoryDto { Name = x.Category!.Name },
                Authors = x.Authors?.Select(a => new AuthorDto
                    {
                        Name = a.Name ?? "",
                        Surname = a.Surname ?? "",
                    }
                ).ToList(),
                IsAvailable = x.IsAvailable
            }).ToList();
    }

    public async Task<List<BookDto>> GetBooksByPublisherAsync(string publisher)
    {
        var publisherInSystem = await publisherRepository.GetPublisherByNameAsync(publisher);
        if (publisherInSystem is null)
        {
            throw new PublisherNotFoundException();
        }

        var booksList = await bookRepository.GetAllAsync();
        return booksList
            .Where(x => string.Equals(x.Publisher!.Name, publisherInSystem.Name,
                StringComparison.CurrentCultureIgnoreCase))
            .Select(x => new BookDto()
            {
                Id = x.Id,
                Name = x.Name,
                PagesCount = x.PagesCount,
                Description = x.Description,
                Publisher = new PublisherDto { Name = x.Publisher!.Name },
                Isbn = x.ISBN,
                YearOfRelease = x.YearOfRelease,
                Category = new CategoryDto { Name = x.Category!.Name },
                Authors = x.Authors?.Select(a => new AuthorDto
                    {
                        Name = a.Name ?? "",
                        Surname = a.Surname ?? "",
                    }
                ).ToList(),
                IsAvailable = x.IsAvailable
            }).ToList();
    }

    public async Task SetBookAsBorrowed(Guid bookId, bool isAvailable)
    {
        var book = await bookRepository.GetBookByIdAsync(bookId);
        if (book == null)
        {
            throw new BookNotFoundException();
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
}
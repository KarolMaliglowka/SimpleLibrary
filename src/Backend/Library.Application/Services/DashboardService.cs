using Library.Application.DTO;
using Library.Core.Repositories;

namespace Library.Application.Services;

public interface IDashboardService
{
    Task<DashboardDto> GetDashboardAsync();
}

public class DashboardService(
    IBookRepository bookRepository,
    IUserRepository userRepository
    ) : IDashboardService
{
    public async Task<DashboardDto> GetDashboardAsync()
    {
        var booksCount = bookRepository.QueryAsNoTracking()
            .Count(x => !x.IsDeleted);

        var userResult = await userRepository.GetUsersAsync();
        var usersCount =userResult.Count(x => !x.IsDeleted);

        var borrowedBooks = await bookRepository.GetBorrowBooksWithUsersAsync();
        var borrowedBooksCount = borrowedBooks.Count(x => !x.Book.IsAvailable);

        return new DashboardDto(
            booksCount,
            usersCount,
            borrowedBooksCount
        );
    }
}
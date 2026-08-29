namespace Library.Application.DTO;

public record DashboardDto(
    int BooksCount,
    int UsersCount,
    int BorrowedBooksCount
);
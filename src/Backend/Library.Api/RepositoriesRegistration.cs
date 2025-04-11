using Library.Core.Repositories;
using Library.Infrastructure.DAL.Repositories;

namespace Library.Api;

public static class RepositoriesRegistration
{
    public static void RegisterRepositories(this IServiceCollection services)
    {
        services.AddScoped<IAuthorRepository, AuthorRepository>();
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IPublisherRepository, PublisherRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IBorrowRepository, BorrowRepository>();
        services.AddScoped<IArchiveRepository, ArchiveRepository>();
    }
}
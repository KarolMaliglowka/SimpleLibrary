using Library.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Application;

public static class ServicesRegistration
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IBookService, BookService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IAuthorService, AuthorService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IPublisherService, PublisherService>();
        services.AddScoped<IBorrowService, BorrowService>();
    }
}
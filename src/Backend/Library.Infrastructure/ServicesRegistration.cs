using Library.Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Library.Infrastructure;

public static class ServicesRegistration
{
    public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<LibraryDbContext>(options =>
        {
            options
                .LogTo(Console.WriteLine, [DbLoggerCategory.Database.Command.Name], LogLevel.Information)
                //.UseLazyLoadingProxies()
                .EnableSensitiveDataLogging()
                //.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .UseSqlServer("Server=localhost\\SQLEXPRESS;Database=LibraryDb;User Id=sa;Password=kalifornia;TrustServerCertificate=True;");
                //.UseNpgsql(configuration.GetSection("ConnectionString:default").Value);


        });
    }
}

using System.Net.Http.Headers;
using Library.Application.Services;
using Library.Core;
using Library.Infrastructure.DAL;
using Library.Infrastructure.External;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Library.Infrastructure;

public static class ServicesRegistration
{
    public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<LibraryDbContext>(options =>
        {
            options
                .LogTo(Console.WriteLine, [DbLoggerCategory.Database.Command.Name], LogLevel.Information)
                .EnableSensitiveDataLogging()
                .UseNpgsql(configuration.GetSection("ConnectionString:default").Value);
        });
        
        services.AddScoped<IUnitOfWork>(
            sp => sp.GetRequiredService<LibraryDbContext>());
    }
    public static void AddAi(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHttpClient<IAiService,
            AiService>(client =>
        {
            client.BaseAddress =
                new Uri("https://api.openai.com/v1/");

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    configuration["OpenAI:ApiKey"]);
        });
    }
    
}
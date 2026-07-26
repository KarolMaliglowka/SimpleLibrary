using FluentValidation;
using Library.Api.EndPoints;
using Library.Api.GlobalHandlers;
using Library.Application;
using Library.Application.DTO;
using Library.Application.Validators;
using Library.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddScoped<IValidator<BookDto>, BookDtoValidator>();
builder.Services.AddScoped<IValidator<AuthorDto>, AuthorDtoValidator>();
builder.Services.AddScoped<IValidator<BorrowDto>, BorrowDtoValidator>();

builder.Services.RegisterRepositories();
builder.Services.RegisterServices();

builder.Services.AddOpenApi();
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        //if (builder.Environment.IsDevelopment())
        //{
            policy.SetIsOriginAllowed(origin =>
            {
                if (!Uri.TryCreate(origin, UriKind.Absolute, out var uri))
                    return false;

                return uri.Host == "localhost"
                       || uri.Host.StartsWith("192.168.77.");
            });
        //}
        //else
        //{
          //  policy.WithOrigins("https://library.twojadomena.pl");
        //}

        policy.AllowAnyHeader()
            .AllowAnyMethod();
    });
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseCors("AllowAngular");
app.MapAuthorEndpoints();
app.MapBookEndpoint();
app.MapCategoriesEndpoints();
app.MapUserEndpoints();
app.MapPublisherEndpoints();
app.MapBorrowEndpoints();

app.Run();




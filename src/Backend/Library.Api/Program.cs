using FluentValidation;
using Library.Api;
using Library.Api.EndPoints;
using Library.Infrastructure;
using Library.Infrastructure.DTO;
using Library.Infrastructure.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IValidator<BookDto>, BookDtoValidator>();
builder.Services.AddScoped<IValidator<AuthorDto>, AuthorDtoValidator>();
builder.Services.AddScoped<IValidator<BorrowDto>, BorrowDtoValidator>();

builder.Services.RegisterRepositories();
builder.Services.RegisterServices();

builder.Services.AddOpenApi();
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy => policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod());
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("AllowAngular");
app.MapAuthorEndpoints();
app.MapBookEndpoint();
app.MapCategoriesEndpoints();
app.MapUserEndpoints();
app.MapPublisherEndpoints();
app.MapBorrowEndpoints();

app.Run();




using FluentValidation;
using Library.Api;
using Library.Api.EndPoints;
using Library.Infrastructure;
using Library.Infrastructure.DTO;
using Library.Infrastructure.Validoators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IValidator<BookDto>, BookDtoValidator>();
builder.Services.RegisterRepositories();
builder.Services.RegisterServices();

builder.Services.AddOpenApi();
builder.Services.AddDatabase(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapAuthorEndpoints();
app.MapBookEndpoint();
app.MapCategoriesEndpoints();
app.MapUserEndpoints();
app.MapPublisherEndpoints();
app.MapBorrowEndpoints();

app.Run();


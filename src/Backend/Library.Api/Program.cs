using Library.Api;
using Library.Api.EndPoints;
using Library.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.RegisterRepositories();
builder.Services.RegisterServices();

builder.Services.AddOpenApi();
builder.Services.AddDatabase(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
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


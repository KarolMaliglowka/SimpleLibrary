using Library.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.DAL.Seed;

public static class DbSeeder
{
    public static async Task SeedDataAsync(LibraryDbContext context)
    {
        if (await context.Categories.AnyAsync())
            return;
        var category = new Category();
        category.SetCategory("Horror");
        context.Categories.Add(category);

        if (await context.Publishers.AnyAsync())
            return;

        var publisher = new Publisher();
        publisher.SetPublisher("Some Publisher");
        context.Publishers.Add(publisher);
        
        if (await context.Authors.AnyAsync())
            return;

        var author = new Author();
        author.SetName("John");
        author.SetSurname("Doe");
        context.Authors.Add(author);
        
        if (await context.Users.AnyAsync())
            return;

        var user = new User();
        user.Name = "John";
        user.Surname = "Smith";
        user.Email = "johnsmith@email.com";
        user.IsActive = true;

        context.Users.Add(user);
        await context.SaveChangesAsync();
        
        if (await context.Books.AnyAsync())
            return;
        var authors = new List<Author> { author };

        var book = new Book(
            "Some Book",
            authors,
            publisher,
            category,
            "1234",
            "Some Description",
            123,
            "2001",
            "1234"
        );
        
        context.Books.Add(book);

        await context.SaveChangesAsync();
    }
}
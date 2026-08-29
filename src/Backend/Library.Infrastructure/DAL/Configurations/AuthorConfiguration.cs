using Library.Core.Entities;
using Library.Core.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.DAL.Configurations;

public class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name)
            .HasConversion(x => x.Value.ToString(), x => new Name(x));
        builder.Property(x => x.Surname);
        builder.HasMany(b => b.Books)
            .WithMany(a => a.Authors)
            .UsingEntity(ab => ab.ToTable("AuthorBooks"));
    }
}
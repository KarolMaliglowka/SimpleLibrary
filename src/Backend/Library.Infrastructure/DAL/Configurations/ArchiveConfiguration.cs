using Library.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.DAL.Configurations;

public class ArchiveConfiguration : IEntityTypeConfiguration<Archive>
{
    public void Configure(EntityTypeBuilder<Archive> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.BookId);
        builder.Property(x => x.BookName);
        builder.Property(x => x.BookAuthors);
        builder.Property(x => x.UserId);
        builder.Property(x => x.UserFullName);
        builder.Property(x => x.BorrowDate);
        builder.Property(x => x.ReturnDate);
    }
}
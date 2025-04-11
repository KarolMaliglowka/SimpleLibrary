using Library.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.DAL.Configurations;

public class BorrowConfiguration : IEntityTypeConfiguration<Borrow>
{
    public void Configure(EntityTypeBuilder<Borrow> builder)
    {
        builder.HasKey(x => new{x.UserId, x.BookId});

        builder.HasOne(u => u.User)
            .WithMany(b => b.Borrows)
            .HasForeignKey(u => u.UserId);

        builder.HasOne(b => b.Book)
            .WithMany(b => b.Borrows)
            .HasForeignKey(b => b.BookId);
        
        builder.ToTable("Borrows");
    }
}
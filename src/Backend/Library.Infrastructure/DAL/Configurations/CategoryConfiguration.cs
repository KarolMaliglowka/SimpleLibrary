using Library.Core.Entities;
using Library.Core.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.DAL.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.CreatedAt);
        builder.Property(c => c.UpdatedAt);
        
        builder.Property(c => c.Name)
            .HasConversion(c => c.ToString(), c => new Name(c))
            .IsRequired()
            .HasMaxLength(200);

        builder.HasMany(c => c.Books)
            .WithOne(c => c.Category)
            .HasForeignKey(c => c.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
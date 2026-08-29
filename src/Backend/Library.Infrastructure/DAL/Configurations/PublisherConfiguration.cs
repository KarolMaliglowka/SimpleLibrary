using Library.Core.Entities;
using Library.Core.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.DAL.Configurations;

public class PublisherConfiguration : IEntityTypeConfiguration<Publisher>
{
    public void Configure(EntityTypeBuilder<Publisher> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .HasIdentityOptions(startValue:100);
        
        builder.Property(x => x.Name)
            .HasConversion(x => x.ToString(), x => new Name(x))
            .IsRequired();

        builder.HasMany(x => x.Books)
            .WithOne(x => x.Publisher)
            .HasForeignKey(x => x.PublisherId);
    }
}
using Library.Core.Entities;
using Library.Core.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.DAL.Configurations;

public class UserConfoguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name)
            .HasConversion(x => x.ToString(), x => new Name(x))
            .IsRequired()
            .HasMaxLength(200);
    }
} 
using Library.Core.Entities;
using Library.Core.Repositories;

namespace Library.Infrastructure.DAL.Repositories;

public class ArchiveRepository(LibraryDbContext context) : IArchiveRepository
{
    public async Task AddArchive(Archive archive)
    {
        await context.Archives.AddAsync(archive);
        await context.SaveChangesAsync();
    }
}
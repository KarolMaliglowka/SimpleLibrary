using Library.Core.Entities;

namespace Library.Core.Repositories;

public interface IArchiveRepository
{
    Task AddArchive(Archive archive);
}
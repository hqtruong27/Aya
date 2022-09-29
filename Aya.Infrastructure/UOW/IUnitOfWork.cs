using Aya.Infrastructure.Repositories;

namespace Aya.Infrastructure.UOW
{
    public interface IUnitOfWork : IDisposable
    {
        Task CreateTransactionAsync();

        Task CommitAsync();

        Task RollbackAsync();

        Task<int> SaveChangeAsync();

        ICategoryRepository Categories { get; }
    }
}

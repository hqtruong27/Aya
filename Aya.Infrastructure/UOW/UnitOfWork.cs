using Aya.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage;

namespace Aya.Infrastructure.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AyaDbContext _context;
        private IDbContextTransaction _transaction = default!;
        public UnitOfWork(AyaDbContext context,
            IHttpContextAccessor httpContextAccessor,
            ICategoryRepository categoryRepository)
        {
            _context = context;
            Categories = categoryRepository ?? new CategoryRepository(context, httpContextAccessor);
        }

        public ICategoryRepository Categories { get; }

        public async Task CreateTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public Task CommitAsync()
        {
            return _transaction.CommitAsync();
        }

        public Task RollbackAsync()
        {
            return _transaction.RollbackAsync();
        }

        public Task<int> SaveChangeAsync()
        {
            return _context.SaveChangesAsync();
        }

        #region IDisposable
        private bool _disposed = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                _disposed = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~UnitOfWork()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}

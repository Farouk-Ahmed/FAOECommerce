using CleanArchitecture.DataAccess.Contexts;
using CleanArchitecture.DataAccess.IRepository;
using CleanArchitecture.DataAccess.IUnitOfWorks;
using CleanArchitecture.DataAccess.Models;
using CleanArchitecture.DataAccess.Repsitory;
using System;
using System.Threading.Tasks;

namespace CleanArchitecture.DataAccess.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private bool _disposed = false;

        public IApplicationUserRepository ApplicationUserRepository { get; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            ApplicationUserRepository = new ApplicationUserRepository(_context);
        }

        public Repository<T> Repository<T>() where T : ModelBase
        {
            return new Repository<T>(_context);
        }

        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

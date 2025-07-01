
namespace CleanArchitecture.DataAccess.IUnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {
        Repository<T> Repository<T>() where T : ModelBase;

        Task<int> Complete();


        IApplicationUserRepository ApplicationUserRepository { get; }
    }
}

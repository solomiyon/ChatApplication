using ChatApp.DAL.Repository;
using System.Threading.Tasks;

namespace ChatApp.DAL.UnitOfWork
{
    public interface IUnitOfWork
    {
        IRepository<TEntity> Repository<TEntity>() where TEntity : class;
        Task<int> SaveChangesAsync();
    }
}
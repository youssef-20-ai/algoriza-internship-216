
using APIDemo.DAL.Interface;


namespace APIDemo.BLL.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IGeniricReposatory<TEntity> Reposatory<TEntity>() where TEntity : class;
        Task<int> Complete();
    }
}

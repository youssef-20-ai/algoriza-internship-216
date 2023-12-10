using AlgorizaProject.DAL.DbContext;
using AlgorizaProject.DAL.Entities;
using APIDemo.BLL.Interface;
using APIDemo.DAL.Interface;
using APIDemo.DAL.Reposatories;
using System.Collections;

namespace APIDemo.BLL.Reposatories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly VezeetaDbContext _context;
        private Hashtable _reposatories;

        public UnitOfWork(VezeetaDbContext context)
        {
            _context = context;
        }

        public async Task<int> Complete()
            => await _context.SaveChangesAsync();

        public void Dispose()
            => _context.Dispose();

        public IGeniricReposatory<TEntity> Reposatory<TEntity>() where TEntity : class
        {
            if(_reposatories is null)
                _reposatories = new Hashtable();

            var type = typeof(TEntity).Name;
            if(!_reposatories.ContainsKey(type))
            {
                var reposatory = new GeniricReposatory<TEntity>(_context);
                _reposatories.Add(type, reposatory);
            }
            return (IGeniricReposatory<TEntity>)_reposatories[type];
        }
    }
}

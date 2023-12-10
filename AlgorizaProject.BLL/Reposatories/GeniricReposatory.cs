using AlgorizaProject.DAL.DbContext;
using AlgorizaProject.DAL.Entities;
using APIDemo.BLL.ISepecification;
using APIDemo.BLL.Sepecifications;
using APIDemo.DAL.Interface;
using Microsoft.EntityFrameworkCore;

namespace APIDemo.DAL.Reposatories
{
    public class GeniricReposatory<T> : IGeniricReposatory<T> where T : class
    {
        private readonly VezeetaDbContext _context;

        public GeniricReposatory(VezeetaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
            => await _context.Set<T>().ToListAsync();

        public async Task<T> GetAsync(int? Id)
            => await _context.Set<T>().FindAsync(Id);

        public async Task<T> GetAsync(string? Id)
            => await _context.Set<T>().FindAsync(Id);

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec)
            => await applySpecs(spec).ToListAsync();

        public async Task<T> GetEntityWithSpecAsync(ISpecification<T> spec)
            => await applySpecs(spec).FirstOrDefaultAsync();

        private IQueryable<T> applySpecs(ISpecification<T> specification)
            => SpecificationEvaluator<T>.GetQuery(_context.Set<T>(), specification);

        public async Task<int> GetTotalItems(ISpecification<T> spec)
            => await applySpecs(spec).CountAsync();

        public void Add(T Entity)
            => _context.Set<T>().Add(Entity);

        public void Update(T Entity)
            => _context.Set<T>().Update(Entity);

        public void Delete(T Entity)
            => _context.Set<T>().Remove(Entity);
    }
}

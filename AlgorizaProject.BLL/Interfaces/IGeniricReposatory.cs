using AlgorizaProject.DAL.Entities;
using APIDemo.BLL.ISepecification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIDemo.DAL.Interface
{
    public interface IGeniricReposatory<T> where T : class
    {
        Task<T> GetAsync(int? Id);
        Task<T> GetAsync(string? Id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetEntityWithSpecAsync(ISpecification<T> spec);
        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec);
        Task<int> GetTotalItems(ISpecification<T> spec);
        void Add(T Entity);
        void Update(T Entity);
        void Delete(T Entity);

    }
}

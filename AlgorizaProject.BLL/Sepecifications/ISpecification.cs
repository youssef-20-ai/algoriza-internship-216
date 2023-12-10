using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace APIDemo.BLL.ISepecification
{
    public interface ISpecification<T>
    {
        Func<T,bool> Criteria { get;}
        List<Expression<Func<T,Object>>> Includes { get;}
        Expression<Func<T, Object>> OrderBy { get;}
        Expression<Func<T, Object>> OrderByDescending { get;}
        int Skip { get; }
        int Take { get; }
        public bool IsPagingEnabled { get; }


    }
}

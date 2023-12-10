using APIDemo.BLL.ISepecification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace APIDemo.BLL.Sepecifications
{
    public class BaseSpecification<T> : ISpecification<T>
    {

        public BaseSpecification() { }
        public BaseSpecification(Func<T, bool> criteria)
        {
            Criteria = criteria;
        }

        protected void addInclude(Expression<Func<T, Object>> expression)
        {
            Includes.Add(expression);
        }

        public Func<T, bool> Criteria { get; }

        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();

        public Expression<Func<T, object>> OrderBy { get; private set; }

        public Expression<Func<T, object>> OrderByDescending { get; private set; }

        public int Skip { get; private set; }

        public int Take { get; private set; }

        public bool IsPagingEnabled { get; private set; }

        protected void addOrderBy(Expression<Func<T, Object>> OrderByExpression)
        {
            OrderBy = OrderByExpression;
        }

        protected void applyPaging(int skip, int take)
        {
            Take = take;
            Skip = skip;
            IsPagingEnabled = true;
        }

        protected void addOrderByDescending(Expression<Func<T, Object>> OrderByDescendingExpression)
        {
            OrderByDescending = OrderByDescendingExpression;
        }
    }
}

using rescute.Shared;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace rescute.Domain.Repositories
{
    public interface IRepository<T> where T : AggregateRoot<T>
    {
        Task<T> GetAsync(Id<T> id);
        Task<IReadOnlyCollection<T>> GetAsync(Expression<Func<T, bool>> predicate);
        Task<IReadOnlyCollection<T>> GetAllAsync();
        Task<IReadOnlyCollection<T>> GetAsync(Expression<Func<T, bool>> predicate, int pageSize, int pageIndex);
        void Add(T item);
        void Remove(T item);
        void RemoveAll();
    }
}

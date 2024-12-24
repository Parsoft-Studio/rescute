using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using rescute.Shared;

namespace rescute.Domain.Repositories;

public interface IRepository<T> where T : AggregateRoot<T>
{
    Task<T> GetAsync(Id<T> id);
    Task<IReadOnlyCollection<T>> GetAsync(Expression<Func<T, bool>> predicate, int pageSize, int pageIndex);
    Task<IReadOnlyCollection<T>> GetAsync(Expression<Func<T, bool>> predicate);
    Task<IReadOnlyCollection<T>> GetAllAsync();
    void Add(T item);
    void Remove(T item);
}
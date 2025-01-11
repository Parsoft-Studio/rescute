using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using rescute.Domain.Aggregates;
using rescute.Domain.ValueObjects;
using rescute.Shared;

namespace rescute.Domain.Repositories;

public interface IRepository<T> where T : AggregateRoot<T>
{
    Task<T> GetAsync(Id<T> id);
    Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate, int pageSize, int pageIndex);
    Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate);
    Task<IReadOnlyList<T>> GetAllAsync();
    void Add(T item);
    void Remove(T item);
}
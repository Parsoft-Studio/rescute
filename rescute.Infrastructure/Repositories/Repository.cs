using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using rescute.Domain.Aggregates;
using rescute.Domain.Repositories;
using rescute.Domain.ValueObjects;
using rescute.Shared;

namespace rescute.Infrastructure.Repositories;

internal abstract class Repository<T> : IRepository<T> where T : AggregateRoot<T>

{
    private readonly DbContext context;

    protected Repository(DbContext context)
    {
        this.context = context;
    }

    public void Add(T item)
    {
        context.Set<T>().Add(item);
    }

    public async Task<T> GetAsync(Id<T> id)
    {
        return await context.Set<T>().SingleOrDefaultAsync(item => item.Id == id);
    }

    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate, int pageSize, int pageIndex)
    {
        return await context.Set<T>().Where(predicate).Skip(pageIndex * pageSize).Take(pageSize).ToArrayAsync();
    }

    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate)
    {
        return await context.Set<T>().Where(predicate).ToArrayAsync();
    }

    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        return await context.Set<T>().ToArrayAsync();
    }

    public void Remove(T item)
    {
        context.Set<T>().Remove(item);
    }
}
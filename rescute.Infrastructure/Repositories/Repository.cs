using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using rescute.Domain.Repositories;
using rescute.Shared;

namespace rescute.Infrastructure.Repositories;

public abstract class Repository<T> : IRepository<T> where T : AggregateRoot<T>

{
    private readonly DbContext context;

    protected Repository(DbContext c)
    {
        context = c;
    }

    public void Add(T item)
    {
        context.Set<T>().Add(item);
    }

    public async Task<T> GetAsync(Id<T> id)
    {
        return await context.Set<T>().SingleOrDefaultAsync(item => item.Id == id);
    }

    public async Task<IReadOnlyCollection<T>> GetAsync(Expression<Func<T, bool>> predicate, int pageSize, int pageIndex)
    {
        return await context.Set<T>().Where(predicate).Skip(pageIndex * pageSize).Take(pageSize).ToArrayAsync();
    }

    public async Task<IReadOnlyCollection<T>> GetAsync(Expression<Func<T, bool>> predicate)
    {
        return await context.Set<T>().Where(predicate).ToArrayAsync();
    }

    public async Task<IReadOnlyCollection<T>> GetAllAsync()
    {
        return await context.Set<T>().ToArrayAsync();
    }

    public void Remove(T item)
    {
        context.Set<T>().Remove(item);
    }
}
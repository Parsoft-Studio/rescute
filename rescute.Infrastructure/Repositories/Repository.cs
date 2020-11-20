using Microsoft.EntityFrameworkCore;
using rescute.Shared;
using rescute.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Linq;

namespace rescute.Infrastructure.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : AggregateRoot<T>

    {
        private DbContext context;
        public Repository(DbContext c)
        {
            this.context = c;
        }
        public void Add(T item)
        {
            context.Add<T>(item);
        }

        public async Task<T> GetAsync(Id<T> id)
        {
            return await context.Set<T>().SingleOrDefaultAsync(item => item.Id == id);
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await context.Set<T>().Where(predicate).ToListAsync();
        }
        public async Task<IReadOnlyList<T>> GeAllAsync()
        {
            return await context.Set<T>().ToListAsync();
        }

        public void Remove(T item)
        {
            context.Set<T>().Remove(item);
        }
    }
}

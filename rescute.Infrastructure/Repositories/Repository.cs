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
        private readonly DbContext context;
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

        public async Task<IReadOnlyCollection<T>> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await context.Set<T>().Where(predicate).ToListAsync();
        }
        public async Task<IReadOnlyCollection<T>> GeAllAsync()
        {
            return await context.Set<T>().ToListAsync();
        }

        public void Remove(T item)
        {
            context.Set<T>().Remove(item);
        }

        public void RemoveAll()
        {
            context.Set<T>().RemoveRange(context.Set<T>().ToArray());
            var entityType = context.Model.FindEntityType(typeof(T));
            var tableName = entityType.GetTableName();
            var schema= entityType.GetSchema();
            context.Database.ExecuteSqlRaw($"DELETE FROM {schema}.{tableName}");
        }
    }
}

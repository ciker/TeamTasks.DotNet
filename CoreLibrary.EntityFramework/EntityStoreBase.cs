﻿using CoreLibrary.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace CoreLibrary.EntityFramework
{
    public abstract class EntityStoreBase<T, TKey> : IAsyncStore<T, TKey>
        where T : class, IIdentifiable<TKey>
    {
        protected DbContext db;

        public EntityStoreBase(DbContext context)
        {
            db = context;
        }

        public virtual async Task CreateAsync(T entity)
        {
            db.Set<T>().Add(entity);
            await db.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(T entity)
        {
            db.Set<T>().Remove(entity);
            await db.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(TKey id)
        {
            T entity = await FindByIdAsync(id);

            if (entity != null)
            {
                await DeleteAsync(entity);
            }
        }
        
        public virtual async Task UpdateAsync(T entity)
        {           
            await db.SaveChangesAsync();
        }

        public virtual async Task<T> FindByIdAsync(TKey id)
        {
            var expr = ExpressionTools.EqualsLambda<T, TKey>(Expression.Parameter(typeof(T), "p"), "Id", id);
            return await db.Set<T>().SingleOrDefaultAsync(expr);
        }
    }
}

﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UniSozluk.Api.Application.Interfaces.Repositories;
using UniSozluk.Api.Domain.Models;
using UniSozluk.Infrastructure.Persistence.Context;

namespace UniSozluk.Api.Infrastructure.Persistence.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly DbContext dbContext;

        protected DbSet<TEntity> entity => dbContext.Set<TEntity>();

        public GenericRepository(DbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        #region Insert Methods
        public virtual int Add(TEntity entity)
        {
            this.entity.Add(entity);
            return dbContext.SaveChanges();
        }

        public virtual int Add(IEnumerable<TEntity> entities)
        {
            if (entities != null && !entities.Any())
                return 0;

            entity.AddRange(entity);
            return dbContext.SaveChanges();
        }

        public virtual async Task<int> AddAsync(TEntity entity)
        {
            await this.entity.AddAsync(entity);
            return await dbContext.SaveChangesAsync();
        }

        public virtual async Task<int> AddAsync(IEnumerable<TEntity> entities)
        {
            if(entities != null && !entities.Any())
                return 0;

            await entity.AddRangeAsync(entities);
            return await dbContext.SaveChangesAsync();
            
        }
        #endregion

        #region AddOrUpdate Methods
        public virtual int AddOrUpdate(TEntity entity)
        {
            if (!this.entity.Local.Any(i => EqualityComparer<Guid>.Default.Equals(i.Id, entity.Id)))
                dbContext.Update(entity);

                return dbContext.SaveChanges();  
        }
        
        public virtual Task<int> AddOrUpdateAsync(TEntity entity)
        {
            if (!this.entity.Local.Any(i => EqualityComparer<Guid>.Default.Equals(i.Id, entity.Id)))
                dbContext.Update(entity);

            return dbContext.SaveChangesAsync();
        }
        #endregion

        public IQueryable<TEntity> AsQueryable() => entity.AsQueryable();

        #region Bulk CRUD Methods
        public virtual Task BulkAdd(IEnumerable<TEntity> entities)
        {
            if(entities!=null&&!entities.Any())
                return Task.CompletedTask;

            foreach (var entityItem in entities)
            {
                entity.Add(entityItem);
            }

            return dbContext.SaveChangesAsync();
        }

        public Task BulkDelete(Expression<Func<TEntity, bool>> predicate)
        {
            dbContext.RemoveRange(entity.Where(predicate));
            return dbContext.SaveChangesAsync();
        }

        public Task BulkDelete(IEnumerable<TEntity> entities)
        {
            if (entities != null && !entities.Any())
                return Task.CompletedTask;

            entity.RemoveRange(entities);
            return dbContext.SaveChangesAsync();
        }

        public Task BulkDeleteById(IEnumerable<Guid> ids)
        {
            if (ids != null && ids.Any())
                return Task.CompletedTask;

            dbContext.RemoveRange(entity.Where(i => ids.Contains(i.Id)));
            return dbContext.SaveChangesAsync();
        }

        public Task BulkUpdate(IEnumerable<TEntity> entities)
        {
            if (entities != null && !entities.Any())
                return Task.CompletedTask;

            foreach (var entityItem in entities)
            {
                entity.Update(entityItem);
            }
            return dbContext.SaveChangesAsync();
        }
        #endregion

        #region Delete Methods
        public virtual int Delete(TEntity entity)
        {
            if (dbContext.Entry(entity).State == EntityState.Detached)
            {
                this.entity.Attach(entity);
            }

            this.entity.Remove(entity);
            return dbContext.SaveChanges();
        }

        public virtual int Delete(Guid id)
        {
            var entity = this.entity.Find(id);
            return Delete(entity);
        }

        public Task<int> DeleteAsync(TEntity entity)
        {
            if (dbContext.Entry(entity).State == EntityState.Detached)
            {
                this.entity.Attach(entity);
            }

            this.entity.Remove(entity);

            return dbContext.SaveChangesAsync();
        }

        public Task<int> DeleteAsync(Guid id)
        {
            var entity=this.entity.Find(id);
            return DeleteAsync(entity); 
        }

        public virtual bool DeleteRange(Expression<Func<TEntity, bool>> predicate)
        {
            dbContext.RemoveRange(entity.Where(predicate));
            return dbContext.SaveChanges()>0;
        }

        public virtual async Task<bool> DeleteRangeAsync(Expression<Func<TEntity, bool>> predicate)
        {
            dbContext.RemoveRange(entity.Where(predicate));
            return await dbContext.SaveChangesAsync() > 0;
        }
        #endregion
        public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes)
        {
            return Get(predicate, noTracking, includes).FirstOrDefaultAsync();
        }

        public virtual IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes)
        {
            var query=entity.AsQueryable();
            if(predicate != null)
                query = query.Where(predicate);

            query = ApplyIncludes(query, includes);

            if (noTracking)
                query = query.AsNoTracking();

            return query;
        }

        public async Task<List<TEntity>> GetAll(bool noTracking = true)
        {
            if(noTracking)
                return await entity.AsNoTracking().ToListAsync();

            return await entity.ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(Guid id, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes)
        {
            TEntity found = await entity.FindAsync(id);

            if (found == null)
                return null;

            if(noTracking)
                dbContext.Entry(found).State = EntityState.Detached;

            foreach (Expression<Func<TEntity,object>> include in includes)
            {
                dbContext.Entry(found).Reference(include).Load();
            }

            return found;
        }

        public virtual async Task<List<TEntity>> GetList(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, Func<IQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = entity;

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            foreach (Expression<Func<TEntity,object>> include in includes)
            {
                query=query.Include(include);
            }

            if (orderBy != null)
            {
                //What a shit ?
                query = orderBy(query);
                //throw new Exception("Lan bu hata nereden geliyor");
            }

            if(noTracking)
                query=query.AsNoTracking();

            return await query.ToListAsync();
        }

        public virtual async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = entity;

            if(predicate!=null)
                query = query.Where(predicate);

            query=ApplyIncludes(query, includes);

            if(noTracking)
                query=query.AsNoTracking();

            return await query.SingleOrDefaultAsync();
        }

        #region Update Methods
        public virtual int Update(TEntity entity)
        {
            this.entity.Attach(entity);
            dbContext.Entry(entity).State = EntityState.Modified;

            return dbContext.SaveChanges();
        }

        public virtual async Task<int> UpdateAsync(TEntity entity)
        {
            this.entity.Attach(entity);
            dbContext.Entry(entity).State = EntityState.Modified;

            return await dbContext.SaveChangesAsync();
        }
        #endregion

        #region SaveChanges Methods
        public Task<int> SaveChangesAsync()
        {
            return dbContext.SaveChangesAsync();
        }

        public int SaveChanges()
        {
            return dbContext.SaveChanges();
        }
        #endregion
        private static IQueryable<TEntity> ApplyIncludes(IQueryable<TEntity> query,params Expression<Func<TEntity, object>>[] includes)
        {
            if (includes != null)
            {
                foreach(var includeItem in includes)
                {
                    query=query.Include(includeItem);
                }
            }

            return query;
        }
    }
}

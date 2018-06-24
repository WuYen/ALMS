using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using ALMS.Models;

namespace ALMS.Repositories
{
    public abstract class RepositoryBase<TEntity, TKey> : IRepositoryBase<TEntity, TKey> where TEntity : class
    {
        private ALMSEntities _Entity;
        private readonly DbSet<TEntity> dbSet;

        protected RepositoryBase(ALMSEntities entity)
        {
            _Entity = entity;
            dbSet = _Entity.Set<TEntity>();
        }

        public virtual void Add(TEntity entity)
        {
            //dbSet.Add(entity);
            _Entity.Entry(entity).State = EntityState.Added;
        }

        public virtual void Update(TEntity entity)
        {
            _Entity.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(TEntity entity)
        {
            //dbSet.Remove(entity);
            _Entity.Entry(entity).State = EntityState.Deleted;
        }

        public virtual void Delete(Expression<Func<TEntity, bool>> where)
        {
            IEnumerable<TEntity> objects = dbSet.Where<TEntity>(where).AsEnumerable();
            foreach (TEntity obj in objects)
                dbSet.Remove(obj);
        }

        public virtual TEntity Get(Expression<Func<TEntity, bool>> where)
        {
            return dbSet.Where(where).FirstOrDefault<TEntity>();
        }

        public virtual TEntity GetByKey(TKey Key)
        {
            return dbSet.Find(Key);
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return dbSet.ToList();
        }

        public virtual IEnumerable<TEntity> GetMany(Expression<Func<TEntity, bool>> where)
        {
            return dbSet.Where(where).ToList();
        }

        public virtual IEnumerable<TEntity> GetMany(string sqlCmd)
        {
            return dbSet.SqlQuery(sqlCmd).ToList();
        }
    }
}
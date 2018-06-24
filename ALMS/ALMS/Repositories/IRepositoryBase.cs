using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace ALMS.Repositories
{
    public interface IRepositoryBase<TEntity, TKey>
          where TEntity : class
    {
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        void Delete(Expression<Func<TEntity, bool>> where);

        TEntity Get(Expression<Func<TEntity, bool>> where);
        TEntity GetByKey(TKey id);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> GetMany(Expression<Func<TEntity, bool>> predicate);
        IEnumerable<TEntity> GetMany(string sqlCmd);
    }
}
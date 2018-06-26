using ALMS.Models;
using ALMS.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace ALMS.ViewModels.BA04.Service
{
    public class BA04Service : ServiceBase
    {
        private BA04ARepository _Repository;

        public BA04Service()
        {
            _entity = new ALMSEntities();
            _Repository = new BA04ARepository(_entity);
        }

        public BA04A GetByKey(int BA04A_ID)
        {
            return _Repository.GetByKey(BA04A_ID);
        }

        public List<BA04A> GetMany(string sqlCmd)
        {
            return _Repository.GetMany(sqlCmd).ToList();
        }

        public IQueryable<BA04A> GetMany(Expression<Func<BA04A, bool>> predicate)
        {
            return _Repository.GetMany(predicate).AsQueryable();
        }

        public string SaveChanges(BA04A entity, EntityState state, string errMsg)
        {
            if (errMsg != "")
            {
                return "";
            }
            using (var trans = _entity.Database.BeginTransaction(IsolationLevel.Snapshot))
            {
                try
                {
                    if (state == EntityState.Added)
                    {
                        _Repository.Add(entity);
                    }
                    else if (state == EntityState.Modified)
                    {
                        _Repository.Update(entity);
                    }
                    else if (state == EntityState.Deleted)
                    {
                        _Repository.Delete(entity);
                    }
                    _entity.SaveChanges();
                    trans.Commit();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                {
                    errMsg = ex.Message;
                }
                catch (Exception ex)
                {
                    errMsg = ex.Message;
                }
            }
            return errMsg;
        }
    }
}
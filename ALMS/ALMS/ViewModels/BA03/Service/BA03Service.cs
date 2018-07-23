using ALMS.Models;
using ALMS.Repositories;
using ALMS.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace ALMS.ViewModels.BA03.Service
{
    public class BA03Service : ServiceBase
    {
        private BA03ARepository _Repository;

        public BA03Service()
        {
            _entity = new ALMSEntities();
            _Repository = new BA03ARepository(_entity);
        }

        public BA03A GetByKey(int BA03A_ID)
        {
            return _Repository.GetByKey(BA03A_ID);
        }

        public List<BA03A> GetMany(string sqlCmd)
        {
            return _Repository.GetMany(sqlCmd).ToList();
        }

        public IQueryable<BA03A> GetMany(Expression<Func<BA03A, bool>> predicate)
        {
            return _Repository.GetMany(predicate).AsQueryable();
        }

        public string SaveChanges(BA03A entity, EntityState state, string errMsg)
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
                    errMsg = SQLHelper.GetSQLMessage(ex);
                }
            }
            return errMsg;
        }
    }
}
using ALMS.Models;
using ALMS.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace ALMS.ViewModels.TR01.Service
{
    public class TR01Service : ServiceBase
    {
        private TR01ARepository _Repository;

        public TR01Service()
        {
            _entity = new ALMSEntities();
            _Repository = new TR01ARepository(_entity);
        }

        public TR01A GetByKey(int TR01A_ID)
        {
            return _Repository.GetByKey(TR01A_ID);
        }

        public List<TR01A> GetMany(Expression<Func<TR01A, bool>> predicate)
        {
            return _Repository.GetMany(predicate).ToList();
        }

        public IQueryable<TR01A> GetQueryable()
        {
            return _Repository.GetQuery();
        }

        public string SaveChanges(List<TR01A> AddList, List<TR01A> UpdateList, List<int> DeleteList, EntityState state)
        {
            var errMsg = "";
            using (var trans = _entity.Database.BeginTransaction(IsolationLevel.Snapshot))
            {
                try
                {
                    foreach (var item in AddList)
                    {
                        _Repository.Add(item);
                    }
                    foreach (var item in UpdateList)
                    {
                        _Repository.Update(item);
                    }
                    _Repository.Delete(x => DeleteList.Contains(x.TR01A_ID));

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
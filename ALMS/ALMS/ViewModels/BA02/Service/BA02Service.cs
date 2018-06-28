using ALMS.Models;
using ALMS.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace ALMS.ViewModels.BA02.Service
{
    public class BA02Service : ServiceBase
    {
        private BA02ARepository _BA02ARepository;
        private BA02BRepository _BA02BRepository;

        public BA02Service()
        {
            _entity = new ALMSEntities();
            _BA02ARepository = new BA02ARepository(_entity);
            _BA02BRepository = new BA02BRepository(_entity);
        }

        public BA02A GetByKey(int BA02A_ID)
        {
            return _BA02ARepository.GetByKey(BA02A_ID);
        }

        public List<BA02A> GetMany(string sqlCmd)
        {
            return _BA02ARepository.GetMany(sqlCmd).ToList();
        }

        public IQueryable<BA02A> GetMany(Expression<Func<BA02A, bool>> predicate)
        {
            return _BA02ARepository.GetMany(predicate).AsQueryable();
        }


        public List<BA02B> GetByMasterKey(int BA02A_ID)
        {
            return _BA02BRepository.GetMany(x => x.BA02A_ID == BA02A_ID).ToList();
        }

        //編輯的時候取得 updae 的資料回來
        public List<BA02B> GetByDetailKeys(List<int> IDlist)
        {
            return _BA02BRepository.GetMany(x => IDlist.Contains(x.BA02B_ID)).ToList();
        }

        public IQueryable<BA02B> GetDetails(Expression<Func<BA02B, bool>> predicate)
        {
            return _BA02BRepository.GetMany(predicate).AsQueryable();
        }

        public string SaveChanges(BA02A entity, EntityState state)
        {
            string errMsg = "";
            using (var trans = _entity.Database.BeginTransaction(IsolationLevel.Snapshot))
            {
                try
                {
                    if (state == EntityState.Deleted)
                    {
                        _BA02BRepository.Delete(x => x.BA02A_ID == entity.BA02A_ID);
                        _BA02ARepository.Delete(entity);
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

        public string SaveChangeBatch(BA02A entity, EntityState state, List<BA02B> CreateD, List<BA02B> UpdateD, List<int> DeleteD)
        {
            var errMsg = "";
            using (var trans = _entity.Database.BeginTransaction(IsolationLevel.Snapshot))
            {
                try
                {
                    if (state == EntityState.Added)
                    {
                        _BA02ARepository.Add(entity);
                    }
                    else if (state == EntityState.Modified)
                    {
                        _BA02ARepository.Update(entity);
                    }
                    _entity.SaveChanges();

                    foreach (var item in CreateD)
                    {
                        item.BA02A_ID = entity.BA02A_ID;
                        _BA02BRepository.Add(item);
                    }
                    foreach (var item in UpdateD)
                    {
                        item.BA02A_ID = entity.BA02A_ID;
                        _BA02BRepository.Update(item);
                    }
                    _BA02BRepository.Delete(x => DeleteD.Contains(x.BA02B_ID));

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
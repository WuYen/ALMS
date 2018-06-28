using ALMS.Models;
using ALMS.Utilities;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ALMS.ViewModels.BA02.Service
{
    public class BA02Business
    {
        private static string Validation(BA02AModel master, ModelStateDictionary ModelState)
        {
            List<string> errMsgList = new List<string>();
            //if (master.REM_MM.Length > 10)
            //{
            //    ModelState.AddModelError("REM_MM", "太長");
            //}
            return string.Join("<br/>", errMsgList);
        }
        private static void ValidateDetail(List<BA02BModel> detail, ModelStateDictionary ModelState, EntityState state)
        {
            var type = state == EntityState.Added ? "Insert" : "Update";
            for (int i = 0; i < detail.Count; i++)
            {
                //if (detail[i].OUT_QT > 10)
                //{
                //    ModelState.AddModelError($"{type}[{i}].OUT_QT", "To bid Error " + i.ToString());
                //}
            }
        }


        public static BA02AModel FromEntity(BA02A entity)
        {
            var data = new BA02AModel();
            if (entity != null)
            {
                var objectHelper = new ObjectHelper();
                objectHelper.CopyValue(entity, data);
            }
            return data;
        }

        public static List<BA02AModel> FromEntity(List<BA02A> entityList)
        {
            var list = new List<BA02AModel>();
            foreach (var item in entityList)
            {
                list.Add(FromEntity(item));
            }
            return list;
        }

        private static void ToEntity(BA02AModel model, ref BA02A entity)
        {
            List<string> exclusiveList = new List<string>() { "CREATE_USER", "CREATE_DATE", "LASTUPDATE_USER", "LASTUPDATE_DATE" };
            var objectHelper = new ObjectHelper();
            objectHelper.CopyValue(model, entity, exclusiveList);
        }

        public static BA02BModel FromEntity(BA02B entity)
        {
            var data = new BA02BModel();
            if (entity != null)
            {
                var objectHelper = new ObjectHelper();
                objectHelper.CopyValue(entity, data);
            }
            return data;
        }

        public static List<BA02BModel> FromEntity(List<BA02B> entityList)
        {
            var list = new List<BA02BModel>();
            foreach (var item in entityList)
            {
                list.Add(FromEntity(item));
            }
            return list;
        }

        private static void ToEntity(BA02BModel model, ref BA02B entity)
        {
            List<string> exclusiveList = new List<string>() { "CREATE_USER", "CREATE_DATE", "LASTUPDATE_USER", "LASTUPDATE_DATE" };
            var objectHelper = new ObjectHelper();
            objectHelper.CopyValue(model, entity, exclusiveList);
        }

        private static List<BA02B> ToEntity(List<BA02BModel> model, List<BA02B> entityList, EntityState state)
        {
            if (state == EntityState.Added)
            {
                foreach (BA02BModel item in model)
                {
                    var entityItem = new BA02B();
                    ToEntity(item, ref entityItem);

                    entityItem.CREATE_USER = "SYSTEM";
                    entityItem.CREATE_DATE = DateTime.Now;
                    entityList.Add(entityItem);
                }
            }
            else
            {
                foreach (BA02BModel item in model)
                {
                    var entityItem = entityList.First(x => x.BA02B_ID == item.BA02B_ID);
                    ToEntity(item, ref entityItem);

                    entityItem.UPDATE_USER = "SYSTEM";
                    entityItem.UPDATE_DATE = DateTime.Now;
                }
            }
            return entityList;
        }

        public static ResultHelper<BA02A> BeforeSave(BA02AModel model, BA02A entity, EntityState state, ModelStateDictionary ModelState)
        {
            var errMsg = "";
            if (state == EntityState.Added) //新增
            {
                errMsg = Validation(model, ModelState);
                if (errMsg == "" || ModelState.IsValid)
                {
                    ToEntity(model, ref entity);
                    entity.CREATE_USER = "SYSTEM";
                    entity.CREATE_DATE = DateTime.Now;
                }
            }
            else if (state == EntityState.Modified)//修改
            {
                errMsg = entity == null ? "資料已被刪除<br/>" : Validation(model, ModelState);
                if (errMsg == "" || ModelState.IsValid)
                {
                    ToEntity(model, ref entity);
                    entity.UPDATE_USER = "SYSTEM";
                    entity.UPDATE_DATE = DateTime.Now;
                }
            }
            else //刪除
            {
                errMsg = entity == null ? "資料已被刪除" : "";
            }

            return new ResultHelper<BA02A>() { Data = entity, Message = errMsg };
        }

        public static ResultHelperBatch<BA02B, int> BeforeSave(MVCxGridViewBatchUpdateValues<BA02BModel, int> values, List<BA02B> updateList, int DeleteCount, ModelStateDictionary ModelState)
        {
            ResultHelperBatch<BA02B, int> result = new ResultHelperBatch<BA02B, int>() { Message = "" };

            ValidateDetail(values.Update, ModelState, EntityState.Modified);
            ValidateDetail(values.Insert, ModelState, EntityState.Added);
            if (updateList.Count != values.Update.Count || DeleteCount != values.DeleteKeys.Count)
            {
                result.Message += "明細資料已被刪除<br/>請重新整理<br/>";
            }

            if (!ModelState.IsValid)
            {
                result.Message += "請檢查紅色驚嘆號<br/>";
            }
            else if (result.Message == "")
            {
                result.Insert = ToEntity(values.Insert, new List<BA02B>(), EntityState.Added);
                result.Update = ToEntity(values.Update, updateList, EntityState.Modified);
                result.Delete = values.DeleteKeys;
            }

            return result;
        }
    }
}
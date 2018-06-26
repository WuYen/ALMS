using ALMS.Models;
using ALMS.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ALMS.ViewModels.BA01.Service
{
    public class BA01Business
    {
        public static string Validation(BA01AModel master)
        {
            List<string> errMsgList = new List<string>();
            //if (master.REM_MM.Length > 10)
            //{
            //    master.ModelState.AddModelError("REM_MM", "太長");
            //}
            if (!master.ModelState.IsValid)
            {
                errMsgList.Add("請檢查紅色驚嘆號");
            }
            return string.Join("<br />", errMsgList);
        }

        public static BA01AModel FromEntity(BA01A entity)
        {
            var data = new BA01AModel();
            if (entity != null)
            {
                var objectHelper = new ObjectHelper();
                objectHelper.CopyValue(entity, data);
            }
            return data;
        }

        public static List<BA01AModel> FromEntity(List<BA01A> entityList)
        {
            var list = new List<BA01AModel>();
            foreach (var item in entityList)
            {
                list.Add(FromEntity(item));
            }
            return list;
        }

        public static void ToEntity(BA01AModel model, ref BA01A entity)
        {
            if (entity == null)
            {
                entity = new BA01A();
            }
            List<string> exclusiveList = new List<string>() { "CREATE_USER", "CREATE_DATE", "LASTUPDATE_USER", "LASTUPDATE_DATE" };
            var objectHelper = new ObjectHelper();
            objectHelper.CopyValue(model, entity, exclusiveList);
        }

        public static ResultHelper<BA01A> BeforeSave(BA01AModel model, BA01A entity, EntityState state)
        {
            var errMsg = ""; if (state == EntityState.Added) //新增
            {
                errMsg = Validation(model);

                ToEntity(model, ref entity);
                entity.CREATE_USER = "SYSTEM";
                entity.CREATE_DATE = DateTime.Now;
            }
            else if (state == EntityState.Modified)//修改
            {
                errMsg = entity == null ? "資料已被刪除" : Validation(model);

                ToEntity(model, ref entity);
                entity.UPDATE_USER = "SYSTEM";
                entity.UPDATE_DATE = DateTime.Now;
            }
            else //刪除
            {
                errMsg = entity == null ? "資料已被刪除" : "";
            }

            return new ResultHelper<BA01A>() { Data = entity, Message = errMsg };
        }
    }
}
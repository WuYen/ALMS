using ALMS.Models;
using ALMS.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ALMS.ViewModels.BA03.Service
{
    public class BA03Business
    {
        public static string Validation(BA03AModel master)
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

        public static BA03AModel FromEntity(BA03A entity)
        {
            var data = new BA03AModel();
            if (entity != null)
            {
                var objectHelper = new ObjectHelper();
                objectHelper.CopyValue(entity, data);
            }
            return data;
        }

        public static List<BA03AModel> FromEntity(List<BA03A> entityList)
        {
            var list = new List<BA03AModel>();
            foreach (var item in entityList)
            {
                list.Add(FromEntity(item));
            }
            return list;
        }

        public static void ToEntity(BA03AModel model, ref BA03A entity)
        {
            if (entity == null)
            {
                entity = new BA03A();
            }
            List<string> exclusiveList = new List<string>() { "CREATE_USER", "CREATE_DATE", "LASTUPDATE_USER", "LASTUPDATE_DATE" };
            var objectHelper = new ObjectHelper();
            objectHelper.CopyValue(model, entity, exclusiveList);
        }

        public static ResultHelper<BA03A> BeforeSave(BA03AModel model, BA03A entity, EntityState state)
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

            return new ResultHelper<BA03A>() { Data = entity, Message = errMsg };
        }
    }
}
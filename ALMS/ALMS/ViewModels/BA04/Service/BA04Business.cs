using ALMS.Models;
using ALMS.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ALMS.ViewModels.BA04.Service
{
    public class BA04Business
    {
        public static string Validation(BA04AModel master)
        {
            List<string> errMsgList = new List<string>();
            if (!master.ModelState.IsValid)
            {
                errMsgList.Add("請檢查紅色驚嘆號");
            }
            return string.Join("<br />", errMsgList);
        }

        public static BA04AModel FromEntity(BA04A entity)
        {
            var data = new BA04AModel();
            if (entity != null)
            {
                var objectHelper = new ObjectHelper();
                objectHelper.CopyValue(entity, data);
            }
            return data;
        }

        public static List<BA04AModel> FromEntity(List<BA04A> entityList)
        {
            var list = new List<BA04AModel>();
            foreach (var item in entityList)
            {
                list.Add(FromEntity(item));
            }
            return list;
        }

        public static void ToEntity(BA04AModel model, ref BA04A entity)
        {
            if (entity == null)
            {
                entity = new BA04A();
            }
            List<string> exclusiveList = new List<string>() { "CREATE_USER", "CREATE_DATE", "LASTUPDATE_USER", "LASTUPDATE_DATE" };
            var objectHelper = new ObjectHelper();
            objectHelper.CopyValue(model, entity, exclusiveList);
        }

        public static ResultHelper<BA04A> BeforeSave(BA04AModel model, BA04A entity, EntityState state)
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

            return new ResultHelper<BA04A>() { Data = entity, Message = errMsg };
        }
    }
}
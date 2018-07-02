using ALMS.Models;
using ALMS.Utilities;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ALMS.ViewModels.TR01.Service
{
    public class TR01Business
    {
        public static string Validation(TR01AModel master, ModelStateDictionary ModelState)
        {
            List<string> errMsgList = new List<string>();
            //if (master.REM_MM.Length > 10)
            //{
            //    master.ModelState.AddModelError("REM_MM", "太長");
            //}
            //if (!master.ModelState.IsValid)
            //{
            //    errMsgList.Add("請檢查紅色驚嘆號");
            //}
            return string.Join("<br />", errMsgList);
        }
        private static void ValidateDetail(List<TR01BModel> detail, ModelStateDictionary ModelState, EntityState state)
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

        public static Tuple<TR01AModel, List<TR01BModel>> FromEntity(List<TR01A> entityList)
        {
            var temp = entityList.First();
            var master = new TR01AModel();
            master.TRN_DT = temp.TRN_DT;
            master.VOU_NO = temp.VOU_NO;
            master.BA02A_ID = temp.BA02A_ID;
            master.BA02B_ID = temp.BA02B_ID;
            master.BA03A_ID = temp.BA03A_ID;
            master.DA03A_ID = temp.DA03A_ID;

            var detailList = new List<TR01BModel>();
            foreach (var item in entityList)
            {
                detailList.Add(new TR01BModel()
                {
                    TR01A_ID = item.TR01A_ID,
                    BA01A_ID = item.BA01A_ID,
                    CRE_MY = item.CRE_MY,
                    DEB_MY = item.DEB_MY,
                    SUM_RM = item.SUM_RM
                });
            }

            return new Tuple<TR01AModel, List<TR01BModel>>(master, detailList);
        }

        public static List<TR01A> ToEntity(TR01AModel master, List<TR01BModel> detailList, List<TR01A> entityList, EntityState state)
        {
            if (state == EntityState.Added)
            {
                foreach (TR01BModel item in detailList)
                {
                    var entityItem = new TR01A();
                    ToEntity(master, item, ref entityItem);

                    entityItem.CREATE_USER = "SYSTEM";
                    entityItem.CREATE_DATE = DateTime.Now;
                    entityList.Add(entityItem);
                }
            }
            else
            {
                foreach (TR01BModel item in detailList)
                {
                    var entityItem = entityList.First(x => x.TR01A_ID == item.TR01A_ID);
                    ToEntity(master, item, ref entityItem);

                    entityItem.UPDATE_USER = "SYSTEM";
                    entityItem.UPDATE_DATE = DateTime.Now;
                }
            }
            return entityList;
        }

        private static void ToEntity(TR01AModel master, TR01BModel detail, ref TR01A entityItem)
        {            
            entityItem.TRN_DT = master.TRN_DT;
            entityItem.VOU_NO = master.VOU_NO;
            entityItem.BA02A_ID = master.BA02A_ID;
            entityItem.BA02B_ID = master.BA02B_ID;
            entityItem.BA03A_ID = master.BA03A_ID;
            entityItem.DA03A_ID = master.DA03A_ID;
            entityItem.BA01A_ID = detail.BA01A_ID;
            entityItem.CRE_MY = detail.CRE_MY;
            entityItem.DEB_MY = detail.DEB_MY;
            entityItem.SUM_RM = detail.SUM_RM;
        }

        public static ResultHelperBatch<TR01A, int> BeforeSave(TR01AModel master, MVCxGridViewBatchUpdateValues<TR01BModel, int> values, List<TR01A> updateList, int DeleteCount, ModelStateDictionary ModelState)
        {
            ResultHelperBatch<TR01A, int> result = new ResultHelperBatch<TR01A, int>() { Message = "" };
            Validation(master, ModelState);
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
                result.Insert = ToEntity(master,values.Insert, new List<TR01A>(), EntityState.Added);
                result.Update = ToEntity(master,values.Update, updateList, EntityState.Modified);
                result.Delete = values.DeleteKeys;
            }

            return result;
        }
    }
}
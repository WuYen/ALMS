using ALMS.Models;
using ALMS.Utilities;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
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
                if (!detail[i].BA01A_ID.HasValue || detail[i].BA01A_ID == 0)
                {
                    ModelState.AddModelError($"{type}[{i}].ACC_NO", "必選");
                }
            }
        }

        public static Tuple<TR01AModel, List<TR01BModel>> FromEntity(List<TR01A> entityList)
        {
            var temp = entityList.First();
            var master = new TR01AModel
            {
                TRN_DT = temp.TRN_DT,
                DtTRN_DT = DateTime.ParseExact(temp.TRN_DT, "yyyyMMdd", CultureInfo.InvariantCulture),
                VOU_NO = temp.VOU_NO,
                BA02A_ID = temp.BA02A_ID,
                BA02B_ID = temp.BA02B_ID,
                BA03A_ID = temp.BA03A_ID,
                DA03A_ID = temp.DA03A_ID,
                EntityState = EntityState.Modified
            };

            var detailList = new List<TR01BModel>();
            foreach (var item in entityList)
            {
                var BA01A = CacheCommonDataModule.GetBA01A(item.BA01A_ID);
                detailList.Add(new TR01BModel()
                {
                    TR01A_ID = item.TR01A_ID,
                    BA01A_ID = item.BA01A_ID,
                    CRE_MY = item.CRE_MY,
                    DEB_MY = item.DEB_MY,
                    SUM_RM = item.SUM_RM,
                    ACC_NO = BA01A.ACC_NO,
                    ACC_NM = BA01A.ACC_NM
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
            entityItem.TRN_DT = master.DtTRN_DT.ToString("yyyyMMdd");
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

        public static ResultHelperBatch<TR01A, int> BeforeSave(TR01AModel master, MVCxGridViewBatchUpdateValues<TR01BModel, int> values, List<TR01A> updateList, int deleteCount, ModelStateDictionary ModelState)
        {
            ResultHelperBatch<TR01A, int> result = new ResultHelperBatch<TR01A, int>() { Message = "" };
            var updateKeys = values.Update.Select(x => x.TR01A_ID).ToList();
            var trueUpdateList = updateList.Where(x => updateKeys.Contains(x.TR01A_ID)).ToList();
            var fakeUpdateList = updateList.Where(x => !updateKeys.Contains(x.TR01A_ID)).ToList();

            Validation(master, ModelState);
            ValidateDetail(values.Update, ModelState, EntityState.Modified);
            ValidateDetail(values.Insert, ModelState, EntityState.Added);
            if (trueUpdateList.Count != values.Update.Count || deleteCount != values.DeleteKeys.Count)
            {
                result.Message += "明細資料已被刪除<br/>請重新整理<br/>";
            }
            result.Message += ValidateBalance(values.Update, values.Insert);

            if (!ModelState.IsValid)
            {
                result.Message += "請檢查紅色驚嘆號<br/>";
            }
            else if (result.Message == "")
            {
                if (master.EntityState == EntityState.Added)
                {
                   
                    master.VOU_NO = new ALMSEntities().Database.SqlQuery<string>("select dbo.Get_VOU_NO(" + master.DtTRN_DT.ToString("yyyyMMdd") + ")").FirstOrDefault();
                }
                result.Insert = ToEntity(master, values.Insert, new List<TR01A>(), EntityState.Added);
                result.Update = ToEntity(master, values.Update, trueUpdateList, EntityState.Modified);
                if (fakeUpdateList.Count > 0)
                {
                    result.Update.AddRange(ToEntity(master, fakeUpdateList));//把detail沒有編輯的 也補上master
                }
                result.Delete = values.DeleteKeys;
            }

            return result;
        }

        private static string ValidateBalance(List<TR01BModel> update, List<TR01BModel> insert)
        {
            string errMsg = "";
            var sumCredit = update.Sum(x => x.CRE_MY) + insert.Sum(x => x.CRE_MY);
            var sumDebit = update.Sum(x => x.DEB_MY) + insert.Sum(x => x.DEB_MY);
            if (sumCredit.HasValue && sumDebit.HasValue)
            {
                if (sumCredit.Value != sumDebit.Value)
                {
                    errMsg += "借貸不平衡<br/>";
                }
            }
            return errMsg;
        }

        private static List<TR01A> ToEntity(TR01AModel master, List<TR01A> updateList2)
        {
            foreach (TR01A item in updateList2)
            {
                item.TRN_DT = master.DtTRN_DT.ToString("yyyyMMdd");
                item.VOU_NO = master.VOU_NO;
                item.BA02A_ID = master.BA02A_ID;
                item.BA02B_ID = master.BA02B_ID;
                item.BA03A_ID = master.BA03A_ID;
                item.DA03A_ID = master.DA03A_ID;
                item.UPDATE_USER = "SYSTEM";
                item.UPDATE_DATE = DateTime.Now;
            }
            return updateList2;
        }
    }
}
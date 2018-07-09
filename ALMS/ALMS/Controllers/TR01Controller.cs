using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ALMS.Utilities;
using ALMS.ViewModels.TR01;
using ALMS.ViewModels.TR01.Service;
using DevExpress.Web.Mvc;

namespace ALMS.Controllers
{
    public class TR01Controller : Controller
    {
        private TR01Service _Service = new TR01Service();
        // GET: TR01
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Edit(string key)
        {
            string errMsg = "";
            ViewData["Master"] = new TR01AModel() { VOU_NO = "系統產生", DtTRN_DT = DateTime.Now, EntityState = EntityState.Added };
            ViewData["Detail"] = new List<TR01BModel>();
            if (!string.IsNullOrWhiteSpace(key))
            {
                var entityList = _Service.GetMany(x => x.VOU_NO == key);
                if (entityList.Count == 0)
                {
                    errMsg = "查無資料";
                }
                else
                {
                    var dataSet = TR01Business.FromEntity(entityList);
                    ViewData["Master"] = dataSet.Item1;
                    ViewData["Detail"] = dataSet.Item2;
                }
            }

            if (Request.IsAjaxRequest())
            {
                return new JsonResult { Data = new { View = ReadViewHelper.PartialView(this, "_Edit", key), ErrMsg = errMsg }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return PartialView("_Edit", key);
        }

        public ActionResult DetailGridBatchUpdate(TR01AModel master, MVCxGridViewBatchUpdateValues<TR01BModel, int> updateValues)
        {
            //Step1: 先把update資料抓回來
            var deleteCount = _Service.GetMany(updateValues.DeleteKeys).Count;
            var entityList = _Service.GetMany(x => x.VOU_NO == master.VOU_NO);

            //Step2: call business before save 
            var result = TR01Business.BeforeSave(master, updateValues, entityList, deleteCount, ModelState);//資料已被刪除<br />"      

            //Step3: call service save change
            if (result.Message == "")
            {
                result.Message += _Service.SaveChanges(result.Insert, result.Update, result.Delete);
            }
            return ResultHandler(result.Message, master, updateValues);
        }

        private ActionResult ResultHandler(string errMsg, TR01AModel master, MVCxGridViewBatchUpdateValues<TR01BModel, int> updateValues)
        {
            if (errMsg == "")
            {
                ViewData["IsSuccess"] = "true";
                var result = TR01Business.FromEntity(_Service.GetMany(x => x.VOU_NO == master.VOU_NO));
                ViewData["MasterForm"] = ReadViewHelper.PartialView(this, "_MasterForm", result.Item1);
                return PartialView("_DetailGrid", result.Item2);
            }
            else
            {
                for (int i = 0; i < updateValues.Insert.Count; i++)
                {
                    ModelState.AddModelError($"Insert[{i}].IsValid", "Error");
                }
                for (int i = 0; i < updateValues.Update.Count; i++)
                {
                    ModelState.AddModelError($"Update[{i}].IsValid", "Error");
                }
                var deleteIDStrList = "";
                for (int i = 0; i < updateValues.DeleteKeys.Count; i++)
                {
                    updateValues.SetErrorText(updateValues.DeleteKeys[i], "Unable to delete!");
                    deleteIDStrList += updateValues.DeleteKeys[i] + ",";
                }
                if (master.EntityState == EntityState.Added)
                {
                    master.VOU_NO = "系統產生";
                }

                errMsg = "存儲失敗<br/>" + errMsg;
                ViewData["DeleteIDList"] = deleteIDStrList;
                ViewData["ErrMsg"] = errMsg.Substring(0, errMsg.Length - 5);
                ViewData["MasterForm"] = ReadViewHelper.PartialView(this, "_MasterForm", master);
                return PartialView("_DetailGrid", new List<TR01BModel>());
            }

        }

        public ActionResult BA02BCombobox(int key)
        {
            ViewBag.DataSource = CacheCommonDataModule.GetBA02B(key);
            return PartialView("_BA02BCombobox");
        }
    }
}
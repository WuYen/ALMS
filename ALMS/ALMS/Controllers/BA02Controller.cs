using ALMS.Models;
using ALMS.Utilities;
using ALMS.ViewModels.BA02;
using ALMS.ViewModels.BA02.Service;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ALMS.Controllers
{
    public class BA02Controller : Controller
    {
        private BA02Service _Service = new BA02Service();

        // GET: BA02PD
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult RedirectIndex(int? key)
        {
            var temp = TempData["SelectedItemKey"] as int?;
            ViewData["selectedItem"] = temp ?? key;
            ViewData["Success"] = TempData["SaveSuccess"] as bool?;
            return View("Index");
        }

        public ActionResult Grid(SearchViewModel search, int? key)
        {
            ViewData["selectedItem"] = key;
            List<BA02AModel> result = GetMasterList(search);
            return PartialView("_Grid", result);
        }

        public ActionResult Delete(int BA02A_ID, SearchViewModel search)
        {
            var entity = _Service.GetByKey(BA02A_ID);
            var result = BA02Business.BeforeSave(null, ref entity, EntityState.Deleted, ModelState);

            if (result.Message == "")
            {
                result.Message += _Service.SaveChanges(entity, EntityState.Deleted);
            }

            if (result.Message == "")
            {
                ViewData["Success"] = true;
            }
            else
            {
                ViewData["ErrMsg"] = result.Message;
                ViewData["selectedItem"] = entity.BA02A_ID;
            }

            List<BA02AModel> resultData = GetMasterList(search);
            return PartialView("_Grid", resultData); ;
        }

        private List<BA02AModel> GetMasterList(SearchViewModel search)
        {
            var queryData = _Service.GetQueryable();
            if (!string.IsNullOrWhiteSpace(search.S_CPN_NO))
            {
                queryData = queryData.Where(x => x.CPN_NO.Contains(search.S_CPN_NO));
            }
            if (!string.IsNullOrWhiteSpace(search.S_CPN_NM))
            {
                queryData = queryData.Where(x => x.CPN_NM.Contains(search.S_CPN_NM));
            }
            var result = BA02Business.FromEntity(queryData.ToList());
            return result;
        }

        public ActionResult Edit(int? key)
        {
            if (key.HasValue && key.Value > 0)
            {
                var entity = _Service.GetByKey(key.Value);
                if (entity == null)
                {
                    ViewData["ErrMsg"] = "資料已被其他使用者刪除";
                }
                ViewData["Master"] = BA02Business.FromEntity(entity); //new FA11AModel();
                ViewData["Detail"] = BA02Business.FromEntity(_Service.GetByMasterKey(key.Value));  //new List<FA11BModel>();
            }
            else
            {
                key = 0;
                ViewData["Master"] = new BA02AModel();
                ViewData["Detail"] = new List<BA02BModel>();
            }
            return View(key);
        }

        public ActionResult DetailGrid(string key)
        {
            int.TryParse(key, out int keyValue);
            return PartialView("_DetailGrid", _Service.GetByMasterKey(keyValue));
        }

        public ActionResult DetailGridBatchUpdate(MVCxGridViewBatchUpdateValues<BA02BModel, int> updateValues, BA02AModel master)
        {
            string errMsg = "";
            //Step1: 先把update資料抓回來
            var entity = _Service.GetByKey(master.BA02A_ID) ?? new BA02A();
            var deleteCount = _Service.GetByDetailKeys(updateValues.DeleteKeys).Count;
            var updateKeys = updateValues.Update.Select(x => x.BA02B_ID).ToList();
            var detailUpdate = _Service.GetByDetailKeys(updateKeys).ToList();

            //Step2: call business before save 
            var state = entity.BA02A_ID == 0 ? EntityState.Added : EntityState.Modified;
            var result1 = BA02Business.BeforeSave(master, ref entity, state, ModelState);//資料已被刪除<br />"
            var result2 = BA02Business.BeforeSave(updateValues, detailUpdate, deleteCount, ModelState); //"明細資料已被刪除<br />請重新整理<br />" +"請檢查紅色驚嘆號<br />";

            errMsg = result1.Message + result2.Message;

            //Step3: call service save change
            if (errMsg == "")
            {
                errMsg += _Service.SaveChangeBatch(ref entity, state, result2.Insert, result2.Update, result2.Delete);
                master.BA02A_ID = entity.BA02A_ID;
            }
            return ResultHandler(errMsg, master, updateValues);
        }

        private ActionResult ResultHandler(string errMsg, BA02AModel master, MVCxGridViewBatchUpdateValues<BA02BModel, int> updateValues)
        {
            if (errMsg == "")
            {
                TempData["SelectedItemKey"] = master.BA02A_ID;
                TempData["SaveSuccess"] = true;
                return RedirectToAction("RedirectIndex");
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
                ViewData["DeleteIDList"] = deleteIDStrList;
                errMsg = "存儲失敗<br/>" + errMsg;
                ViewData["ErrMsg"] = errMsg = errMsg.Substring(0, errMsg.Length - 5);
                ViewData["MasterForm"] = ReadViewHelper.PartialView(this, "_MasterForm", master);

                return PartialView("_DetailGrid", BA02Business.FromEntity(_Service.GetByMasterKey(master.BA02A_ID)));
            }
        }
    }
}
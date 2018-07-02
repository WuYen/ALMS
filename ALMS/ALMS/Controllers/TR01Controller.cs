using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ALMS.Utilities;
using ALMS.ViewModels.TR01;
using ALMS.ViewModels.TR01.Service;

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
            ViewData["Master"] = new TR01AModel() { VOU_NO = "系統產生", DtTRN_DT = DateTime.Now };
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

        public ActionResult BA02BCombobox(int key)
        {
            ViewBag.DataSource = CacheCommonDataModule.GetBA02B(key);
            return PartialView("_BA02BCombobox");
        }
        public ActionResult BA01AGridLookUp()
        {
            return PartialView("_BA01AGridLookUp");
        }
    }
}
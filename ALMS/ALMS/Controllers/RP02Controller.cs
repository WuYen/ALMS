using ALMS.ViewModels.RP02.Service;
using ALMS.ViewModels.RP02;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;

namespace ALMS.Controllers
{
    public class RP02Controller : Controller
    {
        private RP02Service _Service = new RP02Service();
        // GET: RP02
        public ActionResult Index()
        {
            Session["RP02Data"] = null;
            return View();
        }

        public ActionResult Grid(SearchViewModel search)
        {
            return PartialView("_Grid", GetData(search, false));
        }

        public ActionResult GridCustomCall(SearchViewModel search)
        {
            return PartialView("_Grid", GetData(search, true));
        }

        private DataTable GetData(SearchViewModel search, bool reload)
        {
            //search.DateBeg = new DateTime(2018, 07, 01);
            //search.DateEnd = new DateTime(2018, 07, 31);
            var data = Session["RP02Data"] as DataTable;
            if (reload || data == null)
            {
                data = _Service.GetData(search.DateBegStr,search.DateEndStr);
                Session["RP02Data"] = data;
            }
            return data;
        }

        public ActionResult SetMonthClose(DateTime? DateSelector)
        {
            if (!DateSelector.HasValue)
            {
                return Content("請選擇日期");
            }
            else
            {
                var result = _Service.SetMonthClose(DateSelector.Value.ToString("yyyyMMdd"));
                if (result != "")
                {
                    return Content(result);
                }
                return Content("成功");
            }
        }
    }
}
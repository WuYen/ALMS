using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ALMS.ViewModels.RP03;
using ALMS.ViewModels.RP03.Service;

namespace ALMS.Controllers
{
    public class RP03Controller : Controller
    {
        private RP03Service _Service = new RP03Service();
        // GET: RP03
        public ActionResult Index()
        {
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
            search.Type = "";
            search.DateBeg = new DateTime(2018, 07, 01);
            search.DateEnd = new DateTime(2018, 07, 31);
            var data = Session["RP03Data"] as DataTable;
            if (reload || data == null)
            {
                var temp = _Service.GetData(search.Type,search.DateBegStr, search.DateEndStr);
                data = temp.Tables[0];
                ViewBag.Summary = temp.Tables[1].Rows[0];
                Session["RP03Data"] = data;
            }
            return data;
        }
    }
}
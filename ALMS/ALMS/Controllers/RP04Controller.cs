using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ALMS.ViewModels.RP04;
using ALMS.ViewModels.RP04.Service;

namespace ALMS.Controllers
{
    public class RP04Controller : Controller
    {
        private RP04Service _Service = new RP04Service();
        // GET: RP04
        public ActionResult Index()
        {
            Session["RP04Data"] = null;
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
            search.Type = "A";
            search.DateBeg = new DateTime(2018, 07, 01);
            search.DateEnd = new DateTime(2018, 07, 31);
            var data = Session["RP04Data"] as DataTable;
            if (reload || data == null)
            {
                data = _Service.GetData(search.Type, search.DateBegStr, search.DateEndStr);
                Session["RP04Data"] = data;
            }
            return data;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ALMS.ViewModels.RP01;
using ALMS.ViewModels.RP01.Service;

namespace ALMS.Controllers
{
    public class RP01Controller : Controller
    {
        private RP01Service _Service = new RP01Service();

        // GET: RP01
        public ActionResult Index()
        {
            Session["RP01Data"] = null;
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

        private List<RP01AModel> GetData(SearchViewModel search, bool reload)
        {
            var data = Session["RP01Data"] as List<RP01AModel>;
            if (reload || data == null)
            {
                data = RP01Business.FromEntity(_Service.GetMany(search));
                Session["RP01Data"] = data;
            }
            return data;

        }
    }
}
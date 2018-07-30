using ALMS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ALMS.Controllers
{
    public class UtilityController : Controller
    {
        // GET: Utility
        public ActionResult BA01ASelectPopup(string popupElementID)
        {
            ViewBag.PopupElementID = popupElementID ?? "";
            return PartialView("_BA01APopup");
        }

        public ActionResult BA01ALookUpGrid(string selectedItem, bool FirstPage = false)
        {
            if (selectedItem != null && selectedItem.Length > 0)
            {
                ViewData["selectedItem"] = selectedItem;
            }
            else
            {
                ViewData["FirstPage"] = FirstPage;
            }
            return PartialView("_BA01ALookUpGrid");
        }

        public ActionResult YearMonthSelector(string type = "Month", string extensionName= "DateSelector")
        {
            ViewBag.Type = type;
            ViewBag.ExtensionName = extensionName;
            return PartialView("_YearMonthSelector");
        }
    }
}
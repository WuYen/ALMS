using ALMS.Models;
using ALMS.Utilities;
using ALMS.ViewModels.BA04;
using ALMS.ViewModels.BA04.Service;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace ALMS.Controllers
{
    public class BA04Controller : Controller
    {
        private BA04Service _Service = new BA04Service();

        // GET: FA09
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Grid(SearchViewModel search, int? Edit_key)
        {
            var model = GetList(search);
            if (Edit_key.HasValue && model.Count(x => x.BA04A_ID == Edit_key) == 0)
            {
                ViewData["ErrMsg"] = "資料已被其他使用者刪除";
            }
            return PartialView("_Grid", model);
        }

        public ActionResult EditForm(BA04AModel model)
        {
            if (model == null || model.BA04A_ID == 0)//新增的時候初始化
            {
                ModelState.Clear();
            }
            return PartialView("_EditForm", model);
        }

        public ActionResult Insert(BA04AModel model, SearchViewModel search)
        {
            model.ModelState = ModelState;
            var result = Save(model, null, EntityState.Added);
            return ResultHandler(result, search);
        }

        public ActionResult Update(BA04AModel model, SearchViewModel search)
        {
            model.ModelState = ModelState;
            var item = _Service.GetByKey(model.BA04A_ID);
            var result = Save(model, item, EntityState.Modified);
            return ResultHandler(result, search);
        }

        public ActionResult Delete(int BA04A_ID, SearchViewModel search)
        {
            var item = _Service.GetByKey(BA04A_ID);
            var result = Save(null, item, EntityState.Deleted);

            return ResultHandler(result, search);
        }

        public ResultHelper<BA04A> Save(BA04AModel model, BA04A entity, EntityState state)
        {
            var result = BA04Business.BeforeSave(model, entity, state);

            result.Message += _Service.SaveChanges(result.Data, state, result.Message);

            return result;
        }

        private PartialViewResult ResultHandler(ResultHelper<BA04A> result, SearchViewModel search)
        {
            if (result.Message == "")
            {
                ViewData["Success"] = true;
            }
            else
            {
                ViewData["ErrMsg"] = result.Message;
                var master = BA04Business.FromEntity(result.Data);
                master.ModelState = ModelState;
                ViewData["ErrorData"] = master;
            }

            return PartialView("_Grid", GetList(search)); ;
        }

        private List<BA04AModel> GetList(SearchViewModel search)
        {
            //var sqlCmd = new CommonHelper().GetSqlCmd("BA04A", search);
            var queryData = _Service.GetMany(x => x.BA04A_ID > 0);
            if (!string.IsNullOrWhiteSpace(search.S_SET_NO))
            {
                queryData = queryData.Where(x => x.SET_NO.Contains(search.S_SET_NO));
            }
            if (!string.IsNullOrWhiteSpace(search.S_SET_NM))
            {
                queryData = queryData.Where(x => x.SET_NM.Contains(search.S_SET_NM));
            }
            return BA04Business.FromEntity(queryData.ToList());
        }
    }
}
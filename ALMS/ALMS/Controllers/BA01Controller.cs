using ALMS.Models;
using ALMS.Utilities;
using ALMS.ViewModels.BA01;
using ALMS.ViewModels.BA01.Service;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace ALMS.Controllers
{
    public class BA01Controller : Controller
    {
        private BA01Service _Service = new BA01Service();

        // GET: FA09
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Grid(SearchViewModel search, int? Edit_key)
        {
            var model = GetList(search);
            if (Edit_key.HasValue && model.Count(x => x.BA01A_ID == Edit_key) == 0)
            {
                ViewData["ErrMsg"] = "資料已被其他使用者刪除";
            }
            return PartialView("_Grid", model);
        }

        public ActionResult EditForm(BA01AModel model)
        {
            if (model == null || model.BA01A_ID == 0)//新增的時候初始化
            {
                ModelState.Clear();
            }
            return PartialView("_EditForm", model);
        }

        public ActionResult Insert(BA01AModel model, SearchViewModel search)
        {
            model.ModelState = ModelState;
            var result = Save(model, null, EntityState.Added);
            return ResultHandler(result, search);
        }

        public ActionResult Update(BA01AModel model, SearchViewModel search)
        {
            model.ModelState = ModelState;
            var item = _Service.GetByKey(model.BA01A_ID);
            var result = Save(model, item, EntityState.Modified);
            return ResultHandler(result, search);
        }

        public ActionResult Delete(int BA01A_ID, SearchViewModel search)
        {
            var item = _Service.GetByKey(BA01A_ID);
            var result = Save(null, item, EntityState.Deleted);

            return ResultHandler(result, search);
        }

        public ResultHelper<BA01A> Save(BA01AModel model, BA01A entity, EntityState state)
        {
            var result = BA01Business.BeforeSave(model, entity, state);

            result.Message += _Service.SaveChanges(result.Data, state, result.Message);

            return result;
        }

        private PartialViewResult ResultHandler(ResultHelper<BA01A> result, SearchViewModel search)
        {
            if (result.Message == "")
            {
                ViewData["Success"] = true;
            }
            else
            {
                ViewData["ErrMsg"] = result.Message;
                var master = BA01Business.FromEntity(result.Data);
                master.ModelState = ModelState;
                ViewData["ErrorData"] = master;
            }

            return PartialView("_Grid", GetList(search)); ;
        }

        private List<BA01AModel> GetList(SearchViewModel search)
        {
            var queryData = _Service.GetMany(x => x.BA01A_ID > 0);
            if (!string.IsNullOrWhiteSpace(search.S_ACC_NO))
            {
                queryData = queryData.Where(x => x.ACC_NO.Contains(search.S_ACC_NO));
            }
            if (!string.IsNullOrWhiteSpace(search.S_ACC_NM))
            {
                queryData = queryData.Where(x => x.ACC_NM.Contains(search.S_ACC_NM));
            }
            return BA01Business.FromEntity(queryData.ToList());
        }
    }
}
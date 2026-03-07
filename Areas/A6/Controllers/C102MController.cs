using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Turbo.MVC.Base3.DataLayers;
using Turbo.MVC.Base3.Models;
using System.Web.Mvc;
using Turbo.MVC.Base3.Areas.A6.Models;
using Turbo.MVC.Base3.Services;
using System.ComponentModel;
using WDACC.Models;

namespace Turbo.MVC.Base3.Areas.A6.Controllers
{
    public class C102MController : Turbo.MVC.Base3.Controllers.LogController
    {
        // GET: A6/C102M
        [HttpGet]
        [DisplayName("功能進入")]
        public ActionResult Index()
        {
            WDACC.Models.SessionModel sm = SessionModel.Get();
            C102MViewModel model = new C102MViewModel();
            model.Form.userdepart = "";

            return View(model.Form);
        }

        [HttpPost]
        [DisplayName("查詢")]
        public ActionResult Index(C102MFormModel form)
        {
            // 由登入資訊取得當前角色的檢定類別資訊
            SessionModel sm = SessionModel.Get();
            A6DAO dao = new A6DAO();
            if(form.methodname_SHOW!= null)
            {
                var t = form.methodname_SHOW.Distinct().ToList();
            }
            ActionResult rtn = View(form);
            if (ModelState.IsValid || form.useCache == 2)
            {
                ModelState.Clear();
                // 設定查詢分頁資訊
                dao.SetPageInfo(form.rid, form.p);

                // 查詢結果
                form.Grid = dao.QueryC102M(form);

                if (form.Grid.ToCount() == 0)
                {
                    form.Grid = null;
                    sm.LastErrorMessage = "查無資料, 請重新輸入查詢條件 !";
                }

                // 有 result id 資訊, 分頁連結, 返回 GridRows Partial View
                if (!string.IsNullOrEmpty(form.rid) && form.useCache == 0)
                {
                    rtn = PartialView("_GridRows", form);
                }

                // 設定分頁元件(_PagingLink partial view)所需的資訊
                base.SetPagingParams(form, dao, "Index");
            }
            return rtn;
        }

        [HttpGet]
        [DisplayName("檢視進入")]
        public ActionResult Modify(string id)
        {
            // 由登入資訊取得當前角色的檢定類別資訊
            SessionModel sm = SessionModel.Get();
            A6DAO dao = new A6DAO();
            C102MViewModel model = new C102MViewModel();
            model.Detail = new C102MDetailModel();

            model.Detail.id = id.TOInt32();
            model.Detail = dao.GetRow(model.Detail);

            if (model.Detail == null)
            { sm.LastErrorMessage = "找不到指定的資料!"; model.Detail = new C102MDetailModel(); }

            return View("Detail", model.Detail);
        }
    }
}

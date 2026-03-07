using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Turbo.MVC.Base3.DataLayers;
using Turbo.MVC.Base3.Models;
using System.Web.Mvc;
using Turbo.MVC.Base3.Areas.A6.Models;
using Turbo.MVC.Base3.Services;
using Turbo.MVC.Base3.Models.Entities;
using System.ComponentModel;
using WDACC.Models;

namespace Turbo.MVC.Base3.Areas.A6.Controllers
{
    public class C101MController : Turbo.MVC.Base3.Controllers.BaseController
    {
        // GET: A6/C101M
        [HttpGet]
        [DisplayName("功能進入")]
        public ActionResult Index()
        {
            SessionModel sm = SessionModel.Get();
            C101MViewModel model = new C101MViewModel();

            return View(model.Form);
        }

        [HttpPost]
        [DisplayName("查詢")]
        public ActionResult Index(C101MFormModel form)
        {
            // 由登入資訊取得當前角色的檢定類別資訊
            SessionModel sm = SessionModel.Get();
            A6DAO dao = new A6DAO();
            ActionResult rtn = View(form);
            if (ModelState.IsValid || form.useCache == 2)
            {
                ModelState.Clear();
                // 設定查詢分頁資訊
                dao.SetPageInfo(form.rid, form.p);

                // 查詢結果
                form.Grid = dao.QueryC101M(form);

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
        [DisplayName("新增進入")]
        public ActionResult New()
        {
            // 由登入資訊取得當前角色的檢定類別資訊
            SessionModel sm = SessionModel.Get();
            C101MViewModel model = new C101MViewModel();
            model.Detail = new C101MDetailModel();
            model.Detail.sys_id = "0";
            model.Detail.user_enabled = "1";

            return View("Detail", model.Detail);
        }

        [HttpGet]
        [DisplayName("編輯進入")]
        public ActionResult Modify(string user_name, string user_username)
        {
            // 由登入資訊取得當前角色的檢定類別資訊
            SessionModel sm = SessionModel.Get();
            A6DAO dao = new A6DAO();
            C101MViewModel model = new C101MViewModel();
            model.Detail = new C101MDetailModel();

            // 用名稱跟帳號搜尋詳細資料
            model.Detail.user_name = user_name;
            model.Detail.user_username = user_username;
            var getunit = dao.GetRow(model.Detail);
            model.Detail = getunit;

            // 帶出單位代碼
            //e_unit unit = new e_unit();
            //unit.unit_id = getunit.unit_id.TOInt32();
            //var getid = dao.GetRow(unit);
            //model.Detail.unit_id = getid.unit_id;



            if (model.Detail == null)
            { sm.LastErrorMessage = "找不到指定的資料!"; model.Detail = new C101MDetailModel(); }

            model.Detail.IsNew = false;

            return View("Detail", model.Detail);
        }

        [HttpGet]
        [DisplayName("設定權限")]
        public ActionResult SetPower(string user_name, string user_username)
        {
            // 由登入資訊取得當前角色的檢定類別資訊
            SessionModel sm = SessionModel.Get();
            A6DAO dao = new A6DAO();
            C101MViewModel model = new C101MViewModel();
            model.Detail1 = new C101MDetail1Model();

            // 查詢該選取人員
            e_user user = new e_user();
            user.user_name = user_name;
            user.user_username = user_username;
            var getid = dao.GetRow(user);

            // 帶出以勾選項目
            //e_permission permission = new e_permission();
            //permission.userid = getid.user_id;
            //var getpower = dao.GetRowList(permission);
            //var powerList = getpower.Select(m => m.funcid);
            //model.Detail1.sys_a6 = string.Join(",", powerList.Where(m => m.Contains("A6")).ToList());

            //// 暫存使用者ID
            //model.Detail1.getid = getid.user_id;

            if (model.Detail1 == null)
            { sm.LastErrorMessage = "找不到指定的資料!"; model.Detail = new C101MDetailModel(); }

            return View("Detail1", model.Detail1);
        }


        [HttpPost]
        [DisplayName("儲存")]
        public ActionResult Save(C101MDetailModel detail)
        {
            // 由登入資訊取得當前角色的檢定類別資訊
            SessionModel sm = SessionModel.Get();
            A6DAO dao = new A6DAO();
            ActionResult rtn = View("Detail", detail);

            // 檢核是否有填寫密碼(若設為Required Modify儲存時會卡在ModelState因編修不顯示密碼欄位)
            if (detail.IsNew)
            {
                if (string.IsNullOrEmpty(detail.user_password))
                {
                    ModelState.AddModelError("PWD", "請輸入密碼!");
                }
                if (detail.user_password != null)
                {
                    if (detail.user_password.Length < 8)
                    {
                        ModelState.AddModelError("PWD", "密碼長度設定最短為8碼!");
                    }
                }
                if (string.IsNullOrEmpty(detail.user_password_REPEAT))
                {
                    ModelState.AddModelError("PWD_REPEAT", "請輸入重複密碼!");
                }

            }

            if (ModelState.IsValid)
            {
                ModelState.Clear();

                // 檢核
                string ErrorMsg = dao.CheckC101M(detail);
                if (ErrorMsg == "")
                {
                    if (detail.IsNew)
                    {
                        dao.AppendC101MDetail(detail);
                        sm.LastResultMessage = "資料新增成功";
                    }
                    else
                    {
                        dao.UpdateC101MDetail(detail);
                        sm.LastResultMessage = "資料更新成功";
                    }

                    // 先刪除該使用者功能的權限
                   
                    // 根據選擇的整合權限來新增功能
                    var insert_list = new List<string>();

                    foreach (var item in insert_list)
                    {
                        AMFUNCM funWhere = new AMFUNCM();
                        funWhere.SYSID = item;

                        var funList = dao.GetRowList(funWhere).Where(m => m.PRGID != " ").ToList();
                        foreach (var funItem in funList)
                        {
                            e_permission data = new e_permission();
                            data.funcid = funItem.PRGID;
                            data.data = funItem.PRGNAME;
                            data.userid = detail.user_id.TOInt32();
                            data.mtime = DateTime.Now;
                            data.muser = detail.user_id.TOInt32();

                            dao.Insert(data);
                        }
                    }
                    sm.RedirectUrlAfterBlock = Url.Action("Index", "C101M", new { area = "A6", useCache = "2" });
                }
                else
                {
                    sm.LastErrorMessage = ErrorMsg;
                }
            }
            return rtn;
        }

        [HttpPost]
        [DisplayName("解鎖")]
        public ActionResult UnLock(C101MDetailModel detail)
        {
            // 由登入資訊取得當前角色的檢定類別資訊
            SessionModel sm = SessionModel.Get();
            A6DAO dao = new A6DAO();
            ActionResult rtn = View("Detail", detail);


            ModelState.Clear();

            dao.UnLockC101MDetail(detail);
            sm.LastResultMessage = "帳號解鎖成功";

            sm.RedirectUrlAfterBlock = Url.Action("Index", "C101M", new { area = "A6", useCache = "2" });


            return rtn;
        }

        [HttpPost]
        [DisplayName("權限儲存")]
        public ActionResult SavePower(C101MDetail1Model detail1)
        {
            // 由登入資訊取得當前角色的檢定類別資訊
            SessionModel sm = SessionModel.Get();
            A6DAO dao = new A6DAO();
            ActionResult rtn = View("Detail1", detail1);

            if (ModelState.IsValid)
            {
                ModelState.Clear();

                // 檢核
                string ErrorMsg = dao.CheckpowerC101M(detail1);
                if (ErrorMsg == "")
                {
                    dao.AppendpowerC101MDetail(detail1);
                    sm.LastResultMessage = "資料新增成功";

                    sm.RedirectUrlAfterBlock = Url.Action("Index", "C101M", new { area = "A6", useCache = "2" });
                }
                else
                {
                    sm.LastErrorMessage = ErrorMsg;
                }
            }
            return rtn;
        }

        [HttpPost]
        [DisplayName("刪除")]
        public ActionResult Delete(C101MDetailModel detail)
        {
            if (string.IsNullOrEmpty(detail.user_name))
            {
                throw new ArgumentNullException("Detail.user_name");
            }
            if (string.IsNullOrEmpty(detail.user_username))
            {
                throw new ArgumentNullException("Detail.user_username");
            }
            // 由登入資訊取得當前角色的檢定類別資訊
            SessionModel sm = SessionModel.Get();
            A6DAO dao = new A6DAO();

            dao.DeleteC101MDetail(detail);

            sm.LastResultMessage = "資料已刪除";
            sm.RedirectUrlAfterBlock = Url.Action("Index", "C101M", new { area = "A6", useCache = "2" });

            return View("Detail", detail);
        }
    }
}

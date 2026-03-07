using Turbo.DataLayer;
using Turbo.MVC.Base3.DataLayers;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Turbo.Commons;
using Turbo.MVC.Base3.Commons.Filter;
using Omu.ValueInjecter;
using Turbo.MVC.Base3.Models;
using Turbo.MVC.Base3.Commons;

namespace Turbo.MVC.Base3.Controllers
{
    /// <summary>
    /// 不需要登入（權限控管）就能使用系統的訪客來賓共用 Controller 基底類。像 WWWW/C502Q 功能就要繼承這個基底類別。
    /// </summary>
    /// <remarks>
    /// 請注意！若有修改到 SetPagingParams()、ComputePagingParams() 兩個方法程式碼時，
    /// 請務必同步修改在 BaseController.cs 內的 SetPagingParams()、ComputePagingParams() 兩個方法程式碼。
    /// 否則會導致系統分頁處理不一致問題。
    /// </remarks>
    public class BaseGuestController : Controller
    {
        protected static readonly ILog LOG = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly string CACHE_FORM_ACTION = "Index";

        /// <summary>資源存取鎖</summary>
        private static readonly object _ResxLock = new object();

        /// <summary>建立新的訪客使用者資訊物件</summary>
        /// <returns></returns>
        //private LoginUserInfo CreateGuestUserInfo()
        //{
        //    var ret = new LoginUserInfo();
        //    ret.UserNo = "_guest";
        //    ret.NetID = "2";
        //    ret.LoginAuth = "1";
        //    ret.LoginSuccess = false;
        //    ret.LoginIP = "127.0.0.1";
        //    //建立群組
        //    var grp = new ClamUserGroup();
        //    grp.GROUP = "_guest";
        //    grp.GROUP_NAME = "系統訪客";
        //    ret.Groups = new List<ClamUserGroup>();
        //    ret.Groups.Add(grp);
        //    //建立角色
        //    var role = new ClamUserRole();
        //    role.EXAMKIND = "_guest";
        //    role.EXAMKIND_NAME = "系統訪客";
        //    role.ROLE = "_guest";
        //    role.ROLE_NAME = "系統訪客";
        //    ret.Roles = new List<ClamUserRole>();
        //    ret.Roles.Add(role);
        //    //--------------------------------------------------------------------------
        //    ret.User = new ClamUser();
        //    ret.User.AUTHSTATUS = "9";
        //    ret.User.AUTHDESC = "未登入的訪客";
        //    ret.User.USERNO = ret.UserNo;
        //    ret.User.USERNAME = "訪客";
        //    ret.User.UNITID = "_guest";
        //    ret.User.UNIT_NAME = "系統訪客";
        //    ret.User.UNIT_NAME_FULL = ret.User.UNIT_NAME;
        //    var nowDt = DateTime.Now;
        //    ret.User.AUTHDATES = MyCommonUtil.TransToTwYYYMMDD(nowDt, "");
        //    ret.User.AUTHDATEE = MyCommonUtil.TransToTwYYYMMDD(nowDt.AddHours(24), "");
        //    return ret;
        //}

        /// <summary>預設建構子</summary>
        public BaseGuestController()
        {
            //var sm = SessionModel.Get();
            //if (sm.UserInfo == null)
            //{
            //    lock (_ResxLock)
            //    {
            //        if (sm.UserInfo == null)
            //        {
            //            //sm.RoleFuncs = new List<ClamRoleFunc>();
            //            //sm.UserInfo = CreateGuestUserInfo();
            //            sm.Role = sm.UserInfo.Roles[0];
            //        }
            //    }
            //}
        }

        /// <summary>
        /// 設定分頁元件所用到的參數:
        /// pagingModel: 查詢用的 FormModel, 
        /// dao: 執行查詢的DAO, 
        /// action: 分頁連結的 action 名稱
        /// </summary>
        /// <param name="pagingModel"></param>
        /// <param name="dao"></param>
        /// <param name="action"></param>
        protected void SetPagingParams(PagingResultsViewModel pagingModel, BaseDAO dao, string action)
        {
            SetPagingParams(pagingModel, dao, action, "");
        }

        /// <summary>
        /// 依據每頁筆數重新計笡總頁數，與適當調整當前頁次值
        /// </summary>
        /// <param name="info">分頁資訊</param>
        private void ComputePagingParams(PaginationInfo info)
        {
            if (info.PageSize == 0)
            {
                info.TotalPages = 1;
            }
            else
            {
                double v = (info.Total + (info.PageSize - 1)) / info.PageSize;
                info.TotalPages = (info.PageSize <= 0) ? 1 : Convert.ToInt32(Math.Floor(v));
            }

            if (info.PageIdx > info.TotalPages) info.PageIdx = info.TotalPages;
        }

        /// <summary>
        /// 設定分頁元件所用到的參數(指定客制化的分頁資料顯示 callback function):
        /// pagingModel: 查詢用的 FormModel, 
        /// dao: 執行查詢的DAO, 
        /// action: 分頁連結的 action 名稱, 
        /// callback: 分頁資料AJAX載入後會呼叫的 js callback function
        /// </summary>
        /// <param name="pagingModel">查詢用的 FormModel</param>
        /// <param name="dao">執行查詢的DAO</param>
        /// <param name="action">分頁連結的 action</param>
        /// <param name="callback">分頁資料AJAX載入後會呼叫的 js callback function</param>
        protected void SetPagingParams(PagingResultsViewModel pagingModel, BaseDAO dao, string action, string callback)
        {
            //20180608 設定使用者指定的每頁筆數
            dao.PaginationInfo.PageSize = pagingModel.PagingInfo.PageSize;
            pagingModel.PagingInfo = dao.PaginationInfo;
            //20180608 依據每頁筆數重新計笡總頁數，與適當調整當前頁次值
            ComputePagingParams(pagingModel.PagingInfo);

            pagingModel.rid = dao.ResultID;
            pagingModel.action = Url.Action(action);
            pagingModel.ajaxLoadPageCallback = callback;
        }

        /// <summary>
        /// 每個 action 被執行前會觸發這個 event
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string action = ControllerContextHelper.GetAction(filterContext);

            // 將任何的 Model Binding Exception 加到一般的 ModelError 中
            // 以便讓這些 Exception 有機會顯示出來
            var modelState = filterContext.Controller.ViewData.ModelState;
            if (!modelState.IsValid)
            {
                var modelStateErrors = modelState.Values.Where(E => E.Errors.Count > 0)
                    .SelectMany(E => E.Errors);

                List<string> exceptions = new List<string>();
                foreach (var item in modelStateErrors)
                {
                    if (item.Exception != null)
                    {
                        LOG.Error(action + ": ModelError: " + item.Exception.Message);
                        exceptions.Add(item.Exception.Message);
                    }
                }
                if(exceptions.Count > 0)
                {
                    modelState.AddModelError("Exception", string.Join("<br/>\n", exceptions.ToArray() ) );
                }
            }

            // FormModelCacheFilter OnActionExecuting 
            if (CACHE_FORM_ACTION.Equals(action))
            {
                (new FormModelCacheFilter()).OnActionExecuting(filterContext);
            }
            else
            {
                base.OnActionExecuting(filterContext);
            }
        }

        /// <summary>
        /// 每個 action 被執行後會觸發這個 event
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            string action = ControllerContextHelper.GetAction(filterContext);

            if (CACHE_FORM_ACTION.Equals(action))
            {
                (new FormModelCacheFilter()).OnActionExecuted(filterContext);
            }
            else
            {
                base.OnActionExecuted(filterContext);
            }
        }

        /// <summary>
        /// 載入當前 Controller 主查詢(Index) Cached Form Model,
        /// 可用來在 Controller 中直接呼叫 Index(form) 以返回查詢結果,
        /// 並包含其查詢條件及分頁結果狀態.
        /// <para>若找到 cached form 則回傳 true, 若找不到則回傳 false</para>
        /// </summary>
        /// <returns></returns>
        protected bool LoadCachedFormModel(PagingResultsViewModel form)
        {
            PagingResultsViewModel savedForm = null;
            string actionPath = ControllerContextHelper.GetActionPath(this.ControllerContext);
            string action = ControllerContextHelper.GetAction(this.ControllerContext);

            if(!string.IsNullOrEmpty(action))
            {
                // 去掉 context 取得 actionPath 中的 "action name"
                // 並置換為 CACHE_FORM_ACTION (Index)
                int p = actionPath.LastIndexOf(action);
                if(p > -1)
                {
                    actionPath = actionPath.Substring(0, p);
                    actionPath += CACHE_FORM_ACTION;
                }

                savedForm = (PagingResultsViewModel)ControllerContext.HttpContext.Session[actionPath];
                if(savedForm != null)
                {
                    LOG.Info("LoadCachedFormModel: load formModel for '" + actionPath + "', rid=" + form.rid);
                    if(form.useCache == 2)
                    {
                        savedForm.rid = "";
                    }
                    savedForm.useCache = form.useCache;
                    form.InjectFrom(savedForm);
                }
            }

            return (savedForm != null);
        }

        /// <summary>依據「網頁對話視窗類型」傳回對話視窗頁面的 HTTP 回應內容（即傳回 PartialView、View 兩者其中一個）</summary>
        /// <param name="dialog">網頁對話視窗類型。 ("": popupDialog，"W": popupWindow)</param>
        /// <param name="viewName">View 名稱</param>
        /// <param name="model">資料 Model</param>
        /// <returns></returns>
        protected virtual ActionResult BuildDialogViewResult(string dialog, string viewName, object model = null)
        {
            if (string.IsNullOrEmpty(dialog)) return PartialView(viewName, model);
            else
            {
                var masterName = "~/Views/Shared/_MainLayout_NoHeader_NoBg.cshtml";
                return View(viewName, masterName, model);
            }
        }
    }
}

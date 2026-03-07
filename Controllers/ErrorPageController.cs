using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Turbo.MVC.Base3.Commons;
using Turbo.MVC.Base3.Commons.Filter;

namespace Turbo.MVC.Base3.Controllers
{
    public class ErrorPageController : Controller /*Turbo.MVC.Base3.Controllers.BaseController*/
    {
        protected static readonly ILog LOG = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // GET: ErrorPage
        public ActionResult Index(int statusCode, string exMessage, bool isAjaxRequet)
        {
            Exception lastError = null;
            if (Session != null)
            {
                lastError = (Exception)Session["LastException"];
                if (!(lastError != null && lastError is Exception))
                {
                    lastError = new Exception("Session 中沒有 LastException 資訊");
                }
            }

            /*  設定在 ErrorPage 中是否要顯示錯誤 Exception 訊息及 Stacktrace,
                但若在 localhost 執行, 則不受此設定影響, 一律顯示 Exception 訊息及 Stacktrace:
                0. 不顯示
                1. 顯示 Exception Message
                2. 顯示 Exception Message 及 Stacktrace
             * */
            string show = System.Configuration.ConfigurationManager.AppSettings["ErrorPageShowExeption"];
            if (string.IsNullOrEmpty(show)) { show = "0"; }
            if (System.Web.HttpContext.Current.Request.IsLocal) { show = "2"; }
            if (!System.Web.HttpContext.Current.Request.IsLocal) { show = "0"; }
            ViewBag.Show = show;

            //2019-01-22 檢測連線問題
            ViewBag.HostIp2 = MyCommonUtil.GetLocalAddr("2");

            ViewBag.HostIP = "";
            if (System.Web.HttpContext.Current.Request.IsLocal)
            {
                ViewBag.HostIP = System.Web.HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"];
            }
            LOG.Error("ErrorPageController show:" + show);
            if (ViewBag.HostIP != null && !string.IsNullOrEmpty(ViewBag.HostIP))
            {
                LOG.Error("ErrorPageController ViewBag.HostIP:" + ViewBag.HostIP);
            }
            if (ViewBag.HostIp2 != null && !string.IsNullOrEmpty(ViewBag.HostIp2))
            {
                LOG.Error("ErrorPageController ViewBag.HostIp2:" + ViewBag.HostIp2);
            }
            LOG.Error("ErrorPageController exMessage:" + exMessage);
            if (lastError != null)
            {
                LOG.Error("ErrorPageController lastError.ToString():" + lastError.ToString());
            }


            ViewBag.Message = "";
            ViewBag.Exception = null;
            if ("2".Equals(show))
            {
                ViewBag.Message = exMessage;
                ViewBag.Exception = lastError;
            }
            else if ("1".Equals(show))
            {
                ViewBag.Message = exMessage;
                ViewBag.Exception = null;
            }

            //ViewBag.StatusCode = statusCode;
            ViewBag.Time = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Response.StatusCode = (statusCode == 404 ? statusCode : 404);
            return View("~/Views/Shared/Error.cshtml");
        }

        /// <summary>
        /// 沒有權限訊息頁面
        /// </summary>
        /// <returns></returns>
        public ActionResult UnAuth()
        {
            return View();
        }
    }
}
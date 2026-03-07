using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using log4net;

namespace Turbo.MVC.Base3.Commons
{
    /// <summary>
    /// 用來處理全域 Unhandled Exception 的 logging 及 routing 的類
    /// </summary>
    public class ExceptionHandler
    {
        protected ILog LOG = LogManager.GetLogger(typeof(ExceptionHandler));

        private Controller ErrorPageController = null;
        private string ControllerName = "ErrorPage";
        private string DefaultAction = "Index";

        public ExceptionHandler(Controller controller)
        {
            ErrorPageController = controller;
            ControllerName = controller.GetType().Name;
        }

        public string cst_EmailtoMe = "amuting@gmail.com";

        /// <summary>
        /// 取得錯誤相關資訊-lastError
        /// </summary>
        /// <param name="lastError"></param>
        /// <returns></returns>
        public string GetErrorMsg(Exception lastError)
        {
            string sMailBody = string.Empty;
            //string str_LocalAddr = MyCommonUtil.GetLocalAddr("2");
            sMailBody += string.Format("時間：{0}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff")); //存入時間
            //sMailBody += string.Format("LocalAddr：{0}\n", str_LocalAddr);
            if (lastError != null) { sMailBody += string.Format("LastError：{0}\n", lastError.ToString()); }
            //bool isHttpException = false;
            if (lastError == null) { lastError = new System.ArgumentNullException("沒有Exception資訊"); }
            return sMailBody;
        }

        /// <summary>
        /// 取得錯誤相關資訊-context/lastError
        /// </summary>
        /// <param name="context"></param>
        /// <param name="lastError"></param>
        /// <returns></returns>
        public string GetErrorMsg(HttpContext context, Exception lastError)
        {
            if (context == null) { return string.Empty; }
            if (lastError != null)
            {
                //System.Web.HttpResponse.set_StatusCode(Int32 value) 傳送 HTTP 標頭後，伺服器無法設定狀態
                string str_error1a = "傳送 HTTP 標頭後，伺服器無法設定狀態";
                string str_error1b = "System.Web.HttpResponse.set_StatusCode(Int32 value)";
                if (lastError.ToString().Contains(str_error1a) && lastError.ToString().Contains(str_error1b)) { return string.Empty; }

                //LastError：System.Web.HttpException(0x80004005): 具有潛在危險 Request.Path 的值已從用戶端 (:) 偵測到。
                //於 System.Web.HttpRequest.ValidateInputIfRequiredByConfig()
                //於 System.Web.HttpApplication.PipelineStepManager.ValidateHelper(HttpContext context)
                string[] ar_error2b = { "具有潛在危險", "Request.Path", "System.Web.HttpRequest.ValidateInputIfRequiredByConfig()", "System.Web.HttpApplication.PipelineStepManager.ValidateHelper(HttpContext context)" };
                bool flag_error2b = true;
                foreach (string str_V in ar_error2b) { if (!lastError.ToString().Contains(str_V)) { flag_error2b = false; break; } }
                if (flag_error2b) { return string.Empty; }
            }

            //HttpContext context
            //Exception lastError = context.Server.GetLastError();
            string sMailBody = string.Empty;
            string str_UserHostIp = MyCommonUtil.GetIpAddress(context);
            //ToString("yyyy-MM-dd HH:mm:ss")
            //sMailBody += string.Format("時間：{0}\n", DateTime.Now.ToString()); //存入時間
            sMailBody += string.Format("時間：{0}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff")); //存入時間
            sMailBody += string.Format("MachineName：{0}\n", context.Server.MachineName);
            sMailBody += string.Format("UserAgent：{0}\n", context.Request.UserAgent);
            sMailBody += string.Format("IpAddress：{0}\n", str_UserHostIp);
            //sMailBody += string.Format("UserHostAddress：{0}\n", context.Request.UserHostAddress);
            sMailBody += string.Format("UserHostName：{0}\n", context.Request.UserHostName);//UserHostAddress
            sMailBody += string.Format("Url：{0}\n", context.Request.Url);
            sMailBody += string.Format("RawUrl：{0}\n", context.Request.RawUrl);
            if (lastError != null)
            {
                sMailBody += string.Format("LastError：{0}\n", lastError.ToString());
                //sMailBody += string.Format("StackTrace：{0}\n", lastError.StackTrace.ToString());
            }

            //bool isHttpException = false;
            if (lastError == null)
            {
                lastError = new System.ArgumentNullException("沒有Exception資訊");
            }

            int i_statusCode = 500;
            if (lastError.GetType() == typeof(HttpException))
            {
                //isHttpException = true;
                i_statusCode = ((HttpException)lastError).GetHttpCode();
            }
            //if (i_statusCode == 404) { return string.Empty; }
            if (i_statusCode == 404)
            {
                return string.Empty;
                /*
                string[] s_aRawUrlRtnEmpty = { "robots.txt", "apple-touch-icon", "bootstrap.css.map", "browserconfig.xml", "ads.txt", ".php" };
                foreach (string s_v1 in s_aRawUrlRtnEmpty)
                {
                    if (context.Request.RawUrl.Contains(s_v1)) { return string.Empty; }
                }
                 */
            }

            HttpContextWrapper contextWrapper = new HttpContextWrapper(context);
            RouteData thisRouteData = RouteTable.Routes.GetRouteData(contextWrapper);
            if (thisRouteData == null) { sMailBody = string.Empty; return sMailBody; }

            string thisArea = (string)thisRouteData.DataTokens["area"];
            string thisController = (string)thisRouteData.Values["controller"];
            string thisAction = (string)thisRouteData.Values["action"];
            thisArea = (thisArea != null) ? "~/" + thisArea : "~";
            string exMessage = lastError.GetType().FullName + ":\n" + lastError.Message;
            string s_LOGError = string.Format("LOG_Error:\n {0}/ {1}/ {2}: {3}: {4}:\nexMessage: \n{5}\n", thisArea, thisController, thisAction, str_UserHostIp, i_statusCode, exMessage);
            sMailBody += s_LOGError;
            sMailBody += "--ServerVariables:\n" + MyCommonUtil.GetHTTP_HOST(context);
            return sMailBody;
        }

        public void RouteErrorPage(HttpContext context, Exception lastError)
        {
            bool isHttpException = false;
            if (lastError == null) { lastError = new System.ArgumentNullException("沒有Exception資訊"); }

            // Not an HTTP related error so this is a problem in our code, set status to
            // 500 (internal server error)
            int i_statusCode = 500;
            if (lastError.GetType() == typeof(HttpException))
            {
                isHttpException = true;
                i_statusCode = ((HttpException)lastError).GetHttpCode();
            }

            if (context.Session != null) { context.Session["LastException"] = lastError; }

            string str_UserHostIp = MyCommonUtil.GetIpAddress(context);
            // logging current RouteData and lastError
            HttpContextWrapper contextWrapper = new HttpContextWrapper(context);
            RouteData thisRouteData = RouteTable.Routes.GetRouteData(contextWrapper);
            string s_LOGError = string.Empty;
            if (thisRouteData == null)
            {
                s_LOGError = string.Format("thisRouteData == null; {0}: {1}: {2}\n", str_UserHostIp, context.Request.UserHostAddress, i_statusCode);
                LOG.Error(s_LOGError);
                LOG.Error(">>", lastError);
                return;
            }

            string thisArea = (string)thisRouteData.DataTokens["area"];
            string thisController = (string)thisRouteData.Values["controller"];
            string thisAction = (string)thisRouteData.Values["action"];
            thisArea = (thisArea != null) ? "~/" + thisArea : "~";

            //Exception / ] ERROR
            string exMessage = lastError.GetType().FullName + ": " + lastError.Message;
            s_LOGError = string.Format(": {0}/ {1}/ {2}: {3}: {4}: {5}: {6}\n", thisArea, thisController, thisAction, str_UserHostIp, context.Request.UserHostAddress, i_statusCode, exMessage);
            LOG.Error(s_LOGError);
            LOG.Error(">>", lastError);

            if (!isHttpException)
            {
                // pass exception to ErrorsPageController
                RouteData newRouteData = new RouteData();
                newRouteData.Values.Add("controller", ControllerName);
                newRouteData.Values.Add("action", DefaultAction);
                newRouteData.Values.Add("statusCode", i_statusCode);
                newRouteData.Values.Add("exMessage", exMessage);
                newRouteData.Values.Add("isAjaxRequet", contextWrapper.Request.IsAjaxRequest());

                IController controller = this.ErrorPageController;
                RequestContext requestContext = new RequestContext(contextWrapper, newRouteData);
                controller.Execute(requestContext);
            }

            //2018-12-25 fix資安 Struts REST Plugin Remote Code Execution
            //（eric協助debug）當.net偵到有異常的錯誤時改一律回傳404
            //連接-不為404-改為404
            if (context.Response.IsClientConnected && context.Response.StatusCode != 404) { context.Response.StatusCode = 404; }

            if (!context.Response.IsClientConnected)
            {
                //是否仍然連接到伺服器-未連接-清除資訊
                context.Response.Clear();
                return;
            }

            //連接-輸出強制中斷
            context.Response.End();
        }

      

    }

}
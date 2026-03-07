using log4net;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Turbo.Commons;
using Turbo.MVC.Base3.Commons;
using Turbo.MVC.Base3.Controllers;
using Turbo.MVC.Base3.Models;

//namespace Turbo.MVC.Base3
namespace WDACC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected static readonly ILog LOG = LogManager.GetLogger(typeof(MvcApplication));

        protected static Container container;

        //protected void Application_BeginRequest(object sender, EventArgs e)
        //{
        //    LOG.Info("## Application_BeginRequest");
        //    HttpRequest request = HttpContext.Current.Request;
        //    foreach (string xkey in request.Headers.AllKeys) { LOG.Info(string.Format("#request.Headers.Key: [{0}],[{1}]", xkey, request.Headers.Get(xkey))); }
        //    HttpResponse response = HttpContext.Current.Response;
        //    foreach (string xkey in response.Headers.AllKeys) { LOG.Info(string.Format("#response.Headers.Key: [{0}],[{1}]", xkey, response.Headers.Get(xkey))); }
        //}

        //protected void Application_PreSendRequestHeaders(object source, EventArgs e)
        //{
        //    LOG.Info("## Application_PreSendRequestHeaders");
        //    https://kevintsengtw.blogspot.com/2014/02/aspnet-mvc-response-headers.html
        //    response.Headers.Remove("server");
        //    HttpRequest request = HttpContext.Current.Request;
        //    foreach (string xkey in request.Headers.AllKeys) { LOG.Info(string.Format("#request.Headers.Key: [{0}],[{1}]", xkey, request.Headers.Get(xkey))); }
        //    HttpResponse response = HttpContext.Current.Response;
        //    foreach (string xkey in response.Headers.AllKeys) { LOG.Info(string.Format("#response.Headers.Key: [{0}],[{1}]", xkey, response.Headers.Get(xkey))); }
        //}

        public static Container GetContainer()
        {
            return container;
        }

        protected void Application_Start()
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch
                      (new System.IO.FileInfo(Server.MapPath("~/log4net.config")));

            LOG.Info("## Application_Start");

            // 加入自定義的 View/PartialView Location 設定
            ExtendedRazorViewEngine engine = ExtendedRazorViewEngine.Instance();
            // 報表模組-客制化報表結果顯示用的 PartialViews
            engine.AddPartialViewLocationFormat("~/Views/Report/Custom/{0}.cshtml");
            engine.Register();

            //加入技檢系統共用的資料 Model 綁定
            //ModelBinders.Binders.Add(typeof(ExamUnitModel), new ExamUnitModelBinder());

            Container container = new Container();
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();
            ContainerConfig.Register(container);
            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
            container.RegisterMvcIntegratedFilterProvider();

            container.Verify();
            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));

            MvcApplication.container = container;

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error()
        {
            LOG.Info("## Application_Error");
            // Code that runs when an unhandled error occurs
            Exception lastError = Server.GetLastError();

            // handled the exception and route to error page   
            ExceptionHandler handler = new ExceptionHandler(new ErrorPageController());
            string sMailBody = handler.GetErrorMsg(this.Context, lastError);
            if (!string.IsNullOrEmpty(sMailBody))
            {
                //handler.SendMailTest(sMailBody, handler.cst_EmailtoMe);
                LOG.Error("#Application_Error: " + sMailBody, lastError);
            }

            Server.ClearError();

            handler.RouteErrorPage(this.Context, lastError);
        }

        /// <summary>每次頁面的請求都會觸發</summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void Application_BeginRequest(object source, EventArgs e)
        {
            //嚴重風險HTTP Request Smuggling的解決方法。
            //Insecure Deployment: HTTP Request Smuggling(11621.11622)
            //資安特殊狀況hk1控制;
            bool flag_hk1 = chk_HK1();
            if (flag_hk1)
            {
                if (Response.IsClientConnected) { Response.StatusCode = 404; }
                Response.End();
                return;
            }
        }

        /// <summary> request header info </summary>
        /// <param name="s_vk1"></param>
        /// <returns></returns>
        string Get_HeaderValue_info(string s_vk1)
        {
            //string s_vk1 = "X-Scan-Memo";
            //string s_memo = string.Empty;
            string s_rst = string.Empty;
            if (s_vk1.Equals("-1"))
            {
                foreach (string s_k1 in Request.Headers.AllKeys)
                {
                    string[] s_amlist1 = Request.Headers.GetValues(s_k1);
                    if (s_amlist1 != null)
                    {
                        string s_rst2 = string.Empty;
                        foreach (string s_v1 in s_amlist1)
                        {
                            if (!string.IsNullOrEmpty(s_rst2)) { s_rst2 += ";"; }
                            s_rst2 += s_v1;
                        }
                        if (!string.IsNullOrEmpty(s_rst)) { s_rst += "\n"; }
                        s_rst += string.Concat("[", s_k1, "]=" ,s_rst2);
                    }
                }
            }
            else
            {
                StringComparison comp1 = StringComparison.OrdinalIgnoreCase;
                foreach (string s_k1 in Request.Headers.AllKeys)
                {
                    if (s_k1.IndexOf(s_vk1, comp1) > -1)
                    {
                        string[] s_amlist1 = Request.Headers.GetValues(s_k1);
                        if (s_amlist1 != null)
                        {
                            foreach (string s_v1 in s_amlist1)
                            {
                                if (!string.IsNullOrEmpty(s_rst)) { s_rst += ";"; }
                                s_rst += s_v1;
                            }
                        }
                    }
                }
            }
            //if (Request.Headers.AllKeys.Any(k => k.Equals(s_vk1))) { }
            return s_rst;
        }

        /// <summary>Insecure Deployment: HTTP Request Smuggling(11621.11622)</summary>
        /// <returns></returns>
        bool chk_HK1()
        {
            string cst_k1 = "X-Scan-Memo";
            string s_rst = Get_HeaderValue_info( cst_k1);
            StringComparison comp1 = StringComparison.OrdinalIgnoreCase;
            bool flag_hk1 = false;
            if (!string.IsNullOrEmpty(s_rst) && s_rst.Length > 0)
            {
                if (s_rst.IndexOf(@"Engine=""Http+Request+Smuggling""", comp1) > -1)
                {
                    LOG.Error(string.Format("##Application_BeginRequest [chk_HK1]:{0}:{1}", cst_k1, s_rst));
                    flag_hk1 = true;
                }
                //else { LOG.Info(string.Format("##Application_BeginRequest [chk_HK1]:{0}:{1}", cst_k1, s_rst)); }
            }
            return flag_hk1;
        }

    }
}

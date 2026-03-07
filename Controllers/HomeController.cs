using System.IO;
using System.Linq;
using System.Web.Mvc;
using Turbo.MVC.Base3.Commons.Filter;
using Turbo.MVC.Base3.DataLayers;
using Turbo.MVC.Base3.Models;
using Turbo.MVC.Base3.Services;

namespace Turbo.MVC.Base3.Controllers
{
    /// <summary>
    /// 首頁 Controller
    /// </summary>
    [BypassAuthorize]
    public class HomeController : Turbo.MVC.Base3.Controllers.LogController
    {
        public HomeController()
        {
        }

        /// <summary>
        /// 首頁
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            string str_METHOD = (Request != null) ? Request.QueryString["_method"] : null;
            if (str_METHOD != null && !str_METHOD.Equals("")) { return base.SetPageNotFound(); }
            //string str_Todo  = (Request != null) ? Request.QueryString["to"] : null;
            //if (str_Todo == null || !str_Todo.ToLower().Equals("home")) { return base.SetPageNotFound(); }
            return View();
            //if (UserID == null || string.IsNullOrEmpty(UserID)) { return HttpNotFound(); }
            //return HttpNotFound();
            //return RedirectToAction("Index", "Facade", new { id = "Home" });
        }

        /// <summary>
        /// 系統公告
        /// </summary>
        /// <returns></returns>
        public ActionResult News(string seq)
        {
            string str_METHOD = Request.QueryString["_method"];
            if (str_METHOD != null && !str_METHOD.Equals("")) { return base.SetPageNotFound(); }

            HomeViewModel model = new HomeViewModel();
            //AMDAO dao = new AMDAO();
            return View(model);
        }

       
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            string str_METHOD = Request.QueryString["_method"];
            if (str_METHOD != null && !str_METHOD.Equals("")) { return base.SetPageNotFound(); }

            HomeViewModel model = new HomeViewModel();

            ActionResult rtn = new RedirectResult(Url.Action("Index", "Home"));
            return rtn;
        }
    }
}

using Turbo.DataLayer;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Turbo.Commons;
using Turbo.MVC.Base3.Commons.Filter;
using Turbo.DataLayer.RowOpExtension;
using Turbo.MVC.Base3.Models;
using Turbo.MVC.Base3.Models.Entities;
using Turbo.MVC.Base3.Services;
using Turbo.MVC.Base3.DataLayers;
using WDACC.Models;

namespace Turbo.MVC.Base3.Controllers
{
    /// <summary>
    /// 儲存歷程紀錄的的共用 Controller 基底類
    /// </summary>
    public class LogController : BaseController
    {
        private RowBaseDAO dao = new RowBaseDAO();

        /// <summary>
        /// 每個 action 被執行後會觸發這個 event
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            // 取得動作DisplayName的Attr標籤
            int DisplayCount = filterContext.ActionDescriptor.GetCustomAttributes(typeof(System.ComponentModel.DisplayNameAttribute), true).ToCount();
            SessionModel sm = SessionModel.Get();
            if (DisplayCount != 0 && sm.LastActionFunc != null)
            {
                base.OnActionExecuted(filterContext);
                //A6DAO dao = new A6DAO();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WDACC.Commons;

namespace WDACC.Controllers
{
    public class ReportController : Controller
    {
        private MyModelBinder modelBinder;

        public ReportController(MyModelBinder modelBinder)
        {
            this.modelBinder = modelBinder;
        }

        // GET: Report
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Pdf(string id, FormCollection collection)
        {
            ActionResult result = null;
            switch(id) 
            {
                case "AdminExport":  // 資料匯出
                    break;

                default:
                    result = HttpNotFound();
                    break;
            }

            // TODO
            byte[] ba = null;
            return File(ba, "pdf");
        }

        public ActionResult Word(string id, FormCollection collection)
        {
            

            // TODO
            byte[] ba = null;
            return File(ba, "pdf");
        }

    }
}

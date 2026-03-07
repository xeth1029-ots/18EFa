using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WDACC.DataLayers;
using WDACC.Models;
using WDACC.Models.StoreExt;
using WDACC.Models.ViewModel;

namespace WDACC.Services
{
    public class CourseService
    {
        private static readonly ILog LOG = LogManager.GetLogger(typeof(CourseService));
        private MyBaseDAO dao;

        public CourseService(MyBaseDAO dao)
        {
            this.dao = dao;
        }

        public void GetCourseList(CourseViewModel model)
        {
            try
            {
                model.Grid = this.dao.QueryForListAll<Store>("Course.QueryCourseList", model.Form);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WDACC.Models.StoreExt;

namespace WDACC.Models.ViewModel
{
    public class CourseViewModel
    {
        public CourseFormModel Form { get; set; }
        public IList<Store> Grid { get; set; }

        public void Validate(ModelStateDictionary state)
        {

        }
    }

    /// <summary>
    /// 這是CourseFormModel
    /// </summary>
    public class CourseFormModel
    {

    }
}

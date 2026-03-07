using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WDACC.Models.StoreExt;

namespace WDACC.Models.ViewModel.Admin
{
    public class AdminMeetExportModel
    {
        /// <summary>
        /// 年度
        /// </summary>
        public int? Year { get; set; }

        /// <summary>
        /// 會議類型
        /// </summary>
        public int? MClass { get; set; }

        public IList<Store> Grid { get; set; }

    }

}

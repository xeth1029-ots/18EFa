using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WDACC.Models.Entities;
using WDACC.Models.StoreExt;

namespace WDACC.Models.ViewModel.Admin
{
    public class AdminRecruitModel
    {
        /// <summary>
        /// 
        /// </summary>
        public TeaSurvey Teasurvey { get; set; }

        /// <summary>
        /// 清單
        /// </summary>
        public IList<Store> Grid { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WDACC.Models.ViewModel.Member
{
    public class ScoreGridParameter
    {
        /// <summary>
        /// 年度
        /// </summary>
        public int? year { get; set; }

        /// <summary>
        /// 是否顯示於前台
        /// </summary>
        public int? showoff { get; set; }

        /// <summary>
        /// 審核狀態
        /// </summary>
        public string ckeckd { get; set; }

    }
}

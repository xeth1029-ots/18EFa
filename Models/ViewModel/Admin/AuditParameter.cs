using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WDACC.Models.ViewModel.Admin
{
    public class AuditParameter
    {
        /// <summary>
        /// 查詢年度
        /// </summary>
        public int? AuditYear { get; set; }

        /// <summary>
        /// 審核狀態
        /// </summary>
        public byte? Ckeckd { get; set; }

        /// <summary>
        /// 講師編號
        /// </summary>
        public long? Mid { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string RealName { get; set; }

    }
}

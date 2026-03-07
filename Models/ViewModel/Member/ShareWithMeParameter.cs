using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WDACC.Models.ViewModel.Member
{
    public class ShareWithMeParameter
    {
        public int? year { get; set; }

        public int? month { get; set; }

        /// <summary>
        /// 職能類別
        /// </summary>
        public string classid { get; set; }

    }
}

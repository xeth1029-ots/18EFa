using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WDACC.Models.StoreExt;

namespace WDACC.Models.ViewModel.Admin
{
    public class AdminScoreModel
    {

        public int? Year { get; set; }

        /// <summary>
        /// 講師積分資料清單
        /// </summary>
        public IList<Store> Grid { get; set; }

    }
}

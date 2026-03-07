using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WDACC.Models.StoreExt;

namespace WDACC.Models.ViewModel.Member
{
    public class NewsModel
    {
        public IList<Store> NewsList { get; set; }

        public Store NewsItem { get; set; }

    }
}

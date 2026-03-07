using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Turbo.DataLayer;
using WDACC.Models.Entities;
using WDACC.Models.StoreExt;
using WDACC.Models.ViewModel.Facade;

namespace WDACC.Models
{
    public class NewsModel
    {
        /// <summary>
        /// 顯示時是否只顯示資料行, 不顯示整個table
        /// </summary>
        public bool IsGridRowOnly { get; set; }

        public NewsModel()
        {
            this.NewsItem = new News();
        }

        public PagingModel Paging { get; set; }

        /// <summary>
        /// 最新消息
        /// </summary>
        public IList<Store> Grid { get; set; }

        /// <summary>
        /// 置頂 
        /// </summary>
        public IList<Store> HotGrid { get; set; }

        public News NewsItem { get; set; }

        public IList<NewsFile> NewsFiles { get; set; }

    }
}

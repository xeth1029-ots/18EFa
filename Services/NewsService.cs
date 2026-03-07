using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Turbo.DataLayer;
using Turbo.MVC.Base3.DataLayers;
using WDACC.DataLayers;
using WDACC.Models;
using WDACC.Models.Entities;
using WDACC.Models.StoreExt;

namespace WDACC.Services
{
    public class NewsService
    {
        protected static readonly ILog LOG = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //private static readonly ILog LOG = LogManager.GetLogger(typeof(NewsService));
        private MyBaseDAO dao;

        public FormCollection collection;

        /// <summary>
        /// 最新消息消單
        /// </summary>
        public IList<Store> Grid { get; set; }

        public NewsService(MyBaseDAO dao)
        {
            this.dao = dao;
        }

        /// <summary>
        /// 顯示在前台的最新消息清單(不分頁)
        /// </summary>
        /// <param name="model"></param>
        public void GetHomeNewsList(NewsModel model)
        {
            Store param = new Store();
            param["class"] = 2;
            param["ishot"] = "Y";
            model.HotGrid = this.dao.QueryForListAll<Store>("News.GetNewsList", param);

            param["ishot"] = "N";
            model.Grid = this.dao.QueryForListAll<Store>("News.GetNewsList", param);

            model.Grid = model.Grid.Where(x =>
                x.Get("sdate").AsDateTime() <= DateTime.Now.Date &&
                x.Get("edate").AsDateTime() >= DateTime.Now.Date &&
                x.Get("showoff").AsText() == "1")
                .Take(10).ToList();

            model.HotGrid = model.HotGrid.Where(x =>
                x.Get("sdate").AsDateTime() <= DateTime.Now.Date &&
                x.Get("edate").AsDateTime() >= DateTime.Now.Date &&
                x.Get("showoff").AsText() == "1")
                .Take(5).ToList();
        }

        /// <summary>
        /// 顯示在前台的最新消息清單(多頁)
        /// </summary>
        /// <param name="model"></param>
        public void GetNewsList(NewsModel model)
        {
            Store param = new Store();
            param["class"] = 2;
            param["showoff"] = 1;
            param["today"] = DateTime.Now.Date;
            model.Grid = this.dao.QueryForList<Store>("News.GetNewsList", param);
        }

        /// <summary>
        /// 取得最新消息明細
        /// </summary>
        /// <param name="model"> model.NewsItem.id </param>
        public void GetNewsDetail(NewsModel model)
        {
            try
            {
                if (model.NewsItem.id <= 0)
                {
                    throw new Exception("未提供最新消息代碼參數");
                }

                Store param = new Store();
                param["id"] = model.NewsItem.id;
                Store store = this.dao.QueryForObject<Store>("News.GetDetail", param);
                model.NewsItem = store.ConvertTo<News>();
                // model.NewsItem.@class = store.Get("class").AsInt32();  // 欄位名稱包含c#保留字, 特別欄位處理
                model.NewsFiles = this.dao.GetRowList<NewsFile>(new NewsFile { news_id = model.NewsItem.id });

            }
            catch (Exception ex)
            {
                LOG.Error(ex.Message, ex);
                throw ex;
            }
        }

    }
}

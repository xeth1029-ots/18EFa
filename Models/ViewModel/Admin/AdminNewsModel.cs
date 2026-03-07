using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Turbo.MVC.Base3.Commons;
using WDACC.Models.Entities;
using WDACC.Models.StoreExt;

namespace WDACC.Models.ViewModel.Admin
{
    public class AdminNewsModel
    {
        public AdminNewsModel()
        {
            //this.Sdate = new TwDate();
            //this.Edate = new TwDate();
            //this.Sdate.SetDate(DateTime.Now);
            //this.Edate.SetDate(DateTime.Now);
            this.TabPaging1 = new PagingModel();
            this.TabPaging2 = new PagingModel();
            this.TabPaging3 = new PagingModel();
            this.TabPaging4 = new PagingModel();
            this.NewsForm = new News();
        }

        public PagingModel TabPaging1 { get; set; }
        public PagingModel TabPaging2 { get; set; }
        public PagingModel TabPaging3 { get; set; }
        public PagingModel TabPaging4 { get; set; }

        public byte? ClassId { get; set; }

        public byte? Showoff { get; set; }

        /// <summary>
        /// 編輯表單
        /// </summary>
        public News NewsForm { get; set; }

        /// <summary>
        /// 開始日期
        /// </summary>
        public string Sdate { get; set; }

        /// <summary>
        /// 截止日期
        /// </summary>
        public string Edate { get; set; }

        /// <summary>
        /// 檔案
        /// </summary>
        public IList<NewsFile> NewsFiles { get; set; }

        /// <summary>
        /// 檔案刪除註記列表   
        /// </summary>
        public IList<bool> FileRemoveList { get; set; }

        /// <summary>
        /// 上傳檔案
        /// </summary>
        public IList<HttpPostedFileBase> Files { get; set; }

        /// <summary>
        /// 檔案說明清單 
        /// </summary>
        public IList<string> Comments { get; set; }

        /// <summary>
        /// 最新消息清單
        /// </summary>
        public IList<Store> Grid1 { get; set; }
        public IList<Store> Grid2 { get; set; }
        public IList<Store> Grid3 { get; set; }
        public IList<Store> Grid4 { get; set; }

        public void PrepareAfterLoad()
        {
            this.Sdate = MyCommonUtil.TransToYYYYMMDD(this.NewsForm.sdate);
            this.Edate = MyCommonUtil.TransToYYYYMMDD(this.NewsForm.edate);
        }

        public void PrepareBeforeSave()
        {
            this.NewsForm.sdate = MyCommonUtil.TransToDateTime(this.Sdate);
            this.NewsForm.edate = MyCommonUtil.TransToDateTime(this.Edate);
        }

    }
}


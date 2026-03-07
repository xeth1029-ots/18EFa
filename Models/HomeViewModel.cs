using System.Collections.Generic;
using Turbo.DataLayer;

namespace Turbo.MVC.Base3.Models
{
    /// <summary>
    /// 首頁 View Model
    /// </summary>
    public class HomeViewModel
    {
        public HomeViewModel()
        {
        }

        /// <summary>
        /// 一開始顯示筆數
        /// </summary>
        public int _lstGroup { get { return 5; } }

        public List<NewsGridModel> News { get; set; }
    }
    #region 系統公告

    /// <summary>
    /// 系統公告(列表)
    /// </summary>
    public class NewsGridModel
    {
        public NewsGridModel()
        {
        }
    }

    /// <summary>
    /// 系統公告(明細)
    /// </summary>
    public class NewsDetailModel
    {
        public NewsDetailModel()
        {
            // 檔案上傳/下載元件
            this.Upload = new SystemUploadFile();
        }

        /// <summary>
        /// 設定上傳參數
        /// </summary>
        public void SetUploadParm()
        {
            Upload.OutModelName = "";
            Upload.ShowFileUpload = false;
            Upload.ShowDelete = false;
            Upload.FILEPKEY1 = "";
            Upload.FILEPKEY2 = 0.ToString();
            Upload.GetFileGrid();
        }

        /// <summary>
        /// 檔案上傳套件
        /// </summary>
        [NotDBField]
        public SystemUploadFile Upload { get; set; }
    }

    #endregion 系統公告

}
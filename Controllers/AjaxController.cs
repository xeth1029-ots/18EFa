using System;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.Web.Mvc;
using log4net;
using Turbo.MVC.Base3.DataLayers;
using Turbo.MVC.Base3.Commons;
using Turbo.MVC.Base3.Commons.Filter;
using Turbo.MVC.Base3.Models;
using Turbo.Commons;
using Turbo.MVC.Base3.Services;
using System.Web.Script.Serialization;
using System.Linq;
using System.IO;
using Turbo.MVC.Base3.Models.Entities;
using WDACC.Models;

namespace Turbo.MVC.Base3.Controllers
{
    /// <summary>
    /// 這個類集中放置一些 Ajax 動作會用的的下拉代碼清單控制 action  
    /// </summary>
    [BypassAuthorize]
    public class AjaxController : Turbo.MVC.Base3.Controllers.BaseController
    {
        #region GetZIP_CO 傳回郵遞區號中文名稱(單筆)
        /// <summary>
        /// Ajax 傳回郵遞區號中文名稱(單筆)
        /// </summary>
        /// <param name="ZIP_CO">郵遞區號代碼</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetZIP_CO(string CODE)
        {
            var result = new AjaxResultStruct();
            var dao = new MyKeyMapDAO();
            result.data = dao.GetZIP_COName(CODE);
            return Content(result.Serialize(), "application/json");
        }
        #endregion

        #region Gete_unit 傳回單位中文名稱(單筆)
        /// <summary>
        /// Ajax 傳回單位中文名稱(單筆)
        /// </summary>
        /// <param name="e_unit">路段代碼</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Gete_unit(string CODE)
        {
            var result = new AjaxResultStruct();
            var dao = new MyKeyMapDAO();
            result.data = dao.Gete_unitName(CODE);
            return Content(result.Serialize(), "application/json");
        }
        #endregion

        #region 上傳檔案

        /// <summary>
        /// Ajax 上傳檔案/SystemUploadFile
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadFile(SystemUploadFile Upload)
        {
            SessionModel sm = SessionModel.Get();
            // 如果有任何檔案類型才做
            if (Request.Files.AllKeys.Any())
            {
                // 讀取指定的上傳檔案ID
                var httpPostedFile = Request.Files[0];
                // 副檔名
                var Extension = httpPostedFile.FileName.ToSplit('.').LastOrDefault();
                // 允許附檔名
                bool accepted = false;
                IList<AcceptFileType> acceptTypes = Upload.GetAcceptFileTypes();
                if (acceptTypes != null)
                {
                    string ext = Extension;
                    foreach (AcceptFileType type in acceptTypes)
                    {
                        if (type.Equals(ext)) accepted = true;
                    }
                }
                if (!accepted)
                {
                    Upload.ErrorMsg = "副檔名不允許，請檢查檔案後再重新上傳";
                }
                if (httpPostedFile.ContentLength > 10485760)
                {
                    Upload.ErrorMsg = "檔案大於10M，請檢查檔案後再重新上傳";
                }
                if (Upload.fileGrid != null && Upload.fileGrid.Count > 0 && Upload.FILEPKEY1 == "A5/C107M")
                {
                    Upload.ErrorMsg = "檔案上傳數量超過限制";
                }
                if (Upload.ErrorMsg.TONotNullString() == "")
                {
                    // 真實有檔案，進行上傳
                    if (httpPostedFile != null && httpPostedFile.ContentLength != 0)
                    {
                        string Path = "~" + ConfigModel.UploadTempPath + "/" + Upload.FILEPKEY1;
                        string PathDir = Server.MapPath(Path);
                        string file_name = httpPostedFile.FileName.ToSplit('\\').Last();
                        string FileName = DateTime.Now.ToTwNowTime() + file_name.Replace(" ", "_");
                        if (!Directory.Exists(PathDir)) Directory.CreateDirectory(PathDir);

                        string FullPathDir = PathDir + "/" + FileName;
                        httpPostedFile.SaveAs(FullPathDir);

                        TblFILEGRID fileItem = new TblFILEGRID();
                        fileItem.FILENAME = FileName;
                        fileItem.FILEPATH = Path;
                        fileItem.FILECAPACTIY = httpPostedFile.ContentLength.TONotNullString();
                        fileItem.FILEPKEY1 = Upload.FILEPKEY1;
                        fileItem.FILEPKEY2 = Upload.FILEPKEY2;
                        fileItem.FILEPKEY3 = Upload.FILEPKEY3;
                        fileItem.MODIP = Request.UserHostAddress;
                        fileItem.MODTIME = HelperUtil.DateTimeToLongTwString(DateTime.Now);
                        fileItem.MODUSERID = sm.UserInfo.UserNo;
                        fileItem.MODUSERNAME = sm.UserInfo.User.user_username;

                        // 模擬上傳的檔案內容
                        if (Upload.fileGrid.ToCount() == 0) Upload.fileGrid = new List<TblFILEGRID>();
                        Upload.fileGrid.Add(fileItem);
                    }
                }
            }

            //## 將結果回傳
            return PartialView("SystemUploadFile", Upload);
        }

        /// <summary>
        /// Ajax 刪除檔案/SystemUploadFile
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteFile(SystemUploadFile Upload, int idx)
        {
            if (Upload.fileGrid != null)
            {
                // 模擬上傳的檔案內容
                Upload.fileGrid.RemoveAt(idx);
            }

            // 將結果回傳
            return PartialView("SystemUploadFile", Upload);
        }

        #endregion 上傳檔案

        #region Gete_user 傳回對應的username資料(單筆)
        /// <summary>
        /// Ajax 傳回對應的username資料(單筆)
        /// </summary>
        /// <param name="VTC_EDUDATA">機構資料</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Gete_user(string CODE)
        {
            var result = new AjaxResultStruct();
            var dao = new MyKeyMapDAO();
            var data = dao.get_user(CODE);
            result.data = data;
            return Content(result.Serialize(), "application/json");
        }
        #endregion

        #region Gete_user_id 傳回對應的user_id資料(單筆)
        /// <summary>
        /// Ajax 傳回對應的user_id資料(單筆)
        /// </summary>
        /// <param name="VTC_EDUDATA">機構資料</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Gete_user_id(string CODE)
        {
            var result = new AjaxResultStruct();
            var dao = new MyKeyMapDAO();
            var data = dao.get_user_id(CODE);
            result.data = data;
            return Content(result.Serialize(), "application/json");
        }
        #endregion
    }
}

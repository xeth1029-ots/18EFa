using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;
using Turbo.MVC.Base3.Commons;
using WDACC.Commons;
using WDACC.Services;

namespace WDACC.Controllers
{
    public class FileController : Controller
    {
        private FileService fileService;
        private LogService log;
        private MyModelBinder modelBinder;
        private LogCollection lc;

        protected static readonly ILog LOG = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public FileController(FileService fileService, MyModelBinder modelBinder, LogService log, LogCollection lc)
        {
            this.fileService = fileService;
            this.modelBinder = modelBinder;
            this.log = log;
            this.lc = lc;
        }

        /// <summary> 下載檔案 </summary>
        /// <param name="id"></param>
        /// <param name="fid"></param>
        /// <returns></returns>
        public ActionResult Download(string id, string fid)
        {
            //輸入值檢核
            long i_fid = -1;
            long? ifid = null;
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(fid)) { return HttpNotFound(); }
            else if (!long.TryParse(fid, out i_fid)) { return HttpNotFound(); }
            else if (i_fid < 0) { return HttpNotFound(); }
            else { ifid = i_fid; }
            if (!ifid.HasValue) { return HttpNotFound(); }

            ActionResult result = null;
            Tuple<string, byte[]> file = null;
            ActionName.File action = MyCommonUtil.ToEnum<ActionName.File>(id);
            this.modelBinder.SetContext(this.ControllerContext, this.Binders, this.ModelState);
            string contentType = null;
            bool isLoging = true;
            lc.SetActionContext(action);

            try
            {
                switch (action)
                {
                    case ActionName.File.Score: // 積分佐證資料
                        file = this.fileService.DownloadScoreFile(ifid);
                        break;

                    case ActionName.File.News:  // 最新消息
                        file = this.fileService.DownloadNewsFile(ifid);
                        break;

                    case ActionName.File.ShareSFile: // 檔案分享 教材下載
                        file = this.fileService.DownloadShareFile(ifid, action);
                        break;

                    case ActionName.File.ShareIFile: // 檔案分享 說明書下載
                        file = this.fileService.DownloadShareFile(ifid, action);
                        break;

                    case ActionName.File.ShareAFile: // 檔案分享 授權書下載
                        file = this.fileService.DownloadShareFile(ifid, action);
                        break;

                    case ActionName.File.TeachShow: // 授課花絮
                        file = this.fileService.DownloadTeachShowFile(ifid);
                        contentType = "image/jpeg";
                        isLoging = false;   // 下載圖片檔不須寫log
                        break;

                    default:
                        result = HttpNotFound();
                        break;
                }

                if (isLoging)
                {
                    if (file != null && file.Item1 != null)
                    {
                        // 收集log參數
                        this.lc.AddFileCollection(
                                ActionName.EditStatus.SUCCESS,
                                ActionName.FileEditType.DOWNLOAD,
                                file.Item1, file.Item1, null);
                    }
                }
            }
            catch (Exception ex)     // 查無檔案或其它例外狀況
            {
                LOG.Error(ex.Message, ex);

                // 收集log參數
                if (file != null && file.Item1 != null)
                {
                    this.lc.AddFileCollection(
                        ActionName.EditStatus.FAIL,
                        ActionName.FileEditType.DOWNLOAD,
                        file.Item1, file.Item1, ex.Message);
                }
                else
                {
                    this.lc.AddFileCollection(
                        ActionName.EditStatus.FAIL,
                        ActionName.FileEditType.DOWNLOAD,
                        null, null, ex.Message);
                }

                result = Content(string.Format("<div>{0}</div>", ex.Message));
            }

            if (contentType == null) { contentType = "application/octet-stream"; }

            if (result == null)
            {
                if (file == null || file.Item1 == null) { return HttpNotFound(); }

                byte[] ba = null;
                if (file == null || file.Item2 == null)
                {
                    ba = new byte[] { };
                }
                else
                {
                    ba = file.Item2;
                }
                result = File(ba, contentType, file.Item1);
            }

            log.WriteLogWithCollection();   // 寫入log

            return result;
        }

        /// <summary>
        /// 圖檔轉base64供預覽。
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public ActionResult ImageToBase64(HttpPostedFileBase file)
        {
            string res = string.Empty;
            using (var memoryStream = new MemoryStream())
            {
                file.InputStream.CopyTo(memoryStream);
                string base64 = System.Convert.ToBase64String(memoryStream.ToArray());
                res = string.Format("data:{0};base64,{1}", file.ContentType, base64);
                // data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAADoAAAA6CAYAAADhu0ooAAAFP0lEQVR4nO2bX0gcRxzH
            }
            return Content(res, "text/plain");
        }

    }
}

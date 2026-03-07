using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using WDACC.Commons;
using WDACC.DataLayers;
using WDACC.Models;
using WDACC.Models.Entities;
using FileShare = WDACC.Models.Entities.FileShare;

namespace WDACC.Services
{
    public class FileService
    {
        private LogService log;
        private MyBaseDAO dao;
        private MyModelBinder modelBinder;
        private LogCollection logCollection;

        protected static readonly ILog LOG = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public FileService(LogService log,
            MyBaseDAO dao,
            MyModelBinder modelBinder,
            LogCollection logCollection)
        {
            this.log = log;
            this.dao = dao;
            this.modelBinder = modelBinder;
            this.logCollection = logCollection;
        }

        /// <summary>
        /// 上傳積分檔案
        /// </summary>
        /// <param name="file"></param>
        public Tuple<string, string> UploadScoreFile(HttpPostedFileBase file, string postfix)
        {
            Tuple<string, string> filenames = null;
            string saveFileName = null;
            try
            {
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string ext = Path.GetExtension(file.FileName);
                saveFileName = string.Format("f{0}{1}{2}", fileName, postfix, ext);
                string savePath = string.Format("~/Uploads/Score/{0}", saveFileName);

                string realPath = this.modelBinder.controllerContext.HttpContext.Server.MapPath(savePath);

                //XLS、DOC、PPT、PDF、JPG、PNG、MSG，ODT、ODF
                string s_file_accept = Turbo.MVC.Base3.Models.ConfigModel.Get_FileExtAccept;
                string s_file_accept_upper = s_file_accept.ToUpper().Replace(",", "、");
                var s_filePost = (postfix == "1" ? "佐證資料" : "檔案");
                if (!FileTypeDetector.IsFileTypeValid(file, savePath))
                {
                    var ex3 = new Exception(string.Concat(file.FileName, string.Concat("上傳-", s_filePost, "內容有誤 無效的檔案格式(限", ext, "檔案內容)！")));
                    LOG.Error(ex3.Message, ex3);
                    //this.logCollection.AddFileCollection(ActionName.EditStatus.FAIL, ActionName.FileEditType.UPLOAD, file.FileName, saveFileName, ex3.Message);
                    throw ex3;
                }

                file.SaveAs(realPath);

                filenames = new Tuple<string, string>(saveFileName, savePath);

                // Add file log
                this.logCollection.AddFileCollection(ActionName.EditStatus.SUCCESS, ActionName.FileEditType.UPLOAD, file.FileName, saveFileName, null);
            }
            catch (Exception ex)
            {
                LOG.Error(ex.Message, ex);
                this.logCollection.AddFileCollection(ActionName.EditStatus.FAIL, ActionName.FileEditType.UPLOAD, file.FileName, saveFileName, ex.Message);
                throw ex;
            }

            return filenames;
        }

        /// <summary>上傳分享檔案</summary>
        /// <param name="file"></param>
        /// <returns>Item1:saveFileName, Item2: savePath </returns>
        public Tuple<string, string> UploadShareFile(HttpPostedFileBase file, string postfix)
        {
            Tuple<string, string> filenames = null;
            string saveFileName = null;
            try
            {
                filenames = new Tuple<string, string>(saveFileName, null);
                if (file == null) { return filenames; }

                string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string ext = Path.GetExtension(file.FileName);
                saveFileName = string.Format("f{0}{1}{2}", fileName, postfix, ext);
                string savePath = string.Format("~/Uploads/Share/{0}", saveFileName);
                string realPath = this.modelBinder.controllerContext.HttpContext.Server.MapPath(savePath);

                //XLS、DOC、PPT、PDF、JPG、PNG、MSG，ODT、ODF
                string s_file_accept = Turbo.MVC.Base3.Models.ConfigModel.Get_FileExtAccept;
                string s_file_accept_upper = s_file_accept.ToUpper().Replace(",", "、");
                var s_filePost = (postfix == "" ? "教材檔案" : postfix == "-1" ? "說明書檔案" : postfix == "-2" ? "授權書檔案" : "檔案");
                if (!FileTypeDetector.IsFileTypeValid(file, savePath))
                {
                    var ex3 = new Exception(string.Concat(file.FileName, string.Concat("上傳-", s_filePost, "內容有誤 無效的檔案格式(限", ext, "檔案內容)！")));
                    LOG.Error(ex3.Message, ex3);
                    this.logCollection.AddFileCollection(ActionName.EditStatus.FAIL, ActionName.FileEditType.UPLOAD, file.FileName, saveFileName, ex3.Message);
                    throw ex3;
                }

                file.SaveAs(realPath);

                filenames = new Tuple<string, string>(saveFileName, savePath);
                // add file log
                this.logCollection.AddFileCollection(ActionName.EditStatus.SUCCESS, ActionName.FileEditType.UPLOAD, file.FileName, saveFileName, null);
            }
            catch (Exception ex)
            {
                LOG.Error(ex.Message, ex);
                this.logCollection.AddFileCollection(ActionName.EditStatus.FAIL, ActionName.FileEditType.UPLOAD, file.FileName, saveFileName, ex.Message);
                throw ex;
            }

            return filenames;
        }

        /// <summary>上傳最新消息檔案</summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public Tuple<string, string> UploadNewsFile(HttpPostedFileBase file, string postfix)
        {
            Tuple<string, string> filenames = null;
            try
            {
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string ext = Path.GetExtension(file.FileName);
                string saveFileName = string.Format("n{0}{1}{2}", fileName, postfix, ext);
                string savePath = string.Format("~/Uploads/News/{0}", saveFileName);
                string realPath = this.modelBinder.controllerContext.HttpContext.Server.MapPath(savePath);

                //XLS、DOC、PPT、PDF、JPG、PNG、MSG，ODT、ODF
                string s_file_accept = Turbo.MVC.Base3.Models.ConfigModel.Get_FileExtAccept;
                string s_file_accept_upper = s_file_accept.ToUpper().Replace(",", "、");
                var s_filePost = string.Concat(postfix, "檔案");  //var s_filePost = (postfix == "1" ? "積分檔案" : "檔案");
                if (!FileTypeDetector.IsFileTypeValid(file, savePath))
                {
                    var ex3 = new Exception(string.Concat(file.FileName, string.Concat("上傳-", s_filePost, "內容有誤 無效的檔案格式(限", ext, "檔案內容)！")));
                    LOG.Error(ex3.Message, ex3);
                    throw ex3;
                }

                file.SaveAs(realPath);

                filenames = new Tuple<string, string>(saveFileName, savePath);
                // add file log
                // this.log.AddFileLog();
            }
            catch (Exception ex)
            {
                LOG.Error(ex.Message, ex);
                throw ex;
            }

            return filenames;
        }

        /// <summary>
        /// 上傳授課花絮
        /// </summary>
        /// <param name="file"></param>
        /// <returns>item1: 系統產生的新檔名, 系統的儲存路徑 </returns>
        public Tuple<string, string> UploadTeachShowFile(HttpPostedFileBase file, string postfix)
        {
            Tuple<string, string> filenames = null;
            try
            {
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string ext = Path.GetExtension(file.FileName);
                string saveFileName = string.Format("ts{0}{1}{2}", fileName, postfix, ext);
                string savePath = string.Format("~/Uploads/TeachShow/{0}", saveFileName);
                string realPath = this.modelBinder.controllerContext.HttpContext.Server.MapPath(savePath);

                //XLS、DOC、PPT、PDF、JPG、PNG、MSG，ODT、ODF
                string s_file_accept = Turbo.MVC.Base3.Models.ConfigModel.Get_FileExtAccept;
                string s_file_accept_upper = s_file_accept.ToUpper().Replace(",", "、");
                var s_filePost = string.Concat(postfix, "檔案");  //var s_filePost = (postfix == "1" ? "積分檔案" : "檔案");
                if (!FileTypeDetector.IsFileTypeValid(file, savePath))
                {
                    var ex3 = new Exception(string.Concat(file.FileName, string.Concat("上傳-", s_filePost, "內容有誤 無效的檔案格式(限", ext, "檔案內容)！")));
                    LOG.Error(ex3.Message, ex3);
                    throw ex3;
                }

                file.SaveAs(realPath);

                filenames = new Tuple<string, string>(saveFileName, savePath);
                // add file log
                // this.log.AddFileLog();
            }
            catch (Exception ex)
            {
                LOG.Error(ex.Message, ex);
                throw ex;
            }

            return filenames;
        }

        /// <summary>依據提供的檔名，刪除授課花絮檔案</summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public void RemoveTeachShowFile(string filename)
        {
            string savePath = string.Format("~/Uploads/TeachShow/{0}", filename);
            try
            {
                string realPath = this.modelBinder.controllerContext.HttpContext.Server.MapPath(savePath);
                File.Delete(realPath);
            }
            catch (Exception ex)
            {
                LOG.Error(ex.Message, ex);
                throw ex;
            }
        }


        /// <summary>
        /// 下載檔案
        /// </summary>
        /// <param name="fid"></param>
        /// <returns></returns>
        public Tuple<string, byte[]> DownloadScoreFile(long? fid)
        {
            CKFile item = this.dao.GetRow(new CKFile { mmsid = fid });

            string saveFileName = null;
            byte[] ba = null;

            if (item != null)
            {
                saveFileName = item.filname;
                // saveFileName = "f202006182023554539.docx";

                string savePath = string.Format("~/Uploads/Score/{0}", saveFileName);
                string realPath = this.modelBinder.controllerContext.HttpContext.Server.MapPath(savePath);

                using (FileStream fs = new FileStream(realPath, FileMode.Open))
                using (BufferedStream bs = new BufferedStream(fs))
                {
                    ba = new byte[fs.Length];
                    bs.Read(ba, 0, (int)fs.Length);
                }
            }

            return new Tuple<string, byte[]>(saveFileName, ba);
        }

        /// <summary>
        /// 下載分享檔案
        /// </summary>
        /// <param name="fid"></param>
        /// <returns></returns>
        public Tuple<string, byte[]> DownloadShareFile(long? fid, ActionName.File file)
        {
            Memshare item = this.dao.GetRow(new Memshare { id = fid });
            string saveFileName = null;
            byte[] ba = null;

            if (item != null)
            {
                switch (file)
                {
                    case ActionName.File.ShareSFile:
                        saveFileName = item.filename;
                        this.SaveFileShare(fid);
                        break;

                    case ActionName.File.ShareIFile:
                        saveFileName = item.ifilename;
                        break;

                    case ActionName.File.ShareAFile:
                        saveFileName = item.afilename;
                        break;
                }

                string savePath = string.Format("~/Uploads/Share/{0}", saveFileName);
                string realPath = this.modelBinder.controllerContext.HttpContext.Server.MapPath(savePath);

                using (FileStream fs = new FileStream(realPath, FileMode.Open))
                using (BufferedStream bs = new BufferedStream(fs))
                {
                    ba = new byte[fs.Length];
                    bs.Read(ba, 0, (int)fs.Length);
                }
            }

            return new Tuple<string, byte[]>(saveFileName, ba);
        }

        /// <summary>
        /// 最新消息檔案
        /// </summary>
        /// <param name="fid"></param>
        /// <returns></returns>
        public Tuple<string, byte[]> DownloadNewsFile(long? fid)
        {
            Tuple<string, byte[]> result = null;
            NewsFile dtl = this.dao.GetRow(new NewsFile { id = fid });
            if (dtl != null)
            {
                string savePath = string.Format("~/Uploads/News/{0}", dtl.filename);
                string realPath = this.modelBinder.controllerContext.HttpContext.Server.MapPath(savePath);

                FileInfo fi = new FileInfo(realPath);
                byte[] ba = null;
                if (fi.Exists)
                {
                    using (FileStream fs = new FileStream(realPath, FileMode.Open))
                    using (BufferedStream bs = new BufferedStream(fs))
                    {
                        ba = new byte[fs.Length];
                        bs.Read(ba, 0, (int)fs.Length);
                    }
                }
                result = new Tuple<string, byte[]>(dtl.filename, ba);
            }

            return result;
        }

        /// <summary>
        /// 下載授課花絮圖片
        /// </summary>
        /// <param name="fid"></param>
        /// <returns></returns>
        public Tuple<string, byte[]> DownloadTeachShowFile(long? fid)
        {
            Tuple<string, byte[]> result = null;
            TeacherShow dtl = this.dao.GetRow(new TeacherShow { id = fid });
            if (dtl != null)
            {
                string savePath = string.Format("~/Uploads/TeachShow/{0}", dtl.content);
                string realPath = this.modelBinder.controllerContext.HttpContext.Server.MapPath(savePath);

                FileInfo fi = new FileInfo(realPath);
                byte[] ba = null;
                if (fi.Exists)
                {
                    using (FileStream fs = new FileStream(realPath, FileMode.Open))
                    using (BufferedStream bs = new BufferedStream(fs))
                    {
                        ba = new byte[fs.Length];
                        bs.Read(ba, 0, (int)fs.Length);
                    }
                }
                result = new Tuple<string, byte[]>(dtl.content, ba);
            }

            return result;
        }

        /// <summary>
        /// 新增檔案下載記錄
        /// </summary>
        public void SaveFileShare(long? fid)
        {
            SessionModel sess = SessionModel.Get();
            long? mid = sess.UserInfo.User.user_id;

            Memshare ms = this.dao.GetRow(new Memshare { id = fid });
            if (ms != null && ms.mid != mid)    // 下載者非教材擁有者
            {
                FileShare fs = new FileShare
                {
                    mid = mid,
                    mfid = fid
                };

                FileShare oldFs = this.dao.GetRow(fs);
                if (oldFs == null)
                {
                    this.dao.BeginTransaction();
                    try
                    {
                        this.dao.Insert(fs);
                        this.dao.CommitTransaction();
                    }
                    catch (Exception ex)
                    {
                        LOG.Error(ex.Message, ex);
                        this.dao.RollBackTransaction();
                        throw ex;
                    }
                }
            }
        }

    }
}

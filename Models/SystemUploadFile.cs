using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Turbo.Commons;
using Turbo.MVC.Base3.DataLayers;
using Turbo.MVC.Base3.Models.Entities;
using Turbo.MVC.Base3.Services;
using log4net;

namespace Turbo.MVC.Base3.Models
{
    /// <summary>
    /// 檔案上傳類, 可用來上傳
    /// </summary>
    public class SystemUploadFile : UploadFile
    {
        //using log4net;
        protected static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static IList<AcceptFileType> acceptFileTypes = new List<AcceptFileType>();

        /// <summary>
        /// static contrustor
        /// 用來定義 acceptFileTypes
        /// </summary>
        static SystemUploadFile()
        {
            acceptFileTypes.Add(AcceptFileType.BMP);
            acceptFileTypes.Add(AcceptFileType.CSV);
            acceptFileTypes.Add(AcceptFileType.DAT);
            acceptFileTypes.Add(AcceptFileType.DOC);
            acceptFileTypes.Add(AcceptFileType.DOCX);
            acceptFileTypes.Add(AcceptFileType.GIF);
            acceptFileTypes.Add(AcceptFileType.JPG);
            acceptFileTypes.Add(AcceptFileType.ODG);
            acceptFileTypes.Add(AcceptFileType.ODP);
            acceptFileTypes.Add(AcceptFileType.ODS);
            acceptFileTypes.Add(AcceptFileType.ODT);
            acceptFileTypes.Add(AcceptFileType.PDF);
            acceptFileTypes.Add(AcceptFileType.PNG);
            acceptFileTypes.Add(AcceptFileType.PPT);
            acceptFileTypes.Add(AcceptFileType.PPTX);
            acceptFileTypes.Add(AcceptFileType.TIFF);
            acceptFileTypes.Add(AcceptFileType.TXT);
            acceptFileTypes.Add(AcceptFileType.XLS);
            acceptFileTypes.Add(AcceptFileType.XLSX);
        }

        /// <summary>
        /// 預設 SystemUploadFile 建構子
        /// </summary>
        public SystemUploadFile()
        {
        }

        /// <summary>
        /// 指定上傳檔案儲存路徑, 建構 ExcelUploadFile
        /// </summary>
        /// <param name="locationPath">相對於 ContextRoot 的路徑</param>
        public SystemUploadFile(string locationPath) : base(locationPath)
        {
        }

        /// <summary>
        /// 取得可接受的上傳檔案類型
        /// </summary>
        /// <returns></returns>
        public override IList<AcceptFileType> GetAcceptFileTypes()
        {
            return acceptFileTypes;
        }

        /// <summary>
        /// 外層ID(例如:Deatil等...)
        /// </summary>
        public bool ShowFileUpload { get; set; }

        /// <summary>
        /// 刪除按鈕是否顯示
        /// </summary>
        public bool ShowDelete { get; set; }

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string ErrorMsg { get; set; }

        /// <summary>
        /// 外層ID(例如:Deatil等...)
        /// </summary>
        public string OutModelName { get; set; }

        /// <summary>
        /// 程式路徑
        /// </summary>
        public string FILEPKEY1 { get; set; }

        /// <summary>
        /// PKEY1
        /// </summary>
        public string FILEPKEY2 { get; set; }

        /// <summary>
        /// PKEY2
        /// </summary>
        public string FILEPKEY3 { get; set; }

        public IList<TblFILEGRID> fileGrid { get; set; }

        /// <summary>
        /// 取得初始列表
        /// </summary>
        public void GetFileGrid()
        {
            // 若有檔案列表，且FILEPKEY1(程式路徑)跟FILEPKEY1(基本PKEY)沒有資料則不進入
            if (fileGrid.ToCount() == 0 && this.FILEPKEY1 != null && this.FILEPKEY2 != null)
            {
                BaseDAO dao = new BaseDAO();

                TblFILEGRID where = new TblFILEGRID();
                where.FILEPKEY1 = this.FILEPKEY1;
                where.FILEPKEY2 = this.FILEPKEY2;
                where.FILEPKEY3 = this.FILEPKEY3;

                fileGrid = dao.GetRowList(where);
            }
        }

        /// <summary>
        /// 儲存檔案列表
        /// </summary>
        public void SaveFileGrid()
        {
            BaseDAO dao = new BaseDAO();
            dao.BeginTransaction();
            try
            {
                // 刪除舊有檔案資料(必須有FILEPKEY1與FILEPKEY2以上條件才能刪除)
                if (this.FILEPKEY2.TONotNullString() != "")
                {
                    TblFILEGRID fileWhere = new TblFILEGRID();
                    fileWhere.FILEPKEY1 = this.FILEPKEY1;
                    fileWhere.FILEPKEY2 = this.FILEPKEY2;
                    fileWhere.FILEPKEY3 = this.FILEPKEY3;

                    dao.Delete(fileWhere);

                    // 若有檔案列表，且FILEPKEY1(程式路徑)跟FILEPKEY1(基本PKEY)沒有資料則不進入
                    if (fileGrid.ToCount() != 0)
                    {
                        // 新增檔案資料
                        foreach (var file in fileGrid)
                        {
                            file.FILEPKEY2 = this.FILEPKEY2;
                            file.FILEPKEY3 = this.FILEPKEY3;
                            dao.Insert(file);
                        }
                    }

                    dao.CommitTransaction();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                dao.RollBackTransaction();
                throw ex;
            }
        }
    }
}
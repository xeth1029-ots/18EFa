using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Turbo.DataLayer;
using WDACC.DataLayers;
using WDACC.Services;

namespace WDACC.Commons
{
    public class LogCollection
    {
        public string LogId { get; internal set; }
        public ActionName.Admin? AdminAction { get; internal set; }
        public ActionName.Member? MemberAction { get; internal set; }
        public ActionName.File? FileAction { get; internal set; }
        public IList<FuncCollectionModel> FuncCollection { get; }
        public IList<FileCollectionModel> FileCollection { get; }

        public LogCollection()
        {
            this.LogId = Guid.NewGuid().ToString();
            this.FuncCollection = new List<FuncCollectionModel>();
            this.FileCollection = new List<FileCollectionModel>();
        }

        public void AddFuncCollection(ActionName.EditMode mode,
            ActionName.EditStatus status, object arg0, object arg1)
        {
            string tableName = string.Empty;
            if (arg0 != null && arg0 is IDBRow)
            {
                tableName = (arg0 as IDBRow).GetTableName().ToString(); // 取得修改的資料表名稱
            }
            this.FuncCollection.Add(new FuncCollectionModel
            {
                EditMode = mode,
                EditStatus = status,
                TableName = tableName,
                Arg = arg0,
                Arg1 = arg1,
            });
        }

        public void AddFileCollection(ActionName.EditStatus status,
            ActionName.FileEditType editMode,
            string filename, string saveFilename, string message)
        {
            this.FileCollection.Add(new FileCollectionModel
            {
                EditMode = editMode,
                EditStatus = status,
                SaveFilename = saveFilename,
                FileName = filename,
                Message = message
            });
        }

        public void SetActionContext(ActionName.Member action)
        {
            this.MemberAction = action;
        }

        public void SetActionContext(ActionName.Admin action)
        {
            this.AdminAction = action;
        }

        public void SetActionContext(ActionName.File action)
        {
            this.FileAction = action;
        }

    }

    /// <summary>
    /// 系統操作log
    /// </summary>
    public class FuncCollectionModel
    {
        public ActionName.EditStatus EditStatus { get; set; }
        public ActionName.EditMode EditMode { get; set; }
        public string TableName { get; set; }
        public object Arg { get; set; }
        public object Arg1 { get; set; }
    }

    public class FileCollectionModel
    {
        public ActionName.EditStatus EditStatus { get; set; }
        public ActionName.FileEditType EditMode { get; set; }
        public string FileName { get; set; }
        public string SaveFilename { get; set; }
        public string Message { get; set; }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WDACC.Commons;
using WDACC.Models.Entities;

namespace WDACC.Models.ViewModel.Member
{
    public class ShareFileModel
    {
        /// <summary>
        /// 編輯模式 CREATE|UPDATE
        /// </summary>
        // public string EditMode { get; set; }

        public ActionName.EditMode EditMode { get; set; }

        /// <summary>
        /// 表單內容
        /// </summary>
        public Memshare MemShare { get; set; }

        /// <summary>
        /// 選擇教材檔案
        /// </summary>
        public HttpPostedFileBase SFile { get; set; }

        /// <summary>
        /// 選擇說明書檔案
        /// </summary>
        public HttpPostedFileBase IFile { get; set; }

        /// <summary>
        /// 選擇授權書檔案
        /// </summary>
        public HttpPostedFileBase AFile { get; set; }

    }
}

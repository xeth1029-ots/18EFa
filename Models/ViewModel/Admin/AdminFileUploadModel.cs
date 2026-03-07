using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WDACC.Models.Entities;

namespace WDACC.Models.ViewModel.Admin
{
    /// <summary>
    /// 管理者檔案上傳
    /// </summary>
    public class AdminFileUploadModel
    {
        public AdminFileUploadModel()
        {
            this.UploadDate = new TwDate();
            this.UploadDate.Label = " ";
        }

        public Memshare Detail { get; set; }
        public string TeasherName { get; set; }
        public TwDate UploadDate { get; set; }
        public HttpPostedFileBase SFile { get; set; }
        public HttpPostedFileBase IFile { get; set; }
        public HttpPostedFileBase AFile { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WDACC.Models.Entities;
using WDACC.Models.StoreExt;

namespace WDACC.Models.ViewModel.Member
{
    /// <summary>
    /// 積分課程登錄
    /// </summary>
    public class ScoreFormModel
    {
        public ScoreFormModel()
        {
            this.StartDateTw = new DateTimeField();
            this.EndDateTw = new DateTimeField();
            this.Score = new Memsrcs();
        }

        /// <summary>
        /// 編輯模式 CREATE | UPDATE
        /// </summary>
        public string EditMode { get; set; }

        public Memsrcs Score { get; set; }

        public CKFile CkFile { get; set; }

        public DateTimeField StartDateTw { get; set; }

        public DateTimeField EndDateTw { get; set; }

        public HttpPostedFileBase File { get; set; }

        /// <summary>
        /// 審核通過狀況
        /// </summary>
        public IList<Store> AuditList { get; set; }

        public void PrepareAfterLoad()
        {
            this.StartDateTw.SetDate(this.Score.sdate);
            this.EndDateTw.SetDate(this.Score.edate);
        }

        public void PrepareBeforeSave()
        {
            this.Score.sdate = this.StartDateTw.GetDate();
            this.Score.edate = this.EndDateTw.GetDate();
        }

    }

}

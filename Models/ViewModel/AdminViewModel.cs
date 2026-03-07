using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WDACC.Commons;
using WDACC.Models.Entities;
using WDACC.Models.StoreExt;
using WDACC.Models.ViewModel.Admin;

namespace WDACC.Models.ViewModel
{
    public class AdminViewModel
    {
        public AdminViewModel()
        {
            this.AuditParam = new AuditParameter();
            this.MeetingModel = new AdminMeetingModel();
            this.ScoreModel = new AdminScoreModel();
            this.FeedbackModel = new AdminFeedbackModel();
            this.RecruitModel = new AdminRecruitModel();
            this.SystemModel = new AdminSystemModel();
            this.NewsModel = new AdminNewsModel();
            this.TeacherModel = new AdminTeacherModel();
            this.MemberModel = new AdminAccountModel();
            this.ExportModel = new AdminExportModel();
            this.MgrModel = new AdminMgrModel();
            this.ScoreViewExportModel = new AdminScoreViewExportModel();
            this.AuditExportModel = new AdminAuditExportModel();
            this.FileUpload = new AdminFileUploadModel();
            this.FeedbackExportModel = new SqlMapModel { StatementId = () => { return "Admin.GetFeedbackExportList"; } };
            this.MonthlylogsExportModel = new MonthlylogsModel
            {
                StatementId = () => { return "Admin.GetMonthlylogsExportList"; }
                ,
                Parameter = () => { return new Store { { "ExpYear1", this.ExpYear1 }, { "ExpMonth1", this.ExpMonth1 } }; }
            };
        }
        /// <summary>
        /// 匯出年
        /// </summary>
        public string ExpYear1 { get; set; }
        /// <summary>
        /// 匯出月
        /// </summary>
        public string ExpMonth1 { get; set; }

        public ActionName.Admin Action { get; set; }
        /// <summary>
        /// 系統設定
        /// </summary>
        public AdminSystemModel SystemModel { get; set; }
        ///<summary>
        ///最新消息編輯表單
        ///</summary>
        public AdminNewsModel NewsModel { get; set; }
        // <summary>,// 新增講師資料表單,// </summary>,//public AdminMemberForm MemberForm { get; set; },
        /// <summary>
        /// 新增會議資料表單
        /// </summary>
        public AdminMeetingModel MeetingModel { get; set; }
        /// <summary>
        /// 課程清單
        /// </summary>
        public IList<Store> CourseGrid { get; set; }
        /// <summary>
        /// 取得會議清單
        /// </summary>
        public IList<Store> MeetingGrid { get; set; }
        /// <summary>
        /// 講師積分
        /// </summary>
        public AdminScoreModel ScoreModel { get; set; }
        /// <summary>
        /// 最新消息清單
        /// </summary>
        //public IList<Store> NewsGrid { get; set; }
        /// <summary>
        /// 匯出資料清單
        /// </summary>
        public IList<Store> ExportGrid { get; set; }
        /// <summary>
        /// 審核年度查詢欄位
        /// </summary>
        public AuditParameter AuditParam { get; set; }
        /// <summary>
        /// 檔案上傳審核清單
        /// </summary>
        public IList<Store> FileAuditList { get; set; }

        /// <summary>
        /// 進入頁檔案上傳審核清單
        /// </summary>
        public IList<Store> HomeFileAuditList { get; set; }
        /// <summary>
        /// 課程審核清單
        /// </summary>
        public IList<Store> CourseAuditList { get; set; }
        /// <summary>
        /// 講師課程審核清單
        /// </summary>
        public IList<Memsrcs> AuditCourseDetailList { get; set; }
        /// <summary>
        /// 講師上傳檔案清單
        /// </summary>
        public IList<Memshare> AuditShareDetailList { get; set; }
        /// <summary>
        /// 課程師資招募
        /// </summary>
        public AdminRecruitModel RecruitModel { get; set; }
        /// <summary>
        /// 意見回饋
        /// </summary>
        public AdminFeedbackModel FeedbackModel { get; set; }
        /// <summary>
        /// 講師資料設定
        /// </summary>
        public AdminTeacherModel TeacherModel { get; set; }
        /// <summary>
        /// 新增帳號 
        /// </summary>
        public AdminAccountModel MemberModel { get; set; }
        /// <summary>
        /// 資料匯出
        /// </summary>
        public AdminExportModel ExportModel { get; set; }
        /// <summary>
        /// 會議匯出資料
        /// </summary>
        public AdminMeetExportModel MeetExportModel { get; set; }
        /// <summary>
        /// 積分資料匯出
        /// </summary>
        public AdminScoreViewExportModel ScoreViewExportModel { get; set; }
        /// <summary>
        /// 意見回饋匯出
        /// </summary>
        public SqlMapModel FeedbackExportModel { get; set; }
        /// <summary>
        /// 匯出每月登入異常日誌
        /// </summary>
        public MonthlylogsModel MonthlylogsExportModel { get; set; }
        /// <summary>
        /// 審核資料匯出
        /// </summary>
        public AdminAuditExportModel AuditExportModel { get; set; }
        /// <summary>
        /// 管理者帳號管理
        /// </summary>
        public AdminMgrModel MgrModel { get; set; }

        /// <summary>
        /// 管理者檔案上傳
        /// </summary>
        public AdminFileUploadModel FileUpload { get; set; }

    }

}

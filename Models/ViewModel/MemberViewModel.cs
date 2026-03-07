using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SimpleInjector;
using WDACC.Commons;
using WDACC.Models.Entities;
using WDACC.Models.StoreExt;
using WDACC.Models.ViewModel.Member;

namespace WDACC.Models.ViewModel
{
    public class MemberViewModel
    {

        private Container container;
        private LogCollection log;
        private ResultMessage msg;

        public MemberViewModel(
            Container container,
            LogCollection log,
            ResultMessage msg)
        {
            this.container = container;
            this.log = log;
            this.msg = msg;

            this.News = new NewsModel();
            this.ScoreForm = new ScoreFormModel();
            this.ShareFileForm = new ShareFileModel();
            this.ScoreView = new ScoreViewModel();
            this.TeachShow = new TeacherShowModel();
            this.Feedback = new FeedbackModel();
            this.TeacherInfoForm = new BaseInfoModel();
        }

        /// <summary>
        /// 各作業共用Grid Model
        /// </summary>
        public IList<Store> Grid { get; set; }

        /// <summary>
        /// 師資明細
        /// </summary>
        public TeacherDetail TeacherDetail { get; set; }

        /// <summary>
        /// 講師基本資料明細
        /// </summary>
        public BaseInfoModel TeacherInfoForm { get; set; }

        /// <summary>
        /// 最新消息
        /// </summary>
        public NewsModel News { get; set; }

        /// <summary>
        /// 課程師資招募清單
        /// </summary>
        public IList<Store> TcsvGrid { get; set; }

        /// <summary>
        /// 課程師資明細
        /// </summary>
        public Store TeaSurvey { get; set; }

        /// <summary>
        /// 講師修改密碼
        /// </summary>
        public PasswordModel PasswordForm { get; set; }

        /// <summary>
        /// 課程登錄首頁
        /// </summary>
        public Intro ScoreHome { get; set; }

        /// <summary>
        /// 積分課程登錄
        /// </summary>
        public ScoreFormModel ScoreForm { get; set; }

        /// <summary>
        /// 積分課程清單 查詢參數
        /// </summary>
        public ScoreGridParameter ScoreGridParam { get; set; }

        /// <summary>
        /// 積分課程清單
        /// </summary>
        public IList<Memsrcs> ScoreGrid { get; set; }

        /// <summary>
        /// 教材分享訊息頁
        /// </summary>
        public Intro ShareHome { get; set; }

        /// <summary>
        /// 上傳分享檔案表單
        /// </summary>
        public ShareFileModel ShareFileForm { get; set; }

        /// <summary>
        /// 教材分享檔案清單查詢參數　
        /// </summary>
        public ShareWithMeParameter ShareWithMeParam { get; set; }

        /// <summary>
        /// 教材分享檔案清單　
        /// </summary>
        public IList<Store> ShareWithMeList { get; set; }

        /// <summary>
        /// 我的教材分享清單
        /// </summary>
        public IList<Store> MyShareFileList { get; set; }

        /// <summary>
        /// 積分表
        /// </summary>
        public ScoreViewModel ScoreView { get; set; }

        /// <summary>
        /// 授課花絮
        /// </summary>
        public TeacherShowModel TeachShow { get; set; }

        /// <summary>
        /// 意見回饋
        /// </summary>
        public FeedbackModel Feedback { get; set; }


        //public class TeacherShareFileModel
        //{
        //    public string mid { get; set; }
        //    public int year { get; set; }
        //    public int month { get; set; }
        //    public string fclass { get; set; }
        //}

    }

}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using FluentValidation;
using Turbo.DataLayer;
using WDACC.Models.Entities;
using WDACC.Models.StoreExt;

namespace WDACC.Models.ViewModel.Facade
{
    public class FacadeViewModel
    {
        public enum ActionName
        {
            None,
            Home,
            News,
            CourseIntro,
            TeaSurvey,
            TeaSurveyEdit,
            TeacherQuery,
            TeacherSearch,
            TeachShow,
            TeacherDetail,
            TeaSurveyForm,
            Login,
            ForgetPxssword,
            SiteMap,
            Privacy,
            GovAnnounce,
        }

        public FacadeViewModel()
        {
            this.NewsForm = new NewsQueryForm();
            this.CourseIntro = new List<Store>();
            this.CourseForm = new CourseQueryForm();
            this.TeacherForm = new TeacherQueryForm();
            this.TeacherDetail = new TeacherDetail();
            this.TeacherSurveyDetail = new TeaSurveyDetailModel();
            this.NewsGrid = new NewsModel();
            this.NewsDetail = new NewsModel();
            this.Paging = new PagingModel();
            this.NewsGrid.Paging = this.Paging;
            this.ForgetModel = new FacadeForgetModel();

            this.TeachShowGrid = new SqlMapModel
            {
                StatementId = () => { return "Facade.GetLastTeachShow"; }
            };
        }

        public IList<News> MarqueeList { get; set; }

        /// <summary>
        /// 最新消息清單
        /// </summary>
        public NewsModel NewsGrid { get; set; }

        /// <summary>
        /// 授課花絮
        /// </summary>
        public SqlMapModel TeachShowGrid { get; set; }

        /// <summary>
        /// 講師授課花絮
        /// </summary>
        public IList<TeacherShow> MemberTeachShowGrid { get; set; }

        /// <summary>
        /// 課程簡介
        /// </summary>
        public IList<Store> CourseIntro { get; set; }

        /// <summary>
        /// 最新消息查詢表單
        /// </summary>
        public NewsQueryForm NewsForm { get; set; }

        /// <summary>
        /// 最新消息明細
        /// </summary>
        public NewsModel NewsDetail { get; set; }

        /// <summary>
        /// 課程查詢表單
        /// </summary>
        public CourseQueryForm CourseForm { get; set; }

        /// <summary>
        /// 課程師資搜尋
        /// </summary>
        public TeacherQueryForm TeacherForm { get; set; }

        /// <summary>
        /// 師資明細 
        /// </summary>
        public TeacherDetail TeacherDetail { get; set; }

        /// <summary>
        /// 課程需求刊登表單
        /// </summary>
        public TeaSurveyDetailModel TeacherSurveyDetail { get; set; }

        public FacadeForgetModel ForgetModel { get; set; }

        public PagingModel Paging { get; set; }

        /// <summary>
        /// 共用查詢結果
        /// </summary>
        public IList<Store> Grid { get; set; }

        /// <summary>
        /// 共用明細資料結構
        /// </summary>
        public Store Detail { get; set; }

        public string Message { get; set; }

        /// <summary>
        /// 驗證碼
        /// </summary>
        public string ValidateCode { get; set; }

        /// <summary>
        /// 檔案上傳
        /// </summary>
        public IList<HttpPostedFile> Files { get; set; }

    }

    // public class PagingModel : PagingResultsViewModel { }

    /// <summary>
    /// 最新消息查表單
    /// </summary>
    public class NewsQueryForm
    {
        public int? NewsId { get; set; }
        public DateTime? DateS { get; set; }
        public DateTime? DateE { get; set; }
    }


    public class CourseQueryForm
    {
        /// <summary>
        /// 課程名稱
        /// </summary>
        public string CourseName { get; set; }

    }

    /// <summary>
    /// 課程師資搜尋
    /// </summary>
    public class TeacherQueryForm
    {
        public long? Mid { get; set; }
        /// <summary>
        /// 關鍵字
        /// </summary>
        public string KeyWord { get; set; }

        /// <summary>
        /// 授課單元
        /// </summary>
        public string TeachUnit { get; set; }

        /// <summary>
        /// 授課區域
        /// </summary>
        public string RegOp { get; set; }

        /// <summary>
        /// 授課產業別
        /// </summary>
        public string IndsOp { get; set; }

        /// <summary>
        /// 居住地
        /// </summary>
        public string LiveReg { get; set; }

    }

    /// <summary>
    /// 課程需求刊登明細表單
    /// </summary>
    public class TeaSurveyDetailModel
    {
        public TeaSurveyDetailModel()
        {
            this.Teasurvey = new TeaSurvey();
            this.StartDate = new DateField();
            this.EndDate = new DateField();
            this.CouClass = new CouClassField();
        }

        /// <summary>
        /// 授課日期-開始
        /// </summary>
        [DisplayName("授課日期-開始")]
        public DateField StartDate { get; set; }
        /// <summary>
        /// 授課日期-結束
        /// </summary>
        [DisplayName("授課日期-結束")]
        public DateField EndDate { get; set; }

        /// <summary>需求課程類別</summary>
        [DisplayName("需求課程類別(必要的)")]
        public CouClassField CouClass { get; set; }
        public TeaSurvey Teasurvey { get; set; }

        public void PrepareAfterLoad()
        {
            this.StartDate.SetDate(this.Teasurvey.stime);
            this.EndDate.SetDate(this.Teasurvey.etime);
            this.CouClass.SetValue(this.Teasurvey.couclass);
        }

        public void PrepareBeforeSave()
        {
            this.Teasurvey.stime = this.StartDate.GetDate();
            this.Teasurvey.etime = this.EndDate.GetDate();
            this.Teasurvey.couclass = this.CouClass.GetValue();
        }

    }

    /// <summary>
    /// 課程需求刊登檢核器
    /// </summary>
    public class TeaSurveyDetailModelValidator : AbstractValidator<TeaSurveyDetailModel>
    {
        public TeaSurveyDetailModelValidator()
        {
            RuleFor(x => x.Teasurvey.indname).NotEmpty();
            RuleFor(x => x.Teasurvey.indcontact).NotEmpty();
            RuleFor(x => x.Teasurvey.indphone).NotEmpty();
            RuleFor(x => x.Teasurvey.indemail).NotEmpty();
            RuleFor(x => x.Teasurvey.stime).NotEmpty();
            RuleFor(x => x.Teasurvey.etime).NotEmpty();
            RuleFor(x => x.Teasurvey.couhours).NotEmpty();
            RuleFor(x => x.Teasurvey.couplace).NotEmpty();
            RuleFor(x => x.Teasurvey.teano).NotEmpty();
            RuleFor(x => x.Teasurvey.stuno).NotEmpty();
            RuleFor(x => x.Teasurvey.couclass).NotEmpty();
            RuleFor(x => x.Teasurvey.indclass1).NotEmpty();

            //RuleFor(x => x.Forename).NotEmpty().WithMessage("Please specify a first name");
            //RuleFor(x => x.Discount).NotEqual(0).When(x => x.HasDiscount);
            //RuleFor(x => x.Address).Length(20, 250);
            //RuleFor(x => x.Postcode).Must(BeAValidPostcode).WithMessage("Please specify a valid postcode");
        }
    }

    /// <summary>
    /// 忘記密碼
    /// </summary>
    public class FacadeForgetModel
    {
        public FacadeForgetModel()
        {
            this.Birthday = new DateField();
        }

        public string Email { get; set; }
        public DateField Birthday { get; set; }

        public DateTime? BirthDate { get; set; }

        public string ValidateCode { get; set; }

        public void PrepareBeforeSave()
        {
            if (this.Birthday != null)
            {
                if (Birthday.Year.HasValue && Birthday.Month.HasValue && Birthday.Day.HasValue)
                {
                    this.BirthDate = new DateTime(Birthday.Year.Value, Birthday.Month.Value, Birthday.Day.Value);
                }
            }
        }

    }

}


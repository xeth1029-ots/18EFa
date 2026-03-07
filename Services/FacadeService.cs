using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WDACC.DataLayers;
using WDACC.Models;
using WDACC.Models.Entities;
using WDACC.Models.StoreExt;
using WDACC.Models.ViewModel;
using WDACC.Models.ViewModel.Facade;
using FluentValidation.Results;
using WDACC.Commons;
using SimpleInjector;
using Turbo.MVC.Base3.Commons;
using log4net;
using WDACC.Validator;
using FluentValidation;

namespace WDACC.Services
{
    public class FacadeService
    {
        private MyBaseDAO dao;
        private FormCollection collection = null;
        private NewsService newsService;
        private TeacherService teacherService;
        private AccountService accountService;

        protected static readonly ILog LOG =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ModelStateDictionary modelState { get; set; }

        public FacadeService(MyBaseDAO dao, NewsService newService, TeacherService teacherService, AccountService accountService)
        {
            this.dao = dao;
            this.newsService = newService;
            this.teacherService = teacherService;
            this.accountService = accountService;
        }

        /// <summary>
        /// 取得首頁資料
        /// </summary>
        /// <param name="model"></param>
        public void GetHomeData(FacadeViewModel model)
        {
            this.newsService.collection = collection;
            this.newsService.GetHomeNewsList(model.NewsGrid);   // 最新消息清單
            model.TeachShowGrid.QueryForListAll(this.dao);      // 師資授課花絮

            // 跑馬燈
            model.MarqueeList = this.dao.GetRowList(new News { @class = 4 })
                .Where(x => x.sdate <= DateTime.Now.Date && x.edate >= DateTime.Now.Date && x.showoff == 1)
                .ToList();
        }

        /// <summary>課程簡介</summary>
        /// <param name="model"></param>
        public void GetCourseIntro(FacadeViewModel model)
        {
            Store param = new Store();
            model.CourseIntro = this.dao.QueryForListAll<Store>("Intro.GetHomeIntro", param);
        }

        /// <summary>
        /// 最新消息清單
        /// </summary>
        /// <param name="model"></param>
        public void GetNewsList(FacadeViewModel model)
        {
            this.newsService.collection = collection;
            this.newsService.GetNewsList(model.NewsGrid);
        }

        public void GetNewsDetail(FacadeViewModel model)
        {
            this.newsService.GetNewsDetail(model.NewsDetail);
        }

        /// <summary>
        /// 課程師資介紹
        /// </summary>
        /// <param name="model"></param>
        public void GetTeacherQueryList(FacadeViewModel model)
        {
            Store param = new Store();
            param["active"] = 1;
            model.Grid = this.dao.QueryForListAll<Store>("Teacher.GetTeacherList", param);
        }

        /// <summary>
        /// 課程師資搜尋
        /// </summary>
        /// <param name="model"></param>
        public void GetTeacherSearchList(FacadeViewModel model)
        {
            Store param = new Store();
            param["active"] = 1;
            param.Collection(model.TeacherForm);
            model.Grid = this.dao.QueryForListAll<Store>("Teacher.GetTeacherSearchList", param);
        }

        /// <summary>
        /// 取得師資明細資料
        /// </summary>
        /// <param name="model"></param>
        public void GetTeacherDetail(FacadeViewModel model)
        {
            long? mid = model.TeacherDetail.MemDetail.mid;
            this.teacherService.GetTeacherDetail(model.TeacherDetail, mid);
            this.teacherService.GetTeacherShow(model.TeacherDetail, mid);
            this.teacherService.GetTeacherTechRegion(model.TeacherDetail, mid);
            this.teacherService.GetTeacherTechIndClass(model.TeacherDetail, mid);
            this.teacherService.GetTeacherCourseHours(model.TeacherDetail, mid);
            this.teacherService.GetTeacherCourseList(model.TeacherDetail, mid);

            var dtl = model.TeacherDetail;
            if (!string.IsNullOrEmpty(dtl.MemDetail.showoff))
            {
                dtl.ShowOff = dtl.MemDetail.showoff.Select(x => x == '1' ? true : false).ToArray();
            }

            if (model.TeacherDetail == null || !model.TeacherDetail.MemDetail.mid.HasValue)
            {
                Exception ex = new Exception("查無資料");
                LOG.Warn(ex.Message, ex);
                throw ex;
            }
        }

        /// <summary>
        /// 取得講師授課花絮
        /// </summary>
        /// <param name="model"></param>
        /// <param name="lid"></param>
        public void GetMemberTeachShow(FacadeViewModel model, long? mid)
        {
            if (mid.HasValue)
            {
                model.MemberTeachShowGrid = this.dao.GetRowList(new TeacherShow { mid = mid }).ToList();
            }
            else
            {
                ArgumentException ex = new ArgumentException("未提供講師序號");
                LOG.Warn(ex.Message, ex);
                throw ex;
            }
        }

        /// <summary>
        /// 取得使用者資訊
        /// </summary>
        /// <param name="model"></param>
        public void GetLoginUser(LoginViewModel model)
        {
            model.Grid = this.dao.QueryForObject<Store>("Facade.GetLoginUser", model);
        }

        /// <summary>
        /// 儲存課程需求
        /// </summary>
        /// <param name="model"></param>
        public void SaveTeaSurvey(FacadeViewModel model)
        {
            // 資料驗證
            model.TeacherSurveyDetail.PrepareBeforeSave();
            var validator = new TeaSurveyDetailModelValidator();
            var result = validator.Validate(model.TeacherSurveyDetail);
            if (!result.IsValid)
            {
                FluentValidation.ValidationException ex = new FluentValidation.ValidationException("必填欄位不可為空", result.Errors);
                LOG.Warn(ex.Message, ex);
                throw ex;
            }

            var dtl = model.TeacherSurveyDetail.Teasurvey;
            dtl.fdate = dtl.stime.HasValue ? dtl.stime.Value.Date : DateTime.Now;
            dtl.confirm = 1;
            dtl.aodate = DateTime.Now.Date;
            dtl.clicks = 0;
            dtl.coutime = (dtl.coutime ?? "");
            dtl.salary = (dtl.salary ?? "");
            dtl.indinfo = (dtl.indinfo ?? "");

            this.dao.BeginTransaction();
            try
            {
                this.dao.Insert<TeaSurvey>(dtl);
                this.dao.CommitTransaction();
            }
            catch (Exception ex)
            {
                LOG.Warn(ex.Message, ex);
                this.dao.RollBackTransaction();

                FluentValidation.ValidationException vex = new FluentValidation.ValidationException("儲存時發生錯誤", result.Errors);
                LOG.Warn(vex.Message, vex);
                //this.dao.RollBackTransaction();
                throw vex; // FIXME
            }
        }

        /// <summary>
        /// 重新設定登入密碼
        /// /// </summary>
        /// <param name="model"></param>
        public void ResetPassword(FacadeViewModel model)
        {
            // 檢查輸入內容
            var current = model.ForgetModel;
            //string s_log1 = "";
            current.PrepareBeforeSave();

            SessionModel sess = SessionModel.Get();
            var result = new ForgetPasswordValidator(sess).Validate(current);
            if (!result.IsValid)
            {
                ValidationException ex = new ValidationException(result.Errors);
                LOG.Warn(ex.Message, ex);
                throw ex;
            }
            sess.LoginValidateCode = "";

            //合理日期比對 應該沒有200歲以上的人吧
            DateTime now_200 = DateTime.Now.AddYears(-200);
            DateTime? dateBirth = DateTime.Now;
            if (current.BirthDate.HasValue)
            {
                dateBirth = current.BirthDate.Value;
                //小於零 t1 早於 t2。
                if (DateTime.Compare(dateBirth.Value, now_200) < 0)
                {
                    Exception ex = new Exception("查無資料，請重新輸入!");
                    LOG.Warn(ex.Message, ex);
                    throw ex;
                }
            }

            Store param = new Store();
            bool forgetPassword = false;
            // 判斷是忘記密碼 or 忘記帳號
            if (current.Email != null && current.Email.IndexOf("@") > -1)
            {
                // 忘記密碼
                param["UserName"] = current.Email;
                param["Birthday"] = (dateBirth.HasValue ? dateBirth.Value : (DateTime?)null);
                forgetPassword = true;
            }
            else
            {
                // 忘記帳號
                param["Pid"] = current.Email;
                forgetPassword = false;
            }

            Store res = this.dao.QueryForObject<Store>("Account.GetLoginUser", param);
            if (res == null) {
                LOG.Warn("查無資料，請重新輸入!");
                throw new Exception("查無資料，請重新輸入!");
            }

            try
            {
                if (forgetPassword)
                {
                    var mid = res.Get("mid").AsInt64();

                    // 產生隨機密碼
                    string rndPwd = this.accountService.CreateRandomPassword();
                    // string rndPwd = "Abb1234567890";
                    Member mem = new Member();
                    mem.password = MyCommonUtil.ComputeHash(rndPwd);
                    mem.failcount = 0;

                    if (mid.HasValue)
                    {
                        this.dao.BeginTransaction();
                        try
                        {
                            this.dao.Update(mem, new Member { id = mid });
                            this.dao.CommitTransaction();
                        }
                        catch (Exception ex)
                        {
                            LOG.Error(ex.Message, ex);
                            this.dao.RollBackTransaction();
                            throw ex;
                        }
                    }

                    // 寄送密碼至使用者信箱
                    MailSender sender = new MailSender();
                    string htmlBody = @"
                            <div>
                                <div> 您好, </div>
                                <div> </div>
                                <div> 您的登入帳號： {0} </div>
                                <div> 以下為系統重新指定的暫時性密碼: </div>
                                <div>{1}</div>
                                <div> 請利用此密碼登入後進入「基本資料維護／修改密碼」 頁面重新設定您的密碼。 </div> 
                                <div> 網站連結為: <a href='https://corecompetencies.wda.gov.tw/' title='共通核心職能課程'>共通核心職能課程</a></div>
                                <div> 此信件為系統自動發信通知，請勿回覆，謝謝!! </div>
                            </div> ";
                    sender.HtmlBody = string.Format(htmlBody, res.Get("email").AsText(), rndPwd);
                    sender.Subject = "共通核心職能專區系統-忘記密碼通知信";

                    IList<Tuple<string, string>> toAddrList = new List<Tuple<string, string>>();
                    string realname = res.Get("realname").AsText();
                    string email = res.Get("loginemail").AsText();
                    toAddrList.Add(new Tuple<string, string>(realname, email));
                    sender.ToAddress = toAddrList;

                    //s_log1 = sender.HtmlBody;
                    //LOG.Debug(s_log1);

                    sender.Send();
                    model.Message = "已寄出通知信，稍後請您至您的email信箱收信，謝謝！";
                    LOG.Debug(htmlBody);
                }
                else
                {
                    current.Email = res.Get("loginemail").AsText();
                    model.Message = string.Format("您在本系統所登錄之電子郵件為: {0}", current.Email);
                }

            }
            catch (Exception ex)
            {
                LOG.Warn(ex.Message, ex);
                //this.dao.RollBackTransaction();
                throw new Exception("寄送通知信失敗，請洽系統管理員!");
            }

        }

    }
}

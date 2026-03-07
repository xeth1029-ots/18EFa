using FluentValidation;
using log4net;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Turbo.MVC.Base3.Commons;
using Turbo.MVC.Base3.Controllers;
using WDACC.Commons;
using WDACC.DataLayers;
using WDACC.Models;
using WDACC.Models.Entities;
using WDACC.Models.ViewModel;
using WDACC.Models.ViewModel.Facade;
using WDACC.Services;

namespace WDACC.Controllers
{
    public class FacadeController : BaseController
    {
        private Container container;
        private LogCollection logCollection;
        private TeacherService teacherServ;
        private CourseService courseServ;
        private FacadeService facadeServ;
        private ResultMessage msg;
        private MyBaseDAO dao;
        private MyModelBinder modelBinder;

        protected static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public FacadeController(
            Container container,
            LogCollection logCollection,
            ResultMessage msg,
            FacadeService facadeServ,
            TeacherService teacherServ,
            CourseService courseServ,
            MyModelBinder modelBinder,
            MyBaseDAO dao)
        {
            this.container = container;
            this.logCollection = logCollection;
            this.teacherServ = teacherServ;
            this.courseServ = courseServ;
            this.facadeServ = facadeServ;
            this.modelBinder = modelBinder;
            this.msg = msg;
            this.dao = dao;
        }


        public ActionResult Index(string id)
        {
            string str_METHOD = (Request != null) ? Request.QueryString["_method"] : "";
            if (str_METHOD != null && !str_METHOD.Equals("")) { return base.SetPageNotFound(); }

            if (string.IsNullOrEmpty(id)) { return RedirectToAction("Index", new { id = "Home" }); }
            FacadeViewModel.ActionName action = MyCommonUtil.ToEnum<FacadeViewModel.ActionName>(id);

            ActionResult result = null;
            string viewName = id.ToString();
            object model = null;
            FacadeViewModel facade = null;

            try
            {
                switch (action)
                {
                    case FacadeViewModel.ActionName.SiteMap:   // 網站導覽 
                        model = this.container.GetInstance<FacadeViewModel>();
                        ViewBag.TitleS = "網站導覽";
                        break;

                    case FacadeViewModel.ActionName.Privacy:   // 隱私權及安全政策 
                        model = this.container.GetInstance<FacadeViewModel>();
                        // 查詢隱私權及安全政策設定資料
                        ViewBag.Privacy = dao.GetTbContent("44");
                        ViewBag.TitleS = "隱私權及安全政策";
                        break;

                    case FacadeViewModel.ActionName.GovAnnounce:   // 政府網站資訊開放宣告 
                        model = this.container.GetInstance<FacadeViewModel>();
                        // 查詢政府網站資訊開放宣告設定資訊
                        ViewBag.GovAnnounce = dao.GetTbContent("46");
                        ViewBag.TitleS = "政府網站資訊開放宣告";
                        break;

                    case FacadeViewModel.ActionName.Home:   // 首頁
                        model = this.container.GetInstance<FacadeViewModel>();
                        this.facadeServ.GetHomeData(model as FacadeViewModel);
                        //viewName = "~/Views/Home/Index.cshtml";
                        ViewBag.TitleS = "首頁";
                        break;

                    case FacadeViewModel.ActionName.News:   // 最新消息 
                        facade = this.container.GetInstance<FacadeViewModel>();
                        this.UpdateModel<PagingModel>(facade.Paging, new FormValueProvider(this.ControllerContext));
                        var url = Url.Action("Index", "Facade", new { id = FacadeViewModel.ActionName.News });
                        dao.SetPageInfo(facade.Paging.rid, facade.Paging.p);
                        facadeServ.GetNewsList(facade);
                        if (Request.IsAjaxRequest()) { facade.NewsGrid.IsGridRowOnly = true; }
                        this.SetPagingParamsUrl(facade.Paging, dao, url, "ajaxCallback");
                        model = facade;
                        ViewBag.TitleS = "最新消息";
                        break;

                    case FacadeViewModel.ActionName.CourseIntro:    // 課程簡介
                        model = this.container.GetInstance<FacadeViewModel>();
                        this.facadeServ.GetCourseIntro(model as FacadeViewModel);
                        ViewBag.TitleS = "課程簡介";
                        break;

                    case FacadeViewModel.ActionName.TeaSurvey:      // 課程需求刊登
                        model = this.container.GetInstance<FacadeViewModel>();
                        ViewBag.TitleS = "課程需求刊登";
                        result = HttpNotFound();
                        break;

                    case FacadeViewModel.ActionName.TeaSurveyEdit:  // 課程需求刊登編輯頁
                        model = this.container.GetInstance<FacadeViewModel>();
                        ViewBag.TitleS = "課程需求刊登編輯頁";
                        break;

                    case FacadeViewModel.ActionName.TeacherQuery:   // "課程師資介紹"
                        model = new FacadeViewModel();
                        this.facadeServ.GetTeacherQueryList(model as FacadeViewModel);
                        ViewBag.TitleS = "課程師資介紹";
                        break;

                    case FacadeViewModel.ActionName.TeacherSearch:  // "課程師資搜尋"
                        model = this.container.GetInstance<FacadeViewModel>();
                        ViewBag.TitleS = "課程師資搜尋";
                        break;

                    case FacadeViewModel.ActionName.Login: //登入 登入專區
                        model = this.container.GetInstance<LoginViewModel>();
                        ViewBag.TitleS = "登入專區";
                        break;

                    case FacadeViewModel.ActionName.ForgetPxssword: //忘記密碼
                        model = this.container.GetInstance<FacadeViewModel>();
                        ViewBag.TitleS = "忘記密碼";
                        break;

                    default:
                        ViewBag.TitleS = "系統異常";
                        LOG.ErrorFormat("查無連結: {0}", Url.Action("Index", "Facade", new { id = action }));
                        result = HttpNotFound();
                        break;
                }
            }
            catch (Exception ex)
            {
                // this.container.GetInstance(typeof(TeacherViewModel));
                //this.msg.Message = "查詢失敗";
                //this.msg.Status = false;
                Logger.Error(ex.Message, ex);
                result = HttpNotFound();
            }

            if (result == null) { result = View(viewName, model); }

            if (model == null) { result = HttpNotFound(); }
            // for 麵包屑
            ViewBag.action = id;
            // 頁底單位資訊
            ViewBag.Unit = HttpUtility.HtmlDecode(dao.GetTbContent("38"));
            // 頁底版權資訊維護資訊
            ViewBag.Copyright = HttpUtility.HtmlDecode(dao.GetTbContent("39"));

            return result;
        }

        [HttpPost]
        [SecurityFilter]
        public ActionResult Query(string id, FormCollection collection)
        {
            if (string.IsNullOrEmpty(id)) { return HttpNotFound(); }
            if (collection == null) { return HttpNotFound(); }
            FacadeViewModel.ActionName action = MyCommonUtil.ToEnum<FacadeViewModel.ActionName>(id);

            ActionResult result = null;
            string viewName = id;
            object model = null;
            LoginViewModel login = null;
            FacadeViewModel facade = null;

            switch (action)
            {
                case FacadeViewModel.ActionName.TeacherSearch:
                    facade = this.container.GetInstance<FacadeViewModel>();
                    this.UpdateModel<FacadeViewModel>(facade, collection);

                    TeacherQueryForm TQF = facade.TeacherForm;
                    if (TQF.KeyWord != null && TQF.KeyWord.Contains(@"%")) { TQF.KeyWord = TQF.KeyWord.Replace(@"%", @""); }
                    if (TQF.TeachUnit != null && TQF.TeachUnit.Contains(@"%")) { return HttpNotFound(); }
                    if (TQF.RegOp != null && TQF.RegOp.Contains(@"%")) { return HttpNotFound(); }
                    if (TQF.IndsOp != null && TQF.IndsOp.Contains(@"%")) { return HttpNotFound(); }
                    if (TQF.LiveReg != null && TQF.LiveReg.Contains(@"%")) { return HttpNotFound(); }

                    //if (TQF.KeyWord != null && !MyHelperUtil.isInt(TQF.KeyWord)) { return HttpNotFound(); }
                    if (TQF.TeachUnit != null && !MyHelperUtil.isInt(TQF.TeachUnit)) { return HttpNotFound(); }
                    if (TQF.RegOp != null && !MyHelperUtil.isInt(TQF.RegOp)) { return HttpNotFound(); }
                    if (TQF.IndsOp != null && !MyHelperUtil.isInt(TQF.IndsOp)) { return HttpNotFound(); }
                    if (TQF.LiveReg != null && !MyHelperUtil.isInt(TQF.LiveReg)) { return HttpNotFound(); }

                    this.facadeServ.GetTeacherSearchList(facade);
                    model = facade;
                    ViewBag.TitleS = "課程師資搜尋";
                    break;

                case FacadeViewModel.ActionName.Login:  // 登入，查詢使用者資料
                    login = this.container.GetInstance<LoginViewModel>();
                    this.UpdateModel<LoginViewModel>(login, collection);

                    this.facadeServ.GetLoginUser(login);
                    if (login.Grid != null)
                    {
                        result = RedirectToAction("Index", "Member", new { id = ActionName.Member.Home });
                    }
                    else
                    {
                        result = RedirectToAction("Index", new { id = "Home" });
                    }
                    model = login;
                    break;

                default:
                    result = HttpNotFound();
                    break;
            }

            if (result == null) { result = View(viewName, model); }

            // for 麵包屑
            ViewBag.action = id;

            return result;
        }


        public ActionResult Detail(string id)
        {
            string s_log1 = string.Format("##FacadeController, ActionResult Detail(string id): {0}", (!string.IsNullOrEmpty(id) ? id : "[null]"));
            Logger.Debug(s_log1);

            if (string.IsNullOrEmpty(id)) { return HttpNotFound(); }
            this.modelBinder.SetContext(this.ControllerContext, this.Binders, this.ModelState);
            FacadeViewModel.ActionName action = MyCommonUtil.ToEnum<FacadeViewModel.ActionName>(id);

            ActionResult result = null;
            string viewName = id;
            object model = null;
            FacadeViewModel facade = null;
            long? lid = null;
            IValueProvider valueProvider = new QueryStringValueProvider(this.ControllerContext);

            try
            {
                switch (action)
                {
                    case FacadeViewModel.ActionName.News:      // 最新消息明細資料
                        viewName = "NewsDetail";
                        facade = this.container.GetInstance<FacadeViewModel>();
                        facade.NewsDetail.NewsItem.id = this.modelBinder.BindSimply<long?>("NewsId", valueProvider);
                        facadeServ.GetNewsDetail(facade);
                        model = facade;
                        break;
                    case FacadeViewModel.ActionName.TeaSurvey:       // 提示頁
                        result = HttpNotFound();
                        break;
                    case FacadeViewModel.ActionName.TeaSurveyForm:   // 輸入表單
                        result = HttpNotFound();
                        break;

                    //case FacadeViewModel.ActionName.TeaSurvey:       // 提示頁
                    //    facade = this.container.GetInstance<FacadeViewModel>();
                    //    model = facade;
                    //    break;
                    //case FacadeViewModel.ActionName.TeaSurveyForm:   // 輸入表單
                    //    valueProvider = new FormValueProvider(this.ControllerContext);
                    //    ValueProviderResult res_tsagree = valueProvider.GetValue("tsagree");
                    //    //if (res == null) { result = HttpNotFound(); break; }
                    //    //string s_log1
                    //    s_log1 = string.Format("##FacadeController, tsagree: {0}", (res_tsagree == null ? "[Null]" : res_tsagree.AttemptedValue.ToString()));
                    //    Logger.Debug(s_log1);
                    //    if (res_tsagree == null)
                    //    {
                    //        facade = this.container.GetInstance<FacadeViewModel>();
                    //        facade.TeacherSurveyDetail.StartDate.SetDate(DateTime.Now);
                    //        facade.TeacherSurveyDetail.EndDate.SetDate(DateTime.Now);
                    //        viewName = "TeaSurveyForm";
                    //    }
                    //    else if (res_tsagree.AttemptedValue.ToString().Contains(@"%"))
                    //    {
                    //        //Input Validation and Representation
                    //        result = HttpNotFound();
                    //        break;
                    //    }
                    //    else if (res_tsagree.AttemptedValue.ToString().Equals("false"))
                    //    {
                    //        facade = this.container.GetInstance<FacadeViewModel>();
                    //        facade.Message = "您尚未完整閱讀使用須知，並同意相關規定事項！";
                    //        viewName = "TeaSurvey";
                    //    }
                    //    else if (res_tsagree.AttemptedValue.ToString().Equals("1,false"))
                    //    {
                    //        facade = this.container.GetInstance<FacadeViewModel>();
                    //        facade.TeacherSurveyDetail.StartDate.SetDate(DateTime.Now);
                    //        facade.TeacherSurveyDetail.EndDate.SetDate(DateTime.Now);
                    //        viewName = "TeaSurveyForm";
                    //    }
                    //    else
                    //    {
                    //        facade = this.container.GetInstance<FacadeViewModel>();
                    //        facade.Message = "您尚未完整閱讀使用須知，並同意相關規定事項！";
                    //        viewName = "TeaSurvey";
                    //        //result = HttpNotFound(); break;
                    //    }
                    //    model = facade;
                    //    break;

                    case FacadeViewModel.ActionName.TeacherDetail:      // 師資明細資料
                        viewName = "TeacherDetail";
                        facade = this.container.GetInstance<FacadeViewModel>();
                        var mid = this.modelBinder.BindSimply<long?>("mid", valueProvider);
                        var midymd = string.Concat(mid, DateTime.Now.ToString("yyyyMMddHH"));
                        var emid = this.modelBinder.BindSimply<string>("emid", valueProvider);
                        var dec_midymd = MyCommonUtil.DecodeString(emid);
                        if (!mid.HasValue || dec_midymd != midymd)
                        {
                            result = HttpNotFound();
                            break;
                        }
                        this.UpdateModel<MemDetail>(facade.TeacherDetail.MemDetail, valueProvider);
                        this.facadeServ.GetTeacherDetail(facade);
                        model = facade;
                        break;

                    case FacadeViewModel.ActionName.TeachShow:          // 師資授課花絮
                        viewName = "TeacherShow";
                        model = this.container.GetInstance<FacadeViewModel>();
                        this.UpdateModel<MemDetail>((model as FacadeViewModel).TeacherDetail.MemDetail, valueProvider);
                        lid = this.modelBinder.BindSimply<long?>("mid", valueProvider);
                        this.facadeServ.GetTeacherDetail(model as FacadeViewModel);
                        this.facadeServ.GetMemberTeachShow(model as FacadeViewModel, lid);
                        break;

                    default:
                        result = HttpNotFound();
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                result = HttpNotFound();
            }

            if (result == null) { result = View(viewName, model); }
            //if (model == null) { result = HttpNotFound(); }

            // for 麵包屑
            ViewBag.action = id;
            // 頁底單位資訊
            ViewBag.Unit = HttpUtility.HtmlDecode(dao.GetTbContent("38"));
            // 頁底版權資訊維護資訊
            ViewBag.Copyright = HttpUtility.HtmlDecode(dao.GetTbContent("39"));

            return result;
        }

        //WDACC.Models.ViewModel.Facade.FacadeViewModel
        private void InputValidateTSF(Models.ViewModel.Facade.FacadeViewModel form)
        {
            SessionModel sm = SessionModel.Get();
            if (sm.LoginValidateCode == null || string.IsNullOrEmpty(sm.LoginValidateCode))
            {
                Exception ex = new Exception("驗證碼輸出有誤");
                LOG.Warn(ex.Message, ex);
                throw ex;
            }
            if (form.ValidateCode == null || string.IsNullOrEmpty(form.ValidateCode))
            {
                Exception ex = new Exception("驗證碼輸入有誤");
                LOG.Warn(ex.Message, ex);
                throw ex;
            }
            if (!form.ValidateCode.Equals(sm.LoginValidateCode))
            {
                Exception ex = new Exception("驗證碼輸入有誤");
                LOG.Warn(ex.Message, ex);
                throw ex;
            }
            sm.LoginValidateCode = "";
        }

        [HttpPost]
        [SecurityFilter]
        public ActionResult Save(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                string s_log = string.Format("FacadeController, ActionResult Save(id IsNullOrEmpty)!!");
                Logger.Warn(s_log);
                return HttpNotFound();
            }

            FacadeViewModel.ActionName action = MyCommonUtil.ToEnum<FacadeViewModel.ActionName>(id);
            ActionResult result = null;
            FacadeViewModel model = new FacadeViewModel();
            string viewName = string.Empty;
            this.facadeServ.modelState = this.ModelState;

            try
            {
                switch (action)
                {
                    case FacadeViewModel.ActionName.TeaSurvey:
                        // 檢查驗證碼及輸入欄位
                        viewName = "TeaSurveyForm";
                        //this.UpdateModel<FacadeViewModel>(model, new FormValueProvider(this.ControllerContext));
                        //TryUpdateModel
                        this.TryUpdateModel<FacadeViewModel>(model, new FormValueProvider(this.ControllerContext));
                        this.InputValidateTSF(model);
                        this.facadeServ.SaveTeaSurvey(model);
                        result = Json(new { status = true, message = "" });
                        break;

                    case FacadeViewModel.ActionName.ForgetPxssword: //忘記密碼
                        this.UpdateModel<FacadeViewModel>(model, new FormValueProvider(this.ControllerContext));
                        this.facadeServ.ResetPassword(model);
                        result = Json(new { status = true, message = model.Message });
                        break;

                    default:
                        //result = HttpNotFound();
                        string s_log = string.Format("FacadeController, ActionResult Save(string id) :{0}", id);
                        Logger.Warn(s_log);
                        result = Json(new { status = false, message = new string[] { "參數有誤!!" } });
                        break;
                }
            }
            catch (ValidationException ex)
            {
                Logger.Error(string.Format("#ValidationException ex:{0}", ex.Message), ex);
                result = Json(new { status = false, message = ex.Errors.Select(x => x.ErrorMessage).ToList() });
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format("#Exception ex:{0}", ex.Message), ex);
                result = Json(new { status = false, message = new string[] { ex.Message } });
            }

            return result;
        }

        protected void SetPagingParamsUrl(Turbo.DataLayer.PagingResultsViewModel pagingModel, MyBaseDAO dao, string url, string callback)
        {
            pagingModel.PagingInfo = dao.PaginationInfo;
            pagingModel.rid = dao.ResultID;
            pagingModel.action = url;
            pagingModel.ajaxLoadPageCallback = callback;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FluentValidation;
using SimpleInjector;
using Turbo.MVC.Base3.Commons;
using WDACC.Commons;
using WDACC.DataLayers;
using WDACC.Models.ViewModel;
using WDACC.Services;
using Turbo.MVC.Base3.Services;
using Turbo.Commons;
using WDACC.Models.StoreExt;
using System.Collections;
using log4net;
using System.Text;
using WDACC.Models;
using OfficeOpenXml;
using System.IO;
using WDACC.Models.Entities;
using Turbo.ReportTK.Excel;

namespace WDACC.Controllers
{
    public class AdminController : Controller
    {
        private Container container;
        private MyModelBinder modelBinder;
        private LogCollection logCollection;
        private LogService logService;
        private AdminService adminService;
        private TeacherService teacherService;
        private MyBaseDAO dao;

        protected static readonly ILog LOG =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public AdminController(
            Container container,
            MyModelBinder modelBinder,
            LogCollection logCollection,
            AdminService adminServ,
            TeacherService teachServ,
            LogService logServ,
            MyBaseDAO dao)
        {
            this.container = container;
            this.modelBinder = modelBinder;
            this.logCollection = logCollection;
            this.adminService = adminServ;
            this.teacherService = teachServ;
            this.logService = logServ;
            this.dao = dao;
        }

        //public ActionResult Index() { return View(); }


        // GET: Admin
        public ActionResult Index(string id)
        {

            if (id == null || string.IsNullOrEmpty(id)) { return HttpNotFound(); }
            if (!IsLogin()) { return GoLogin(); }
            ActionName.Admin action = MyCommonUtil.ToEnum<ActionName.Admin>(id);
            this.logCollection.SetActionContext(action);

            ActionResult result = null;
            string viewName = id;
            object model = null;

            try
            {
                switch (action)
                {
                    case ActionName.Admin.Home:
                        model = this.container.GetInstance<AdminViewModel>();
                        this.adminService.GetFeedbackList(model as AdminViewModel, 1);
                        this.adminService.GetReviewCountData(model as AdminViewModel);
                        this.adminService.GetTeacherSurveyList(model as AdminViewModel, 1);
                        ViewBag.TitleS = "首頁";
                        break;

                    case ActionName.Admin.System:       //系統管理 系統設定
                        model = this.container.GetInstance<AdminViewModel>();
                        this.adminService.GetSystemDataDetail(model as AdminViewModel, null, true);   // 取得全部資料
                        ViewBag.TitleS = "系統管理";
                        break;

                    case ActionName.Admin.News:         // 最新消息
                        model = this.container.GetInstance<AdminViewModel>();
                        this.adminService.GetAdminNewsList(model as AdminViewModel);
                        ViewBag.TitleS = "最新消息";
                        break;

                    case ActionName.Admin.Teacher:      //資料設定 講師資料設定
                        model = this.container.GetInstance<AdminViewModel>();
                        ViewBag.TitleS = "資料設定";
                        break;

                    case ActionName.Admin.Account:      // 新增帳號
                        model = this.container.GetInstance<AdminViewModel>();
                        string s_drsp = Guid.NewGuid().ToString();
                        string s_DRSP = string.Concat(s_drsp.Replace("-", "").Substring(0, 6).ToLower(), s_drsp.Replace("-", "").Substring(6, 6).ToUpper());
                        ViewBag.STRDRSP = s_DRSP;
                        LOG.Debug(string.Concat("#ViewBag.STRDRSP :", s_DRSP));
                        ViewBag.TitleS = "新增帳號";
                        break;

                    case ActionName.Admin.Meeting:      // 會議活動
                        model = this.container.GetInstance<AdminViewModel>();
                        this.adminService.GetAdminMeeting(model as AdminViewModel);
                        ViewBag.TitleS = "會議活動";
                        break;

                    case ActionName.Admin.ClassReview:  // 資料審核
                        model = this.container.GetInstance<AdminViewModel>();
                        this.adminService.GetReviewCountData(model as AdminViewModel);
                        ViewBag.TitleS = "資料審核";
                        break;

                    case ActionName.Admin.Score:        // 積分資料
                        model = this.container.GetInstance<AdminViewModel>();
                        this.adminService.GetScoreList(model as AdminViewModel);
                        ViewBag.TitleS = "積分資料";
                        break;

                    case ActionName.Admin.Feedback:     // 意見回饋 
                        model = this.container.GetInstance<AdminViewModel>();
                        this.adminService.GetFeedbackList(model as AdminViewModel, null);
                        ViewBag.TitleS = "意見回饋";
                        break;

                    case ActionName.Admin.Recruit:      // 師資招募
                        model = this.container.GetInstance<AdminViewModel>();
                        this.adminService.GetTeacherSurveyList(model as AdminViewModel, null);
                        ViewBag.TitleS = "師資招募";
                        break;

                    case ActionName.Admin.Export:       // 資料匯出
                        model = this.modelBinder.BindModel<AdminViewModel>(null);
                        //this.adminService.GetExportList(model as AdminViewModel);
                        //this.adminService.GetTeacherExport(model as AdminViewModel, null);
                        ViewBag.TitleS = "資料匯出";
                        break;

                    case ActionName.Admin.MgrAccount:   // 管理者帳號管理
                        viewName = "AdminAccount";
                        model = this.modelBinder.BindModel<AdminViewModel>(null);
                        this.adminService.GetMgrAccountList(model as AdminViewModel);
                        ViewBag.TitleS = "管理者帳號設定";
                        break;

                    case ActionName.Admin.Log:          //LOG紀錄 使用歷程
                        viewName = "Log";
                        model = this.modelBinder.BindModel<LogViewModel>(null);
                        (model as LogViewModel).LoginLogForm.rid = Guid.NewGuid().ToString();
                        (model as LogViewModel).FileLogForm.rid = Guid.NewGuid().ToString();
                        (model as LogViewModel).FuncLogForm.rid = Guid.NewGuid().ToString();
                        ViewBag.TitleS = "LOG紀錄";
                        break;

                    case ActionName.Admin.FileUpload:   // 管理者檔案上傳
                        viewName = "FileUpload";
                        model = this.modelBinder.BindModel<AdminViewModel>(null);
                        ViewBag.TitleS = "教材上傳";
                        break;

                    default:
                        ViewBag.TitleS = "系統異常";
                        LOG.ErrorFormat("查無連結: {0}", Url.Action("Index", "Admin", new { id = action }));
                        result = HttpNotFound();
                        break;
                }
            }
            catch (Exception ex)
            {
                LOG.Error(ex.Message, ex);
            }

            if (model != null && model.GetType() == typeof(AdminViewModel))
            {
                (model as AdminViewModel).Action = action;
            }

            if (result == null) { result = View(viewName, model); }

            this.logService.WriteFuncLogWithCollection();

            ViewBag.action = id;    // for 麵包屑

            return result;
        }

        [HttpPost]
        public ActionResult Query(string id, FormCollection collection)
        {
            if (!IsLogin()) { return GoLogin(); }

            ActionName.Admin action = MyCommonUtil.ToEnum<ActionName.Admin>(id);
            this.modelBinder.SetContext(this.ControllerContext, this.Binders, this.ModelState);

            ActionResult result = null;
            string viewName = id;
            LogViewModel logModel = null;
            //CourseViewModel courseModel = null;
            object model = null;
            var qs = new QueryStringValueProvider(this.ControllerContext);
            string formid = this.modelBinder.BindSimply<string>("formid", qs);
            this.dao.SetPageInfo(null, "1", "30");

            switch (action)
            {
                case ActionName.Admin.LoginLog:     // 登入log
                    logModel = this.modelBinder.BindModel<LogViewModel>(collection);
                    this.adminService.QueryLoginLogList(logModel);
                    this.SetRowNumber<Store>((logModel as LogViewModel).LoginGrid, 30, 1);
                    var resList = this.GetRowPartial<Store>(this.ControllerContext, "Log/_LoginLogGridRows", logModel.LoginGrid);
                    result = Json(new { data = resList, totalNumber = this.dao.TotalRecords }, JsonRequestBehavior.AllowGet);
                    break;

                case ActionName.Admin.FileLog:      // 檔案上傳log
                    logModel = this.modelBinder.BindModel<LogViewModel>(collection);
                    this.adminService.QueryFileLogList(logModel);
                    this.SetRowNumber<Store>((logModel as LogViewModel).FileGrid, 30, 1);
                    resList = this.GetRowPartial<Store>(this.ControllerContext, "Log/_FileLogGridRows", logModel.FileGrid);
                    result = Json(new { data = resList, totalNumber = this.dao.TotalRecords }, JsonRequestBehavior.AllowGet);
                    break;

                case ActionName.Admin.FuncLog:      // 功能操作log
                    logModel = this.modelBinder.BindModel<LogViewModel>(collection);
                    this.adminService.QueryFuncLogList(logModel);
                    this.SetRowNumber<Store>((logModel as LogViewModel).FuncGrid, 30, 1);
                    resList = this.GetRowPartial<Store>(this.ControllerContext, "Log/_FuncLogGridRows", logModel.FuncGrid);
                    result = Json(new { data = resList, totalNumber = this.dao.TotalRecords }, JsonRequestBehavior.AllowGet);
                    break;

                case ActionName.Admin.CourseAudit:  // 查詢課程審核資料
                    model = this.modelBinder.BindModel<AdminViewModel>(collection);
                    this.adminService.GetReviewCountData(model as AdminViewModel);
                    result = PartialView("ClassReview/_CourseAuditRows", model);
                    break;

                case ActionName.Admin.FileAudit:    // 查詢檔案上傳審核資料
                    model = this.modelBinder.BindModel<AdminViewModel>(collection);
                    this.adminService.GetReviewCountData(model as AdminViewModel);
                    result = PartialView("ClassReview/_FileAuditRows", model);
                    break;

                case ActionName.Admin.Meeting:      // 查詢年度會議資料
                    model = this.modelBinder.BindModel<AdminViewModel>(collection);
                    this.adminService.GetAdminMeeting(model as AdminViewModel);
                    result = PartialView("Meeting/_GridRows", model);
                    break;

                case ActionName.Admin.MeetingMember:    // 查詢會議參與人員
                    model = this.modelBinder.BindModel<AdminViewModel>(collection);
                    var name = collection.GetValue("member").AttemptedValue;
                    this.adminService.GetMeetMemberList(model as AdminViewModel, name);
                    result = PartialView("Meeting/_MemberResult", model);
                    break;

                case ActionName.Admin.Score:    // 講師積分
                    model = this.modelBinder.BindModel<AdminViewModel>(collection);
                    this.adminService.GetScoreList(model as AdminViewModel);
                    break;

                case ActionName.Admin.Teacher:  // 講師資料設定
                    model = this.modelBinder.BindModel<AdminViewModel>(collection);
                    this.adminService.GetTeacherInfoList(model as AdminViewModel);    // 取得查詢的講師資料清單
                    break;

                case ActionName.Admin.MgrAccount:   // 管理者帳號查詢
                    viewName = "AdminAccount";
                    model = this.modelBinder.BindModel<AdminViewModel>(collection);
                    this.adminService.GetMgrAccountList(model as AdminViewModel);
                    break;

                default:
                    LOG.ErrorFormat("查無連結: {0}", Url.Action("Query", "Admin", new { id = action }));
                    result = HttpNotFound();
                    break;
            }

            if (!string.IsNullOrEmpty(formid)) { Session[formid] = this.dao.ResultID; }

            if (result == null) { result = View(viewName, model); }

            this.logCollection.SetActionContext(action);

            this.logService.WriteFuncLogWithCollection();

            return result;
        }

        /// <summary>
        ///  分頁
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Paging(string id)
        {
            ActionName.Admin action = MyCommonUtil.ToEnum<ActionName.Admin>(id);
            this.modelBinder.SetContext(this.ControllerContext, this.Binders, this.ModelState);
            IValueProvider vp = new QueryStringValueProvider(this.ControllerContext);

            string formid = this.modelBinder.BindSimply<string>("formid", vp);
            string rid = this.modelBinder.BindSimply<string>("rid", vp);
            if (string.IsNullOrEmpty(rid)) { rid = Session[formid].ToString(); }
            string p = this.modelBinder.BindSimply<string>("pageNumber", vp);
            string size = this.modelBinder.BindSimply<string>("pageSize", vp);

            this.dao.SetPageInfo(rid, p);
            ActionResult result = null;
            object model = null;
            IList<string> resList = new List<string>();
            string viewName = string.Empty;

            try
            {
                switch (action)
                {
                    case ActionName.Admin.News:
                        model = this.container.GetInstance<AdminViewModel>();
                        var gridType = vp.GetValue("tab").AttemptedValue.ToString();
                        IList<Store> grid = null;
                        switch (gridType)
                        {
                            case "1":
                                this.adminService.GetAdminNewsTab1(model as AdminViewModel);
                                grid = (model as AdminViewModel).NewsModel.Grid1;
                                break;
                            case "2":
                                this.adminService.GetAdminNewsTab2(model as AdminViewModel);
                                grid = (model as AdminViewModel).NewsModel.Grid2;
                                break;
                            case "3":
                                this.adminService.GetAdminNewsTab3(model as AdminViewModel);
                                grid = (model as AdminViewModel).NewsModel.Grid3;
                                break;
                            case "4":
                                this.adminService.GetAdminNewsTab4(model as AdminViewModel);
                                grid = (model as AdminViewModel).NewsModel.Grid4;
                                break;
                        }
                        this.SetRowNumber<Store>(grid, size.TOInt32(), p.TOInt32());
                        model = this.modelBinder.BindModel<AdminViewModel>(null);
                        resList = this.GetRowPartial<Store>(this.ControllerContext, "News/_GridRows", grid);
                        break;

                    case ActionName.Admin.LoginLog:
                        if (rid != null)
                        {
                            model = this.container.GetInstance<LogViewModel>();
                            this.adminService.QueryLoginLogList(model as LogViewModel, true);
                            this.SetRowNumber<Store>((model as LogViewModel).LoginGrid, size.TOInt32(), p.TOInt32());
                            resList = this.GetRowPartial<Store>
                                (this.ControllerContext, "Log/_LoginLogGridRows", (model as LogViewModel).LoginGrid);
                        }
                        break;

                    case ActionName.Admin.FileLog:
                        if (rid != null)
                        {
                            model = this.container.GetInstance<LogViewModel>();
                            this.adminService.QueryFileLogList(model as LogViewModel, true);
                            this.SetRowNumber<Store>((model as LogViewModel).FileGrid, size.TOInt32(), p.TOInt32());
                            resList = this.GetRowPartial<Store>
                                (this.ControllerContext, "Log/_FileLogGridRows", (model as LogViewModel).FileGrid);
                        }
                        break;

                    case ActionName.Admin.FuncLog:
                        if (rid != null)
                        {
                            model = this.container.GetInstance<LogViewModel>();
                            this.adminService.QueryFuncLogList(model as LogViewModel, true);
                            this.SetRowNumber<Store>((model as LogViewModel).FuncGrid, size.TOInt32(), p.TOInt32());
                            resList = this.GetRowPartial<Store>
                                (this.ControllerContext, "Log/_FuncLogGridRows", (model as LogViewModel).FuncGrid);
                        }
                        break;

                    default:
                        LOG.ErrorFormat("查無連結: {0}", Url.Action("Paging", "Admin", new { id = action }));
                        result = HttpNotFound();
                        break;
                }
            }
            catch (Exception ex)
            {
                LOG.Error(ex.Message, ex);
                result = HttpNotFound();
            }

            if (result == null)
            {
                result = Json(new { data = resList, totalNumber = this.dao.TotalRecords }, JsonRequestBehavior.AllowGet);
            }

            this.logCollection.SetActionContext(action);

            this.logService.WriteFuncLogWithCollection();

            return result;
        }

        public ActionResult Detail(string id)
        {
            if (!IsLogin()) { return GoLogin(); }

            ActionName.Admin action = MyCommonUtil.ToEnum<ActionName.Admin>(id);
            this.modelBinder.SetContext(this.ControllerContext, this.Binders, this.ModelState);
            ActionResult result = null;
            string viewName = id;
            object model = null;

            try
            {
                switch (action)
                {
                    case ActionName.Admin.System:       // 系統設定
                        viewName = "SystemDetail";
                        model = this.modelBinder.BindModel<AdminViewModel>(null);
                        short? nid = new QueryStringValueProvider(this.ControllerContext).GetValue("nid").AttemptedValue.TOInt16();
                        this.adminService.GetSystemDataDetail(model as AdminViewModel, nid, false);
                        break;

                    case ActionName.Admin.NewsEdit:     // 最新消息編輯
                        viewName = "News";
                        model = this.modelBinder.BindModel<AdminViewModel>(null);
                        long? newsid = new QueryStringValueProvider(this.ControllerContext).GetValue("nid").AttemptedValue.TOInt64();
                        this.adminService.GetAdminNewsList(model as AdminViewModel);
                        this.adminService.GetAdminNewsDetail(model as AdminViewModel, newsid);
                        break;

                    case ActionName.Admin.CourseAudit:  // 課程審核編輯頁
                        viewName = "CourseAudit";
                        model = this.modelBinder.BindModel<AdminViewModel>(null);
                        this.UpdateModel((model as AdminViewModel).AuditParam, new QueryStringValueProvider(this.ControllerContext));
                        this.adminService.GetReviewDataCourse(model as AdminViewModel);
                        break;

                    case ActionName.Admin.FileAudit:    // 檔案上傳審核編輯頁
                        viewName = "FilesAudit";
                        model = this.modelBinder.BindModel<AdminViewModel>(null);
                        this.UpdateModel((model as AdminViewModel).AuditParam, new QueryStringValueProvider(this.ControllerContext));
                        this.adminService.GetReviewDataFile(model as AdminViewModel);
                        break;

                    case ActionName.Admin.Meeting:      // 會議編輯頁
                        viewName = "MeetingDetail";
                        model = this.modelBinder.BindModel<AdminViewModel>(null);
                        long? metid = new QueryStringValueProvider(this.ControllerContext).GetValue("metid").AttemptedValue.TOInt64();
                        this.adminService.GetAdminMeetingDetail(model as AdminViewModel, metid);
                        break;

                    case ActionName.Admin.Recruit:      // 師資招募
                        viewName = "RecruitDetail";
                        model = this.modelBinder.BindModel<AdminViewModel>(null);
                        long? tsid = new QueryStringValueProvider(this.ControllerContext).GetValue("tsid").AttemptedValue.TOInt64();
                        this.adminService.GetTeacherSurveyDetail(model as AdminViewModel, tsid);
                        break;

                    case ActionName.Admin.Feedback:     // 意見回饋
                        viewName = "FeedbackDetail";
                        model = this.modelBinder.BindModel<AdminViewModel>(null);
                        long? opid = new QueryStringValueProvider(this.ControllerContext).GetValue("opid").AttemptedValue.TOInt64();
                        this.adminService.GetFeedbackDetail(model as AdminViewModel, opid);
                        break;

                    case ActionName.Admin.Teacher:      // 講師資料編輯
                        viewName = "TeacherDetail";
                        model = this.modelBinder.BindModel<AdminViewModel>(null);
                        long? mid = new QueryStringValueProvider(this.ControllerContext).GetValue("mid").AttemptedValue.TOInt64();
                        this.adminService.GetTeacherInfoDetail(model as AdminViewModel, mid);    // 取得查詢的講師資料清單
                        break;

                    case ActionName.Admin.MgrAccount:   // 管理者帳號明細
                        viewName = "AdminAccountDetail";
                        model = this.modelBinder.BindModel<AdminViewModel>(null);
                        long? maid = new QueryStringValueProvider(this.ControllerContext).GetValue("mid").AttemptedValue.TOInt64();
                        this.adminService.GetMgrAccountDetail(model as AdminViewModel, maid);
                        break;

                    default:
                        LOG.ErrorFormat("查無連結: {0}", Url.Action("Detail", "Admin", new { id = action }));
                        result = HttpNotFound();
                        break;
                }
            }
            catch (Exception ex)
            {
                LOG.Error(ex.Message, ex);
                result = HttpNotFound();
            }

            if (result == null) { result = View(viewName, model); }

            this.logCollection.SetActionContext(action);

            this.logService.WriteFuncLogWithCollection();

            ViewBag.action = id;

            return result;
        }

        [HttpPost]
        public ActionResult Ajax(string id)
        {
            ActionName.Admin action = MyCommonUtil.ToEnum<ActionName.Admin>(id);
            this.modelBinder.SetContext(this.ControllerContext, this.Binders, this.ModelState);

            ActionResult result = null;
            string viewName = id;
            object model = null;
            IList<Store> list = null;

            FormValueProvider vp = new FormValueProvider(this.ControllerContext);

            switch (action)
            {
                case ActionName.Admin.CityZipCode:
                    list = this.teacherService.GetZipCode();
                    result = Json(new { status = true, msg = "", data = list });
                    break;

                case ActionName.Admin.QueryMemberName:
                    var keyword = this.modelBinder.BindSimply<string>("keyword", vp);
                    list = this.adminService.GetMemberList(keyword);
                    result = Json(new { status = true, msg = "", data = list });
                    break;

                default:
                    result = HttpNotFound();
                    break;
            }

            if (result == null) { result = View(viewName, model); }

            return result;
        }

        [HttpPost]
        public ActionResult Save(string id, FormCollection collection)
        {
            if (!IsLogin()) { return GoLogin(); }

            ActionName.Admin action = MyCommonUtil.ToEnum<ActionName.Admin>(id);
            this.modelBinder.SetContext(this.ControllerContext, this.Binders, this.ModelState);

            ActionResult result = null;
            string viewName = id;
            object model = null;

            ValueProviderCollection vpc = new ValueProviderCollection();
            HttpFileCollectionValueProvider filevp = new HttpFileCollectionValueProvider(this.ControllerContext);
            vpc.Add(collection);
            vpc.Add(filevp);

            try
            {
                switch (action)
                {
                    case ActionName.Admin.System:       // 系統設定
                        model = this.modelBinder.BindModel<AdminViewModel>(collection);
                        this.adminService.SaveIntro(model as AdminViewModel);
                        result = Json(new { status = true, message = "" });
                        break;

                    case ActionName.Admin.TeacherInfo:  // 儲存講師資料編輯
                        model = this.modelBinder.BindModel<AdminViewModel>(collection);
                        this.adminService.SaveTeacherInfo(model as AdminViewModel);
                        result = Json(new { status = true, message = "" });
                        break;

                    case ActionName.Admin.Induclass:    // 修改個人基本資料可授課產業別
                        model = this.modelBinder.BindModel<AdminViewModel>(collection);
                        this.adminService.SaveTeachIndClass(model as AdminViewModel);
                        result = Json(new { status = true, message = "" });
                        break;

                    case ActionName.Admin.ChangePxssword:   // 修改密碼
                        model = this.modelBinder.BindModel<AdminViewModel>(collection);
                        this.adminService.SaveTeacherPassword(model as AdminViewModel);
                        result = Json(new { status = true, message = "" });
                        break;

                    case ActionName.Admin.Account:      // 儲存帳號 
                        model = this.modelBinder.BindModel<AdminViewModel>(collection);
                        this.adminService.SaveAccount(model as AdminViewModel);
                        result = Json(new { status = true, message = "" });
                        break;

                    case ActionName.Admin.Meeting:      // 儲存會議資料
                        model = this.modelBinder.BindModel<AdminViewModel>(collection);
                        this.UpdateModel((model as AdminViewModel).MeetingModel, collection);
                        this.adminService.SaveMeeting(model as AdminViewModel);
                        result = Json(new { status = true, message = "" });
                        break;

                    case ActionName.Admin.CourseAudit:  // 課程審核
                        model = this.modelBinder.BindModel<AdminViewModel>(collection);
                        this.adminService.SaveCourseAudit(model as AdminViewModel);
                        result = Json(new { status = true, message = "" });
                        break;

                    case ActionName.Admin.FileAudit:    // 檔案審核
                        model = this.modelBinder.BindModel<AdminViewModel>(collection);
                        this.adminService.SaveShareFileAudit(model as AdminViewModel);
                        result = Json(new { status = true, message = "" });
                        break;

                    case ActionName.Admin.Feedback:    // 意見回覆 
                        model = this.modelBinder.BindModel<AdminViewModel>(collection);
                        this.adminService.SaveFeedbackDetail(model as AdminViewModel);
                        result = Json(new { status = true, message = "" });
                        break;

                    case ActionName.Admin.News:     // 最新消息儲存
                        model = this.modelBinder.BindModel<AdminViewModel>(collection);
                        (model as AdminViewModel).NewsModel.Files = new List<HttpPostedFileBase>();
                        (model as AdminViewModel).NewsModel.Comments = new List<string>();
                        this.UpdateModel((model as AdminViewModel).NewsModel, new HttpFileCollectionValueProvider(this.ControllerContext));
                        this.UpdateModel((model as AdminViewModel).NewsModel, collection);
                        this.adminService.SaveNewsDetail(model as AdminViewModel);
                        result = Json(new { status = true, message = "" });
                        break;

                    case ActionName.Admin.Recruit:  // 師資招募審核
                        model = this.modelBinder.BindModel<AdminViewModel>(collection);
                        this.adminService.SaveTeasurveyDetail(model as AdminViewModel);
                        result = Json(new { status = true, message = "" });
                        break;

                    case ActionName.Admin.NewsOpen:     // 最新消息上架 &
                    case ActionName.Admin.NewsClose:    // 最新消息下架, 共用同一邏輯
                        model = this.modelBinder.BindModel<AdminViewModel>(collection);
                        long nid = new QueryStringValueProvider(this.ControllerContext).GetValue("nid").AttemptedValue.TOInt64();
                        this.adminService.SaveNewsCloseOpen(model as AdminViewModel, nid);
                        result = Json(new { status = true, message = "" });
                        break;

                    case ActionName.Admin.MgrAccount:   // 管理者帳號儲存
                        model = this.modelBinder.BindModel<AdminViewModel>(collection);
                        this.adminService.SaveMgrAccount(model as AdminViewModel);
                        result = Json(new { status = true, message = "" });
                        break;

                    case ActionName.Admin.FileUpload:   // 管理者檔案上傳
                        model = this.modelBinder.BindModel<AdminViewModel>(vpc);
                        this.adminService.SaveFileUpload(model as AdminViewModel);
                        result = Json(new { status = true, message = "" });
                        break;

                    default:
                        LOG.ErrorFormat("查無連結: {0}", Url.Action("Save", "Admin", new { id = action }));
                        result = HttpNotFound();
                        break;
                }
            }
            catch (ValidationException ex)
            {
                LOG.Warn(ex.Message, ex);
                // 欄位檢核錯誤
                result = Json(new { status = false, message = ex.Errors.Select(x => x.ErrorMessage).ToList() });
            }
            catch (Exception ex)
            {
                LOG.Error(ex.Message, ex);
                // 意外錯誤
                result = Json(new { status = false, message = ex.Message.Split(',').ToList() });
            }

            if (model != null) { (model as AdminViewModel).Action = action; }

            if (result == null) { result = View(viewName, model); }

            this.logCollection.SetActionContext(action);

            this.logService.WriteLogWithCollection();

            return result;
        }

        /// <summary> 匯出報表 </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Export(string id, FormCollection collection)
        {
            ActionName.Admin action = MyCommonUtil.ToEnum<ActionName.Admin>(id);
            this.modelBinder.SetContext(this.ControllerContext, this.Binders, this.ModelState);

            ActionResult result = null;
            object model = null;
            string viewName = string.Empty;
            string downloadFileName = string.Empty;
            Action<ExcelWorksheet> Proc = (ExcelWorksheet sheet) =>
            {
                for (int i = 1; i <= sheet.Dimension.End.Column; i++)
                {
                    sheet.Column(i).Width = 40;
                }
                sheet.Cells.Style.WrapText = true;
            };

            try
            {
                switch (action)
                {
                    case ActionName.Admin.CourseAudit:
                        viewName = "Export/_ExportScoreAudit";
                        downloadFileName = string.Format("export_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd"));
                        model = this.modelBinder.BindModel<AdminViewModel>(collection);
                        (model as AdminViewModel).AuditExportModel.ScoreAuditGrid.QueryForListAll(this.dao);
                        break;

                    case ActionName.Admin.FileAudit:
                        viewName = "Export/_ExportFileAudit";
                        downloadFileName = string.Format("export_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd"));
                        model = this.modelBinder.BindModel<AdminViewModel>(collection);
                        (model as AdminViewModel).AuditExportModel.FileAuditGrid.QueryForListAll(this.dao);
                        break;

                    case ActionName.Admin.Meeting:      // 會議匯出報表
                        viewName = "Export/_ExportMeeting";
                        downloadFileName = string.Format("export_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd"));
                        model = this.modelBinder.BindModel<AdminViewModel>(collection);
                        this.adminService.GetSessionExportList(model as AdminViewModel);
                        break;

                    case ActionName.Admin.Score:        // 積分資料匯出
                        viewName = "Export/_ExportScoreView";
                        downloadFileName = string.Format("export_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd"));
                        model = this.modelBinder.BindModel<AdminViewModel>(collection);
                        (model as AdminViewModel).ScoreViewExportModel.Grid.GetData(this.dao);
                        break;

                    case ActionName.Admin.TeacherInfo:  // 選單 > 資料匯出
                        viewName = "Export/_ExportMemberInfo";
                        downloadFileName = string.Format("export_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd"));
                        model = this.modelBinder.BindModel<AdminViewModel>(collection);
                        this.adminService.GetExportList(model as AdminViewModel);
                        (model as AdminViewModel).ExportModel.SyncItemList();
                        Proc = (ExcelWorksheet sheet) => { (model as AdminViewModel).ExportModel.SetupColunm(sheet); };
                        break;

                    case ActionName.Admin.Feedback:   // 意見回饋 
                        viewName = "Export/_ExportFeedback";
                        downloadFileName = string.Format("export_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd"));
                        model = this.modelBinder.BindModel<AdminViewModel>(collection);
                        (model as AdminViewModel).FeedbackExportModel.GetData(this.dao);
                        break;

                    case ActionName.Admin.Exportlogs: // 匯出每月登入異常日誌
                        viewName = "Export/_ExportMonthlylogs";
                        downloadFileName = string.Format("export_{0}.xlsx", DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                        model = this.modelBinder.BindModel<AdminViewModel>(collection);
                        if (collection != null) {
                            (model as AdminViewModel).ExpYear1 = collection["LoginLogForm.ExpYear1"];
                            (model as AdminViewModel).ExpMonth1 = collection["LoginLogForm.ExpMonth1"];
                        }
                        (model as AdminViewModel).MonthlylogsExportModel.GetData(this.dao);
                        break;

                    default:
                        result = HttpNotFound();
                        break;
                }

                if (result == null)
                {
                    StringBuilder content = ControllerContextHelper.RenderRazorPartialViewToString(this.ControllerContext, viewName, model);
                    //LOG.Debug("#content#"); //LOG.Debug(content.ToString());
                    HtmlTableToExcel hte = new HtmlTableToExcel();
                    ExcelPackage excel = new ExcelPackage();
                    MemoryStream ms = new MemoryStream();
                    hte.Process(content.ToString(), out ms);
                    excel.Load(ms);

                    var sheet = excel.Workbook.Worksheets.FirstOrDefault();
                    sheet.Cells.Style.Font.Bold = false;
                    if (Proc != null && sheet != null) { Proc.Invoke(sheet); }

                    MemoryStream savems = new MemoryStream();
                    excel.SaveAs(savems);
                    result = File(savems.ToArray(), "application/octet-stream", downloadFileName);
                }
            }
            catch (Exception ex)
            {
                LOG.Error(ex.Message, ex);
            }

            // this.logService.WriteFuncLogWithCollection();
            return result;
        }


        public ActionResult Remove(string id)
        {
            ActionName.Admin action = MyCommonUtil.ToEnum<ActionName.Admin>(id);
            this.logCollection.SetActionContext(action);
            ActionResult result = null;
            //AdminViewModel model = null;
            IValueProvider collection = new FormValueProvider(this.ControllerContext);

            try
            {
                switch (action)
                {
                    case ActionName.Admin.MgrAccount:   // 刪除管理者帳號
                        long? mid = this.modelBinder.BindSimply<long?>("mid", collection);
                        this.adminService.RemoveMgrAccount(mid);
                        result = Json(new { status = true, messag = "" });
                        break;

                    default:
                        result = HttpNotFound();
                        break;
                }
            }
            catch (Exception ex)
            {
                LOG.Error(ex.Message, ex);
                result = HttpNotFound();
            }

            this.logCollection.SetActionContext(action);

            this.logService.WriteFuncLogWithCollection();

            return result;
        }

        public ActionResult FixUpload(
            string fid,
            string name,
            string title,
            HttpPostedFileBase SFile,
            HttpPostedFileBase IFile,
            HttpPostedFileBase AFile)
        {
            if (!IsLogin()) { return GoLogin(); }

            ActionResult result = null;
            this.modelBinder.SetContext(this.ControllerContext, this.Binders, this.ModelState);

            if (Request.HttpMethod == "GET")
            {
                if (!string.IsNullOrEmpty(fid))
                {
                    ViewBag.fid = fid;
                    ViewBag.name = name;
                    ViewBag.filetitle = title;
                    ViewBag.prevurl = System.Web.HttpContext.Current.Request.UrlReferrer;

                    result = View("FixUpload");
                }
                else
                {
                    result = RedirectToAction("Index", new { id = "Home" });
                }
            }
            else if (Request.HttpMethod == "POST")
            {
                var vp = new FormValueProvider(this.ControllerContext);
                var vpf = new HttpFileCollectionValueProvider(this.ControllerContext);
                long? ssid = this.modelBinder.BindSimply<long?>("Id", vp);

                // 欄位檢核
                var afudt = new Models.ViewModel.Admin.AdminFileUploadModel();
                afudt.SFile = SFile;
                afudt.IFile = IFile;
                afudt.AFile = AFile;
                // 欄位檢核
                var rst = new Validator.FileUplaodModelValidator2().Validate(afudt);
                if (!rst.IsValid)
                {
                    ValidationException ex = new ValidationException(rst.Errors);
                    LOG.Warn(ex.Message, ex); //throw ex; //LOG.Error(ex.Message, ex);

                    ViewBag.fid = fid;
                    ViewBag.name = name;
                    ViewBag.filetitle = title;
                    ViewBag.message = ex.Message;
                    ViewBag.disabled = "disabled";
                    //result = RedirectToAction("FixUpload", "Admin", new { fid = fid, name = name, title = title });
                    result = View("FixUpload");
                    return result;
                }

                Tuple<string, string> sfile = null;
                Tuple<string, string> ifile = null;
                Tuple<string, string> afile = null;
                try
                {
                    FileService fs = this.container.GetInstance<FileService>();
                    sfile = fs.UploadShareFile(afudt.SFile, "");
                    ifile = fs.UploadShareFile(afudt.IFile, "-1");
                    afile = fs.UploadShareFile(afudt.AFile, "-2");
                }
                catch (Exception ex)
                {
                    LOG.Error(ex.Message, ex);
                    dao.RollBackTransaction();

                    ViewBag.fid = fid;
                    ViewBag.name = name;
                    ViewBag.filetitle = title;
                    ViewBag.message = ex.Message;
                    ViewBag.disabled = "disabled";
                    result = View("FixUpload");
                    return result;
                }

                dao.BeginTransaction();
                try
                {
                    var model = new Memshare
                    {
                        filename = sfile.Item1,
                        ifilename = ifile.Item1,
                        afilename = afile.Item1
                    };

                    dao.Update(model, new Memshare { id = ssid });

                    dao.CommitTransaction();

                    ViewBag.message = "上傳成功";
                    ViewBag.disabled = "disabled";
                    result = View("FixUpload");
                }
                catch (Exception ex)
                {
                    LOG.Error(ex.Message, ex);
                    dao.RollBackTransaction();

                    ViewBag.fid = fid;
                    ViewBag.name = name;
                    ViewBag.filetitle = title;
                    ViewBag.message = ex.Message;
                    ViewBag.disabled = "disabled";
                    result = View("FixUpload");
                    //result = RedirectToAction("FixUpload", "Admin", new { fid = fid, name = name, title = title });
                    return result;
                }
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

        private void SetRowNumber<T1>(IList<T1> grid, int size, int p) where T1 : Hashtable
        {
            for (int i = 0; i < grid.Count; i++)
            {
                var item = grid[i];
                ((T1)item)["sno"] = MyCommonUtil.GetPageRowIndex(size, p, i);
            }
        }

        private IList<string> GetRowPartial<T>(ControllerContext context, string viewName, IList<T> grid) where T : Hashtable
        {
            IList<string> resList = new List<string>();
            foreach (var item in grid)
            {
                var sb = ControllerContextHelper.RenderRazorPartialViewToString(context, viewName, item);
                resList.Add(sb.ToString());
            }
            return resList;
        }

        private bool IsLogin()
        {
            bool result = true;
            SessionModel sess = SessionModel.Get();
            if (sess.UserInfo == null || sess.UserInfo.LoginSuccess == false || sess.UserInfo.User.role != 0)
            {
                result = false;
            }
            return result;
        }

        private ActionResult GoLogin()
        {
            return RedirectToAction("Index", "Facade", new { id = "Login" });
        }

    }
}

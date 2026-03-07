using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WDACC.Commons;
using SimpleInjector;
using WDACC.Services;
using WDACC.Models.ViewModel;
using WDACC.Models.StoreExt;
using FluentValidation;
using WDACC.Models.ViewModel.Member;
using Turbo.MVC.Base3.Services;
using log4net;
using Turbo.MVC.Base3.Commons;
using WDACC.Models;

namespace WDACC.Controllers
{
    public class MemberController : Controller
    {
        private Container container;
        private MyModelBinder modelBinder;
        private LogCollection logCollection;
        private TeacherService teacherServ;
        private CourseService courseServ;
        private ResultMessage msg;
        private LogService logServ;

        protected static readonly ILog LOG =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public MemberController(
            Container container, MyModelBinder modelBinder,
            LogCollection logCollection, ResultMessage msg,
            TeacherService teacherServ,
            CourseService courseServ,
            LogService logServ)
        {
            this.container = container;
            this.modelBinder = modelBinder;
            this.logCollection = logCollection;
            this.teacherServ = teacherServ;
            this.courseServ = courseServ;
            this.msg = msg;
            this.logServ = logServ;
        }

        // GET: Teacher
        public ActionResult Index(string id)
        {
            if (id == null || string.IsNullOrEmpty(id)) { return HttpNotFound(); }
            if (!IsLogin()) { return GoLogin(); }
            // 如果未修改密碼，強制導到修改密碼頁 
            if (IsChgPwdRequired() && id != "Home" && id != "MemdeupHome")
            {
                return RedirectToAction("Index", "Member", new { id = "Home" });
            }

            ActionName.Member action = MyCommonUtil.ToEnum<ActionName.Member>(id);
            this.modelBinder.SetContext(this.ControllerContext, this.Binders, this.ModelState);
            ActionResult result = null;
            string viewName = id;
            object model = null;
            MemberViewModel teacher = null;

            try
            {
                switch (action)
                {
                    case ActionName.Member.Home:    //首頁 師資登入首頁
                        viewName = "Home";
                        teacher = this.container.GetInstance<MemberViewModel>();
                        this.teacherServ.GetTeacherNews(teacher);   // 取得最新消息
                        (teacher as MemberViewModel).ScoreView.Year = DateTime.Now.Year;
                        this.teacherServ.ScoreSummary(teacher);     // 讀取積分表資料
                        ViewBag.TitleS = "首頁";
                        break;

                    case ActionName.Member.News:    // 師資最新消息
                        viewName = "News";
                        teacher = this.container.GetInstance<MemberViewModel>();
                        this.teacherServ.GetTeacherNews(teacher);
                        ViewBag.TitleS = "師資最新消息";
                        break;

                    case ActionName.Member.Tcsv:    // 課程師資招募
                        viewName = "Tcsv";
                        teacher = this.container.GetInstance<MemberViewModel>();
                        this.teacherServ.GetTeacherTcsvList(teacher);
                        ViewBag.TitleS = "課程師資招募";
                        break;

                    case ActionName.Member.ScoreHome:  //師資積分課程登錄說明事項 課程登錄首頁宣告, 顯示登錄事項說明
                        viewName = "ScoreHome";
                        teacher = this.container.GetInstance<MemberViewModel>();
                        teacherServ.GetMessage(teacher, "Score");
                        ViewBag.TitleS = "師資積分課程登錄說明事項";
                        break;

                    case ActionName.Member.ShareHome:       //教材分享區 教材分享訊息頁
                        viewName = "ShareHome";
                        teacher = this.container.GetInstance<MemberViewModel>();
                        teacherServ.GetMessage(teacher, "Share");
                        ViewBag.TitleS = "教材分享區";
                        break;

                    case ActionName.Member.MemberShare:     // 教材分享表單, 共3個分頁
                        viewName = "MemberShare";
                        teacher = this.container.GetInstance<MemberViewModel>();
                        teacherServ.PrepareSharePage(teacher);      // 讀取頁面相關資料
                        teacher.ShareFileForm.EditMode = ActionName.EditMode.CREATE;    // 新增模式
                        ViewBag.TitleS = "教材分享區-新增";
                        break;

                    case ActionName.Member.ScoreRegister:   //積分課程登錄 課程編輯表單
                        viewName = "ScoreRegister";
                        teacher = this.container.GetInstance<MemberViewModel>();
                        this.teacherServ.ScoreRegister(teacher);  // 讀取去年度之課程資料
                        teacher.ScoreForm.EditMode = "CREATE";
                        ViewBag.TitleS = "積分課程登錄";
                        break;

                    case ActionName.Member.TeachShow:   // 授課花絮
                        viewName = "TeachShow";
                        teacher = this.container.GetInstance<MemberViewModel>();
                        this.teacherServ.GetTeacherShowDetail(teacher);   // 讀取師資授課花絮內維護表單
                        ViewBag.TitleS = "授課花絮";
                        break;

                    case ActionName.Member.ScoreView:   // 積分表 
                        viewName = "ScoreView";
                        teacher = this.container.GetInstance<MemberViewModel>();
                        teacher.ScoreView.Year = DateTime.Now.Year;
                        this.teacherServ.ScoreSummary(teacher);  // 讀取積分表資料
                        ViewBag.TitleS = "積分表";
                        break;

                    case ActionName.Member.Feedback:    // 意見回饋
                        viewName = "Feedback";
                        teacher = this.container.GetInstance<MemberViewModel>();
                        ViewBag.TitleS = "意見回饋";
                        break;

                    case ActionName.Member.MemdeupHome: // 基本資料維護
                        viewName = "Memdeup";
                        teacher = this.container.GetInstance<MemberViewModel>();
                        this.teacherServ.GetTeacherInfo(teacher);
                        ViewBag.TitleS = "基本資料維護";
                        break;

                    default:
                        ViewBag.TitleS = "系統異常";
                        LOG.ErrorFormat("查無連結: {0}", Url.Action("Index", "Member", new { id = action }));
                        result = HttpNotFound();
                        break;
                }
            }
            catch (Exception ex)
            {
                LOG.Error(ex.Message, ex);
                this.msg.Message = "查詢失敗";
                this.msg.Status = false;
            }

            if (model == null) { model = teacher; }

            if (result == null) { result = View(viewName, model); }

            this.logCollection.SetActionContext(action);

            this.logServ.WriteFuncLogWithCollection();

            ViewBag.action = id;    // for 麵包屑

            return result;
        }


        public ActionResult Query(string id, FormCollection collection)
        {
            if (!IsLogin()) { return GoLogin(); }

            ActionName.Member action = MyCommonUtil.ToEnum<ActionName.Member>(id);
            this.modelBinder.SetContext(this.ControllerContext, this.Binders, this.ModelState);

            ActionResult result = null;
            string viewName = id;
            object model = null;
            MemberViewModel teacher = null;

            switch (action)
            {
                case ActionName.Member.ScoreRegister:   // 積分登錄 編輯已有資料
                    viewName = "Score";
                    teacher = this.modelBinder.BindModel<MemberViewModel>(collection);
                    this.teacherServ.ScoreSummary(teacher);
                    model = teacher;
                    break;

                case ActionName.Member.ShareWithMe: // 分享檔案查詢
                    viewName = "MemberShare/_ShareWithMeGridRows";
                    teacher = this.modelBinder.BindModel<MemberViewModel>(collection);
                    this.teacherServ.QueryShareWithMeFileList(teacher);
                    result = PartialView(viewName, teacher);
                    break;

                case ActionName.Member.ScoreView:   // 查詢積分表 
                    viewName = "ScoreView";
                    teacher = this.modelBinder.BindModel<MemberViewModel>(collection);
                    this.teacherServ.ScoreSummary(teacher);
                    model = teacher;
                    break;

                case ActionName.Member.Score:
                    teacher = container.GetInstance<MemberViewModel>();
                    teacher.ScoreGridParam = this.modelBinder.BindModel<ScoreGridParameter>
                        (new QueryStringValueProvider(this.ControllerContext));
                    viewName = teacher.ScoreGridParam.showoff != null ? "ScoreShowoff" : "ScoreGrid";
                    this.teacherServ.GetScoreCourseList(teacher);
                    model = teacher;
                    break;

                default:
                    LOG.ErrorFormat("查無連結: {0}", Url.Action("Query", "Member", new { id = action }));
                    result = HttpNotFound();
                    break;
            }

            if (result == null) { result = View(viewName, model); }

            this.logCollection.SetActionContext(action);
            this.logServ.WriteFuncLogWithCollection();

            ViewBag.action = id;

            return result;
        }

        public ActionResult Detail(string id)
        {
            if (!IsLogin()) { return GoLogin(); }

            ActionResult result = null;
            string viewName = id;
            object model = null;
            MemberViewModel teacher = null;
            long? lid = null;

            ActionName.Member action = MyCommonUtil.ToEnum<ActionName.Member>(id);
            this.modelBinder.SetContext(this.ControllerContext, this.Binders, this.ModelState);

            IValueProvider vp = new QueryStringValueProvider(this.ControllerContext);

            switch (action)
            {
                case ActionName.Member.News:
                    viewName = "News";
                    teacher = this.container.GetInstance<MemberViewModel>();
                    long? newsId = System.Convert.ToInt64(new QueryStringValueProvider(this.ControllerContext).GetValue("newsid").AttemptedValue);
                    this.teacherServ.GetTeacherNewsDetail(teacher, newsId);
                    break;

                case ActionName.Member.Score:
                    viewName = "ScoreRegister";
                    model = this.modelBinder.BindModel<MemberViewModel>(null);
                    lid = this.modelBinder.BindSimply<long?>("lid", vp);
                    this.teacherServ.ScoreRegister(model as MemberViewModel);   // 讀取去年度之課程資料
                    this.teacherServ.GetScoreCourseDetail(model as MemberViewModel, lid);
                    break;

                case ActionName.Member.Tcsv:
                    viewName = "Tcsv";
                    teacher = container.GetInstance<MemberViewModel>();
                    long? tcsvId = new QueryStringValueProvider(this.ControllerContext)
                        .GetValue("tsid").AttemptedValue.TOInt64();
                    this.teacherServ.GetTeacherTcsvDetail(teacher, tcsvId);
                    this.teacherServ.SaveTeacherSurveyClickInclement(tcsvId);   // 點擊數加1
                    break;

                case ActionName.Member.FileShareEdit:
                    viewName = "MemberShare";
                    teacher = this.container.GetInstance<MemberViewModel>();
                    lid = this.modelBinder.BindSimply<long?>("sfid", vp);
                    teacherServ.PrepareSharePage(teacher);      // 讀取頁面相關資料
                    teacherServ.GetShareFileDetail(teacher as MemberViewModel, lid);
                    teacher.ShareFileForm.EditMode = ActionName.EditMode.UPDATE;    // 修改模式
                    break;

                default:
                    LOG.ErrorFormat("查無連結: {0}", Url.Action("Detail", "Member", new { id = action }));
                    result = HttpNotFound();
                    break;
            }

            if (model == null) { model = teacher; }

            if (result == null) { result = View(viewName, model); }

            this.logCollection.SetActionContext(action);

            this.logServ.WriteFuncLogWithCollection();

            ViewBag.action = id;

            return result;
        }


        [HttpPost]
        public ActionResult Save(string id, FormCollection collection)
        {
            if (!IsLogin()) { return GoLogin(); }
            if (string.IsNullOrEmpty(id)) { return HttpNotFound(); }

            ActionName.Member action = MyCommonUtil.ToEnum<ActionName.Member>(id);
            this.modelBinder.SetContext(this.ControllerContext, this.Binders, this.ModelState);

            object model = null;
            string viewName = id;
            ActionResult result = null;
            MemberViewModel teacher = null;

            try
            {
                switch (action)
                {
                    case ActionName.Member.ScoreRegister:   // 儲存積分課程
                        teacher = this.modelBinder.BindModel<MemberViewModel>(collection);
                        this.TryUpdateModel(teacher.ScoreForm, new HttpFileCollectionValueProvider(this.ControllerContext)); // 綁定上傳檔案
                        this.teacherServ.SaveScore(teacher);
                        model = teacher;
                        result = Json(new { status = true, message = "" });
                        break;

                    case ActionName.Member.ScoreShowoff:    // 儲存前台顯示狀態
                        teacher = this.modelBinder.BindModel<MemberViewModel>(collection);
                        this.UpdateModel(teacher.ScoreGrid, new FormValueProvider(this.ControllerContext));
                        this.teacherServ.SaveScoreShowoff(teacher);
                        result = Json(new { status = true, message = "" });
                        model = teacher;
                        break;

                    case ActionName.Member.ShareRegister:   // 儲存分享檔案
                        teacher = this.modelBinder.BindModel<MemberViewModel>(collection);
                        this.UpdateModel(teacher.ShareFileForm, new HttpFileCollectionValueProvider(this.ControllerContext));
                        this.teacherServ.SaveShareFileDetail(teacher);
                        result = Json(new { status = true, message = "" });
                        break;

                    case ActionName.Member.TeachShow:       // 儲存授課花絮
                        teacher = this.modelBinder.BindModel<MemberViewModel>(collection);
                        this.UpdateModel(teacher.TeachShow, new HttpFileCollectionValueProvider(this.ControllerContext));
                        this.teacherServ.SaveTeacherShowDetail(teacher);
                        result = Json(new { status = true, message = "" });
                        break;

                    case ActionName.Member.Feedback:        // 儲存意見回饋
                        teacher = this.modelBinder.BindModel<MemberViewModel>(collection);
                        this.teacherServ.SaveFeedback(teacher);
                        result = Json(new { status = true, message = "" });
                        break;

                    case ActionName.Member.ChangePxssword:  // 變更密碼
                        teacher = this.modelBinder.BindModel<MemberViewModel>(collection);
                        this.teacherServ.SavePassword(teacher);
                        result = Json(new { status = true, message = "" });
                        break;

                    case ActionName.Member.TeacherInfo:     // 講師個人基本資料
                        teacher = this.modelBinder.BindModel<MemberViewModel>(collection);
                        this.teacherServ.SaveTeacherInfo(teacher);
                        result = Json(new { status = true, message = "" });
                        break;

                    case ActionName.Member.TeacherIndesc:   // 可授課產業別
                        teacher = this.modelBinder.BindModel<MemberViewModel>(collection);
                        this.teacherServ.SaveTeacherIndClass(teacher);
                        result = Json(new { status = true, message = "" });
                        break;

                    default:
                        LOG.ErrorFormat("查無連結: {0}", Url.Action("Save", "Member", new { id = action }));
                        result = HttpNotFound();
                        break;
                }
            }
            catch (ValidationException ex)  // 輸入檢核錯誤
            {
                LOG.Warn(string.Format("#ValidationException ex:{0}", ex.Message), ex);
                result = Json(new { status = false, message = ex.Errors.Select(x => x.ErrorMessage).ToList() });
            }
            catch (Exception ex)
            {
                LOG.Warn(string.Format("#Exception ex:{0}", ex.Message), ex);
                switch (action)
                {
                    case ActionName.Member.ScoreShowoff:
                        var stringList = new List<string> { "儲存失敗" };
                        result = Json(new { status = false, message = stringList });
                        break;

                    case ActionName.Member.ScoreRegister:
                        var stringList2 = new List<string> { ex.Message };
                        result = Json(new { status = false, message = stringList2 });
                        break;

                    default:
                        result = HttpNotFound();
                        break;
                }
            }

            if (result == null) { result = View(viewName, model); }

            this.logCollection.SetActionContext(action);

            this.logServ.WriteLogWithCollection();       // 寫入操作歷程Log

            return result;
        }

        public ActionResult Remove(string id)
        {
            ActionName.Member action = MyCommonUtil.ToEnum<ActionName.Member>(id);
            this.modelBinder.SetContext(this.ControllerContext, this.Binders, this.ModelState);

            ActionResult result = null;
            string viewName = id;
            object model = null;
            long? lid = null;

            IValueProvider collection = new FormValueProvider(this.ControllerContext);

            try
            {
                switch (action)
                {
                    case ActionName.Member.Score:
                        model = this.modelBinder.BindModel<MemberViewModel>(null);
                        lid = this.modelBinder.BindSimply<long?>("lid", collection);
                        this.teacherServ.RemoveScoreCourse(model as MemberViewModel, lid);
                        result = Json(new { status = true, message = "" });
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                LOG.Error(ex.Message, ex);
                result = Json(new { status = false, message = ex.Message });
            }

            return result;
        }

        [HttpPost]
        public ActionResult Ajax(string id)
        {
            ActionName.Member action = MyCommonUtil.ToEnum<ActionName.Member>(id);
            this.modelBinder.SetContext(this.ControllerContext, this.Binders, this.ModelState);

            ActionResult result = null;
            string viewName = id;
            object model = null;
            IList<Store> list = null;

            switch (action)
            {
                case ActionName.Member.CityZipCode:
                    list = this.teacherServ.GetZipCode();
                    result = Json(new { status = true, msg = "", data = list });
                    break;

                default:
                    result = HttpNotFound();
                    break;
            }

            if (result == null) { result = View(viewName, model); }

            return result;
        }

        private bool IsLogin()
        {
            bool result = true;
            SessionModel sess = SessionModel.Get();
            if (sess.UserInfo == null || sess.UserInfo.LoginSuccess == false)
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// 是否須修改密碼
        /// </summary>
        /// <returns></returns>
        private bool IsChgPwdRequired()
        {
            SessionModel sess = SessionModel.Get();
            return sess.UserInfo.ChangePwdRequired;
        }

        private ActionResult GoLogin()
        {
            return RedirectToAction("Index", "Facade", new { id = "Login" });
        }

    }
}

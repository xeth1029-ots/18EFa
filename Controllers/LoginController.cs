using log4net;
using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web.Mvc;
//using Turbo.MVC.Base3.Commons;
using Turbo.MVC.Base3.DataLayers;
using Turbo.MVC.Base3.Models;
//using Turbo.MVC.Base3.Models.Entities;
//using Turbo.MVC.Base3.Services;
using WDACC.Models.ViewModel;
using WDACC.DataLayers;
using Turbo.MVC.Base3.Commons;
using WDACC.Services;
using WDACC.Commons;
using WDACC.Models;
using SimpleInjector;
using System.Web;
using WDACC.Models.StoreExt;

namespace WDACC.Controllers
{
    public class LoginController : Controller
    {
        protected static readonly ILog LOG = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private AccountService accountService;
        private MyModelBinder modelBinder;
        private Container container;

        public LoginController(Container container, AccountService accountService, MyModelBinder modelBinder)
        {
            this.container = container;
            this.accountService = accountService;
            this.modelBinder = modelBinder;
        }

        //GET: Login
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index()
        {
            //LoginViewModel viewModel = new LoginViewModel();
            //ActionResult rtn = View("Index", viewModel);
            //SessionModel.Get().UserInfo = new LoginUserInfo { User = new ClamUser { user_name = "test" } };
            //if (UserID == null || string.IsNullOrEmpty(UserID)) { return HttpNotFound(); }
            return HttpNotFound();
            //return RedirectToAction("Index", "Facade", new { id = "Home" });
        }

        /// <summary>
        /// 使用者按下登入按鈕
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(Models.ViewModel.LoginViewModel form)
        {
            ActionResult rtn = null;
            try
            {
                this.modelBinder.SetContext(this.ControllerContext, this.Binders, this.ModelState);

                // 系統管理邏輯
                //MyBaseDAO dao = this.container.GetInstance<MyBaseDAO>();

                // 檢查驗證碼及輸入欄位
                this.InputValidate(form);

                // 登入帳密檢核, 並取得使用者帳號及權限角色清單資料
                //LoginUserInfo userInfo = this.LoginValidate(form.UserName, hsshPass);
                LoginUserInfo userInfo = accountService.GetLoginUser(form);
                userInfo.LoginIP = HttpContext.Request.UserHostAddress;

                string cst_LOGINFAIL = "LOGINFAIL";
                string cst_LOGIN = "LOGIN";
                // 登入失敗, 丟出錯誤訊息
                if (!userInfo.LoginSuccess)
                {
                    accountService.SaveUserFailCount(form.UserName);    // 錯誤次數加1
                    accountService.SetLoginLog(form.UserName, cst_LOGINFAIL, userInfo.LoginErrMessage);

                    LoginExceptions ex = new LoginExceptions(userInfo.LoginErrMessage);
                    LOG.Warn(ex.Message, ex);
                    throw ex;
                }
                else
                {
                    accountService.SaveUserFailCount(form.UserName, 0);     // 錯誤次數歸零
                    accountService.SetLoginLog(form.UserName, cst_LOGIN, "登入成功");
                }

                // 將登入者資訊保存在 SessionModel 中
                SessionModel sm = SessionModel.Get();
                sm.UserInfo = userInfo;

                LOG.DebugFormat("user no: {0}", userInfo.UserNo);

                // 將登入者群組權限功能清單保存在 SessionModel 中
                //sm.RoleFuncs = dao.GetUserRoleFuncs(userInfo.UserNo);

                if (userInfo.User.role == 0) // 管理者 
                {
                    string url = Url.Action("Index", "Admin", new { id = ActionName.Admin.Home });
                    LOG.DebugFormat("redirection to action: {0}", url);
                    rtn = RedirectToAction("Index", "Admin", new { id = ActionName.Admin.Home });
                }
                else if (userInfo.User.role == 2)  // 師資
                {
                    string url = Url.Action("Index", "Admin", new { id = ActionName.Member.Home });
                    LOG.DebugFormat("redirection to action: {0}", url);
                    rtn = RedirectToAction("Index", "Member", new { id = ActionName.Member.Home });
                }
            }
            catch (LoginExceptions ex)
            {
                LOG.Warn("Login(" + form.UserName + ") Failed from " + Request.UserHostAddress + ": " + ex.Message, ex);
                // 清除不想要 Cache POST data 的欄位
                ModelState.Remove("form.ValidateCode");

                //Models.ViewModel.LoginViewModel model = new Models.ViewModel.LoginViewModel();
                //model.UserName = form.UserName;
                //model.ErrorMessage = ex.Message;
                //rtn = View("Index", "", form);

                TempData["ErrorMessage"] = ex.Message;

                rtn = RedirectToAction("Index", "Facade", new { id = "Login" });
            }

            return rtn;
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            SessionModel sess = SessionModel.Get();
            if (sess != null && sess.UserInfo != null)
            {
                SetLogoutLog(sess.UserInfo.UserNo);
            }
            Session.RemoveAll();
            return RedirectToAction("Index", "Facade", new { id = "Home" });
        }

        /// <summary>
        /// 圖型驗證碼轉語音撥放頁
        /// </summary>
        /// <returns></returns>
        public ActionResult VCodeAudio()
        {
            return View();
        }

        /// <summary>
        /// 音檔撥放 網頁輸出
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult GetValidateAudio()
        {
            string s_urlAct = Url.Action("GetValidateCodeAudio", "Login");
            DateTime myDate1 = new DateTime(1970, 1, 9, 0, 0, 00);
            DateTime myDate2 = DateTime.Now;
            TimeSpan interval = myDate2 - myDate1;
            string s_time = interval.TotalSeconds.ToString();

            StringBuilder sb_html = new StringBuilder();
            sb_html.Append("<html lang=\"zh-Hant\">");
            sb_html.Append("<head>");
            sb_html.Append("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\"/>");
            sb_html.Append("<meta http-equiv=\"cache-control\" content=\"max-age=0\" />");
            sb_html.Append("<meta http-equiv=\"cache-control\" content=\"no-cache\" />");
            sb_html.Append("<meta http-equiv=\"expires\" content=\"0\" />");
            sb_html.Append("<meta http-equiv=\"expires\" content=\"Tue, 01 Jan 1980 1:00:00 GMT\" />");
            sb_html.Append("<meta http-equiv=\"pragma\" content=\"no-cache\" />");
            sb_html.Append("<title>音檔撥放</title>");
            //<link href="~/style/bootstrap.css" rel="stylesheet" type="text/css" />
            //<script src="~/Scripts/bootstrap.min.js"></script>
            //sb_html.Append("<link href=\"~/style/bootstrap.css\" rel=\"stylesheet\" type=\"text/css\" />");
            sb_html.Append("<link rel=\"stylesheet\" href=\"https://use.fontawesome.com/releases/v5.0.13/css/all.css\" integrity=\"sha384-DNOHZ68U8hZfKXOrtjWvjxusGo9WQnrNx2sqG0tfsghAvtVlRW3tvkXWZh58N9jp\" crossorigin=\"anonymous\">");
            //sb_html.Append("<script src=\"~/Scripts/bootstrap.min.js\"></script>");
            sb_html.Append("</head>");
            sb_html.Append("<body style=\"margin: 0; padding: 0\">");
            sb_html.Append("<audio controls=\"controls\" autoplay=\"autoplay\">");
            sb_html.Append(string.Format("<source src =\"{0}?time={1}\" type=\"audio/mpeg\" />", s_urlAct,s_time));
            sb_html.Append("您的 Browser 不支援 &lt;audio&gt; 音檔撥放");
            sb_html.Append("</audio>");
            //sb_html.Append("<h1><span style=\"display: none;\" class=\"sr-only\">音檔撥放</span></h1>");
            sb_html.Append("<h1><span class=\"sr-only\">音檔撥放</span></h1>");
            //sb_html.Append(" <noscript>");
            //sb_html.Append(" <div class=\"text-center\" >");
            //sb_html.Append(" 您的瀏覽器不支援JavaScript功能，若網頁功能無法正常使用時，請開啟瀏覽器JavaScript狀態");
            //sb_html.Append(" </div>");
            //sb_html.Append(" <style type=\"text/css\">");
            //sb_html.Append(" html { display: block; }");
            //sb_html.Append(" body { display: block !important; }");
            //sb_html.Append(" </style>");
            //sb_html.Append(" </noscript>");
            sb_html.Append("</body></html>");
            return Content(sb_html.ToString(), "text/html");
            //return View("_ValidateAudio", Facade); ;
            //return RedirectToAction("_ValidateAudio", "Facade", new { id = "ValidateAudio" });
        }

        /// <summary>
        /// 重新產生並回傳驗證碼圖片檔案內容
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult GetValidateCode()
        {
            Turbo.Commons.ValidateCode vc = new Turbo.Commons.ValidateCode();
            string vCode = vc.CreateValidateCode(4);
            SessionModel.Get().LoginValidateCode = vCode;
            MemoryStream stream = vc.CreateValidateGraphic(vCode);
            return File(stream.ToArray(), "image/jpeg");
        }

        /// <summary>
        /// 將當前的驗證碼轉成 Wav audio 輸出 frmPlayer
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult GetValidateCodeAudio()
        {
            string vCode = SessionModel.Get().LoginValidateCode;
            if (string.IsNullOrEmpty(vCode)) { return HttpNotFound(); }
            string audioPath = HttpContext.Server.MapPath("~/Content/audio/");
            Turbo.Commons.ValidateCode vc = new Turbo.Commons.ValidateCode();
            MemoryStream stream = vc.CreateValidateAudio(vCode, audioPath);
            //return File(stream.ToArray(), "audio/mpeg");
            return File(stream.ToArray(), "audio/wav");

        }



        private void InputValidate(Models.ViewModel.LoginViewModel form)
        {

            if (string.IsNullOrEmpty(form.UserName) || string.IsNullOrEmpty(form.Pxssword))
            {
                LoginExceptions ex = new LoginExceptions("請輸入正確的電子郵件及密碼！！本登入專區為共通核心職能課程師資專用，如無法順利登入請與專案辦公室聯絡，感謝您的配合。");
                LOG.Warn(ex.Message, ex);
                throw ex;
            }

            SessionModel sm = SessionModel.Get();
            if (sm.LoginValidateCode == null || string.IsNullOrEmpty(sm.LoginValidateCode))
            {
                LoginExceptions ex = new LoginExceptions("驗證碼輸出有誤");
                LOG.Warn(ex.Message, ex);
                throw ex;
            }
            if (form.ValidateCode == null || string.IsNullOrEmpty(form.ValidateCode))
            {
                LoginExceptions ex = new LoginExceptions("驗證碼輸入有誤");
                LOG.Warn(ex.Message, ex);
                throw ex;
            }
            if (!form.ValidateCode.Equals(sm.LoginValidateCode))
            {
                //if (form.ValidateCode != "1A2B")
                //{
                //    LoginExceptions ex = new LoginExceptions("驗證碼輸入錯誤");
                //    throw ex;
                //}
                LoginExceptions ex = new LoginExceptions("驗證碼輸入錯誤");
                LOG.Warn(ex.Message, ex);
                throw ex;
            }
            sm.LoginValidateCode = "";
        }

        public void SetLogoutLog(string username)
        {
            HttpRequestBase req = this.Request;
            string userAgent = req.UserAgent;
            userAgent = userAgent.Count() <= 200 ? userAgent : userAgent.Substring(0, 200);
            string ip = req.UserHostAddress;

            Store param = new Store();
            param["username"] = username;
            param["logtype"] = "LOGOUT";
            param["ip"] = ip;
            param["message"] = "登出成功";
            param["useragent"] = userAgent;

            BaseDAO dao = new BaseDAO();
            dao.BeginTransaction();
            try
            {
                dao.Insert("Log.SaveLoginLog", param);
                dao.CommitTransaction();
            }
            catch (Exception ex)
            {
                LOG.Warn(ex.Message, ex);
                dao.RollBackTransaction();
                throw ex;
            }
        }

        //private LoginUserInfo LoginValidate(string username, string password)
        //{
        //    return null;
        //}

        /// <summary>
        /// 傳入使用者輸入的密碼明文, 加密後回傳
        /// </summary>
        /// <param name="usePwd"></param>
        /// <returns></returns>
        //private string EncPassword(string userPwd)
        //{
        //    if (string.IsNullOrWhiteSpace(userPwd))
        //    {
        //        throw new ArgumentNullException("userPwd");
        //    }
        //    //TODO: 置換 RSACSP 改成不可逆的 Hash 方法
        //    RSACSP.RSACSP rsa = new RSACSP.RSACSP();
        //    return rsa.Utl_Encrypt(userPwd);
        //}

        #region 忘記密碼

        /*
    /// <summary>
    /// 忘記密碼
    /// </summary>
    /// <returns></returns>
    public ActionResult ForgetPWD()
    {
        LoginViewModel viewModel = new LoginViewModel();

        LoginForgetPWDModel forgetPWDModel = new LoginForgetPWDModel();
        viewModel.forgetPWD = forgetPWDModel;
        return View(viewModel);
    }

    /// <summary>
    /// 重新設定密碼
    /// </summary>
    /// <param name="USERNO">使用者帳號</param>
    /// <param name="USERNAME">使用者姓名</param>
    /// <param name="AUTHNUM">身分證字號末4碼</param>
    /// <param name="BIRTHDAY">生日</param>
    /// <param name="EMAIL">電子郵件信箱</param>
    /// <param name="CODE">輸入的認證碼</param>
    /// <returns></returns>
    [HttpPost]
    //public ActionResult SetPassword(string USERNO,string USERNAME,string AUTHNUM,string BIRTHDAY,string EMAIL,string CODE)
    public ActionResult SetPassword(LoginForgetPWDModel form)
    {
        if (!ModelState.IsValid)
        {
            string msg = MyCommonUtil.GetModelStateErrors(ModelState, "\r\n");
            return MyCommonUtil.BuildAjaxResult(new { ERRMSG = msg, RESULT = "N" }, false);
        }
        else
        {
            string user_username = form.user_username;
            string user_name = form.user_name;
            string EMAIL = form.EMAIL;
            string ValidateCode = form.ValidateCode;

            //開始CLASM01M的sUtl_DoServerCheck的等效部分--資料檢查/
            StringBuilder sbErrorMessage = new StringBuilder();
            A6DAO dao = new A6DAO();
            if (!string.IsNullOrEmpty(EMAIL))
            {
                string[] str1 = EMAIL.Split('@');
                string[] str2 = EMAIL.Split('.');
                if ((string.IsNullOrEmpty(str1[0]) || str1.Length < 2) || (str2.Length < 2 || (str2.Length == 2 && string.IsNullOrEmpty(str2[2]))))
                {
                    //
                    //  沒有@號,或@號後面沒字了,或沒有.號,或只有一個.號而這.號之後也沒字了
                    //
                    sbErrorMessage.Append("E-Mail的格式錯誤\n");
                }
            }
            e_user clamdburm = dao.GetRow(new e_user { user_username = user_username });
            if (clamdburm == null)
            {
                sbErrorMessage.Append("使用者帳號不存在，請重新輸入\n");
            }
            else
            {
                if (!user_name.Equals(clamdburm.user_name))
                {
                    sbErrorMessage.Append("使用者姓名不正確\n");
                }
            }
            if (!ValidateCode.Equals(SessionModel.Get().LoginValidateCode))
            {
                sbErrorMessage.Append("驗證碼輸入錯誤，請重新輸入\n");
            }
            if (!string.IsNullOrEmpty(sbErrorMessage.ToString()))
            {
                var data = new { ERRMSG = sbErrorMessage.ToString(), RESULT = "N" };
                return MyCommonUtil.BuildAjaxResult(data, false);
            }


            PWDTOKEN token = new PWDTOKEN();
            string tokenPwd = "";
            while (token != null)
            {
                tokenPwd = GetRandomString("ABCDEFGHIJKLMNPQRSTUVWXYZ23456789", 60);
                token = dao.GetRow(new PWDTOKEN { token = tokenPwd });
            }

            PWDTOKEN tokenInsert = new PWDTOKEN();
            tokenInsert.user_username = user_username;
            tokenInsert.token = tokenPwd;
            tokenInsert.Validate = DateTime.Now.AddMinutes(30);
            tokenInsert.enable = "0";
            dao.Insert(tokenInsert);

            var tokenUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/Login/ResetPwd?token=" + tokenPwd;


            // 最後是 send_Process，通知信/
            string Today = MyHelperUtil.TransAdDateToTwDate(DateTime.Now.ToString("yyyy/MM/dd"));
            Today = Today.Substring(0, 3) + "年" + Today.Substring(3, 2) + "月" + Today.Substring(5, 2) + "日";
            //轉成今天是yyy年MM月dd日
            string sBody = user_name + "，您於" + Today + "申請寄發密碼重設驗證，\n" +
                           "您的重設密碼連結為 " + tokenUrl + "\n請妥善保管您的信件，以防有心人士盜用!!";
            MailMessage mailmessage = CommonsServices.NewMail(null, EMAIL, ConfigModel.SYSNAME + "-申請寄發密碼作業", sBody, (string)null, false);
            MailSentResult mailsentresult = CommonsServices.SendMail(mailmessage);
            // sendProcess的等效部分END,「確定」鍵流程順利結束/
            if (mailsentresult.IsSuccess)
            {
                return MyCommonUtil.BuildAjaxResult(new { RESULT = "Y" }, true);
            }
            else
            {
                return MyCommonUtil.BuildAjaxResult(new { ERRMSG = mailsentresult.ResultText, RESULT = "N" }, false);
            }
        }
    }

    /// <summary>
    /// 重設密碼
    /// </summary>
    /// <param name="form"></param>
    /// <returns></returns>
    [HttpGet]
    public ActionResult ResetPwd(string token)
    {
        ActionResult rtn;
        SessionModel sm = SessionModel.Get();
        ResetPwdFormModel form = new ResetPwdFormModel();

        PWDTOKEN tokenWhere = new PWDTOKEN();
        tokenWhere.token = token.TONotDashString();
        tokenWhere.enable = "0";
        A6DAO dao = new A6DAO();
        var tokenList = dao.GetRowList(tokenWhere);
        tokenList = tokenList.Where(m => m.Validate <= DateTime.Now).ToList();
        if (tokenList.ToCount() > 0)
        {
            form.Token = token;
            form.user_username = tokenList.FirstOrDefault().user_username;
        }
        else
        {
            sm.LastErrorMessage = "此連結未有開設相關變更機制，請確認後再連結。";
        }
        rtn = View("ResetPwd", form);
        return rtn;
    }

    /// <summary>
    /// 會員註冊
    /// </summary>
    /// <param name="form"></param>
    /// <returns></returns>
    [HttpPost]
    public ActionResult SavePwd(ResetPwdFormModel form)
    {
        SessionModel sm = SessionModel.Get();
        ActionResult rtn;
        rtn = View("ResetPwd", form);
        var errorMsg = "";
        if (form.user_username.TONotNullString() == "")
        {
            errorMsg = "帳號不得為空!";
        }
        if (form.NewUserPwd.TONotNullString() != "")
        {
            bool NewPwd = false;
            if (CommonsServices.IsMatch(form.NewUserPwd, "^.*(?=.{10,})(?=.*\\d)(?=.*[A-Z])(?=.*[@#$%^&+=]).*$"))
            {
                NewPwd = true;
            }
            if (CommonsServices.IsMatch(form.NewUserPwd, "^.*(?=.{10,})(?=.*\\d)(?=.*[A-Z])(?=.*[@#$%^&+=]).*$"))
            {
                NewPwd = true;
            }
            if (CommonsServices.IsMatch(form.NewUserPwd, "^.*(?=.{10,})(?=.*\\d)(?=.*[a-z])(?=.*[@#$%^&+=]).*$"))
            {
                NewPwd = true;
            }
            if (CommonsServices.IsMatch(form.NewUserPwd, "^.*(?=.{10,})(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).*$"))
            {
                NewPwd = true;
            }
            if (!NewPwd)
            {
                errorMsg = "此密碼未符合規則，請重設密碼(密碼規則需符合包含英文大寫、英文小寫、數字以及特殊字元，4種字元至少包含以上3種，密碼長度10碼以上) !";
            }

        }

        if (form.NewUserPwd.TONotNullString() == "")
        {
            errorMsg = "新密碼未填寫";
        }

        if (form.NewUserPwd2.TONotNullString() == "")
        {
            errorMsg = "再次輸入新密碼未填寫";
        }
        if (form.NewUserPwd.TONotNullString() != form.NewUserPwd2.TONotNullString())
        {
            errorMsg = "新密碼兩次輸入不相同，請確認後重新輸入";
        }

        if (errorMsg == "")
        {
            A6DAO dao = new A6DAO();
            PWDTOKEN Check3Twhere = new PWDTOKEN();
            Check3Twhere.user_username = form.user_username;
            var PwdToken = dao.GetRowList(Check3Twhere).Where(m => m.Password.TONotNullString() != "").OrderByDescending(m => m.Validate).Take(3).ToList();
            if (PwdToken.ToCount() > 0)
            {
                var Check3T = PwdToken.Select(m => m.Password).ToList().Contains(form.NewUserPwd);
                if (Check3T)
                {
                    errorMsg = "不得與前" + PwdToken.ToCount() + "次密碼重複!";
                }
            }
            e_user ChkWhere = new e_user();
            ChkWhere.user_username = form.user_username;
            ChkWhere.user_password = form.NewUserPwd;
            var ChkUser = dao.GetRowList(ChkWhere);
            if (ChkUser.ToCount() > 0)
            {
                errorMsg = "不得輸入目前相同密碼!";
            }

            if (errorMsg == "")
            {

                e_user where = new e_user();
                where.user_username = form.user_username;
                e_user user = new e_user();
                user.user_password = form.NewUserPwd;
                dao.Update(user, where);

                PWDTOKEN PWDwhere = new PWDTOKEN();
                PWDwhere.user_username = form.user_username;
                PWDTOKEN PWD = new PWDTOKEN();
                PWD.enable = "1";
                dao.Update(PWD, PWDwhere);

                PWDTOKEN PWD2where = new PWDTOKEN();
                PWD2where.user_username = form.user_username;
                PWD2where.token = form.Token;
                PWDTOKEN PWD2 = new PWDTOKEN();
                PWD2.Password = form.NewUserPwd;
                dao.Update(PWD2, PWD2where);

                sm.LastResultMessage = "密碼變更成功!請使用新密碼重新登入";
                rtn = Index();
            }
            else
            {
                sm.LastErrorMessage = errorMsg;
            }
        }
        else
        {
            sm.LastErrorMessage = errorMsg;
        }
        return rtn;
    }

    /// <summary>
    /// 產生一個字串，長度是指定長度，內容由指定的可用字元當中隨機組合而成。
    /// </summary>
    /// <param name="AvailableCharSet">可用字元。</param>
    /// <param name="NeededLength">所需字串長度。</param>
    /// <returns></returns>
    private string GetRandomString(string AvailableCharSet, uint NeededLength)
    {
        return CommonsServices.GetRandomString(AvailableCharSet, NeededLength);
    }
*/
        #endregion
    }
}

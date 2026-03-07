using Turbo.MVC.Base3.DataLayers;
using Turbo.MVC.Base3.Services;
using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Linq;
using Turbo.Commons;
using Turbo.MVC.Base3.Models.Entities;
using Turbo.MVC.Base3.Models;

namespace WDACC.Models
{
    public class SessionModel
    {
        protected static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private HttpSessionStateBase _session;

        private HttpSessionStateBase session
        {
            get
            {
                if (_session == null)
                {
                    throw new NullReferenceException("session object is null");
                }
                return _session;
            }
        }

        private SessionModel()
        {
            this._session = new HttpSessionStateWrapper(HttpContext.Current.Session);
            if (this._session == null)
            {
                throw new NullReferenceException("HttpContext.Current.Session");
            }

            _session.Timeout = 20;
            logger.Debug("SessionModel(), SessionID=" + _session.SessionID);
        }

        /// <summary>
        /// 取得/建立 Login SessionModel 
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public static SessionModel Get()
        {
            return new SessionModel();
        }


        [Obsolete("use SessionModel.get() instead")]
        public static SessionModel Get(HttpSessionStateBase session)
        {
            return new SessionModel();
        }

        private static readonly string VALIDATE_CODE = "SYS.LOGIN.VALIDATECODE";
        private static readonly string USER_INFO = "SYS.LOGIN.USER";

        //private static readonly string CUR_ROLE = "SYS.LOGIN.ROLE";

        private static readonly string CUR_ROLE_FUNCTION = "SYS.LOGIN.ROLE.FUNCTION";

        private static readonly string LAST_ACTION_FUNC = "SYS.MENU.LAST_ACTION_FUNC";
        private static readonly string LAST_ACTION_PATH = "SYS.MENU.LAST_ACTION_PATH";
        //private static readonly string LAST_ACTION_NAME = "SYS.MENU.LAST_ACTION_NAME";
        //private static readonly string BREADCRUMB_PATH = "SYS.MENU.BREADCRUMB_PATH";
        //private static readonly string BREADCRUMB_PATH_STORE = "SYS.MENU.BREADCRUMB_PATH_STORE";

        private static readonly string LAST_ERROR_MESSAGE = "USER.LAST_ERROR_MESSAGE";
        private static readonly string LAST_RESULT_MESSAGE = "USER.LAST_RESULT_MESSAGE";
        //private static readonly string CLOSE_AFTER_DIALOG = "USER.CLOSE_AFTER_DIALOG";
        private static readonly string REDIRECT_AFTER_BLOCK = "USER.REDIRECT_AFTER_BLOCK";

        /// <summary>使用 HTTP Get 方式導向指定網址</summary>
        private static readonly string REDIRECT_AFTER_BLOCK_2 = "USER.REDIRECT_AFTER_BLOCK_2";

        private static readonly string IS_SHOW_MEMBER_HINT = "USER.IS_SHOW_MEMBER_HINT";

        /// <summary>
        /// 是否顯是講師登入首頁積分提示彈跳視窗
        /// </summary>
        public string IsShowMemberHint
        {
            get
            {
                object hint = this.session[IS_SHOW_MEMBER_HINT];
                if (hint == null)
                {
                    hint = string.Empty;
                }
                return hint.ToString();
            }
            set { this.session[IS_SHOW_MEMBER_HINT] = value; }
        }

        /// <summary>
        /// 使用者登入驗證碼
        /// </summary>
        public string LoginValidateCode
        {
            get { return (string)this.session[VALIDATE_CODE]; }
            set { this.session[VALIDATE_CODE] = value; }
        }

        #region 登入者使用資訊

        /// <summary>
        /// 登入者使用者帳號資訊
        /// </summary>
        public LoginUserInfo UserInfo
        {
            get
            {
                LoginUserInfo userInfo = null;
                string jsonUserInfo = (string)this.session[USER_INFO];
                if (!string.IsNullOrWhiteSpace(jsonUserInfo))
                {
                    userInfo = JsonConvert.DeserializeObject<LoginUserInfo>(jsonUserInfo);
                }
                return userInfo;
            }
            set
            {
                if (value != null && value.UserType == null)
                {
                    value.UserType = LoginUserType.SKILL_USER;
                }
                this.session[USER_INFO] = JsonConvert.SerializeObject(value);
            }
        }

        /// <summary>
        /// 作用中角色對應的權限功能清單
        /// </summary>
        public IList<ClamRoleFunc> RoleFuncs
        {
            get
            {
                IList<ClamRoleFunc> roleFuncs = new List<ClamRoleFunc>();
                string jsonRoleFunc = (string)this.session[CUR_ROLE_FUNCTION];
                if (!string.IsNullOrWhiteSpace(jsonRoleFunc))
                {
                    roleFuncs = JsonConvert.DeserializeObject<IList<ClamRoleFunc>>(jsonRoleFunc);
                }
                return roleFuncs;
            }
            set
            {
                this.session[CUR_ROLE_FUNCTION] = JsonConvert.SerializeObject(value);
            }
        }
        #endregion

        #region 錯誤訊息及導向

        /// <summary>
        /// 最後被記錄的應用功能錯誤提示訊息, 設定這個值, 在下一個頁面中會觸發 blockAlert() 顯示這個訊息,
        /// 每次這個訊息被讀取後會自動清除, 確保這個訊息只會在一個頁面中被觸發.
        /// </summary>
        public string LastErrorMessage
        {
            get
            {
                string message = (string)this.session[LAST_ERROR_MESSAGE];
                this.session[LAST_ERROR_MESSAGE] = string.Empty;
                return (string.IsNullOrEmpty(message) ? string.Empty : message.Replace("\n", "<br/>").Replace("'", "\""));
            }
            set { this.session[LAST_ERROR_MESSAGE] = value; }
        }

        /// <summary>
        /// 最後被記錄的應用功能操作結果提示訊息, 設定這個值, 在下一個頁面中會觸發 blockResult() 顯示這個訊息,
        /// 每次這個訊息被讀取後會自動清除, 確保這個訊息只會在一個頁面中被觸發.
        /// </summary>
        public string LastResultMessage
        {
            get
            {
                string message = (string)this.session[LAST_RESULT_MESSAGE];
                this.session[LAST_RESULT_MESSAGE] = string.Empty;
                return (string.IsNullOrEmpty(message) ? string.Empty : message.Replace("\n", "<br/>").Replace("'", "\""));
            }
            set { this.session[LAST_RESULT_MESSAGE] = value; }
        }

        /// <summary>
        /// 配合 LastResultMessage 運作，若這個屬性不為空，則在前端 blockResult() 訊息確認後，
        /// 會以 HTTP POST 方式重導至指定 URL。POST 參數可以用 ?parm1=value1&amp;parm2=value2 的方式傳入
        /// </summary>
        public string RedirectUrlAfterBlock
        {
            get
            {
                string url = (string)this.session[REDIRECT_AFTER_BLOCK];
                this.session[REDIRECT_AFTER_BLOCK] = string.Empty;
                return url;
            }
            set { this.session[REDIRECT_AFTER_BLOCK] = value; }
        }

        /// <summary>
        /// 配合 LastResultMessage、LastErrorMessage 運作，若這個屬性不為空，則在前端 blockResult() 訊息確認後， 
        /// 會以 HTTP GET 方式重導至指定 URL。若要傳遞參數時給目的網頁時，可以使用 ?parm1=value1&amp;parm2=value2 方式來設定。
        /// <para>注意！！！ 一律優先使用 RedirectUrlAfterBlock 屬性（使用 HTTP POST 方式）, 除非遇到無法克服的困難才使用這個GET模式。</para>
        /// </summary>
        public string RedirectUrlAfterBlockViaGet
        {
            get
            {
                string url = (string)this.session[REDIRECT_AFTER_BLOCK_2];
                this.session[REDIRECT_AFTER_BLOCK_2] = string.Empty;
                return url;
            }
            set { this.session[REDIRECT_AFTER_BLOCK_2] = value; }
        }

        #endregion

        #region 程式代碼名稱(上方URL)

        /// <summary>
        /// 使用者當前執行的 程式完整 ACTION PATH
        /// </summary>
        public string LastActionPath
        {
            get { return (string)this.session[LAST_ACTION_PATH]; }
            set { this.session[LAST_ACTION_PATH] = value; }
        }

        /// <summary>
        /// 使用者當前執行的 功能項目,
        /// 當使用者有登入時且執行系統中有定義的功能時, 才會有值, 否則為 null
        /// </summary>
        public AMFUNCM LastActionFunc
        {
            get
            {
                AMFUNCM func = null;
                string jsonFunc = (string)this.session[LAST_ACTION_FUNC];
                if (!string.IsNullOrWhiteSpace(jsonFunc))
                {
                    func = JsonConvert.DeserializeObject<AMFUNCM>(jsonFunc);
                }
                return func;
            }
            set
            {
                this.session[LAST_ACTION_FUNC] = JsonConvert.SerializeObject(value);
            }
        }

        #endregion
    }

    
}

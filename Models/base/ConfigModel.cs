using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

namespace Turbo.MVC.Base3.Models
{
    // 這個檔案集中定義系統組態設定變數(舊系統 DefineVar 有用到的值也納入這裡)
    public class ConfigModel
    {
        /// <summary>
        /// 系統預設最高管理帳號
        /// </summary>
        public const string Admin = "superadmin";

        /// <summary>
        /// 預設分頁筆數(常數定義), 如果 web.config 中有設定 DefaultPageSize 則為以 web.config 中設定的為主
        /// </summary>
        private const int _DefaultPageSize = 20;


        /// <summary>
        /// 是否啟用壓力測試模式(AppSettings StressTestMode=Y), 
        /// 若啟用則 LoginRequired 會以設定的StressTestUserInfo
        /// 測試帳號覆寫 LoginUserInfo
        /// </summary>
        public static bool StressTestMode
        {
            get
            {
                string strTestModel = ConfigurationManager.AppSettings["StressTestMode"];
                bool testModel = "Y".Equals(strTestModel);
                return testModel;
            }
        }

        /// <summary>
        /// 啟用壓力測試模式(AppSettings StressTestMode=Y)時,
        /// 系統會引用模擬的使用者資訊
        /// </summary>
        //public static LoginUserInfo StressTestUserInfo
        //{
        //    get
        //    {
        //        LoginUserInfo user = new LoginUserInfo();
        //        string strUserNo = ConfigurationManager.AppSettings["StressTestUserNo"];
        //        string strUserName = ConfigurationManager.AppSettings["StressTestUserName"];
        //        string strUserExamKind = ConfigurationManager.AppSettings["StressTestUserExamKind"];
        //        string strUserRole = ConfigurationManager.AppSettings["StressTestUserRole"];
        //        if (string.IsNullOrEmpty(strUserNo))
        //        {
        //            strUserNo = "nobody";
        //        }
        //        if (string.IsNullOrEmpty(strUserName))
        //        {
        //            strUserName = strUserNo;
        //        }
        //        if (string.IsNullOrEmpty(strUserExamKind))
        //        {
        //            strUserExamKind = "1";
        //        }
        //        if (string.IsNullOrEmpty(strUserRole))
        //        {
        //            strUserRole = "0";
        //        }
        //        user.UserNo = strUserNo;
        //        user.User = new ClamUser();
        //        user.User.USERNO = strUserNo;
        //        user.User.USERNAME = strUserName;
        //        user.Roles.Add(
        //            new ClamUserRole {
        //                EXAMKIND = strUserExamKind,
        //                EXAMKIND_NAME = strUserExamKind,
        //                ROLE = strUserRole,
        //                ROLE_NAME = strUserRole
        //            });
        //        return user;
        //    }
        //}

        /// <summary>
        /// 主機環境角色設定: 1.內網環境, 2.外網環境
        /// </summary>
        public static string NetID
        {
            get
            {
                string netId = ConfigurationManager.AppSettings["NetID"];
                if (string.IsNullOrEmpty(netId)) { netId = "1"; }
                return netId;
            }
        }

        /// <summary>
        /// 預設分頁筆數
        /// </summary>
        public static int DefaultPageSize
        {
            get
            {
                int iPageSize;
                string pageSize = ConfigurationManager.AppSettings["DefaultPageSize"];
                if (int.TryParse(pageSize, out iPageSize))
                {
                    return iPageSize;
                }
                else
                {
                    return _DefaultPageSize;
                }
            }
        }

        /// <summary>電子抽籤網站連結網址（AT/C201 畫面查詢結果資料會使用到）</summary>
        /// <remarks>在 AT/C201 畫面 輸入 在校生檢定，106年度，第1梯次，術科單位1039，按下查詢即會查到資料，再按下每筆資料右邊的「連結」文字即可</remarks>
        public static string BallotUrl
        {
            get
            {
                string path = ConfigurationManager.AppSettings["BallotUrl"];
                if (string.IsNullOrEmpty(path)) path = (new UrlHelper()).Content("~/Turbo.MVC.Base3/Ballot/Ballot.aspx");
                return path;
            }
        }

        /// <summary>
        /// 暫存路徑
        /// </summary>
        public static string TempPath
        {
            get
            {
                string path = ConfigurationManager.AppSettings["TempPath"];
                if (string.IsNullOrEmpty(path))
                {
                    path = (new UrlHelper()).Content("~/App_Data/Temp");
                }
                return path;
            }
        }

        /// <summary>
        /// 上傳檔案暫存路徑
        /// </summary>
        public static string UploadTempPath
        {
            get
            {
                string path = ConfigurationManager.AppSettings["UpLoadFile"];
                if (string.IsNullOrEmpty(path))
                {
                    path = (new UrlHelper()).Content("~/Uploads");
                }
                return path;
            }
        }

        /// <summary>
        /// 取得在角色選擇頁中, 指定檢定類別所要使用的圖示檔名
        /// </summary>
        /// <param name="examkind"></param>
        /// <returns></returns>
        public static string GetExamkindIcon(string examkind)
        {
            string icon = "icon-type-default.png";
            switch (examkind)
            {
                case ExamKind.EK_1_全國檢定:
                    icon = "icon-type-01taiwan.svg";
                    break;
                case ExamKind.EK_3_在校生專案檢定:
                    icon = "icon-type-02reading.svg";
                    break;
                case ExamKind.EK_5_職訓機構受訓學員專案檢定:
                    icon = "icon-type-secretary.svg";
                    break;
                case ExamKind.EK_6_監院所收容人專案檢定:
                    icon = "icon-type-prisoner.svg";
                    break;
                case ExamKind.EK_7_國軍人員專案檢定:
                    icon = "icon-type-03soldier.svg";
                    break;
                case ExamKind.EK_9_事業機構員工專案檢定:
                    icon = "icon-type-suitcase.svg";
                    break;
                case ExamKind.EK_B_營造業工會專案檢定:
                    icon = "icon-type-04miner.svg";
                    break;
                case ExamKind.EK_C_即測即評學科測試檢定:
                    icon = "icon-type-05exam.svg";
                    break;
                case ExamKind.EK_G_即測即評及發證檢定:
                    icon = "icon-type-06clipboard.svg";
                    break;
                case ExamKind.EK_H_營造業專班檢定:
                    icon = "icon-type-07carpenter.svg";
                    break;
                case ExamKind.EK_I_縣市政府勞工安全作業:
                    icon = "icon-type-helmet.svg";
                    break;
            }
            return icon;
        }

        /// <summary>
        /// 判斷登入帳號是否為技能檢定中心角色
        /// </summary>
        /// <param name="examkind">檢定類別代碼</param>
        /// <param name="role">角色代碼</param>
        /// <returns></returns>
        public static bool IsWdaseRole(string examkind, string role)
        {
            bool bFlag = false;
            //目前只判斷 role 為 0 :勞動部勞動力發展署技檢中心，
            //examking 預留參數 已後可能會使用到
            if (role == "0")
            {
                bFlag = true;
            }
            return bFlag;
        }

        /// <summary>
        /// 系統電子郵件服務主機 IP
        /// </summary>
        public static string MailServer
        {
            get
            {
                string value = ConfigurationManager.AppSettings["MailServer"];
                return (string.IsNullOrEmpty(value)) ? "127.0.0.1" : value;
            }
        }

        /// <summary>
        /// 系統電子郵件服務主機 IP
        /// </summary>
        public static int MailServerPort
        {
            get
            {
                string value = ConfigurationManager.AppSettings["MailServerPort"];
                value = (string.IsNullOrEmpty(value)) ? "587" : value;
                return System.Convert.ToInt32(value);
            }
        }

        /// <summary>
        /// 系統寄件者電子郵件地址
        /// </summary>
        public static string MailSenderAddr
        {
            get
            {
                string value = ConfigurationManager.AppSettings["MailSenderAddr"];
                return (string.IsNullOrEmpty(value)) ? "service@turbotech.com.tw" : value;
            }
        }

        /// <summary>
        /// 系統寄件者電子郵件密碼
        /// </summary>
        public static string MailSenderPwd
        {
            get
            {
                string value = ConfigurationManager.AppSettings["MailSenderPWD"];
                return (string.IsNullOrEmpty(value)) ? "service!QAZ2wsx" : value;
            }
        }

        public static string TestMailTo
        {
            get
            {
                string value = ConfigurationManager.AppSettings["TestMailTo"];
                return (string.IsNullOrEmpty(value)) ? "henrylin@turbotech.com.tw" : value;
            }
        }

        /// <summary>
        /// 是否為測試模式 N: 
        /// </summary>
        public static bool IsProductionMode
        {
            get
            {
                bool result = false;
                string value = ConfigurationManager.AppSettings["IsProductionMode"];
                if (string.IsNullOrEmpty(value))
                {
                    result = false;
                }
                else
                {
                    if (value == "Y")
                    {
                        result = true;
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// 系統寄件者電子郵件密碼
        /// </summary>
        public static string FtpNasUser
        {
            get
            {
                string value = ConfigurationManager.AppSettings["FtpNasUser"];
                return (string.IsNullOrEmpty(value)) ? "labor" : value;
            }
        }

        /// <summary>
        /// 系統寄件者電子郵件密碼
        /// </summary>
        public static string FtpNasServer
        {
            get
            {
                string value = ConfigurationManager.AppSettings["FtpNasServer"];
                return (string.IsNullOrEmpty(value)) ? "" : value;
            }
        }

        /// <summary>
        /// 系統寄件者電子郵件密碼
        /// </summary>
        public static string FtpNasPassword
        {
            get
            {
                string value = ConfigurationManager.AppSettings["FtpNasPassword"];
                return (string.IsNullOrEmpty(value)) ? "labor22595700" : value;
            }
        }

        /// <summary>
        /// 命題人員登入/更新API, 密碼字串加密的 AES KEY,
        /// REST API 會使用到這個設定
        /// </summary>
        public static string WRS_AES_KEY
        {
            get
            {
                return "WRS001004Staff";
            }
        }

        /// <summary>
        /// 系統名稱(忘記密碼通知信會用)
        /// </summary>
        public static string SYSNAME
        {
            get
            {
                return "退輔會職訓整合系統";
            }
        }

        /// <summary>
        /// 系統主功能表樣式（0: 系統預設功能表，1: 樹型功能表）
        /// </summary>
        public static string MainMenuType
        {
            get
            {
                try
                {
                    string value = ConfigurationManager.AppSettings["MainMenuType"];
                    return string.IsNullOrEmpty(value) ? "0" : value;
                }
                catch
                {
                    return "0";
                }
            }
        }

        /// <summary>
        /// 是否啟用內網使用者「12-20 碼密碼長度」檢查。（true: 啟用，false: 不啟用 (即維持原 8-20 碼密碼長度)）
        /// </summary>
        public static bool Net1UserPwdLen1220Check
        {
            get
            {
                try
                {
                    string value = ConfigurationManager.AppSettings["Net1UserPwdLen1220Check"];
                    return string.IsNullOrEmpty(value) ? false : (value == "Y");
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 自動依據當前「內網環境、外網環境」傳回使用者登入密碼長度最小值。
        /// </summary>
        /// <remarks>依據 20180521 署方來信，署資安管控條件。</remarks>
        public static int PwdLenMin
        {
            get
            {
                if (ConfigModel.Net1UserPwdLen1220Check)
                {
                    return (ConfigModel.NetID == "2") ? 8 : 12;
                }
                else
                {
                    return (ConfigModel.NetID == "2") ? 8 : 8;
                }
            }
        }

        /// <summary>
        /// 自動依據當前「內網環境、外網環境」傳回使用者登入密碼長度最大值。
        /// </summary>
        public static int PwdLenMax
        {
            get { return (ConfigModel.NetID == "2") ? 20 : 20; }
        }

        /// <summary>
        /// (測試環境) TestEnvironmentMode
        /// </summary>
        public static bool TestEnvironment
        {
            get
            {
                return "Y".Equals(ConfigurationManager.AppSettings["TestEnvironmentMode"] ?? "");
            }
        }

        /// <summary>
        /// (測試環境-首頁暫停) TestHomeStopMode
        /// </summary>
        public static bool TestHomeStop1
        {
            get
            {
                return "Y".Equals(ConfigurationManager.AppSettings["TestHomeStopMode"] ?? "");
            }
        }

        /// <summary> 可上傳檔案副檔名 </summary>
        public static string Get_FileExtAccept
        {
            get
            {   
                return ".xls,.xlsx,.doc,.docx,.ppt,.pptx,.pdf,.jpg,.png,.msg,.odt,.ods";
            }
        }
    }
}

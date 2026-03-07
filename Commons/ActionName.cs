using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Turbo.MVC.Base3.Commons;

namespace WDACC.Commons
{
    public class ActionName
    {
        /// <summary>
        /// 管理者動作
        /// </summary>
        public enum Admin
        {
            /// <summary>
            /// 無，無資料預設值
            /// </summary>
            None,

            /// <summary>
            /// 首頁
            /// </summary>
            Home,

            /// <summary>
            /// 系統管理
            /// </summary>
            System,

            /// <summary>
            /// 最新消息
            /// </summary>
            News,

            /// <summary>
            /// 最新消息 編輯
            /// </summary>
            NewsEdit,

            /// <summary>
            /// 最新消息 下架 
            /// </summary>
            NewsClose,

            /// <summary>
            /// 最新消息 上架 
            /// </summary>
            NewsOpen,

            /// <summary>
            /// 講師資料設定
            /// </summary>
            Teacher,

            /// <summary>
            /// 新增帳號 
            /// </summary>
            Account,

            /// <summary>
            /// 課程管理
            /// </summary>
            Course,

            /// <summary>
            /// log紀錄
            /// </summary>
            Log,

            LoginLog,
            FileLog,
            FuncLog,

            /// <summary>
            /// 會議活動
            /// </summary>
            Meeting,

            /// <summary>
            /// 會議參與人員
            /// </summary>
            MeetingMember,

            /// <summary>
            /// 資料審核 
            /// </summary>
            ClassReview,

            /// <summary>
            /// 積分資料
            /// </summary>
            Score,

            /// <summary>
            /// 師資招募
            /// </summary>
            Recruit,

            /// <summary>
            /// 課程審核
            /// </summary>
            CourseAudit,

            /// <summary>
            /// 檔案上傳審核
            /// </summary>
            FileAudit,

            /// <summary>
            /// 意見回饋
            /// </summary>
            Feedback,

            /// <summary>
            /// 資料匯出
            /// </summary>
            Export,

            /// <summary>
            /// 修改密碼
            /// </summary>
            ChangePxssword,

            /// <summary>
            /// 修改講師基本資料
            /// </summary>
            TeacherInfo,

            /// <summary>
            /// 哥授課產業別
            /// </summary>
            Induclass,

            /// <summary>
            /// 取得縣市鄉鎮市區代碼
            /// </summary>
            CityZipCode,

            /// <summary>
            /// 管理者帳號申請
            /// </summary>
            MgrAccount,

            /// <summary>
            /// 管理者檔案上傳 
            /// </summary>
            FileUpload,

            /// <summary>
            /// 查詢講師名稱
            /// </summary>
            QueryMemberName,
            /// <summary>
            /// 匯出每月登入異常日誌,Export monthly login anomaly logs
            /// </summary>
            Exportlogs,
        }

        /// <summary>
        /// 師資動作
        /// </summary>
        public enum Member
        {
            None,

            /// <summary>
            /// 師資首頁
            /// </summary>
            Home,

            /// <summary>
            /// 師資最新消息
            /// </summary>
            News,

            /// <summary>
            /// 績分課程
            /// </summary>
            ScoreEdit,

            /// <summary>
            /// 教材分享
            /// </summary>
            FileShareEdit,

            /// <summary>
            /// 授課影片
            /// </summary>
            TeachVideo,

            /// <summary>
            /// 授課照片
            /// </summary>
            TeachImage,

            /// <summary>
            /// 儲存積分課程 & 查詢積分資料
            /// </summary>
            ScoreRegister,

            /// <summary>
            /// 教材分享
            /// </summary>
            ShareRegister,

            /// <summary>
            /// 教材分享訊息頁
            /// </summary>
            ShareHome,

            /// <summary>
            /// 教材分享表單, 共3個分頁
            /// </summary>
            MemberShare,

            /// <summary>
            /// 社群分享清單
            /// </summary>
            ShareWithMe,

            /// <summary> 
            /// 課程登錄首頁宣告, 顯示登錄事項說明
            /// </summary>
            ScoreHome,

            /// <summary>
            /// 儲存前台顯示狀態
            /// </summary>
            ScoreShowoff,

            /// <summary>
            /// 積分表
            /// </summary>
            ScoreView,

            /// <summary>
            /// 授課花絮
            /// </summary>
            TeachShow,

            /// <summary>
            /// 積分明細
            /// </summary>
            Score,

            /// <summary>
            /// 課程師資招募
            /// </summary>
            Tcsv,

            /// <summary>
            /// 意見回饋
            /// </summary>
            Feedback,

            /// <summary>
            /// 基本資料維護--進入頁
            /// </summary>
            MemdeupHome,

            /// <summary>
            /// 基本資料維護--更改密碼
            /// </summary>
            ChangePxssword,

            /// <summary>
            /// 基本資料維護--個人資料
            /// </summary>
            TeacherInfo,

            /// <summary>
            /// 基本資料維護--可授課產業別
            /// </summary>
            TeacherIndesc,

            /// <summary>
            /// 取得縣市鄉鎮市區代碼
            /// </summary>
            CityZipCode

        }

        /// <summary>
        /// 檔案上傳log使用功能
        /// </summary>
        public enum FileLogFunc
        {
            None,
            /// <summary>
            /// 積分課程登錄
            /// </summary>
            ScoreEdit,
            /// <summary>
            /// 教材分享
            /// </summary>
            FileShare,
            /// <summary>
            /// 授課影片
            /// </summary>
            TeachVideo,
            /// <summary>
            /// 授課照片
            /// </summary>
            TeachImage
        }

        /// <summary>
        /// 檔案下載
        /// </summary>
        public enum File
        {
            /// <summary>
            /// default
            /// </summary>
            None,
            /// <summary>
            /// 積分課程佐證資料
            /// </summary>
            Score,

            /// <summary>
            /// 最新消息
            /// </summary>
            News,

            /// <summary>
            /// 檔案分享 教材
            /// </summary>
            ShareSFile,

            /// <summary>
            /// 檔案分享 說明書
            /// </summary>
            ShareIFile,

            /// <summary>
            /// 檔案分享 授權書
            /// </summary>
            ShareAFile,

            /// <summary>
            /// 授課花絮
            /// </summary>
            TeachShow,

        }

        public enum EditMode
        {
            None,
            CREATE,
            QUERY,
            UPDATE,
            DELETE,
        }

        public enum FileEditType
        {
            None,

            /// <summary>
            /// 上傳
            /// </summary>
            UPLOAD,

            /// <summary>
            /// 下載
            /// </summary>
            DOWNLOAD,

            /// <summary>
            /// 上傳失敗
            /// </summary>
            UPLOAD_FAIL,
        }

        /// <summary>
        /// 執行狀態
        /// </summary>
        public enum EditStatus
        {
            None,
            SUCCESS,
            FAIL
        }

        public static string GetActionText(ActionName.Member action)
        {
            string result = string.Empty;
            switch (action)
            {
                case ActionName.Member.ChangePxssword:
                    result = "修改密碼";
                    break;
                case ActionName.Member.Feedback:
                    result = "意見回饋";
                    break;
                case ActionName.Member.FileShareEdit:
                    result = "教材分享";
                    break;
                case ActionName.Member.MemberShare:
                    result = "教材分享";
                    break;
                case ActionName.Member.News:
                    result = "最新消息";
                    break;
                case ActionName.Member.Score:
                    result = "積分明細";
                    break;
                case ActionName.Member.ScoreEdit:
                    result = "積分課程";
                    break;
                case ActionName.Member.ScoreHome:
                    result = "課程登錄首頁";
                    break;
                case ActionName.Member.ScoreRegister:
                    result = "課程登錄";
                    break;
                case ActionName.Member.ScoreShowoff:
                    result = "課程登錄前台顯示狀況";
                    break;
                case ActionName.Member.ScoreView:
                    result = "積分表";
                    break;
                case ActionName.Member.ShareHome:
                    result = "教材分享資料頁";
                    break;
                case ActionName.Member.ShareRegister:
                    result = "教材分享";
                    break;
                case ActionName.Member.ShareWithMe:
                    result = "社群分享";
                    break;
                case ActionName.Member.Tcsv:
                    result = "課程師資招募";
                    break;
                case ActionName.Member.TeacherIndesc:
                    result = "基本資料維護-可授課產業別";
                    break;
                case ActionName.Member.TeacherInfo:
                    result = "基本資料維護-個人資料";
                    break;
                case ActionName.Member.TeachImage:
                    result = "授課圖片";
                    break;
                case ActionName.Member.TeachShow:
                    result = "授課花絮";
                    break;
                case ActionName.Member.TeachVideo:
                    result = "授課影片";
                    break;
                default:
                    result = action.ToString();
                    break;
            }

            return result;
        }

        public static string GetAdminActionText(string action)
        {
            ActionName.Admin actionName = MyCommonUtil.ToEnum<ActionName.Admin>(action);
            return GetActionText(actionName);
        }

        public static string GetMemberActionText(string action)
        {
            ActionName.Member actionName = MyCommonUtil.ToEnum<ActionName.Member>(action);
            return GetActionText(actionName);
        }

        public static string GetFileActionText(string action)
        {
            ActionName.File actionName = MyCommonUtil.ToEnum<ActionName.File>(action);
            return GetActionText(actionName);
        }

        public static string GetActionText(ActionName.File action)
        {
            string result = string.Empty;
            switch (action)
            {
                case ActionName.File.News:
                    result = "最新消息";
                    break;
                case ActionName.File.Score:
                    result = "積分課程佐證資料";
                    break;
                case ActionName.File.ShareAFile:
                    result = "教材分享-授權書";
                    break;
                case ActionName.File.ShareIFile:
                    result = "教材分享-說明書";
                    break;
                case ActionName.File.ShareSFile:
                    result = "教材分享-教材";
                    break;
                case ActionName.File.TeachShow:
                    result = "授課花絮";
                    break;
            }
            return result;
        }

        public static string GetActionText(ActionName.Admin action)
        {
            string result = string.Empty;
            switch (action)
            {
                case ActionName.Admin.Account:
                    result = "新增帳號";
                    break;
                case ActionName.Admin.ChangePxssword:
                    result = "修改密碼";
                    break;
                case ActionName.Admin.ClassReview:
                    result = "資料審核";
                    break;
                case ActionName.Admin.CourseAudit:
                    result = "課程審核";
                    break;
                case ActionName.Admin.Export:
                    result = "資料匯出";
                    break;
                case ActionName.Admin.FileAudit:
                    result = "檔案上傳審核";
                    break;
                case ActionName.Admin.Feedback:
                    result = "意見回饋";
                    break;
                case ActionName.Admin.Induclass:
                    result = "檔案上傳審核";
                    break;
                case ActionName.Admin.Meeting:
                    result = "會議活動";
                    break;
                case ActionName.Admin.MgrAccount:
                    result = "管理者帳號";
                    break;
                case ActionName.Admin.News:
                    result = "最新消息";
                    break;
                case ActionName.Admin.NewsClose:
                    result = "最新消息下架";
                    break;
                case ActionName.Admin.NewsEdit:
                    result = "最新消息編輯";
                    break;
                case ActionName.Admin.NewsOpen:
                    result = "最新消息上架";
                    break;
                case ActionName.Admin.Score:
                    result = "積分審核";
                    break;
                case ActionName.Admin.Recruit:
                    result = "師資招募";
                    break;
                case ActionName.Admin.System:
                    result = "系統設定";
                    break;
                case ActionName.Admin.Teacher:
                    result = "講師資料設定";
                    break;
                case ActionName.Admin.TeacherInfo:
                    result = "講師基本資料";
                    break;
                default:
                    result = action.ToString();
                    break;
                
            }

            return result;
        }

        public static string GetStatusText(ActionName.EditStatus status)
        {
            string result = string.Empty;
            switch (status)
            {
                case ActionName.EditStatus.FAIL:
                    result = "失敗";
                    break;
                case ActionName.EditStatus.SUCCESS:
                    result = "成功";
                    break;
                default:
                    result = "無";
                    break;
            }
            return result;
        }

        public static string GetEditModeText(ActionName.EditMode mode)
        {
            string result = string.Empty;
            switch (mode)
            {
                case ActionName.EditMode.QUERY:
                    result = "查詢";
                    break;
                case ActionName.EditMode.CREATE:
                    result = "新增";
                    break;
                case ActionName.EditMode.UPDATE:
                    result = "修改";
                    break;
                case ActionName.EditMode.DELETE:
                    result = "刪除";
                    break;
                default:
                    result = "無";
                    break;
            }
            return result;
        }

        public static string GetFileEditTypeText(ActionName.FileEditType mode)
        {
            string result = string.Empty;
            switch (mode)
            {
                case ActionName.FileEditType.UPLOAD:
                    result = "上傳";
                    break;
                case ActionName.FileEditType.DOWNLOAD:
                    result = "下載";
                    break;
                case ActionName.FileEditType.UPLOAD_FAIL:
                    result = "上傳失敗";
                    break;
                default:
                    result = "無";
                    break;
            }
            return result;
        }

        public static string GetEditModelText(string mode)
        {
            ActionName.EditMode modeName = MyCommonUtil.ToEnum<ActionName.EditMode>(mode);
            return GetEditModeText(modeName);
        }

        public static string GetFileEditTypeText(string mode)
        {
            ActionName.FileEditType modeName = MyCommonUtil.ToEnum<ActionName.FileEditType>(mode);
            return GetFileEditTypeText(modeName);
        }



    }
}

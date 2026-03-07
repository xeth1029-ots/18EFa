using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Turbo.MVC.Base3.DataLayers;
using Turbo.MVC.Base3.Models;
using Turbo.MVC.Base3.Models.Entities;
using Turbo.MVC.Base3.Commons;
using Turbo.MVC.Base3.Services;

namespace Turbo.Helpers
{
    /// <summary>
    /// HTML 按鈕 產生輔助方法類別
    /// </summary>
    public static class ButtonExtension
    {
        /// <summary>
        /// HTML 按鈕 產生方法
        /// </summary>
        /// <param name="list">顯示按鈕，可參照AM/C101M方式，Search:查詢/New:新增/Save:儲存/Delete:刪除/Close:關閉 ，後續逐漸增加...</param>
        /// <param name="dis_list">將該按鈕鎖定(加入disabled狀態)，以list的key值名稱若相同則鎖定</param>
        /// <returns></returns>
        public static MvcHtmlString ButtonForTurbo<TModel>(this HtmlHelper<TModel> htmlHelper, Dictionary<string, string> list, IList<string> dis_list = null)
        {
            // HTML標籤
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class='col-sm-12'>");
            sb.Append("<div class='btn-group btn-group-right btn-group-sm clearfix'>");

            // 根據list遍歷相符的KeyName，產生button HTML語法
            foreach (var item in list)
            {
                // 取出list-Key
                var _fun_NAME = item.Value;
                // 若dis_list有包含此list-Key，表示會加入狀態disabled
                var _dis_NAME = "";
                if (dis_list != null) _dis_NAME = dis_list.Contains(item.Key) ? "disabled" : "";
                // 根據list-Key產製 button HTML語法
                switch (item.Key.ToUpper())
                {
                    // 查詢
                    case "SEARCH":
                        sb.Append("<button type='button' class='btn btn-info' onclick='" + _fun_NAME + "' " + _dis_NAME + "><i class='fa fa-search' aria-hidden='true'></i>查詢</button>");
                        break;
                    // 匯入
                    case "FILEINSERT":
                        sb.Append("<button type='button' class='btn btn-info' onclick='" + _fun_NAME + "' " + _dis_NAME + "><i class='fa fa-floppy-o' aria-hidden='true'></i>匯入</button>");

                        break;
                    // 新增
                    case "NEW":
                        sb.Append("<button type='button' class='btn btn-info' onclick='" + _fun_NAME + "' " + _dis_NAME + "><i class='fa fa-pencil-square-o' aria-hidden='true'></i>新增</button>");

                        break;
                    // 儲存
                    case "SAVE":
                        sb.Append("<button type='button' class='btn btn-info' onclick='" + _fun_NAME + "' " + _dis_NAME + "><i class='fa fa-pencil-square-o' aria-hidden='true'></i>儲存</button>");

                        break;
                    // 刪除
                    case "DELETE":
                        sb.Append("<button type='button' class='btn btn-danger' onclick='" + _fun_NAME + "' " + _dis_NAME + "><i class='fa fa-trash' aria-hidden='true'></i>刪除</button>");

                        break;
                    // 列印
                    case "PRINT":
                        sb.Append("<button type='button' class='btn btn-info' onclick='" + _fun_NAME + "' " + _dis_NAME + "><i class='fa fa-download' aria-hidden='true'></i>列印</button>");

                        break;

                    // 回上頁
                    case "BACK":
                        sb.Append("<button type='button' class='btn btn-info' onclick='" + _fun_NAME + "' " + _dis_NAME + "><i class='fa fa-reply' aria-hidden='true'></i>回上頁</button>");
                        break;
                    // 清除畫面
                    case "CLEAR":
                        sb.Append("<button type='button' class='btn btn-info' onclick='" + _fun_NAME + "' " + _dis_NAME + "><i class='fa fa-times-circle' aria-hidden='true'></i>清除畫面</button>");

                        break;
                    // 關閉
                    case "CLOSE":
                        sb.Append("<button type='button' class='btn btn-info' onclick='" + _fun_NAME + "' " + _dis_NAME + "><i class='fa fa-times-circle' aria-hidden='true'></i>關閉</button>");
                        break;
                    // 重設密碼
                    case "RESET":
                        sb.Append("<button type='button' class='btn btn-info' onclick='" + _fun_NAME + "' " + _dis_NAME + ">重設密碼</button>");

                        break;

                    // A1/C101M/施訓單位師資
                    case "TEACHERNEW":
                        sb.Append("<button type='button' class='btn btn-info' onclick='" + _fun_NAME + "' " + _dis_NAME + "><i class='fa fa-search' aria-hidden='true'></i>新增師資資料</button>");
                        break;

                    // A1/C102M/課程資料維護
                    case "SAVETEMP":
                        sb.Append("<button type='button' class='btn btn-info' onclick='" + _fun_NAME + "' " + _dis_NAME + "><i class='glyphicon glyphicon-save-file' aria-hidden='true'></i>儲存草稿</button>");
                        break;

                    // A1/C102M/課程資料維護
                    case "COPY":
                        sb.Append("<button type='button' class='btn btn-info' onclick='" + _fun_NAME + "' " + _dis_NAME + "><i class='glyphicon glyphicon-copy' aria-hidden='true'></i>複製課程</button>");
                        break;

                    // A2/C106M/公告錄取結果
                    case "BOARD":
                        sb.Append("<button type='button' class='btn btn-info' onclick='" + _fun_NAME + "' " + _dis_NAME + "><i class='glyphicon glyphicon-flash' aria-hidden='true'></i>公告錄取結果</button>");
                        break;

                    // A2/C104M 批次錄取
                    case "OFFER":
                        sb.Append("<button type='button' class='btn btn-info' onclick='" + _fun_NAME + "' " + _dis_NAME + "><i class='fa fa-pencil-square-o' aria-hidden='true'></i>批次錄取</button>");
                        break;

                    // A2/C104M 批次未錄取
                    case "NO_OFFER":
                        sb.Append("<button type='button' class='btn btn-info' onclick='" + _fun_NAME + "' " + _dis_NAME + "><i class='fa fa-pencil-square-o' aria-hidden='true'></i>批次未錄取</button>");
                        break;

                    // A2/C104M 批次備取
                    case "BACK_OFFER":
                        sb.Append("<button type='button' class='btn btn-info' onclick='" + _fun_NAME + "' " + _dis_NAME + "><i class='fa fa-pencil-square-o' aria-hidden='true'></i>批次備取</button>");
                        break;

                    // 列印錄取名冊
                    case "PRINT_OFFER":
                        sb.Append("<button type='button' class='btn btn-info' onclick='" + _fun_NAME + "' " + _dis_NAME + "><i class='fa fa-download' aria-hidden='true'></i>列印錄審名冊</button>");

                        break;

                    // A2/C107M 批次歸檔
                    case "STUDY_COLLECT":
                        sb.Append("<button type='button' class='btn btn-info' onclick='" + _fun_NAME + "' " + _dis_NAME + "><i class='fa fa-pencil-square-o' aria-hidden='true'></i>批次歸檔</button>");
                        break;

                    // A3/C102M 結訓作業
                    case "TRAINDONE":
                        sb.Append("<button type='button' class='btn btn-info' onclick='" + _fun_NAME + "' " + _dis_NAME + "><i class='fa fa-pencil-square-o' aria-hidden='true'></i>批次結訓</button>");
                        break;

                    // A3/C102M 退訓
                    case "OUTDONE":
                        sb.Append("<button type='button' class='btn btn-info' onclick='" + _fun_NAME + "' " + _dis_NAME + "><i class='fa fa-pencil-square-o' aria-hidden='true'></i>退訓</button>");
                        break;

                    // A3/C102M 離訓
                    case "LEAVEDONE":
                        sb.Append("<button type='button' class='btn btn-info' onclick='" + _fun_NAME + "' " + _dis_NAME + "><i class='fa fa-pencil-square-o' aria-hidden='true'></i>離訓</button>");
                        break;
                    // A4/C103M 身分查核
                    case "CHECK":
                        sb.Append("<button type='button' class='btn btn-info' onclick='" + _fun_NAME + "' " + _dis_NAME + "><i class='fa fa-sign-in' aria-hidden='true'></i>身分查核</button>");
                        break;
                    // A4/C103M 列印申請表
                    case "PRINTAPPLY":
                        sb.Append("<button type='button' class='btn btn-info' onclick='" + _fun_NAME + "' " + _dis_NAME + "><i class='fa fa-download' aria-hidden='true'></i>列印申請表</button>");
                        break;

                    // A3/C102M 結訓
                    case "DONE":
                        sb.Append("<button type='button' class='btn btn-info' onclick='" + _fun_NAME + "' " + _dis_NAME + "><i class='fa fa-pencil-square-o' aria-hidden='true'></i>結訓</button>");
                        break;

                    // 列印
                    case "PRINTODS":
                        sb.Append("<button type='button' class='btn btn-info' onclick='" + _fun_NAME + "' " + _dis_NAME + "><i class='fa fa-download' aria-hidden='true'></i>ODS列印</button>");

                        break;

                    // A4/C104M 退件
                    case "RETURN":
                        sb.Append("<button type='button' class='btn btn-info' onclick='" + _fun_NAME + "' " + _dis_NAME + "><i class='fa fa-pencil-square-o' aria-hidden='true'></i>退件</button>");
                        break;

                    // A3/C107R 開班資料表
                    case "PRINT_CLASS":
                        sb.Append("<button type='button' class='btn btn-info' onclick='" + _fun_NAME + "' " + _dis_NAME + "><i class='fa fa-pencil-square-o' aria-hidden='true'></i>開班資料表匯出</button>");
                        break;

                    // A3/C107R 學員資料
                    case "PRINT_MEMBER":
                        sb.Append("<button type='button' class='btn btn-info' onclick='" + _fun_NAME + "' " + _dis_NAME + "><i class='fa fa-pencil-square-o' aria-hidden='true'></i>學員資料匯出</button>");
                        break;

                    // A2/C101M 報名登錄
                    case "UPLOAD":
                        sb.Append("<button type='button' class='btn btn-info' onclick='" + _fun_NAME + "' " + _dis_NAME + "><i class='fa fa-pencil-square-o' aria-hidden='true'></i>匯入</button>");
                        break;

                    // A2/C101M 報名登錄
                    case "DOWNLOAD_FILE":
                        sb.Append("<button type='button' class='btn btn-info' onclick='" + _fun_NAME + "' " + _dis_NAME + "><i class='fa fa-pencil-square-o' aria-hidden='true'></i>下載範例檔</button>");
                        break;

                    // A4/C103M 參訓課程補助
                    case "COUNTCHECK":
                        sb.Append("<button type='button' class='btn btn-info' onclick='" + _fun_NAME + "' " + _dis_NAME + "><i class='fa fa-download' aria-hidden='true'></i>身分查核統計剩餘額度</button>");
                        break;
                    // A6/C101M 帳號解鎖
                    case "UNLOCK":
                        sb.Append("<button type='button' class='btn btn-info' onclick='" + _fun_NAME + "' " + _dis_NAME + ">帳號解鎖</button>");

                        break;
                }
            }

            sb.Append("</div>");
            sb.Append("</div>");

            return MvcHtmlString.Create(sb.ToString());
        }
    }
}
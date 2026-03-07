using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using Turbo.Commons;
using Turbo.DataLayer;
using WDACC.Models.Entities;
using WDACC.Models.StoreExt;

namespace WDACC.Commons
{
    public class CommonOptions
    {
        /// <summary>
        /// 授課單元
        /// </summary>
        /// <returns></returns>
        public static IList<SelectListItem> TeachUnit()
        {
            RowBaseDAO dao = new RowBaseDAO();
            IList<KeyMapModel> list = dao.QueryForListAll<KeyMapModel>("KeyMap.TeachUnit", new { });
            IList<SelectListItem> result = CommonUtil.ConvertSelItems(list);
            return result;
        }

        /// <summary>
        /// 服務單位屬性 KeyMap.OrgAttrib
        /// </summary>
        /// <returns></returns>
        public static IList<SelectListItem> OrgAttrib()
        {
            RowBaseDAO dao = new RowBaseDAO();
            IList<KeyMapModel> list = dao.QueryForListAll<KeyMapModel>("KeyMap.OrgAttrib", new { });
            IList<SelectListItem> result = CommonUtil.ConvertSelItems(list);
            return result;
        }

        /// <summary>
        /// 縣市別
        /// </summary>
        /// <returns></returns>
        public static IList<SelectListItem> Region()
        {
            RowBaseDAO dao = new RowBaseDAO();
            IList<KeyMapModel> list = dao.QueryForListAll<KeyMapModel>("KeyMap.Region", new { });
            IList<SelectListItem> result = CommonUtil.ConvertSelItems(list);
            return result;
        }

        /// <summary>
        /// 鄉鎮市區 
        /// </summary>
        /// <returns></returns>
        public static IList<SelectListItem> Zip(int? zipcodeInt = null)
        {
            IList<SelectListItem> result = new List<SelectListItem>();
            RowBaseDAO dao = new RowBaseDAO();
            IList<Store> list = dao.QueryForListAll<Store>("KeyMap.CityRegionZip", new { });
            string zipcode = null;
            if (zipcodeInt.HasValue)
            {
                zipcode = zipcodeInt.ToString();
            }

            if (list != null)
            {
                if (zipcode != null)
                {
                    var item = list.Where(x => x.Get("zipcode").AsText() == zipcode).FirstOrDefault();
                    if (item != null)
                    {
                        var city = item.Get("city").AsText();
                        var cityList = list.Where(x => x.Get("city").AsText() == city).ToList();
                        result = cityList
                            .Select(x => new SelectListItem { Value = x.Get("zipcode").AsText(), Text = x.Get("zipname").AsText() })
                            .ToList();
                    }
                }
                else
                {
                    result = list
                        .Select(x => new SelectListItem { Value = x.Get("zipcode").AsText(), Text = x.Get("zipname").AsText() })
                        .ToList();
                }
            }

            return result;
        }

        /// <summary>
        /// 包含縣市別的郵遞區號名稱選單
        /// </summary>
        /// <param name="zipcodeInt"></param>
        /// <returns></returns>
        public static IList<SelectListItem> ZipWithRegion(int? zipcodeInt = null)
        {
            IList<SelectListItem> result = new List<SelectListItem>();
            RowBaseDAO dao = new RowBaseDAO();
            IList<Store> list = dao.QueryForListAll<Store>("KeyMap.CityRegionZip", new { });
            string zipcode = null;
            if (zipcodeInt.HasValue)
            {
                zipcode = zipcodeInt.ToString();
            }

            if (list != null)
            {
                if (zipcode != null)
                {
                    var item = list.Where(x => x.Get("zipcode").AsText() == zipcode).FirstOrDefault();
                    if (item != null)
                    {
                        var city = item.Get("city").AsText();
                        var cityList = list.Where(x => x.Get("city").AsText() == city).ToList();
                        result = cityList
                            .Select(x => new SelectListItem { Value = x.Get("zipcode").AsText(), Text = x.Get("cityname").AsText() + x.Get("zipname").AsText() })
                            .ToList();
                    }
                }
                else
                {
                    result = list
                        .Select(x => new SelectListItem { Value = x.Get("zipcode").AsText(), Text = x.Get("cityname").AsText() + x.Get("zipname").AsText() })
                        .ToList();
                }
            }

            return result;
        }

        /// <summary>
        /// 學歷
        /// </summary>
        /// <returns></returns>
        public static IList<SelectListItem> Degree()
        {
            RowBaseDAO dao = new RowBaseDAO();
            IList<KeyMapModel> list = dao.QueryForListAll<KeyMapModel>("KeyMap.Degree", new { });
            IList<SelectListItem> result = CommonUtil.ConvertSelItems(list);
            return result;
        }

        /// <summary>
        /// 所屬社群
        /// </summary>
        /// <returns></returns>
        public static IList<SelectListItem> Group()
        {
            RowBaseDAO dao = new RowBaseDAO();
            IList<KeyMapModel> list = dao.QueryForListAll<KeyMapModel>("KeyMap.Group", new { });
            IList<SelectListItem> result = CommonUtil.ConvertSelItems(list);
            return result;
        }

        /// <summary>
        /// 產學類別 
        /// </summary>
        /// <returns></returns>
        public static IList<SelectListItem> AcadeType()
        {
            RowBaseDAO dao = new RowBaseDAO();
            IList<KeyMapModel> list = dao.QueryForListAll<KeyMapModel>("KeyMap.AcadeType", new { });
            IList<SelectListItem> result = CommonUtil.ConvertSelItems(list);
            return result;
        }

        /// <summary>
        /// 產業類別
        /// </summary>
        /// <returns></returns>
        public static IList<SelectListItem> IndsClass(long? sel = null)
        {
            RowBaseDAO dao = new RowBaseDAO();
            IList<KeyMapModel> list = dao.QueryForListAll<KeyMapModel>("KeyMap.IndsClass", new { });
            IList<SelectListItem> result = CommonUtil.ConvertSelItems(list);

            if (sel != null)
            {
                var item = result.Where(x => x.Value == sel.ToString()).FirstOrDefault();
                if (item != null)
                {
                    item.Selected = true;
                }
            }

            return result;
        }

        /// <summary>
        /// 勞動部補助計畫 
        /// </summary>
        /// <returns></returns>
        public static IList<SelectListItem> GovPlan()
        {
            RowBaseDAO dao = new RowBaseDAO();
            IList<KeyMapModel> list = dao.QueryForListAll<KeyMapModel>("KeyMap.GovPlan", new { });
            IList<SelectListItem> result = CommonUtil.ConvertSelItems(list);
            return result;
        }

        /// <summary>
        /// 登入紀錄類型
        /// </summary>
        /// <returns></returns>
        public static IList<SelectListItem> LoginLogType()
        {
            RowBaseDAO dao = new RowBaseDAO();
            IList<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Value = "Login", Text = "登入" });
            list.Add(new SelectListItem { Value = "Logout", Text = "登出" });
            list.Add(new SelectListItem { Value = "LoginFail", Text = "登入失敗" });
            return list;
        }
        public static IList<SelectListItem> LoginLogYearslist()
        {
            RowBaseDAO dao = new RowBaseDAO();
            IList<SelectListItem> list = new List<SelectListItem>();
            int YY1= DateTime.Now.Year - 4;
            int YY2 = DateTime.Now.Year +1;
            // 取最小值與最大值
            int Low = YY1; // 預設值
            int High = YY2; // 預設值
            // 程式跑迴圈，產生清單
            for (int i = Low; i <= High; i++)
            {
                list.Insert(0, new SelectListItem() { Value = $"{i}", Text = $"{(i - 1911)}" });
            }
            return list;
        }
        public static IList<SelectListItem> LoginLogMonthlist()
        {
            RowBaseDAO dao = new RowBaseDAO();
            IList<SelectListItem> list = new List<SelectListItem>();
            // 取最小值與最大值
            int Low = 1; // 預設值
            int High = 12; // 預設值
            // 程式跑迴圈，產生清單
            for (int i = Low; i <= High; i++)
            {
                list.Insert(0, new SelectListItem() { Value = $"{i}", Text = $"{i}" });
            }
            return list;
        }

        /// <summary>
        /// 檔案上傳類型
        /// </summary>
        /// <returns></returns>
        public static IList<SelectListItem> FileLogType()
        {
            RowBaseDAO dao = new RowBaseDAO();
            IList<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Value = "Upload", Text = "上傳" });
            list.Add(new SelectListItem { Value = "Download", Text = "下載" });
            list.Add(new SelectListItem { Value = "UploadFail", Text = "上傳失敗" });
            return list;
        }

        /// <summary>
        /// 功能項目清單 
        /// </summary>
        /// <returns></returns>
        public static IList<SelectListItem> FuncLogItems()
        {
            RowBaseDAO dao = new RowBaseDAO();
            IList<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Value = ActionName.Member.ScoreRegister.ToString(), Text = "積分課程登錄" });
            list.Add(new SelectListItem { Value = ActionName.Member.ShareRegister.ToString(), Text = "教材分享" });
            list.Add(new SelectListItem { Value = ActionName.Member.TeachShow.ToString(), Text = "授課花絮" });
            //list.Add(new SelectListItem { Value = ActionName.Member.TeachImage.ToString(), Text = "授課照片" });
            return list;
        }

        /// <summary>
        /// 功能項目清單 
        /// </summary>
        /// <returns></returns>
        public static IList<SelectListItem> FuncLogType()
        {
            RowBaseDAO dao = new RowBaseDAO();
            IList<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Value = ActionName.EditMode.CREATE.ToString(), Text = "新增" });
            list.Add(new SelectListItem { Value = ActionName.EditMode.UPDATE.ToString(), Text = "修改" });
            list.Add(new SelectListItem { Value = ActionName.EditMode.DELETE.ToString(), Text = "刪除" });
            return list;
        }

        /// <summary>
        /// 檔案上傳使用功能
        /// </summary>
        /// <returns></returns>
        public static IList<SelectListItem> FileLogFunc()
        {
            RowBaseDAO dao = new RowBaseDAO();
            IList<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Value = "1", Text = "積分課程登錄" });
            list.Add(new SelectListItem { Value = "2", Text = "教材分享" });
            list.Add(new SelectListItem { Value = "3", Text = "授課影片" });
            list.Add(new SelectListItem { Value = "4", Text = "授課照片" });
            return list;
        }

        /// <summary>
        /// 開課單位
        /// </summary>
        /// <returns></returns>
        public static IList<SelectListItem> TeachUnitClass()
        {
            RowBaseDAO dao = new RowBaseDAO();
            IList<KeyMapModel> list = dao.QueryForListAll<KeyMapModel>("KeyMap.TeachUnitClass", new { });
            IList<SelectListItem> result = CommonUtil.ConvertSelItems(list);
            return result;
        }

        /// <summary>
        /// 職能類別
        /// </summary>
        /// <returns></returns>
        public static IList<SelectListItem> Fclass()
        {
            IList<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Value = "1", Text = "BC" });
            list.Add(new SelectListItem { Value = "2", Text = "DC" });
            list.Add(new SelectListItem { Value = "3", Text = "KC" });
            return list;
        }

        /// <summary>
        /// 意見回饋類別
        /// </summary>
        /// <returns></returns>
        public static IList<SelectListItem> FeedbackClass()
        {
            IList<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Value = "1", Text = "網站系統操作相關" });
            list.Add(new SelectListItem { Value = "2", Text = "共通核心職能課程相關" });
            list.Add(new SelectListItem { Value = "3", Text = "師資積分審查制度相關" });
            list.Add(new SelectListItem { Value = "4", Text = "會議活動相關" });
            list.Add(new SelectListItem { Value = "5", Text = "貼心建議相關" });
            list.Add(new SelectListItem { Value = "6", Text = "其他" });
            return list;
        }

        /// <summary>
        /// 審核狀態
        /// </summary>
        /// <returns></returns>
        public static IList<SelectListItem> AuditStatus(string sel = null)
        {
            IList<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Value = "1", Text = "審核中" });
            list.Add(new SelectListItem { Value = "2", Text = "通過" });
            list.Add(new SelectListItem { Value = "3", Text = "未通過" });
            list.Add(new SelectListItem { Value = "4", Text = "退回" });
            if (sel != null)
            {
                var item = list.Where(x => x.Value == sel).FirstOrDefault();
                if (item != null)
                {
                    item.Selected = true;
                }
            }
            return list;
        }

        /// <summary>
        /// 會議類型 
        /// </summary>
        /// <returns></returns>
        public static IList<SelectListItem> MeetingClass()
        {
            IList<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Value = "1", Text = "社群會議" });
            list.Add(new SelectListItem { Value = "2", Text = "社群讀書會" });
            list.Add(new SelectListItem { Value = "3", Text = "發展署會議" });
            return list;
        }

        /// <summary>
        /// 年份
        /// </summary>
        /// <param name="yearCount"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static IList<SelectListItem> BeforeYears(int yearCount, int year)
        {
            IList<SelectListItem> list = Enumerable.Range(year - 10, yearCount + 1).OrderByDescending(x => x)
                .Select(x => new SelectListItem { Value = x.ToString(), Text = x.ToString() }).ToList();
            return list;
        }

        /// <summary>
        /// 年份
        /// </summary>
        /// <returns></returns>
        public static IList<SelectListItem> RangeYears(int? startYear, int? endYear, int? selYear)
        {
            IList<SelectListItem> list = Enumerable.Range(startYear.Value, endYear.Value - startYear.Value + 1).OrderByDescending(x => x)
                .Select(x => new SelectListItem { Value = x.ToString(), Text = x.ToString() }).ToList();

            if (selYear.HasValue)
            {
                var item = list.Where(x => x.Value == selYear.Value.ToString()).FirstOrDefault();
                if (item != null)
                {
                    item.Selected = true;
                }
            }

            return list;
        }

        /// <summary>
        /// 當年度至後N個年度
        /// </summary>
        /// <param name="nextNYears"></param>
        /// <returns></returns>
        public static IList<SelectListItem> NextYears(int nextNYears)
        {
            int thisYear = DateTime.Now.Year;
            IList<SelectListItem> list = Enumerable.Range(thisYear, nextNYears + 1).OrderBy(x => x)
                .Select(x => new SelectListItem { Value = x.ToString(), Text = x.ToString() }).ToList();
            return list;
        }

        /// <summary>
        /// 月份
        /// </summary>
        /// <returns></returns>
        public static IList<SelectListItem> Months()
        {
            IList<SelectListItem> list = Enumerable.Range(1, 12).OrderBy(x => x)
                .Select(x => new SelectListItem { Value = x.ToString(), Text = x.ToString() }).ToList();
            return list;
        }

        /// <summary>
        /// 日期
        /// </summary>
        /// <returns></returns>
        public static IList<SelectListItem> Days()
        {
            IList<SelectListItem> list = Enumerable.Range(1, 31).OrderBy(x => x)
                .Select(x => new SelectListItem { Value = x.ToString(), Text = x.ToString() }).ToList();
            return list;
        }

        /// <summary>
        /// 分享人員清單
        /// </summary>
        /// <returns></returns>
        public static IList<SelectListItem> MemberList(string value = null)
        {
            RowBaseDAO dao = new RowBaseDAO();
            var list = dao.GetRowList(new MemDetail { active = 1 })
                .OrderBy(x => x.realname)
                .Select(x => new SelectListItem { Value = x.mid.ToString(), Text = x.realname })
                .ToList();

            if (value != null)
            {
                var item = list.Where(x => x.Value == value).FirstOrDefault();
                item.Selected = true;
            }

            return list;
        }

        /// <summary>
        /// 審核狀態
        /// </summary>
        /// <returns></returns>
        public static IList<SelectListItem> Confirm()
        {
            IList<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Value = "1", Text = "審核中" });
            list.Add(new SelectListItem { Value = "2", Text = "通過" });
            list.Add(new SelectListItem { Value = "3", Text = "未通過" });
            return list;
        }

        /// <summary>
        /// 意見回饋回覆狀態
        /// </summary>
        /// <returns></returns>
        public static IList<SelectListItem> FeedbackStatus()
        {
            IList<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Value = "1", Text = "尚未回覆" });
            list.Add(new SelectListItem { Value = "2", Text = "已回覆" });
            return list;
        }

        /// <summary>
        /// 公告類別
        /// </summary>
        /// <returns></returns>
        public static IList<SelectListItem> NewsClass()
        {
            IList<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Value = "2", Text = "課程" });
            list.Add(new SelectListItem { Value = "3", Text = "師資" });
            list.Add(new SelectListItem { Value = "4", Text = "跑馬燈" });
            return list;
        }

        /// <summary>
        /// 是/否
        /// </summary>
        /// <returns></returns>
        public static IList<SelectListItem> YesNo()
        {
            IList<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Value = "0", Text = "否" });
            list.Add(new SelectListItem { Value = "1", Text = "是" });
            return list;
        }

        /// <summary>
        /// 是/否
        /// </summary>
        /// <returns></returns>
        public static IList<SelectListItem> YesNoYN()
        {
            IList<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Value = "N", Text = "否" });
            list.Add(new SelectListItem { Value = "Y", Text = "是" });
            return list;
        }

        public static IList<SelectListItem> ActionEditStatus()
        {
            IList<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Value = ActionName.EditStatus.SUCCESS.ToString(), Text = "成功" });
            list.Add(new SelectListItem { Value = ActionName.EditStatus.FAIL.ToString(), Text = "失敗" });
            return list;
        }

        /// <summary> 性別 gender </summary>
        /// <returns></returns>
        public static IList<SelectListItem> Gender()
        {
            IList<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Value = "2", Text = "女" });
            list.Add(new SelectListItem { Value = "1", Text = "男" });
            return list;
        }


    }
}

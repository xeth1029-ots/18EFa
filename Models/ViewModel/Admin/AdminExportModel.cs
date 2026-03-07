using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OfficeOpenXml;
using WDACC.Models.StoreExt;

namespace WDACC.Models.ViewModel.Admin
{
    public class AdminExportModel
    {
        public AdminExportModel()
        {
            PrepareItem();
        }

        public IList<string[]> ItemList { get; set; }

        public IList<bool> ExportList { get; set; }

        public long? Active { get; set; }

        public IList<Store> Grid { get; set; }

        public string GetItemName(string value)
        {
            string result = null;
            var item = ItemList.Where(x => x[0] == value).FirstOrDefault();
            if (item != null) { result = item[1]; }
            return result;
        }

        private void PrepareItem()
        {
            IList<string[]> list = new List<string[]>();
            list.Add(new string[] { "pid", "身分證字號", "N" });
            list.Add(new string[] { "realname", "姓名", "N" });
            list.Add(new string[] { "nickname", "別名", "N" });
            list.Add(new string[] { "regyear", "加入年份", "N" }); //加入年份
            list.Add(new string[] { "gender", "性別", "N" });
            list.Add(new string[] { "birthdat", "生日", "N" });
            list.Add(new string[] { "address", "地址", "N" });
            list.Add(new string[] { "phone", "電話", "N" });
            list.Add(new string[] { "mobile", "手機", "N" });
            list.Add(new string[] { "fax", "傳真", "N" });
            list.Add(new string[] { "website", "網址", "N" });
            list.Add(new string[] { "email", "電子郵件", "N" });
            list.Add(new string[] { "jemail", "公司電子郵件", "N" });
            list.Add(new string[] { "facebook", "臉書", "N" });
            list.Add(new string[] { "degree", "學歷", "N" });
            list.Add(new string[] { "school", "畢業學校", "N", "50" });
            list.Add(new string[] { "major", "主修", "N" });
            list.Add(new string[] { "skill", "專長", "N", "50" });
            list.Add(new string[] { "jobcomp", "公司名稱", "N" });
            list.Add(new string[] { "jobtitle", "公司職稱", "N" });
            list.Add(new string[] { "teachunit", "授課單元", "N" });
            list.Add(new string[] { "group", "所屬群別", "N" });
            list.Add(new string[] { "acadetype", "產學類別", "N" });
            list.Add(new string[] { "history", "簡歷", "N", "50" });
            list.Add(new string[] { "orgattrib", "服務單位屬性", "N" });
            list.Add(new string[] { "offreason", "下線原因", "N" });
            list.Add(new string[] { "offdate", "下線日期", "N" });

            this.ItemList = list;

            this.ExportList = new List<bool>();

            foreach (var item in this.ItemList)
            {
                this.ExportList.Add(false);  // 初始化
            }

        }

        public void SyncItemList()
        {
            for (int i = 0; i < ItemList.Count; i++)
            {
                var item = ItemList[i];

                item[2] = this.ExportList[i] ? "Y" : "N";
            }
        }

        public void SetupColunm(ExcelWorksheet sheet)
        {
            if (sheet == null) { return; }

            if (sheet.Dimension == null) { return; }

            double i_DefaultWidth = 22;

            IList<string[]> cols = this.ItemList;

            for (int i = 1; i <= sheet.Dimension.End.Column; i++)
            {
                string[] col = cols[i - 1];
                
                //4:為寬度設定，若沒有使用 i_DefaultWidth
                sheet.Column(i).Width = (col.Length == 4) ? System.Convert.ToInt32(col[3]) : i_DefaultWidth;
            }
            sheet.Cells.Style.WrapText = true;
        }

    }
}

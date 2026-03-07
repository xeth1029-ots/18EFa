using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WDACC.Models.StoreExt;
using WDACC.Models.Entities;
using Turbo.MVC.Base3.Commons;

namespace WDACC.Models.ViewModel.Admin
{
    /// <summary>
    /// 講師資料設定 
    /// </summary>
    public class AdminTeacherModel
    {
        /// <summary>講師姓名</summary>
        public string RealName { get; set; }

        /// <summary>講師帳號</summary>
        public string LoginEmail { get; set; }

        /// <summary>講師EMAIL</summary>
        public string Email { get; set; }

        /// <summary>講師身分證字號</summary>
        public string PID { get; set; }

        /// <summary> 查詢結果 </summary>
        public IList<Store> Grid { get; set; }

        /// <summary> 登入資訊 </summary>
        public WDACC.Models.Entities.Member Mem { get; set; }

        /// <summary> 講師基本資料 </summary>
        public MemDetail Detail { get; set; }

        /// <summary> 個資法遮罩-身份證號 </summary>
        public string pid_mk { get; set; }

        public string[] Showoff { get; set; }

        /// <summary>
        /// 生日年
        /// </summary>
        public int? BirthYear { get; set; }

        /// <summary>
        /// 生日月
        /// </summary>
        public int? BirthMonth { get; set; }

        /// <summary>
        /// 生日日
        /// </summary>
        public int? BirthDay { get; set; }

        /// <summary>
        /// 下線日期(西元)
        /// </summary>
        public string offdate_AD { get; set; }

        /// <summary>
        /// 縣市別
        /// </summary>
        public int? City { get; set; }

        /// <summary>
        /// 服務單位屬性 MemOrgAttrib id, title
        /// </summary>
        public IList<long?> OrgAttribList { get; set; }

        /// <summary>
        /// 授課區域
        /// </summary>
        //public IList<MemTechReg> TechRegList { get; set; }
        public IList<long?> TechRegList { get; set; }

        /// <summary>
        /// 授課單元
        /// </summary>
        public IList<bool> TechUnitList { get; set; }

        /// 可授課產業別清單
        /// </summary>
        public IList<MemIndusClass> IndusList { get; set; }

        /// <summary>
        /// 新密碼
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 密碼確認
        /// </summary>
        public string PasswordConfirm { get; set; }

        public void PrepareAfterLoad()
        {
            if (this.Detail != null && this.Detail.birthdat.HasValue)
            {
                var birth = this.Detail.birthdat.Value;
                this.BirthYear = birth.Year;
                this.BirthMonth = birth.Month;
                this.BirthDay = birth.Day;
            }

            if (!string.IsNullOrEmpty(this.Detail.showoff))
            {
                this.Showoff = new string[10];
                int i = 0;
                foreach (var c in this.Detail.showoff)
                {
                    this.Showoff[i] = c.ToString();
                    i++;
                }
            }
            else
            {
                this.Showoff = new string[] { "0", "0", "0", "0", "0", "0", "0", "0", "0", "0" };
            }

            if (!string.IsNullOrEmpty(this.Detail.teachunit))
            {
                this.TechUnitList = new bool[3];
                this.TechUnitList = this.Detail.teachunit.Select(x => x.ToString() == "1").ToArray();
            }
            //下線日期(民國)
            this.offdate_AD = MyCommonUtil.TransToYYYYMMDD(this.Detail.offdate);
        }

        public void PrepareBeforeSave()
        {
            if (this.BirthYear.HasValue && this.BirthMonth.HasValue && this.BirthDay.HasValue)
            {
                this.Detail.birthdat = new DateTime
                        (this.BirthYear.Value, this.BirthMonth.Value, this.BirthDay.Value);
            }

            if (this.Showoff != null)
            {
                this.Detail.showoff = string.Join("", this.Showoff);
            }
            else
            {
                this.Detail.showoff = "0000000000";
            }

            if (this.TechUnitList != null)
            {
                IList<string> list = this.TechUnitList.Select(x => x == true ? "1" : "0").ToList();
                this.Detail.teachunit = string.Join("", list);
            }
            //下線日期(民國)
            this.Detail.offdate = MyCommonUtil.TransToDateTime(this.offdate_AD);

        }

    }
}

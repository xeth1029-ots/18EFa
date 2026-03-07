using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WDACC.Models.Entities;

namespace WDACC.Models.ViewModel.Member
{
    public class BaseInfoModel
    {
        /// <summary>
        /// 講師明細
        /// </summary>
        public MemDetail MemberDetail { get; set; }

        public string[] Showoff { get; set; }

        /// <summary>
        /// 地址-縣市
        /// </summary>
        public int? City { get; set; }

        public int? BirthYear { get; set; }
        public int? BirthMonth { get; set; }
        public int? BirthDay { get; set; }

        /// <summary>
        /// 可授課產業別
        /// </summary>
        public IList<MemIndusClass> IndusList { get; set; }

        /// <summary>
        /// 可授課區域別
        /// </summary>
        public IList<long?> TechRegList { get; set; }

        /// <summary>
        /// 服務單位屬性 MemOrgAttrib
        /// </summary>
        public IList<long?> OrgAttribList { get; set; }
        
        /// <summary>
        /// 授課單元
        /// </summary>
        public IList<bool> TechUnitList{ get; set; }

        public void PrepareAfterLoad()
        {
            if (this.MemberDetail != null && this.MemberDetail.birthdat.HasValue)
            {
                this.BirthYear = this.MemberDetail.birthdat.Value.Year;
                this.BirthMonth = this.MemberDetail.birthdat.Value.Month;
                this.BirthDay = this.MemberDetail.birthdat.Value.Day;
            }

            if (!string.IsNullOrEmpty(this.MemberDetail.showoff))
            {
                this.Showoff = new string[10];
                int i = 0;
                foreach (var c in this.MemberDetail.showoff)
                {
                    this.Showoff[i] = c.ToString();
                    i++;
                }
            }
            else
            {
                this.Showoff = new string[] { "0", "0", "0", "0", "0", "0", "0", "0", "0", "0" };
            }
        }

        public void PrepareBeforeSave()
        {
            if (this.BirthYear.HasValue && this.BirthMonth.HasValue && this.BirthDay.HasValue)
            {
                this.MemberDetail.birthdat = new DateTime(BirthYear.Value, BirthMonth.Value, BirthDay.Value);
            }

            if (this.Showoff != null)
            {
                this.MemberDetail.showoff = string.Join("", this.Showoff);
            }
            else
            {
                this.MemberDetail.showoff = "0000000000";
            }
        }

    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WDACC.Models.Entities;
using WDACC.Models.StoreExt;
using WDACC.Services;

namespace WDACC.Models
{
    public class TeacherDetail
    {

        public TeacherDetail()
        {
            this.MemDetail = new MemDetail();
            this.TeachUnit = new TeachUnitField();
        }

        /// <summary>
        /// 師資登入檔
        /// </summary>
        public Member MemberBase { get; set; }

        /// <summary>
        /// 師資明細
        /// </summary>
        public MemDetail MemDetail { get; set; }

        /// <summary>
        /// 授課區域 id, title
        /// </summary>
        public IList<Store> MemTechReg { get; set; }

        /// <summary>
        /// 服務單位屬性 MemOrgAttrib id, title
        /// </summary>
        public IList<Store> MemOrgAttrib { get; set; }

        /// <summary>
        /// 曾開授課程
        /// </summary>
        public IList<Store> CourseList { get; set; }

        /// <summary>
        /// 授課花絮
        /// </summary>
        public IList<TeacherShow> TeacherShow { get; set; }

        /// <summary>
        /// 授課單元 
        /// </summary>
        public TeachUnitField TeachUnit { get; set; }

        /// <summary>
        /// 授課產業別
        /// </summary>
        public IList<Store> IndClass { get; set; }

        /// <summary>
        /// 授課時數 mid, realname, sum_total,  
        /// </summary>
        public Store CourseHours { get; set; }

        /// <summary>
        /// 是否顯示欄位 (1:性別,2:生日,3:電話,4:手機,5:傳真,6:EMail,7:EMail(工作),8:聯絡地址,9:個人網站,10:FaceBook)
        /// </summary>
        public bool[] ShowOff { get; set; }

    }
}

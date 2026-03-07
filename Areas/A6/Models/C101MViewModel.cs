using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Turbo.Commons;
using Turbo.DataLayer;
using Turbo.MVC.Base3.Commons;
using Turbo.MVC.Base3.Models;
using Turbo.MVC.Base3.Models.Entities;
using System.Linq;
using Turbo.MVC.Base3.Services;

namespace Turbo.MVC.Base3.Areas.A6.Models
{
    /// <summary>
    /// A6/C101M ViewModel
    /// </summary>
    public class C101MViewModel
    {
        public C101MViewModel()
        {
            this.Form = new C101MFormModel();
        }

        /// <summary>
        /// 查詢條件 FromModel
        /// </summary>
        public C101MFormModel Form { get; set; }

        /// <summary>
        ///  Detail
        /// </summary>
        public C101MDetailModel Detail { get; set; }

        /// <summary>
        ///  Detail
        /// </summary>
        public C101MDetail1Model Detail1 { get; set; }
    }

    /// <summary>
    /// A6/C101M FormModel
    /// </summary>
    public class C101MFormModel : PagingResultsViewModel
    {
        /// <summary>
        /// 姓名
        /// </summary>
        [Display(Name = "姓名")]
        [Control(Mode = Control.TextBox, group = 1, size = "22", maxlength = "12")]
        public string user_name { get; set; }

        /// <summary>
        /// 帳號
        /// </summary>
        [Display(Name = "帳號")]
        [Control(Mode = Control.TextBox, group = 1, size = "20", maxlength = "10")]
        public string user_username { get; set; }

        /// <summary>
        /// 單位(代碼)
        /// </summary>
        [Display(Name = "單位")]
        [Control(Mode = Control.e_unit, group = 2)]
        public string unit_code { get; set; }

        /// <summary>
        /// 單位名稱
        /// </summary>
        [NotDBField]
        public string unit_code_TEXT { get; set; }
        
        public IList<C101MGridModel> Grid { get; set; }
    }

    /// <summary>
    /// A6/C101M GridModel
    /// </summary>
    public class C101MGridModel : e_user
    {
    }

    /// <summary>
    /// A6/C101M DetailModel
    /// </summary>
    public class C101MDetailModel : e_user
    {
        public C101MDetailModel()
        {
            this.IsNew = true;
        }

        /// <summary>
        /// Detail必要控件(Hidden)
        /// </summary>
        [Control(Mode = Control.Hidden)]
        [NotDBField]
        public bool IsNew { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Control(Mode = Control.Hidden)]
        [IdentityDBField]
        public string user_id { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Display(Name = "姓名")]
        [Control(Mode = Control.TextBox, group = 1, size = "22", maxlength = "12", IsOpenNew = true)]
        [Required]
        public string user_name { get; set; }

        /// <summary>
        /// 帳號
        /// </summary>
        [Display(Name = "帳號")]
        [Control(Mode = Control.TextBox, group = 1, size = "20", maxlength = "10", IsOpenNew = true)]
        [Required]
        public string user_username { get; set; }

        /// <summary>
        /// 密碼
        /// </summary>
        [Display(Name = "密碼")]
        [Control(Mode = Control.PassWord, size = "25", maxlength = "15", IsOpenNew = true)]
        public string user_password { get; set; }

        /// <summary>
        /// 密碼確認
        /// </summary>
        [Display(Name = "密碼確認")]
        [NotDBField]
        public string user_password_REPEAT { get; set; }

        /// <summary>
        /// 單位(代碼)
        /// </summary>
        [Display(Name = "單位")]
        [Control(Mode = Control.e_unit)]
        [NotDBField]
        public string unit_id { get; set; }

        /// <summary>
        /// 單位名稱
        /// </summary>
        [NotDBField]
        public string unit_id_TEXT { get; set; }
        
    }

    /// <summary>
    /// A6/C101M Detail1Model
    /// </summary>
    public class C101MDetail1Model : e_permission
    {
        public C101MDetail1Model()
        {
            
        }

        /// <summary>
        /// 儲存塞值(使用者ID)
        /// </summary>
        public Int32? getid { get; set; }
        
        /// <summary>
        /// 系統管理
        /// </summary>
        public string sys_a6 { get; set; }

        [Display(Name = "系統管理")]
        [Control(Mode = Control.CheckBoxList)]
        [NotDBField]
        public string[] sys_a6_SHOW
        {
            get
            {
                if (this.sys_a6 != null)
                {
                    return this.sys_a6.Replace("'", "").Split(',');
                }
                else
                {
                    return new string[0];
                }
            }
            set
            {
                if (value != null)
                {
                    this.sys_a6 = string.Join(",", value.ToList().Select(m => "'" + m.TONotNullString() + "'").ToList());
                }
            }
        }

        /// <summary>
        /// 系統管理_清單 checkbox
        /// </summary>
        [NotDBField]
        public IList<CheckBoxListItem> sys_a6_SHOW_list
        {
            get
            {
                ShareCodeListModel list = new ShareCodeListModel();
                return list.sys_a6_checkbox_list;
            }
        }
       
    }
}
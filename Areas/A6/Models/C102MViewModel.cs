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
    /// A6/C102M ViewModel
    /// </summary>
    public class C102MViewModel
    {
        public C102MViewModel()
        {
            this.Form = new C102MFormModel();
        }

        /// <summary>
        /// 查詢條件 FromModel
        /// </summary>
        public C102MFormModel Form { get; set; }

        /// <summary>
        ///  Detail
        /// </summary>
        public C102MDetailModel Detail { get; set; }
    }

    /// <summary>
    /// A6/C102M FormModel
    /// </summary>
    public class C102MFormModel : PagingResultsViewModel
    {
        /// <summary>
        /// 姓名
        /// </summary>
        [Display(Name = "姓名")]
        [Control(Mode = Control.TextBox, group = 1, size = "22", maxlength = "12")]
        public string username { get; set; }

        /// <summary>
        /// 帳號
        /// </summary>
        [Display(Name = "帳號")]
        [Control(Mode = Control.TextBox, group = 1, size = "20", maxlength = "10")]
        public string useraccount { get; set; }

        /// <summary>
        /// 單位(代碼)
        /// </summary>
        [Display(Name = "單位")]
        [Control(Mode = Control.e_unit, group = 2)]
        public string userdepartname { get; set; }

        /// <summary>
        /// 單位名稱
        /// </summary>
        [NotDBField]
        public string userdepartname_TEXT { get; set; }

        /// <summary>
        /// 系統功能
        /// </summary>
        [Display(Name = "系統功能")]
        [Control(Mode = Control.DropDownList)]
        public string userdepart { get; set; }

        /// <summary>
        /// 系統功能_清單
        /// </summary>
        [NotDBField]
        public IList<SelectListItem> userdepart_list
        {
            get
            {
                ShareCodeListModel model = new ShareCodeListModel();
                SelectListItem item = new SelectListItem();
                item.Text = "全部";
                item.Value = "";
                var list = model.functionname_list;
                list.Add(item);
                list = list.OrderBy(m => m.Value).ToList();
                return list;
            }
        }

        /// <summary>
        /// 使用功能
        /// </summary>
        public string methodname { get; set; }

        [Display(Name = "使用功能")]
        [Control(Mode = Control.CheckBoxList)]
        [NotDBField]
        public string[] methodname_SHOW
        {
            get
            {
                if (this.methodname != null)
                {
                    return this.methodname.Replace("'", "").Split(',');
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
                    this.methodname = string.Join(",", value.ToList().Select(m => "'" + m.TONotNullString() + "'").ToList());
                }
            }
        }

        /// <summary>
        /// 使用功能_清單 checkbox
        /// </summary>
        [NotDBField]
        public IList<CheckBoxListItem> methodname_SHOW_list
        {
            get
            {
                ShareCodeListModel list = new ShareCodeListModel();
                return list.methodname_checkbox_list;
            }
        }

        public IList<C102MGridModel> Grid { get; set; }
    }

    /// <summary>
    /// A6/C102M GridModel
    /// </summary>
    public class C102MGridModel : e_userlog
    {
    }

    /// <summary>
    /// A6/C102M DetailModel
    /// </summary>
    public class C102MDetailModel : e_userlog
    {
        public C102MDetailModel()
        {
        }

        /// <summary>
        /// 日期
        /// </summary>
        [Display(Name = "日期")]
        [Control(Mode = Control.TextBox, group = 1, size = "25", IsReadOnly = true)]
        public string indate { get; set; }

        // <summary>
        /// 序號
        /// </summary>
        [Display(Name = "序號")]
        [Control(Mode = Control.TextBox, group = 1, size = "25", IsReadOnly = true)]
        public string Sn { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Display(Name = "姓名")]
        [Control(Mode = Control.TextBox, group = 2, size = "22", IsReadOnly = true)]
        public string username { get; set; }

        /// <summary>
        /// 帳號
        /// </summary>
        [Display(Name = "帳號")]
        [Control(Mode = Control.TextBox, group = 2, size = "20", IsReadOnly = true)]
        public string useraccount { get; set; }

        /// <summary>
        /// 部門
        /// </summary>
        [Display(Name = "部門")]
        [Control(Mode = Control.TextBox, group = 3, size = "25", IsReadOnly = true)]
        public string userdepart { get; set; }

        /// <summary>
        /// 部門名稱
        /// </summary>
        [Display(Name = "部門名稱")]
        [Control(Mode = Control.TextBox, group = 3, size = "25", IsReadOnly = true)]
        public string userdepartname { get; set; }

        /// <summary>
        /// 功能名稱
        /// </summary>
        [Display(Name = "功能名稱")]
        [Control(Mode = Control.TextBox, group = 4, size = "25", IsReadOnly = true)]
        public string functionname { get; set; }

        /// <summary>
        /// 方法名稱
        /// </summary>
        [Display(Name = "方法名稱")]
        [Control(Mode = Control.TextBox, group = 4, size = "25", IsReadOnly = true)]
        public string methodname { get; set; }

        /// <summary>
        /// 方法目標
        /// </summary>
        [Display(Name = "方法目標")]
        [Control(Mode = Control.TextBox, group = 5, size = "25", IsReadOnly = true)]
        public string methodtarget { get; set; }

        /// <summary>
        /// 群組名稱
        /// </summary>
        [Display(Name = "群組名稱")]
        [Control(Mode = Control.TextBox, group = 5, size = "25", IsReadOnly = true)]
        public string groupName { get; set; }
    }
}
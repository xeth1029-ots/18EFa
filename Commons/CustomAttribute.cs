using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Turbo.MVC.Base3.Commons;
using Turbo.DataLayer;
using System.Reflection;

namespace Turbo.MVC.Base3.Commons
{
    /// <summary>
    /// 控制項Attribute名稱
    /// </summary>
    public class ControlAttribute : Attribute
    {
        /// <summary>
        /// 控項類別
        /// </summary>
        public Control Mode { get; set; }

        /// <summary>
        /// 屬性
        /// </summary>
        public PropertyInfo pi { get; set; }

        /// <summary>
        /// 控制項狀態
        /// </summary>
        public object HtmlAttribute { get; set; }

        /// <summary>
        /// 控制項NewOrModify情況
        /// </summary>
        public bool IsOpenNew { get; set; }

        /// <summary>
        /// 控制項ReadOnly情況
        /// </summary>
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// 寬度
        /// </summary>
        public string size { get; set; }

        /// <summary>
        /// 列數(TextArea專用)
        /// </summary>
        public int rows { get; set; }

        /// <summary>
        /// 行數(TextArea專用)
        /// </summary>
        public int columns { get; set; }

        /// <summary>
        /// 文字(最大長度)
        /// </summary>
        public string maxlength { get; set; }

        /// <summary>
        /// 控制項提示
        /// </summary>
        public string placeholder { get; set; }

        /// <summary>
        /// 觸發事件
        /// </summary>
        public string onblur { get; set; }

        #region 同一個form裡面(同一行)
        /// <summary>
        /// 放置於同一個欄位
        /// </summary>
        public int group { get; set; }

        /// <summary>
        /// 設定group_form Id
        /// </summary>
        public string form_id { get; set; }
        #endregion
        #region 同一個toggle_block裡面(同一區塊)

        /// <summary>
        /// 放置於同一個區塊
        /// </summary>
        public int block_toggle_group { get; set; }

        /// <summary>
        /// 放置於同一個區塊
        /// </summary>
        public string block_toggle_id { get; set; }

        /// <summary>
        /// 是否展示縮合區塊
        /// </summary>
        public bool block_toggle { get; set; }

        /// <summary>
        /// 縮合名稱
        /// </summary>
        public string toggle_name { get; set; }

        #endregion
        #region 同一個block裡面(同一區塊)

        /// <summary>
        /// 放置於同一個區塊
        /// </summary>
        public int block_group { get; set; }

        /// <summary>
        /// 放置於同一個區塊
        /// </summary>
        public string block_id { get; set; }

        #endregion
        #region 最外層的Block DIV(包在最外面的)
        /// <summary>
        /// 放置於同一個區塊
        /// </summary>
        public string block_BIG_id { get; set; }
        #endregion


    }

    /// <summary>
    /// 控項類別
    /// </summary>
    public enum Control
    {
        /// <summary>
        /// 隱藏元件
        /// </summary>
        Hidden,

        /// <summary>
        /// 輸入框
        /// </summary>
        Model,

        /// <summary>
        /// 輸入框
        /// </summary>
        TextBox,

        /// <summary>
        /// 密碼輸入框
        /// </summary>
        PxssWord,

        /// <summary>
        /// 伸縮輸入框
        /// </summary>
        TextArea,

        /// <summary>
        /// 下拉選單
        /// </summary>
        DropDownList,

        /// <summary>
        /// 單選
        /// </summary>
        Radio,

        /// <summary>
        /// 單選群組
        /// </summary>
        RadioGroup,

        /// <summary>
        /// 檢核
        /// </summary>
        CheckBox,

        /// <summary>
        /// 多選群組
        /// </summary>
        CheckBoxList,

        /// <summary>
        /// 日期輸入框
        /// </summary> 
        DatePicker,

        /// <summary>
        /// 行政區+地址代碼
        /// </summary>
        ZIP_CO_FULL,

        /// <summary>
        /// 行政區代碼
        /// </summary>
        ZIP_CO,

        /// <summary>
        /// 單位輸入框
        /// </summary>
        e_unit,

        /// <summary>
        /// 使用者輸入框
        /// </summary>
        e_user,

        /// <summary>
        /// 使用者輸入框
        /// </summary>
        e_id_user,

        /// <summary>
        /// 檔案上傳
        /// </summary>
        FILEGRID
    }
}

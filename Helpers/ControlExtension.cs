using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Turbo.MVC.Base3.Commons;
using Turbo.MVC.Base3.Services;
using Turbo.Commons;

namespace Turbo.Helpers
{
    /// <summary>
    /// HTML 控項 產生輔助方法類別(快速產生)
    /// </summary>
    public static class ControlExtension
    {
        /// <summary>
        /// HTML 控項，節儉版面及快速產製使用，將需CustomLabelFor與其他共項元件產生部分一行解決
        /// </summary>
        /// <param name="expression">Model 欄位的 Lambda 表達式物件</param>
        /// <param name="Control">其他共項元件</param>
        /// <returns></returns>
        public static MvcHtmlString ControlForTurbo<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, string>> expression, MvcHtmlString Control)
        {
            // HTML標籤
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class='form-group'>");
            sb.Append(htmlHelper.CustomLabelFor(expression, new { @class = "col-sm-2 control-label label-set" }));
            sb.Append("<div class='col-sm-10 form-inline'>");
            if (Control != null) sb.Append(Control);
            sb.Append("</div>");
            sb.Append("</div>");

            return MvcHtmlString.Create(sb.ToString());
        }

        /// <summary>
        /// HTML 控項，節儉版面及快速產製使用，將需CustomLabelFor與其他共項元件產生部分一行解決
        /// </summary>
        /// <param name="expression">Model 欄位的 Lambda 表達式物件</param>
        /// <param name="Control">其他共項元件</param>
        /// <param name="IsNew">明細專用，需配合Attr.IsNew</param>
        /// <param name="Model_tag">明細專用，若有子類別，需將子類別名稱鍵入，便可達到二階層</param>
        /// <returns></returns>
        public static MvcHtmlString ControlForTurbo<TModel, T>(this HtmlHelper<TModel> htmlHelper, T Model, bool IsNew = true)
        {
            // HTML標籤
            //var Id_tag = Model_tag == "" ? Model_tag : Model_tag + "_";
            //var Name_tag = Model_tag == "" ? Model_tag : Model_tag + ".";

            StringBuilder sb = new StringBuilder();
            // 設定ControlAttribute清單
            IList<Turbo.MVC.Base3.Commons.ControlAttribute> ca = new List<Turbo.MVC.Base3.Commons.ControlAttribute>();
            // 搜尋該ViewModel屬性(畫面Model請選擇第二層Model(FormModel,DetailModel等...)為主)
            foreach (var pi in Model.GetType().GetProperties())
            {
                // 取得屬性自定義控件狀態
                var attr = pi.GetCustomAttribute<Turbo.MVC.Base3.Commons.ControlAttribute>();
                if (attr != null)
                {
                    attr.pi = pi;
                    ca.Add(attr);
                }
            }

            // 算出最大的Block_group數目，讓其餘未有加group屬性保證為單一不重複
            //int max_Bgroup_value = ca.Select(m => m.block_group).Max();
            //foreach (var c in ca)
            //{
            //    ++max_Bgroup_value;
            //    if (c.block_group == 0)
            //    { c.block_group = max_Bgroup_value; }
            //}

            // 算出最大的group數目，讓其餘未有加group屬性保證為單一不重複
            int max_group_value = ca.Select(m => m.group).Max();
            foreach (var c in ca)
            {
                ++max_group_value;
                if (c.group == 0)
                { c.group = max_group_value; }
            }

            // 依據group分類
            var Ca_Block_Group = ca.GroupBy(m => m.block_toggle_group).Select(m => m.ToList()).ToList();

            foreach (var ca_Group in Ca_Block_Group)
            {
                var Ca_blockDiv_id = ca_Group.Where(m => m.block_BIG_id.TONotNullString() != "").ToList();
                var blockIDivd = Ca_blockDiv_id.ToCount() > 0 ? Ca_blockDiv_id.FirstOrDefault().block_BIG_id : "";

                if (blockIDivd != "")
                {
                    sb.Append("<div id='" + blockIDivd + "' >");
                }

                var Ca_block_id = ca_Group.Where(m => m.block_toggle_id.TONotNullString() != "").ToList();
                var blockId = Ca_block_id.ToCount() > 0 ? Ca_block_id.FirstOrDefault().block_toggle_id : "";

                // 是否顯示縮合
                var Ca_toggle = ca_Group.Where(m => m.block_toggle).ToList();
                var toggle_class = "";
                if (Ca_toggle.ToCount() > 0)
                {
                    toggle_class = "panel-collapse collapse";

                    sb.Append("<div class='panel panel-info'>");

                    sb.Append("<div class='panel-heading'>");

                    sb.Append("<h4 class='panel-title' data-toggle='collapse' data-parent='#" + blockId + "' href = '#" + blockId + "'> ");
                    sb.Append("<a>");
                    sb.Append(Ca_toggle.FirstOrDefault().toggle_name + "(點擊展開)");
                    sb.Append("</a>");
                    sb.Append("</h4>");

                    sb.Append("</div>");
                }

                sb.Append("<div id='" + blockId + "' class='" + toggle_class + "'>");

                if (Ca_toggle.ToCount() > 0)
                {
                    sb.Append(" <div class='panel-body'>");
                }

                // 依據block_group分類
                var Ca_Block_in_Group = ca_Group.GroupBy(m => m.block_group).Select(m => m.ToList()).ToList();

                foreach (var ca_Group_in in Ca_Block_in_Group)
                {
                    var ca_Group_in_id = ca_Group_in.Where(m => m.block_id.TONotNullString() != "").ToList();
                    var InBlockId = ca_Group_in_id.ToCount() > 0 ? ca_Group_in_id.FirstOrDefault().block_id : "";

                    sb.Append("<div id='" + InBlockId + "'>");

                    // 依據group分類
                    var Ca_Group = ca_Group_in.GroupBy(m => m.group).Select(m => m.ToList()).ToList();

                    foreach (var Ca in Ca_Group)
                    {
                        var CaCount = Ca.ToCount();
                        // 計算Css賦予(NormalController) 1:col-sm-10 / 2: col-sm-4 / else:col-sm-2 
                        var col_sm = "col-sm-2";
                        switch (CaCount)
                        {
                            case 1:
                                col_sm = "col-sm-10";
                                break;
                            case 2:
                                col_sm = "col-sm-4";
                                break;
                        }

                        var Ca_form_id = Ca.Where(m => m.form_id.TONotNullString() != "").ToList();
                        var form_Id = Ca_form_id.ToCount() > 0 ? Ca_form_id.FirstOrDefault().form_id : "";

                        sb.Append("<div class='form-group' id='" + form_Id + "'>");

                        foreach (var attr in Ca)
                        {
                            // 生成控制項

                            // Detail視窗專用
                            // 關閉控項狀態(平常控項使用)
                            var IsReadOnly = attr.IsReadOnly ? attr.IsReadOnly : (attr.IsOpenNew && !IsNew);
                            // 關閉控項狀態(特殊控項使用)
                            var DisabledString = "pointer-events:none;background:#DDDDDD";
                            var IsDisabled = attr.IsReadOnly ? DisabledString : (attr.IsOpenNew && !IsNew) ? DisabledString : "";

                            // property轉換成expression
                            var property = attr.pi;
                            var target = Expression.Parameter(typeof(TModel));
                            var getPropertyValue = Expression.Property(target, property);
                            if (property.PropertyType.Name == "Boolean")
                            {

                                var expression = Expression.Lambda<Func<TModel, Boolean>>(getPropertyValue, target);

                                // 建立 Hidden 控件
                                if (attr.Mode == Control.Hidden)
                                {
                                    sb.Append(htmlHelper.HiddenFor(expression));
                                }
                            }
                            // 建立 CheckBoxList控件
                            else if (attr.Mode == Control.CheckBoxList)
                            { 
                                var expression = Expression.Lambda<Func<TModel, IList<object>>>(getPropertyValue, target);
                                var plist = Model.GetType().GetProperties().Where(m => m.Name == attr.pi.Name + "_list").FirstOrDefault();
                                var value = plist.GetValue(Model);
                                var safeValue = (IList<CheckBoxListItem>)value;
                                sb.Append(htmlHelper.CustomLabelFor(expression, new { @class = "col-sm-2 control-label label-set" }));
                                sb.Append("<div class='" + col_sm + " form-inline'>");
                                sb.Append(htmlHelper.CheckBoxListFor(expression, safeValue, new { style = IsDisabled }));
                                sb.Append("</div>");
                            }
                            // 建立 FILEGRID控件
                            else if (attr.Mode == Control.FILEGRID)
                            {
                                var expression = Expression.Lambda<Func<TModel, object>>(getPropertyValue, target);

                                sb.Append(htmlHelper.EditorFor(expression));
                            }
                            else
                            {
                                var expression = Expression.Lambda<Func<TModel, object>>(getPropertyValue, target);

                                // 建立 Hidden 控件
                                if (attr.Mode == Control.Hidden)
                                {
                                    sb.Append(htmlHelper.HiddenFor(expression));
                                }
                                // 建立 PxssWord 控件
                                else if (attr.Mode == Control.PxssWord)
                                {
                                    if (!IsReadOnly)
                                    {
                                        var piText = Model.GetType().GetProperties().Where(m => m.Name == attr.pi.Name + "_REPEAT").FirstOrDefault();
                                        // property轉換成expression
                                        var getPropertyValue_REPEAT = Expression.Property(target, piText);
                                        var expressionREPEAT = Expression.Lambda<Func<TModel, object>>(getPropertyValue_REPEAT, target);

                                        // 第一次輸入
                                        sb.Append(htmlHelper.CustomLabelFor(expression, new { @class = "col-sm-2 control-label label-set" }));
                                        sb.Append("<div class='col-sm-4 form-inline'>");
                                        sb.Append(htmlHelper.PasswordFor(expression, new { @class = "form-control", size = attr.size, maxlength = attr.maxlength, placeholder = attr.placeholder }));
                                        sb.Append("</div>");
                                        // 第二次輸入
                                        sb.Append(htmlHelper.CustomLabelFor(expressionREPEAT, new { @class = "col-sm-2 control-label label-set" }));
                                        sb.Append("<div class='col-sm-4 form-inline'>");
                                        sb.Append(htmlHelper.PasswordFor(expressionREPEAT, new { @class = "form-control", size = attr.size, maxlength = attr.maxlength, placeholder = attr.placeholder }));
                                        sb.Append("<span style='color:red;display:none' id='PwdError'></span>");
                                        sb.Append("</div>");

                                        // JS區塊
                                        var Id1 = attr.pi.Name;
                                        var Id2 = attr.pi.Name + "_REPEAT";
                                        sb.Append("<script type='text/javascript'>");
                                        sb.Append("$(document).ready(function () {");
                                        sb.Append("var Id1 = $('#" + Id1 + "');");
                                        sb.Append("var Id2 = $('#" + Id2 + "');");
                                        sb.Append("var PwdError = $('#PwdError');");
                                        sb.Append("Id2.on('change', function(){");
                                        sb.Append("PwdError.removeAttr('style');");
                                        sb.Append("if(Id1.val()==Id2.val()){");
                                        sb.Append("PwdError.attr('style','color:green;');");
                                        sb.Append("PwdError.text('密碼相同');");
                                        sb.Append("}");
                                        sb.Append("else{");
                                        sb.Append("PwdError.attr('style','color:red;');");
                                        sb.Append("PwdError.text('密碼不相同，請檢查');");
                                        sb.Append("};");
                                        sb.Append("});");
                                        sb.Append("});");
                                        sb.Append("</script>");
                                    }
                                }
                                else
                                {
                                    sb.Append(htmlHelper.CustomLabelFor(expression, new { @class = "col-sm-2 control-label label-set" }));
                                    sb.Append("<div class='" + col_sm + " form-inline'>");

                                    // 建立 TextBox 控件
                                    if (attr.Mode == Control.TextBox) sb.Append(htmlHelper.TextBoxFor(expression, new { @class = "form-control", size = attr.size, maxlength = attr.maxlength, placeholder = attr.placeholder }, IsReadOnly));

                                    // 建立 TextArea 控件
                                    if (attr.Mode == Control.TextArea) sb.Append(htmlHelper.TextAreaFor(expression, attr.rows, attr.columns, new { @class = "form-control", maxlength = attr.maxlength, style = IsDisabled }));

                                    // 建立 DatePicker 控件
                                    if (attr.Mode == Control.DatePicker) sb.Append(htmlHelper.DatePickerTWFor(expression, new { placeholder = attr.placeholder, style = IsDisabled, onblur = attr.onblur }));

                                    // 建立 DropDownList控件
                                    if (attr.Mode == Control.DropDownList)
                                    {
                                        var plist = Model.GetType().GetProperties().Where(m => m.Name == attr.pi.Name + "_list").FirstOrDefault();
                                        var value = plist.GetValue(Model);
                                        var safeValue = (IList<SelectListItem>)value;
                                        sb.Append(htmlHelper.DropDownListFor(expression, safeValue, new { @class = "form-control formbar-bg", style = IsDisabled }));
                                    }


                                    // 建立 RadioGroup控件
                                    if (attr.Mode == Control.RadioGroup)
                                    {
                                        var plist = Model.GetType().GetProperties().Where(m => m.Name == attr.pi.Name + "_list").FirstOrDefault();
                                        var value = plist.GetValue(Model);
                                        var safeValue = (IList<SelectListItem>)value;
                                        sb.Append(htmlHelper.RadioGroupFor(expression, safeValue, new { style = IsDisabled }));
                                    }
                                    
                                    // 建立 ZIP_CO_FULL控件
                                    if (attr.Mode == Control.ZIP_CO_FULL)
                                    {
                                        var piText = Model.GetType().GetProperties().Where(m => m.Name == attr.pi.Name + "_TEXT").FirstOrDefault();
                                        // property轉換成expression
                                        var getPropertyValue_TEXT = Expression.Property(target, piText);
                                        var expressionTEXT = Expression.Lambda<Func<TModel, object>>(getPropertyValue_TEXT, target);

                                        var piADDR = Model.GetType().GetProperties().Where(m => m.Name == attr.pi.Name + "_ADDR").FirstOrDefault();
                                        // property轉換成expression
                                        var getPropertyValue_ADDR = Expression.Property(target, piADDR);
                                        var expressionADDR = Expression.Lambda<Func<TModel, object>>(getPropertyValue_ADDR, target);

                                        sb.Append(htmlHelper.ZIP_CO_FULLForTurbo(expression, expressionTEXT, expressionADDR, IsReadOnly));
                                    }
                                    
                                    // 建立 ZIP_CO控件
                                    if (attr.Mode == Control.ZIP_CO)
                                    {
                                        var piText = Model.GetType().GetProperties().Where(m => m.Name == attr.pi.Name + "_TEXT").FirstOrDefault();
                                        // property轉換成expression
                                        var getPropertyValue_TEXT = Expression.Property(target, piText);
                                        var expressionTEXT = Expression.Lambda<Func<TModel, object>>(getPropertyValue_TEXT, target);

                                        sb.Append(htmlHelper.ZIP_COForTurbo(expression, expressionTEXT, IsReadOnly));
                                    }

                                    // 建立 e_unit控件
                                    if (attr.Mode == Control.e_unit)
                                    {
                                        var piText = Model.GetType().GetProperties().Where(m => m.Name == attr.pi.Name + "_TEXT").FirstOrDefault();
                                        // property轉換成expression
                                        var getPropertyValue_TEXT = Expression.Property(target, piText);
                                        var expressionTEXT = Expression.Lambda<Func<TModel, object>>(getPropertyValue_TEXT, target);

                                        sb.Append(htmlHelper.e_unitForTurbo(expression, expressionTEXT, IsReadOnly));
                                    }
                                    sb.Append("</div>");
                                }
                            }
                        }


                        sb.Append("</div>");
                    }

                    sb.Append("</div>");
                }



                sb.Append("</div>");
                if (Ca_toggle.ToCount() > 0)
                {
                    sb.Append("</div>");
                    sb.Append("</div>");
                }

                if (blockIDivd != "")
                {
                    sb.Append("</div>");
                }
            }







            return MvcHtmlString.Create(sb.ToString());
        }
    }
}

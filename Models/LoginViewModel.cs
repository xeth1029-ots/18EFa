using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Turbo.MVC.Base3.Commons;
using Turbo.MVC.Base3.Models.Entities;
using Turbo.Commons;
using Turbo.DataLayer;

namespace Turbo.MVC.Base3.Models
{
    public class LoginViewModel
    {
        public LoginViewModel()
        {
            this.form = new LoginFormModel();
        }

        public LoginFormModel form { get; set; }

        public LoginForgetPWDModel forgetPWD { get; set; }


        /// <summary>
        /// 登入失敗的錯誤訊息
        /// </summary>
        public string ErrorMessage { get; set; }
    }

    /// <summary>
    /// 登入表單 Model
    /// </summary>
    public class LoginFormModel
    {
        /// <summary>
        /// 帳號
        /// </summary>
        [Display(Name="帳號")]
        [Required]
        public string UserNo { get; set; }

        /// <summary>
        /// 密碼
        /// </summary>
        [Display(Name = "密碼")]
        [Required]
        public string UserPwd { get; set; }

        /// <summary>
        /// 驗證碼
        /// </summary>
        [Display(Name = "驗證碼")]
        [Required]
        public string ValidateCode { get; set; }
    }

    /// <summary>
    /// 忘記密碼表單(繼承LoginForm) Model
    /// </summary>
    public class LoginForgetPWDModel
    {
        /// <summary>
        /// 帳號
        /// </summary>
        [Display(Name = "帳號")]
        [Required]
        public string user_username { get; set; }

        /// <summary>
        /// 使用者名稱
        /// </summary>
        [Display(Name = "使用者名稱")]
        [Required]
        public string user_name { get; set; }

        /// <summary>
        /// 驗證碼
        /// </summary>
        [Display(Name = "驗證碼")]
        [Required]
        public string ValidateCode { get; set; }

        /// <summary>
        /// 信箱
        /// </summary>
        [Display(Name = "信箱")]
        [Required]
        public string EMAIL { get; set; }
    }

    /// <summary>
    /// 忘記密碼 Model
    /// </summary>
    public class ResetPwdFormModel
    {
        /// <summary>
        /// Token碼
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 帳號
        /// </summary>
        [Display(Name = "帳號")]
        public string user_username { get; set; }

        /// <summary>
        /// 帳號
        /// </summary>
        [Display(Name = "舊有密碼")]
        [Required]
        public string OldUserPwd { get; set; }

        /// <summary>
        /// 帳號
        /// </summary>
        [Display(Name = "新密碼")]
        [Required]
        public string NewUserPwd { get; set; }

        /// <summary>
        /// 密碼
        /// </summary>
        [Display(Name = "再次輸入新密碼")]
        [Required]
        public string NewUserPwd2 { get; set; }
    }
}

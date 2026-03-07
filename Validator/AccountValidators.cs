using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using FluentValidation;
using Turbo.Commons;
using Turbo.MVC.Base3.Commons;
using Turbo.MVC.Base3.Models;
using WDACC.DataLayers;
using WDACC.Models;
using WDACC.Models.Entities;
using WDACC.Models.StoreExt;
using WDACC.Models.ViewModel.Admin;
using WDACC.Models.ViewModel.Facade;
using WDACC.Models.ViewModel.Member;

namespace WDACC.Validator
{
    /// <summary>
    /// 變更密碼超過2個月檢核
    /// </summary>
    public class LoginPasswordValidator : AbstractValidator<ClamUser>
    {
        /// <summary>
        /// 變更密碼超過2個月檢核
        /// </summary>
        public LoginPasswordValidator()
        {
            // 小於2個月
            RuleFor(x => x.pwdupdate).Must(x => x.Value.AddMonths(2) >= DateTime.Now.Date);
        }
    }

    /// <summary>
    /// 密碼規則檢核(長度，英文大小寫，前3次密碼不得相同)  
    /// </summary>
    public class PasswordResetValidator : BaseValidator<PasswordModel>
    {
        /// <summary>
        /// 密碼規則檢核(長度，英文大小寫，前3次密碼不得相同)
        /// </summary>
        public PasswordResetValidator(MyBaseDAO dao, long? mid)
        {
            IList<Store> list = dao.QueryForListAll<Store>("Account.GetLess3Password", mid).Take(3).ToList();  // 起碼會有1筆
            IList<string> last3Pass = list.Select(x => x.Get("password").AsText()).ToList();
            Store lastPwd = list.Count > 1 ? list[1] : null;    // index[0]為目前密碼，index[2]為前次密碼。

            // 近24小時變更檢核
            DateTime afterOneDay = DateTime.Now;
            if (lastPwd != null)
            {
                afterOneDay = lastPwd.Get("date").AsDateTime().Value.AddHours(24);  // +24小時
                RuleFor(x => x).Must(x => DateTime.Now > afterOneDay).WithMessage("一天內僅能變更1次密碼!");
            }

            RuleFor(x => x.OldPassword)
                .NotNull().WithMessage("舊密碼為必填")
                .DependentRules(() =>
                {
                    RuleFor(x => x.OldPassword)
                        .Transform(x => MyCommonUtil.ComputeHash(x))
                        .Equal(last3Pass[0])
                        .WithMessage("舊密碼輸入錯誤");
                });

            RuleFor(x => x.PasswordConfirm)
                .NotNull().WithMessage("密碼確認為必填");

            // 最小長度 & 英文大小寫
            RuleFor(x => x.Password)
                .NotNull()
                .WithMessage("新密碼欄位為必填")
                .Length(12, 16)
                .WithMessage("新密碼長度應為12至16字元")
                .Must(x => base.CheckPasswordChar(x))
                .WithMessage("密碼字元應包含英文大小寫+數字及符號@#$%_的組合")
                .Equal(x => x.PasswordConfirm)
                .WithMessage("2次輸入密碼須相同")
                .DependentRules(() =>
                {
                    // 最近3次密碼不可相同 
                    RuleFor(x => x.Password)
                        .Transform(x => MyCommonUtil.ComputeHash(x))
                        .Must(x => !last3Pass.Contains(x))
                        .WithMessage("近3次密碼不可相同");
                });
        }

    }

    public class AdminPasswordResetValidator : BaseValidator<AdminTeacherModel>
    {
        /// <summary>
        /// 密碼規則檢核(長度，英文大小寫，前3次密碼不得相同)
        /// </summary>
        public AdminPasswordResetValidator(MyBaseDAO dao, long? mid)
        {
            IList<Store> list = dao.QueryForListAll<Store>("Account.GetLess3Password", mid).Take(3).ToList();  // 起碼會有1筆
            IList<string> last3Pass = list.Select(x => x.Get("password").AsText()).ToList();

            RuleFor(x => x.PasswordConfirm)
                .NotNull().WithMessage("密碼確認為必填");

            // 最小長度 & 英文大小寫
            RuleFor(x => x.Password)
                .NotNull()
                .WithMessage("新密碼欄位為必填")
                .Length(12, 16)
                .WithMessage("新密碼長度應為12至16字元")
                .Must(x => base.CheckPasswordChar(x))
                .WithMessage("密碼字元應包含英文大小寫+數字及符號@#$%_的組合")
                .Equal(x => x.PasswordConfirm)
                .WithMessage("2次輸入密碼須相同")
                .DependentRules(() =>
                {
                    // 最近3次密碼不可相同 
                    RuleFor(x => x.Password)
                        .Transform(x => MyCommonUtil.ComputeHash(x))
                        .Must(x => !last3Pass.Contains(x))
                        .WithMessage("近3次密碼不可相同");
                });
        }

        public class AdminMemberValidator : BaseValidator<AdminAccountModel>
        {
            MyBaseDAO dao;
            public AdminMemberValidator(MyBaseDAO dao)
            {
                this.dao = dao;

                RuleFor(x => x.Mem.email).Empty().WithMessage("目前暫時不提供帳號新增功能，若有需要請洽系統管理員。");

                //email (帳號欄位)
                RuleFor(x => x.Mem.email)
                    .NotEmpty().WithMessage("帳號欄位為必要項")
                    .NotNull().WithMessage("帳號欄位為必要")
                    .EmailAddress().WithMessage("帳號不符合email格式")
                    .Must(IncludeAccountKeyword).WithMessage("帳號不符合格式")
                    .Must(x => MemberNotExist(x)).WithMessage("帳號重複");

                RuleFor(x => x.Mem.password)
                    .NotEmpty().WithMessage("密碼欄位為必要項")
                    .NotNull().WithMessage("密碼欄位為必要")
                    .Must(Check_PWD).WithMessage("密碼欄位不符合格式")
                    .MinimumLength(12).WithMessage("密碼欄位 的長度必須大於 12")
                    .Length(11, 22).WithMessage("密碼欄位 的長度有誤"); ;

                RuleFor(x => x.Detail.pid)
                    .NotEmpty().WithMessage("身分證號欄位為必要項")
                    .NotNull().WithMessage("身分證號欄位為必要")
                    .Length(10, 10).WithMessage("身分證號欄位 的長度有誤")
                    .Must(Check_IDNO).WithMessage("身分證號欄位不符合格式");

                RuleFor(x => x.Detail.realname)
                    .NotEmpty().WithMessage("姓名欄位為必要項")
                    .NotNull().WithMessage("姓名欄位為必要")
                    .Length(1, 150).WithMessage("姓名欄位 的長度有誤");

            }

            /// <summary> 身分證號欄位 必須符合格式 </summary>
            /// <param name="s_StrV"></param>
            /// <returns></returns>
            public bool Check_IDNO(string s_StrV)
            {
                if (string.IsNullOrEmpty(s_StrV)) { return false; }
                string c_Eng = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                string c_Num = "0123456789";
                //string c_symbol = @"!@#$%^&*()-_+=\[]{};':"",.<>/?`~";
                if (s_StrV.Length != 10) { return false; }
                bool ck_EU = c_Eng.Contains(s_StrV[0]);
                if (!ck_EU) { return false; }
                int i = 0;
                foreach (char v_C1 in s_StrV)
                {
                    if (i != 0)
                    {
                        bool ck_N = c_Num.Contains(v_C1);
                        if (!ck_N) { return false; }
                    }
                    i += 1;
                }
                return true;
            }

            //密碼-且要有大小寫英文及數字之組合，不可有特殊符號 通過正常 true
            public bool Check_PWD(string s_StrV)
            {
                if (string.IsNullOrEmpty(s_StrV)) { return false; }
                string c_Eng = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                string c_Num = "0123456789";
                string c_symbol = @"!@#$%^&*()-_+=\[]{};':"",.<>/?`~";
                foreach (char v_C1 in s_StrV)
                {
                    bool ck_EU = c_Eng.Contains(v_C1);
                    bool ck_EL = c_Eng.ToLower().Contains(v_C1);
                    bool ck_N = c_Num.Contains(v_C1);
                    bool ck_Sb = c_symbol.Contains(v_C1);
                    if (!ck_EU && !ck_EL && !ck_N && !ck_Sb) { return false; }
                }
                return true;
            }

            /// <summary> Accounts(email) 帳號不符合格式-必須包含@. true </summary>
            /// <param name="email"></param>
            /// <returns></returns>
            public bool IncludeAccountKeyword(string email)
            {
                if (string.IsNullOrEmpty(email)) { return false; }
                bool f_c1 = email.Contains("@");
                bool f_c2 = email.Contains(".");
                return (f_c1 && f_c2);
            }

            /// <summary> email 檢核未重複true </summary>
            /// <param name="email"></param>
            /// <returns></returns>
            public bool MemberNotExist(string email)
            {
                if (string.IsNullOrEmpty(email)) { return true; }

                var res = this.dao.GetRow(new Member { email = email });

                return res == null;
            }

        }

        public class AdminMgrValidator : BaseValidator<AdminMgrModel>
        {
            private MyBaseDAO dao;
            public AdminMgrValidator(MyBaseDAO dao, long? mid)
            {
                ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure;
                this.dao = dao;

                IList<string> last3Pass = new List<string>();
                When(x => x.Form.mid.HasValue, () =>
                {
                    IList<Store> list = dao.QueryForListAll<Store>("Account.GetLess3Password", mid).Take(3).ToList();  // 起碼會有1筆
                    last3Pass = list.Select(x => x.Get("password").AsText()).ToList();
                });

                RuleFor(x => x.Mem.email)
                    .NotNull().WithMessage("帳號欄位為必要項")
                    .Must(x => MemberNotExist(x, mid)).WithMessage("帳號重複");
                // RuleFor(x => x.Mem.email).Must(x => MemberNotExist(x, mid)).WithMessage("帳號重複");

                // 最小長度 & 英文大小寫
                RuleFor(x => x.Mem.password)
                    .NotNull()
                    .WithMessage("密碼欄位為必填")
                    .Equal(x => x.PasswordConfirm)
                    .WithMessage("密碼確認欄位輸入錯誤")
                    .Length(12, 16)
                    .WithMessage("新密碼長度應為12至16字元")
                    .Must(x => base.CheckPasswordChar(x))
                    .WithMessage("密碼字元應包含英文大小寫+數字及符號@#$%_的組合")
                    .DependentRules(() =>
                    {
                        // 最近3次密碼不可相同 
                        RuleFor(x => x.Mem.password)
                            .Transform(x => MyCommonUtil.ComputeHash(x))
                            .Must(x => !last3Pass.Contains(x))
                            .WithMessage("近3次密碼不可相同");
                    });

                RuleFor(x => x.PasswordConfirm)
                    .NotNull().WithMessage("密碼確認為必要項");

                RuleFor(x => x.Form.pid).NotNull().WithMessage("身分證號欄位為必要項");
                RuleFor(x => x.Form.realname).NotNull().WithMessage("姓名欄位為必要項");
            }

            public bool MemberNotExist(string email, long? mid)
            {
                bool result = true;
                if (mid.HasValue)
                {
                    var res = this.dao.GetRowList(new Member { email = email });
                    var item = res.Where(x => x.id != mid).ToList();
                    if (item.Count > 0 || string.IsNullOrEmpty(email))
                    {
                        result = false;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(email))
                    {
                        var res = this.dao.GetRowList(new Member { email = email });
                        if (res.Count > 0 || string.IsNullOrEmpty(email))
                        {
                            result = false;
                        }
                    }
                }
                return result;
            }

        }


    }

    public class ForgetPasswordValidator : BaseValidator<FacadeForgetModel>
    {
        public ForgetPasswordValidator(SessionModel sess)
        {
            ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Email).NotNull().WithMessage("帳號為必要項").DependentRules(() =>
            {
                When(x => x.Email.IndexOf("@") >= 0, () =>
                {
                    RuleFor(x => x.BirthDate).Must(x => x.HasValue).WithMessage("取得登入密碼，生日為必要項");
                });
            });

            RuleFor(x => x.ValidateCode)
                .NotNull().WithMessage("驗證碼為必要項")
                .Equal(x => sess.LoginValidateCode).WithMessage("驗證碼輸入錯誤");
        }
    }

}

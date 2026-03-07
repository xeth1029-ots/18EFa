using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Turbo.DataLayer;
using WDACC.Models.StoreExt;
using WDACC.Models.ViewModel;
using WDACC.Models.Entities;
using Turbo.Commons;
using Turbo.MVC.Base3.Models;
using Turbo.MVC.Base3.Commons;
using WDACC.Validator;
using WDACC.DataLayers;
using WDACC.Commons;
using System.Security.Cryptography;
using log4net;

namespace WDACC.Services
{
    public class AccountService
    {
        private static readonly ILog LOG = LogManager.GetLogger(typeof(AccountService));
        private MyBaseDAO dao;
        private MyModelBinder binder;

        public AccountService(MyBaseDAO dao, MyModelBinder binder)
        {
            this.dao = dao;
            this.binder = binder;
        }

        /// <summary>
        /// 取得登入人員帳號
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public LoginUserInfo GetLoginUser(Models.ViewModel.LoginViewModel model)
        {
            model.Pxssword = MyCommonUtil.ComputeHash(model.Pxssword);

            Store param = new Store();
            param.Collection(model);
            Store res = this.dao.QueryForObject<Store>("Account.GetLoginUser", param);
            Store resLog = this.dao.QueryForObject<Store>("Account.GetLastLoginLogFail", param);

            LoginUserInfo loginUser = new LoginUserInfo();

            bool isLock = false;

            if (res == null)
            {
                loginUser.LoginErrMessage = "帳號或密碼輸入錯誤!!";
                return loginUser;
            }

            int? failcount = res.Get("failcount").AsInt32();

            DateTime? createtime = DateTime.Now;
            if (resLog != null)
            {
                createtime = resLog.Get("createtime").AsDateTime();
            }

            if (failcount.HasValue && failcount.Value >= 3)  // 連續登入錯誤已達3次
            {
                if (DateTime.Now.AddMinutes(-15) <= createtime)  // 少於15分鐘
                {
                    loginUser.LoginSuccess = false;
                    loginUser.LoginErrMessage = "登入錯誤次數已達3次，帳號鎖定15分鐘!";
                    isLock = true;
                }
            }

            // 檢查是否下線
            IList<Store> actRes = this.dao.QueryForList<Store>("KeyMap.ExcuteDeactiveEmail", null);
            string loginemail = res["loginemail"].ToString();
            var matchExcute = actRes.Where(x => x.Get("email").AsText() == loginemail).FirstOrDefault();
            if ((res.Get("active").AsText() == "0" || string.IsNullOrEmpty(res.Get("active").ToString())) && matchExcute == null)  // 判斷是否為下線人員
            {
                loginUser.LoginSuccess = false;
                loginUser.LoginErrMessage = "此帳號已停用，如有疑義請洽系統管理員";
                isLock = true;
            }

            if (!isLock)
            {
                ClamUser user = new ClamUser
                {
                    user_name = res.Get("realname").AsText(),
                    user_id = res.Get("mid").AsInt64(),
                    role = res.Get("role").AsInt32(),
                    pwdupdate = res.Get("pwdupdate").AsDateTime()
                };

                var result = new LoginPasswordValidator().Validate(user);  // 密碼檢核

                loginUser.UserNo = res["loginemail"].ToString();
                loginUser.ChangePwdRequired = !result.IsValid;    // 是否已超過密碼期限,須重新設定密碼
                loginUser.User = user;

                loginUser.LoginSuccess = false;
                //if (HttpContext.Current.Request.IsLocal) { loginUser.LoginSuccess = true; }
                if (res.Get("password").AsText().Equals(model.Pxssword)) { loginUser.LoginSuccess = true; }

                if (!loginUser.LoginSuccess)
                {
                    loginUser.LoginSuccess = false;
                    loginUser.LoginErrMessage = "帳號或密碼輸入錯誤!";
                }
            }

            return loginUser;
        }

        public void SetLoginLog(string username, string logtype, string result)
        {
            HttpRequestBase req = this.binder.controllerContext.RequestContext.HttpContext.Request;
            string userAgent = req.UserAgent;
            userAgent = userAgent.Count() <= 200 ? userAgent : userAgent.Substring(0, 200);
            string ip = req.UserHostAddress;

            Store param = new Store
            {
                ["username"] = username,
                ["logtype"] = logtype,
                ["ip"] = ip,
                ["message"] = result,
                ["useragent"] = userAgent
            };

            this.dao.BeginTransaction();
            try
            {
                this.dao.Insert("Log.SaveLoginLog", param);
                this.dao.CommitTransaction();
            }
            catch (Exception ex)
            {
                LOG.Warn(ex.Message, ex);
                this.dao.RollBackTransaction();
                throw ex;
            }

        }

        /// <summary>
        /// 產生隨機密碼
        /// </summary>
        /// <returns></returns>
        public string CreateRandomPassword()
        {
            string result = null;
            var now = DateTime.Now;
            var seed = now.ToString("yyyyMMddHHmmss");
            var encoding = new System.Text.UTF8Encoding();
            byte[] keyByte = encoding.GetBytes("turbo");
            byte[] messageBytes = encoding.GetBytes(seed);
            using (var hmacSHA256 = new HMACSHA256(keyByte))
            {
                byte[] hashMessage = hmacSHA256.ComputeHash(messageBytes);
                result = BitConverter.ToString(hashMessage).Replace("-", "").ToLower();
            }

            return "A" + result.Substring(0, 10) + "=";
        }

        public void SaveUserFailCount(string emailAccount, byte? value = null)
        {
            Member member = this.dao.GetRow(new Member { email = emailAccount });

            byte failcount = 0;
            failcount = 1;// 預設值
            if (!value.HasValue)  // failcount + 1
            {
                // 無值
                if (member != null && member.failcount.HasValue)
                {
                    failcount = (byte)(member.failcount.Value > 3 ? 4 : member.failcount + 1);
                }
            }
            else
            {
                // 有值-failcount 歸零
                failcount = value.Value;
            }

            if (member == null) { return; }

            this.dao.BeginTransaction();
            try
            {
                this.dao.Update(new Member { failcount = failcount }, new Member { email = emailAccount });
                this.dao.CommitTransaction();
            }
            catch (Exception ex)
            {
                LOG.Warn(ex.Message, ex);
                this.dao.RollBackTransaction();
                throw ex;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Turbo.DataLayer;
using Turbo.MVC.Base3.Models.Entities;
using Omu.ValueInjecter;
using Turbo.MVC.Base3.Services;
using System.Collections;
using Turbo.MVC.Base3.Models;
using System.Text;
using Turbo.MVC.Base3.Areas.A6.Models;
using WDACC.Models.Entities;
using WDACC.DataLayers;
using WDACC.Models;

namespace Turbo.MVC.Base3.DataLayers
{
    /// <summary>
    /// 系統管理Dao
    /// </summary>
    public class A6DAO : BaseDAO
    {
        #region 登入用Func.
        /// <summary>
        /// (AD用)使用者登入帳密檢核, 檢核結果以 LoginUserInfo 返回, 
        /// 應檢查 LoginUserInfo.LoginSuccess 判斷登入是否成功
        /// </summary>
        /// <param name="userNo">使用者ID</param>
        /// <param name="userPwd">使用者登入密碼(明碼)</param>
        /// <returns></returns>
        public LoginUserInfo LoginValidate(string userNo)
        {
            LoginUserInfo userInfo = new LoginUserInfo();
            userInfo.UserNo = userNo;
            userInfo.LoginSuccess = true;

            A6DAO dao = new A6DAO();

            // 取得使用者帳號資料
            e_user userWhere = new e_user();
            userWhere.user_username = userNo;
            var user = dao.GetRow(userWhere);
            // 
            ClamUser dburm = new ClamUser();
            if (user != null)
            {
                dburm.InjectFrom(user);
            }
            // 確認帳號啟用狀態
            if (dburm.user_enabled != "1")
            {
                userInfo.LoginErrMessage = "此帳號尚未啟用 !";
                userInfo.LoginSuccess = false;
            }

            // 確認帳號密碼
            if (dburm.user_username.TONotNullString() == "")
            {
                userInfo.LoginErrMessage = "單登失敗，請檢查是否為以下原因：1.帳號或密碼錯誤  2.尚未在本系統建立單一登入帳號及設置權限 !!";
                userInfo.LoginSuccess = false;
            }

            // 
            if (dburm.errCnt > 5)
            {
                userInfo.LoginErrMessage = "帳號密碼錯誤5次，請聯絡系統管理員解鎖帳號 !!";
                userInfo.LoginSuccess = false;
            }

            if (userInfo.LoginSuccess)
            {
                userInfo.User = dburm;
                userInfo.LoginAuth = "1";

                e_user Where = new e_user();
                Where.user_username = userNo;
                e_user User = new e_user();
                User.errCnt = 0;
                dao.Update(User, Where);
            }
            else
            {
                var eUserErrCnt = 0;
                e_user Where = new e_user();
                Where.user_username = userNo;

                var eUser = dao.GetRowList(Where);
                if (eUser.ToCount() > 0)
                {
                    eUserErrCnt = eUser.FirstOrDefault().errCnt.TOInt32() + 1;
                }
                e_user User = new e_user();
                User.errCnt = eUserErrCnt;
                dao.Update(User, Where);
            }
            return userInfo;
        }


        /// <summary>
        /// 使用者登入帳密檢核, 檢核結果以 LoginUserInfo 返回, 
        /// 應檢查 LoginUserInfo.LoginSuccess 判斷登入是否成功
        /// </summary>
        /// <param name="userNo">使用者ID</param>
        /// <param name="userPwd">使用者登入密碼(明碼)</param>
        /// <returns></returns>
        public LoginUserInfo LoginValidate(string userNo, string userPwd)
        {
            LoginUserInfo userInfo = new LoginUserInfo();
            userInfo.UserNo = userNo;
            userInfo.LoginSuccess = true;

            // A6DAO dao = new A6DAO();
            MyBaseDAO dao = null; // new MyBaseDAO();

            // 取得使用者帳號資料
            Member userWhere = new Member();
            userWhere.email = userNo;
            userWhere.password = Commons.MyCommonUtil.ComputeHash(userPwd);
            var user = dao.GetRow(userWhere);

            // SessionModel.Get().UserInfo.UserNo;

            ClamUser dburm = new ClamUser();
            if (user != null)
            {
                dburm.InjectFrom(user);
            }

            // 確認帳號啟用狀態
            if (dburm.user_enabled != "1")
            {
                userInfo.LoginErrMessage = "此帳號尚未啟用 !";
                userInfo.LoginSuccess = false;
            }

            // 確認帳號密碼
            if (dburm.user_username.TONotNullString() == "")
            {
                userInfo.LoginErrMessage = "帳號或密碼錯誤，請檢查 !!";
                userInfo.LoginSuccess = false;
            }

            // 
            if (dburm.errCnt > 5)
            {
                userInfo.LoginErrMessage = "帳號密碼錯誤5次，請聯絡系統管理員解鎖帳號 !!";
                userInfo.LoginSuccess = false;
            }
            bool NewPwd = false;
            if (CommonsServices.IsMatch(userPwd, "^.*(?=.{10,})(?=.*\\d)(?=.*[A-Z])(?=.*[@#$%^&+=]).*$"))
            {
                NewPwd = true;
            }
            if (CommonsServices.IsMatch(userPwd, "^.*(?=.{10,})(?=.*\\d)(?=.*[A-Z])(?=.*[@#$%^&+=]).*$"))
            {
                NewPwd = true;
            }
            if (CommonsServices.IsMatch(userPwd, "^.*(?=.{10,})(?=.*\\d)(?=.*[a-z])(?=.*[@#$%^&+=]).*$"))
            {
                NewPwd = true;
            }
            if (CommonsServices.IsMatch(userPwd, "^.*(?=.{10,})(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).*$"))
            {
                NewPwd = true;
            }
            if (!NewPwd)
            {
                userInfo.LoginErrMessage = "此密碼未符合規則，請重設密碼(密碼規則需符合包含英文大寫、英文小寫、數字以及特殊字元，4種字元至少包含以上3種，密碼長度10碼以上) !";
                userInfo.LoginSuccess = false;
            }

            if (userInfo.LoginSuccess)
            {
                userInfo.User = dburm;
                userInfo.LoginAuth = "1";

                e_user Where = new e_user();
                Where.user_username = userNo;
                e_user User = new e_user();
                User.errCnt = 0;
                dao.Update(User, Where);
            }
            else
            {
                var eUserErrCnt = 0;
                e_user Where = new e_user();
                Where.user_username = userNo;

                var eUser = dao.GetRowList(Where);
                if (eUser.ToCount() > 0)
                {
                    eUserErrCnt = eUser.FirstOrDefault().errCnt.TOInt32() + 1;
                }
                e_user User = new e_user();
                User.errCnt = eUserErrCnt;
                dao.Update(User, Where);
            }
            return userInfo;
        }


        /// <summary>
        /// 取得角色群組權限功能清單
        /// </summary>
        /// <param name="examKind">檢定類別ID</param>
        /// <param name="role">角色群組ID</param>
        /// <param name="userNo">使用者ID</param>
        /// <param name="netID"></param>
        /// <returns></returns>
        public IList<ClamRoleFunc> GetUserRoleFuncs(string userNo)
        {
            Hashtable parms = new Hashtable();
            parms["USERNO"] = userNo;
            return base.QueryForListAll<ClamRoleFunc>("A6.getClamGroupFuncs", parms);
        }
        #endregion
        
        #region C101M
        /// <summary>
        /// 查詢 C101M
        /// </summary>
        /// <param name="detail"></param>
        public IList<C101MGridModel> QueryC101M(C101MFormModel parms)
        {
            return base.QueryForList<C101MGridModel>("A6.queryC101M", parms);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        public string CheckC101M(C101MDetailModel detail)
        {
            // 僅有新增須檢核
            if (detail.IsNew)
            {
                // 檢核是否有重複PKEY-ID
                e_user where = new e_user();
                where.user_name = detail.user_name;
                where.user_username = detail.user_username;
                var model = base.GetRowList(where);

                if (model.ToCount() > 0) return "姓名、帳號 已重複，請檢查後再行輸入。\n";
            }
            // 僅有修改須檢核
            if (!detail.IsNew)
            {

            }
            // 全部都要檢核項目

            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        public string CheckpowerC101M(C101MDetail1Model detail1)
        {

            // 全部都要檢核項目

            return "";
        }

        /// <summary>
        /// 新增 C101M
        /// </summary>
        /// <param name="detail"></param>
        public void AppendC101MDetail(C101MDetailModel detail)
        {
            SessionModel sm = SessionModel.Get();
            A6DAO dao = new A6DAO();
            var getuser = sm.UserInfo.User.user_id;

            //整批交易管理
            base.BeginTransaction();
            try
            {
                // 抓單位id
                e_unit unit = new e_unit();
                unit.unit_id = detail.unit_id.TOInt32();
                var getid = dao.GetRow(unit);

                // 新增 e_user
                e_user data = new e_user();
                data.InjectFrom(detail);
                data.unit_id = getid.unit_id;
                data.user_org = getid.unit_name;
                data.user_state = "A";      // 先寫死代號不明
                data.user_power = "A";      // 先寫死代號不明                
                data.indate = DateTime.Now;
                data.sys_id = detail.sys_id;

                base.Insert<e_user>(data);

                base.CommitTransaction();
            }
            catch (Exception ex)
            {
                base.RollBackTransaction();
                throw new Exception("AppendC101MDetail failed:" + ex.Message, ex);
            }
        }

        /// <summary>
        /// 更新 C101M
        /// </summary>
        /// <param name="detail"></param>
        public void UpdateC101MDetail(C101MDetailModel detail)
        {
            //A6DAO dao = new A6DAO();
            //SessionModel sm = SessionModel.Get();
            //var getuser = sm.UserInfo.User.user_id;

            ////整批交易管理
            //base.BeginTransaction();
            //try
            //{
            //    //更新 e_user 資料表
            //    e_user where = new e_user();
            //    where.user_username = detail.user_username;
            //    where.user_name = detail.user_name;

            //    e_user newdata = new e_user();
            //    newdata.sys_id = detail.sys_id;
            //    newdata.user_enabled = detail.user_enabled;
            //    newdata.mtime = DateTime.Now;
            //    newdata.muser = getuser;
            //    newdata.unit_id = detail.unit_id.TOInt32();

            //    base.Update(newdata, where);
            //    base.CommitTransaction();
            //}
            //catch (Exception ex)
            //{
            //    base.RollBackTransaction();
            //    throw new Exception("UpdateC101MDetail failed:" + ex.Message, ex);
            //}
        }

        /// <summary>
        /// 解鎖 C101M
        /// </summary>
        /// <param name="detail"></param>
        public void UnLockC101MDetail(C101MDetailModel detail)
        {
            //A6DAO dao = new A6DAO();
            //SessionModel sm = SessionModel.Get();
            //var getuser = sm.UserInfo.User.user_id;

            ////整批交易管理
            //base.BeginTransaction();
            //try
            //{
            //    //更新 e_user 資料表
            //    e_user where = new e_user();
            //    where.user_username = detail.user_username;
            //    where.user_name = detail.user_name;

            //    e_user newdata = new e_user();
            //    newdata.errCnt = 0;
            //    newdata.mtime = DateTime.Now;
            //    newdata.muser = getuser;

            //    base.Update(newdata, where);
            //    base.CommitTransaction();
            //}
            //catch (Exception ex)
            //{
            //    base.RollBackTransaction();
            //    throw new Exception("UnLockC101MDetail failed:" + ex.Message, ex);
            //}
        }

        /// <summary>
        /// 刪除 C101M
        /// </summary>
        /// <param name="detail"></param>
        public void DeleteC101MDetail(C101MDetailModel detail)
        {
            //整批交易管理
            base.BeginTransaction();
            try
            {
                // 刪除 e_user
                A6DAO dao = new A6DAO();

                e_user where = new e_user();
                where.user_name = detail.user_name;
                where.user_username = detail.user_username;
                base.Delete<e_user>(where);

                base.CommitTransaction();
            }
            catch (Exception ex)
            {
                base.RollBackTransaction();
                throw new Exception("DeleteC101MDetail failed:" + ex.Message, ex);
            }
        }


        /// <summary>
        /// 新增 C101M
        /// </summary>
        /// <param name="detail"></param>
        public void AppendpowerC101MDetail(C101MDetail1Model detail1)
        {
            //SessionModel sm = SessionModel.Get();

            //// 取得登入者ID
            //var userid = sm.UserInfo.User.user_id;

            ////整批交易管理
            //base.BeginTransaction();

            //try
            //{
            //    A6DAO dao = new A6DAO();

            //    // 功能選單
            //    var func = ApplicationModel.GetClamFuncsAll();

            //    // 先刪除該使用者所有權限
            //    e_permission deldata = new e_permission();
            //    deldata.userid = detail1.getid;
            //    base.Delete<e_permission>(deldata);

            //    // 新增權限
              
            //    var a6 = detail1.sys_a6_SHOW;
            //    foreach (var item in a6)
            //    {
            //        e_permission data = new e_permission();
            //        data.funcid = item;
            //        data.data = func.Where(m => m.PRGID == item).FirstOrDefault().PRGNAME;
            //        data.userid = detail1.getid;
            //        data.mtime = DateTime.Now;
            //        data.muser = userid;

            //        base.Insert<e_permission>(data);
            //    }

            //    base.CommitTransaction();
            //}
            //catch (Exception ex)
            //{
            //    base.RollBackTransaction();
            //    throw new Exception("AppendpowerC101MDetail failed:" + ex.Message, ex);
            //}
        }
        #endregion

        #region C102M
        /// <summary>
        /// 查詢 C102M
        /// </summary>
        /// <param name="detail"></param>
        public IList<C102MGridModel> QueryC102M(C102MFormModel parms)
        {
            return base.QueryForList<C102MGridModel>("A6.queryC102M", parms);
        }
        #endregion
    }
}

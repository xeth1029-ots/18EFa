using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Turbo.MVC.Base3.DataLayers;
using Turbo.MVC.Base3.Models.Entities;
using Turbo.MVC.Base3.Services;
using WDACC.Models;

namespace Turbo.MVC.Base3.Models
{
    /// <summary>
    /// 提供跨Session共用資料的存取類
    /// </summary>
    public class ApplicationModel: ApplicationBaseModel
    {
        #region Private Methods
        private static object _lock = new object();
        private static ApplicationModel _instance = null;

        private ApplicationModel()
        {

        }


        private static ApplicationModel GetInstance()
        {
            lock (_lock)
            {
                if(_instance == null)
                {
                    _instance = new ApplicationModel();
                }
                return _instance;
            }
        }
        #endregion

        #region 取得系統清單

        /// <summary>
        /// 取得系統最外層清單(沒有PRGID)
        /// </summary>
        /// <returns></returns>
        public static IList<AMFUNCM> GetClamFuncsOutAll()
        {
            const string _KEY = "TblCLAMFUNCMAll";
            ApplicationModel model = GetInstance();
            object value = model.GetApplicationVar(_KEY);

            if (value != null && value is IList<AMFUNCM>)
            {
                // 已存在, 直接返回
                return (IList<AMFUNCM>)value;
            }
            else
            {
                // 不存在或過期, 從DB中載入
                BaseDAO dao = new BaseDAO();
                // 載出所有程式代碼(顯示在清單的)
                IList<AMFUNCM> list = dao.QueryForListAll<AMFUNCM>("A6.getClamFuncsOutAll", null);

                // 將 list 儲存至 ApplictionModel 中
                // 並設定有效時間至系統時間當天的 23:59:59 
                // (也就是隔天的 00:00:00 失效)
                DateTime now = DateTime.Now;
                now = now.AddDays(1);
                DateTime expire = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);

                model.SetApplicationVar(_KEY, list, expire);
                return list;
            }
        }

        /// <summary>
        /// 取得系統已啟用的全部功能清單(已排序)
        /// </summary>
        /// <returns></returns>
        //public static IList<TblAMFUNCM> GetClamFuncsAll()
        //{
        //    const string _KEY = "TblCLAMFUNCM";
        //    ApplicationModel model = GetInstance();
        //    object value = model.GetApplicationVar(_KEY);

        //    if (value != null && value is IList<TblAMFUNCM>)
        //    {
        //        // 已存在, 直接返回
        //        return (IList<TblAMFUNCM>)value;
        //    }
        //    else
        //    {
        //        // 不存在或過期, 從DB中載入
        //        BaseDAO dao = new BaseDAO();
        //        // 載出所有程式代碼(顯示在清單的)
        //        IList<TblAMFUNCM> list = dao.QueryForListAll<TblAMFUNCM>("AM.getClamFuncsAll", null);

        //        // 將 list 儲存至 ApplictionModel 中
        //        // 並設定有效時間至系統時間當天的 23:59:59 
        //        // (也就是隔天的 00:00:00 失效)
        //        DateTime now = DateTime.Now;
        //        now = now.AddDays(1);
        //        DateTime expire = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);

        //        model.SetApplicationVar(_KEY, list, expire);
        //        return list;
        //    }
        //}

        #endregion

        #region 取得路徑抬頭

        /// <summary>
        /// 組合路徑_主要
        /// </summary>
        /// <returns></returns>
        public static string GetHeader()
        {
            SessionModel sm = SessionModel.Get();
            var dao = new BaseDAO();
            var FUNCMSTR = "首頁";

            if (sm.LastActionFunc != null)
            {
                var SYSID = sm.LastActionFunc.SYSID;
                var MODULES = sm.LastActionFunc.MODULES;
                var SUBMODULES = sm.LastActionFunc.SUBMODULES;
                var PRGID = sm.LastActionFunc.PRGID;

                // 第一層
                FUNCMSTR = GetFUNCM(SYSID).PRGNAME;
                // 第二層(Y/N)
                FUNCMSTR += MODULES.TONotNullString().Trim() != "" ? "＞" + GetFUNCM(SYSID, MODULES).PRGNAME : "";
                // 第三層(Y/N)
                FUNCMSTR += SUBMODULES.TONotNullString().Trim() != "" ? "＞" + GetFUNCM(SYSID, MODULES, SUBMODULES).PRGNAME : "";
                // 程式名稱(Y/N)
                FUNCMSTR += "＞" + sm.LastActionFunc.PRGNAME;
            }
            return FUNCMSTR;
        }

        /// <summary>
        /// 取得程式代碼相關物件
        /// </summary>
        /// <param name="SYSID"></param>
        /// <returns></returns>
        public static AMFUNCM GetFUNCM(string SYSID, string MODULES = " ", string SUBMODULES = " ", string PRGID = " ")
        {
            SessionModel sm = SessionModel.Get();
            AMFUNCM where = new AMFUNCM();
            where.SYSID = SYSID;
            where.MODULES = MODULES;
            where.SUBMODULES = SUBMODULES;
            where.PRGID = " ";
            var dao = new BaseDAO();
            var AMFUNCM = dao.GetRow(where);

            return AMFUNCM;
        }
        #endregion

        ///// <summary>
        ///// 從資料庫取得最新已啟用的功能表項目清單，並將功能表項目清單放入系統快取
        ///// </summary>
        ///// <param name="key">資料快取鍵值</param>
        ///// <param name="model">應用程式共用資料 Model 物件</param>
        ///// <returns></returns>
        //private static IList<ClamRoleFunc> getFuncModules(string key, ApplicationModel model)
        //{
        //    // 取出舊的功能表項目清單集合
        //    var oldList = model.GetApplicationVar(key) as IList<ClamRoleFunc>;

        //    // 不存在或過期, 從DB中載入
        //    ClamDAO dao = new ClamDAO();
        //    IList<ClamRoleFunc> list = dao.getClamFuncModules();

        //    // 將 list 儲存至 ApplictionModel 中
        //    // 並設定有效時間至系統時間當天的 23:59:59 
        //    // (也就是隔天的 00:00:00 失效)
        //    DateTime nextDt = DateTime.Now.AddDays(1);
        //    DateTime expire = new DateTime(nextDt.Year, nextDt.Month, nextDt.Day, 0, 0, 0);

        //    model.SetApplicationVar(key, list, expire);
        //    if (oldList != null)
        //    {
        //        oldList.Clear();
        //        oldList = null;
        //    }

        //    return list;
        //}

        ///// <summary>
        ///// 取得系統已啟用的全部功能清單模組
        ///// </summary>
        ///// <param name="forceNew">（非必要）指示是否一律取得最新資料。(true: 一律取得最新資料，false: 從系統快取獲得)</param>
        ///// <returns></returns>
        //public static IList<ClamRoleFunc> getFuncModules(bool forceNew = false)
        //{
        //    const string _KEY = "ClamFuncModules";
        //    ApplicationModel model = GetInstance();

        //    //20180116 增加「一律取得最新資料」判斷程式碼
        //    if (forceNew)
        //    {
        //        return getFuncModules(_KEY, model);
        //    }
        //    else
        //    {
        //        var value = model.GetApplicationVar(_KEY) as IList<ClamRoleFunc>;
        //        return (value != null) ? value : getFuncModules(_KEY, model);
        //    }
        //}

        /// <summary>
        /// 取得系統已啟用的全部功能清單(已排序)
        /// </summary>
        /// <returns></returns>
        public static IList<AMFUNCM> GetClamFuncsAll()
        {
            const string _KEY = "TblCLAMFUNCM";
            ApplicationModel model = GetInstance();
            object value = model.GetApplicationVar(_KEY);

            if (value != null && value is IList<AMFUNCM>)
            {
                // 已存在, 直接返回
                return (IList<AMFUNCM>)value;
            }
            else
            {
                // 不存在或過期, 從DB中載入
                BaseDAO dao = new BaseDAO();
                // 載出所有程式代碼(顯示在清單的)
                IList<AMFUNCM> list = dao.QueryForListAll<AMFUNCM>("A6.getClamFuncsAll", null);

                // 將 list 儲存至 ApplictionModel 中
                // 並設定有效時間至系統時間當天的 23:59:59 
                // (也就是隔天的 00:00:00 失效)
                DateTime now = DateTime.Now;
                now = now.AddDays(1);
                DateTime expire = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);

                model.SetApplicationVar(_KEY, list, expire);
                return list;
            }
        }


    }
}

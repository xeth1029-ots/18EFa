using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Script.Serialization;
using log4net;
using Newtonsoft.Json;
using WDACC.Commons;
using WDACC.DataLayers;
using WDACC.Models;
using WDACC.Models.Entities;

namespace WDACC.Services
{
    public class LogService
    {
        protected static readonly ILog LOG =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private MyBaseDAO dao;
        private LogCollection logCollection;

        public LogService(MyBaseDAO dao, LogCollection logCollection)
        {
            this.dao = dao;
            this.logCollection = logCollection;
        }

        public void WriteFileLogWithCollection()
        {
            this.dao.BeginTransaction();
            try
            {
                var lc = this.logCollection;
                SessionModel sess = SessionModel.Get();

                foreach (var item in lc.FileCollection)
                {
                    string func = string.Empty;
                    if (lc.AdminAction.HasValue)
                    {
                        func = lc.AdminAction.Value.ToString();
                    }
                    else if (lc.MemberAction.HasValue)
                    {
                        func = lc.MemberAction.Value.ToString();
                    }
                    else if (lc.FileAction.HasValue)
                    {
                        func = lc.FileAction.Value.ToString();
                    }
                    if (string.IsNullOrEmpty(func)) { continue; }

                    string logtype = item.EditMode.ToString();
                    string result = item.EditStatus.ToString();

                    FileLog log = new FileLog
                    {
                        logfunc = func,
                        logtype = logtype,
                        filename = item.FileName,
                        result = result,
                        message = item.Message,
                        createtime = DateTime.Now,
                        username = sess.UserInfo.UserNo,
                    };

                    this.dao.Insert(log, true);
                }

                this.dao.CommitTransaction();
            }
            catch (Exception ex)
            {
                LOG.Error(ex.Message, ex);
                this.dao.RollBackTransaction();
                throw ex;
            }
        }

        /// <summary>
        /// 新增系統操作log
        /// </summary>
        public void WriteFuncLogWithCollection()
        {
            this.dao.BeginTransaction();
            try
            {
                var lc = this.logCollection;
                SessionModel sess = SessionModel.Get();

                foreach (var item in lc.FuncCollection)
                {
                    string func = string.Empty;
                    if (lc.AdminAction.HasValue)
                    {
                        func = lc.AdminAction.Value.ToString();
                    }
                    else if (lc.MemberAction.HasValue)
                    {
                        func = lc.MemberAction.Value.ToString();
                    }
                    if (string.IsNullOrEmpty(func)) { continue; }

                    string logtype = item.EditMode.ToString();
                    string arg = new LogPresenter().ToString(item);
                    if (string.IsNullOrEmpty(arg)) { continue; }

                    FuncLog log = new FuncLog
                    {
                        logid = lc.LogId,
                        result = item.EditStatus.ToString(),
                        logfunc = func,
                        logtype = logtype,
                        arg = arg,
                        username = sess.UserInfo.UserNo,
                        createtime = DateTime.Now,
                    };
                    this.dao.Insert(log, true);
                }

                this.dao.CommitTransaction();
            }
            catch (Exception ex)
            {
                LOG.Error(ex.Message, ex);
                this.dao.RollBackTransaction();
                throw ex;
            }
        }

        public void WriteLogWithCollection()
        {
            this.WriteFuncLogWithCollection();
            this.WriteFileLogWithCollection();
        }

    }

    public class LogPresenter
    {
        public string ToString(FuncCollectionModel cm)
        {
            string result = string.Empty;
            IList<Tuple<string, string>> fieldList = this.GetFieldDisplayName(cm.Arg);
            IList<string> subList = new List<string>();

            foreach (var item in fieldList)
            {
                string subContent = string.Format("{0}:{1}", item.Item1, item.Item2);
                subList.Add(subContent);
            }
            result = string.Join("、", subList);

            return result;
        }

        /// <summary>
        /// 取得Entity model上的DisplayName 屬性
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>Item1: 顯示名稱，顯示值 </returns>
        public IList<Tuple<string, string>> GetFieldDisplayName(object obj)
        {
            List<Tuple<string, string>> list = new List<Tuple<string, string>>();

            foreach (var props in obj.GetType().GetProperties())
            {
                var dn = props.GetCustomAttribute(typeof(DisplayNameAttribute));
                if (dn != null)
                {
                    string name = ((DisplayNameAttribute)dn).DisplayName;
                    string value = string.Empty;
                    if (props.GetValue(obj) != null)
                    {
                        if (props.DeclaringType == typeof(DateTime?))
                        {
                            value = ((DateTime?)props.GetValue(obj)).Value.ToString("yyyy/MM/dd HH:mm:ss");
                        }
                        else
                        {
                            value = props.GetValue(obj).ToString();
                        }
                    }
                    list.Add(new Tuple<string, string>(name, value));
                }
            }

            return list;
        }

    }

}

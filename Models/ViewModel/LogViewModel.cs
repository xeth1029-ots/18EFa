using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Turbo.DataLayer;
using WDACC.Models.StoreExt;

namespace WDACC.Models.ViewModel
{
    public class LogViewModel
    {
        public LogViewModel()
        {
            this.LoginLogForm = new LoginLogForm();
            this.FileLogForm = new FileLogForm();
            this.FuncLogForm = new FuncLogForm();
        }

        public LoginLogForm LoginLogForm { get; set; }
        public FileLogForm FileLogForm { get; set; }
        public FuncLogForm FuncLogForm { get; set; }
        public IList<Store> LoginGrid { get; set; }
        public IList<Store> FileGrid { get; set; }
        public IList<Store> FuncGrid { get; set; }

    }

    public class LoginLogForm : PagingResultsViewModel
    {
        public LoginLogForm()
        {
            this.StartDateTw = new TwDate();
            this.EndDateTw = new TwDate();
            this.ExpYear1 = $"{DateTime.Now.AddMonths(-1).Year}";
            this.ExpMonth1 = $"{DateTime.Now.AddMonths(-1).Month}";
        }

        /// <summary>
        /// 查詢區間-起
        /// </summary>
        public TwDate StartDateTw { get; set; }

        /// <summary>
        /// 查詢區間-迄
        /// </summary>
        public TwDate EndDateTw { get; set; }

        /// <summary>
        /// 查詢區間-起
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 查詢區間-迄
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 類型
        /// </summary>
        public string LogType { get; set; }

        /// <summary>
        /// 帳號
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 匯出年月
        /// </summary>
        public string ExpYearMonth1 { get; set; }
        /// <summary>
        /// 匯出年
        /// </summary>
        public string ExpYear1 { get; set; }
        /// <summary>
        /// 匯出月
        /// </summary>
        public string ExpMonth1 { get; set; }
    }

    public class FileLogForm : PagingResultsViewModel
    {
        public FileLogForm()
        {
            this.StartDateTw = new TwDate();
            this.EndDateTw = new TwDate();
        }

        public TwDate StartDateTw { get; set; }

        public TwDate EndDateTw { get; set; }

        /// <summary>
        /// 查詢區間-起
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 查詢區間-迄
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 類型
        /// </summary>
        public string LogType { get; set; }

        /// <summary>
        /// 帳號
        /// </summary>
        public string UserName { get; set; }

    }

    public class FuncLogForm : PagingResultsViewModel
    {
        public FuncLogForm()
        {
            this.StartDateTw = new TwDate();
            this.EndDateTw = new TwDate();
        }

        public TwDate StartDateTw { get; set; }

        public TwDate EndDateTw { get; set; }

        /// <summary>
        /// 查詢區間-起
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 查詢區間-迄
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 功能項目
        /// </summary>
        public string LogFunc { get; set; }

        /// <summary>
        /// 操作類型
        /// </summary>
        public string LogType { get; set; }

        /// <summary>
        /// 帳號
        /// </summary>
        public string UserName { get; set; }

    }

}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Turbo.DataLayer;
using Turbo.MVC.Base3.Models;
using Turbo.MVC.Base3.Models.Entities;
using Turbo.MVC.Base3.Services;
using WDACC.Models;

namespace Turbo.MVC.Base3.DataLayers
{
    /// <summary>
    /// 原本 BaseDAO 的內容, 已拉到 Turbo.DataLayer.RowBaseDAO,
    /// 保留這個class以維持原有程式相容性
    /// </summary>
    public class BaseDAO : RowBaseDAO
    {
        /// <summary>
        /// 以預設的 SqlMap config 連接資料庫
        /// </summary>
        public BaseDAO() : base()
        {
            base.PageSize = ConfigModel.DefaultPageSize;
        }

        /// <summary>
        /// 以指定的 SqlMap config 連接資料庫
        /// </summary>
        /// <param name="sqlMapConfig"></param>
        public BaseDAO(string sqlMapConfig)
            : base(sqlMapConfig)
        {
            base.PageSize = ConfigModel.DefaultPageSize;
        }

        public override IList<T> QueryForList<T>(String statementId, Object arg)
        {
            // 找尋
            var result = base.QueryForListAll<T>(statementId, arg);
            var results = result;
            // 進行分頁處理
            return PagingList<T>(statementId, results);
        }

        public override IList<T> QueryForListAll<T>(String statementId, Object arg, Boolean pagingCount = false)
        {
            // 找尋
            var result = base.QueryForListAll<T>(statementId, arg);
            var results = result;
            // 進行分頁處理
            return results;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Turbo.DataLayer;

namespace Turbo.MVC.Base3.Commons
{
    /// <summary>
    /// 系統代碼及表格名稱列舉
    /// </summary>
    public partial class StaticCodeMap
    {
        /// <summary>
        /// 系統表格名稱列舉
        /// </summary>
        public class TableName
        {
            /// <summary>
            /// 
            /// </summary>
            public static DBRowTableName __MigrationHistory = DBRowTableName.Instance("__MigrationHistory");

            /// <summary>
            /// 
            /// </summary>
            public static DBRowTableName AMFUNCM = DBRowTableName.Instance("AMFUNCM");
         
            /// <summary>
            /// 
            /// </summary>
            public static DBRowTableName DECITYTOWN = DBRowTableName.Instance("DECITYTOWN");

            /// <summary>
            /// 
            /// </summary>
            public static DBRowTableName PWDTOKEN = DBRowTableName.Instance("PWDTOKEN");

            /// <summary>
            /// 
            /// </summary>
            public static DBRowTableName e_permission = DBRowTableName.Instance("e_permission");

            /// <summary>
            /// 
            /// </summary>
            public static DBRowTableName e_unit = DBRowTableName.Instance("e_unit");

            /// <summary>
            /// 
            /// </summary>
            public static DBRowTableName e_user = DBRowTableName.Instance("e_user");

            /// <summary>
            /// 
            /// </summary>
            public static DBRowTableName e_userlog = DBRowTableName.Instance("e_userlog");
            
            /// <summary>
            /// 郵遞區號代碼檔
            /// </summary>
            public static DBRowTableName ZIP_CO = DBRowTableName.Instance("ZIP_CO");

            /// <summary>
            /// 附件上傳
            /// </summary>
            public static DBRowTableName FILEGRID = DBRowTableName.Instance("FILEGRID");
            
        }

    }

}
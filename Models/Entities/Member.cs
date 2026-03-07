using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Turbo.DataLayer;

namespace WDACC.Models.Entities
{
    public class Member : IDBRow
    {
        /// <summary>
        /// 序號 ref MemDetail.mid
        /// </summary>
        [IdentityDBField]
        public long? id { get; set; }

        /// <summary>
        /// 使用者名稱
        /// </summary>
        public string username { get; set; }

        /// <summary>
        /// 密碼
        /// </summary>
        public string password { get; set; }

        /// <summary>
        /// 密碼更新日期
        /// </summary>
        public DateTime? pwdupdate { get; set; }

        /// <summary>OWASP,角X色 0:非教師, 2:教師可能,3:（未使用）,4:（未使用）</summary>
        public short? ROPLE { get; set; }

        /// <summary>
        /// 登入失敗次數
        /// </summary>
        public byte? failcount { get; set; }

        /// <summary>
        /// 電子郵件
        /// </summary>
        public string email { get; set; }

        public DBRowTableName GetTableName()
        {
            return DBRowTableName.Instance("member");
        }

    }
}

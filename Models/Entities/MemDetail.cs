using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Turbo.DataLayer;

namespace WDACC.Models.Entities
{
    /// <summary> 教師資料 </summary>
    public class MemDetail : IDBRow
    {
        /// <summary> 序號 </summary>
        [IdentityDBField]
        public long? id { get; set; }
        /// <summary>member id </summary>
        public long? mid { get; set; }
        /// <summary>
        /// 身分證號欄位為必要項
        /// </summary>
        public string pid { get; set; }
        /// <summary>
        /// 姓名欄位為必要項
        /// </summary>
        public string realname { get; set; }
        public string nickname { get; set; }
        public string gender { get; set; }
        /// <summary>出年年月日</summary>
        public DateTime? birthdat { get; set; }
        public Int32? zipcode { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public string mobile { get; set; }
        public string fax { get; set; }
        public string website { get; set; }
        /// <summary>電子郵件</summary>
        public string email { get; set; }
        public string jemail { get; set; }
        public string facebook { get; set; }
        public Int16? degree { get; set; }
        public string school { get; set; }
        public string major { get; set; }
        public string skill { get; set; }
        public string jobcomp { get; set; }
        public string jobtitle { get; set; }
        public string teachunit { get; set; }
        public Int16? group { get; set; }
        public string acadetype { get; set; }
        public string history { get; set; }
        /// <summary>
        /// 1:性別,2:生日,3:電話,4:手機,5:傳真,6:EMail,7:EMail(工作),8:聯絡地址,9:個人網站,10:FaceBook
        /// </summary>
        public string showoff { get; set; }
        /// <summary>
        /// 1:在線,0:下線,null(不知道),2(不知道)
        /// </summary>
        public long? active { get; set; }        
        /// <summary>加入年份</summary>
        public Int32? regyear { get; set; }

        /// <summary>下線原因</summary>
        public string offreason { get; set; }
        /// <summary>下線日期</summary>
        public DateTime? offdate { get; set; }
        /// <summary>不顯示此資訊</summary>
        public string noshow { get; set; }
        public DBRowTableName GetTableName()
        {
            return DBRowTableName.Instance("memdetail");
        }

    }
}

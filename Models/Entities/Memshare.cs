using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using Turbo.DataLayer;

namespace WDACC.Models.Entities
{
    public class Memshare : IDBRow
    {
        [IdentityDBField]
        public long? id { get; set; }

        [DisplayName("講師識別碼")]
        public long? mid { get; set; }

        [DisplayName("教材")]
        public string filename { get; set; }

        [DisplayName("說明書")]
        public string ifilename { get; set; }

        [DisplayName("授權書")]
        public string afilename { get; set; }

        [DisplayName("標題")]
        public string title { get; set; }

        [DisplayName("內容")]
        public string content { get; set; }

        public byte? classid { get; set; }

        [DisplayName("審核狀態")]
        public Int16? @checked { get; set; }

        [DisplayName("建立日期")]
        public DateTime? date { get; set; }

        [DisplayName("審核意見")]
        public string reason { get; set; }

        public Int16? dlok { get; set; }

        public DBRowTableName GetTableName()
        {
            return DBRowTableName.Instance("memshare");
        }
    }
}

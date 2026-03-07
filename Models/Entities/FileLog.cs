using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Turbo.DataLayer;

namespace WDACC.Models.Entities
{
    public class FileLog : IDBRow
    {
        [IdentityDBField]
        public long? id { get; set; }
        public string username { get; set; }
        public string logtype { get; set; }
        public DateTime? createtime { get; set; }
        public string logfunc { get; set; }
        public string result { get; set; }
        public string filename { get; set; }
        public string message { get; set; }

        public DBRowTableName GetTableName()
        {
            return DBRowTableName.Instance("filelog");
        }
    }
}

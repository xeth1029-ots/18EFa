using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Turbo.DataLayer;

namespace WDACC.Models.Entities
{
    public class FuncLog : IDBRow
    {
        [IdentityDBField]
        public long? id { get; set; }

        public string logid { get; set; }

        public string username { get; set; }

        public string logtype { get; set; }

        public string logfunc { get; set; }

        public DateTime? createtime { get; set; }

        public string arg { get; set; }

        public string result { get; set; }

        public DBRowTableName GetTableName()
        {
            return DBRowTableName.Instance("funclog");
        }
    }
}

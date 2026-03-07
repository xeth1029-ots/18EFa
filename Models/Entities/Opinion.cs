using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Turbo.DataLayer;

namespace WDACC.Models.Entities
{
    public class Opinion : IDBRow
    {
        [IdentityDBField]
        public long? id { get; set; }
        public long? mid { get; set; }
        public byte? @class { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
        public string content { get; set; }
        public DateTime? date { get; set; }
        public byte? answer { get; set; }
        public DateTime? rdate { get; set; }
        public string response { get; set; }

        public DBRowTableName GetTableName()
        {
            return DBRowTableName.Instance("opinion");
        }
    }
}

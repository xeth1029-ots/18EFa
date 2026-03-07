using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Turbo.DataLayer;

namespace WDACC.Models.Entities
{
    public class Meeting : IDBRow
    {
        /// <summary>
        /// 序號
        /// </summary>
        [IdentityDBField]
        public long? id { get; set; }
        public DateTime? date { get; set; }

        [NotDBField]
        public TimeSpan? time { get; set; }
        public string metname { get; set; }
        public string metloc { get; set; }
        public byte? mclass { get; set; }

        [NotDBField]
        public TimeSpan? etime { get; set; }
        public DateTime? starttime { get; set; }
        public DateTime? endtime { get; set; }

        public DBRowTableName GetTableName()
        {
            return DBRowTableName.Instance("meeting");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Turbo.DataLayer;

namespace WDACC.Models.Entities
{
    public class Metdetail : IDBRow
    {
        /// <summary>
        /// 序號
        /// </summary>
        [IdentityDBField]
        public long? id { get; set; }
        public long? mid { get; set; }
        public long? score { get; set; }
        public long? metid { get; set; }
        public byte? mcont { get; set; }

        public DBRowTableName GetTableName()
        {
            return DBRowTableName.Instance("metdetail");
        }
    }
}

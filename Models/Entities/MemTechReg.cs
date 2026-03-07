using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Turbo.DataLayer;

namespace WDACC.Models.Entities
{
    public class MemTechReg : IDBRow
    {
        [IdentityDBField]
        public long? id { get; set; }

        public long? mid { get; set; }

        /// <summary>
        /// 授課區域
        /// </summary>
        public long? cityid { get; set; }

        public DBRowTableName GetTableName()
        {
            return DBRowTableName.Instance("memteachreg");
        }
    }
}

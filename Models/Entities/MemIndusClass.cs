using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Turbo.DataLayer;
using WDACC.DataLayers;

namespace WDACC.Models.Entities
{
    public class MemIndusClass : IDBRow
    {
        [IdentityDBField]
        public long? id { get; set; }
        public long? mid { get; set; }
        public long? indscid { get; set; }
        public byte? @order { get; set; }

        public DBRowTableName GetTableName()
        {
            return DBRowTableName.Instance("memindusclass");
        }

        public void Insert(MyBaseDAO dao)
        {
            dao.Insert("DBTable.MemIndusClass_Insert", this);
        }

    }
}

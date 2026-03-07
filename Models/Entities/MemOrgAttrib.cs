using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Turbo.DataLayer;

namespace WDACC.Models.Entities
{
    public class MemOrgAttrib : IDBRow
    {
        [IdentityDBField]
        public long? id { get; set; }

        public long? mid { get; set; }

        public long? attrid { get; set; }

        public DBRowTableName GetTableName()
        {
            return DBRowTableName.Instance("memorgattrib");
        }
    }
}

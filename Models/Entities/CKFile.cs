using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Turbo.DataLayer;

namespace WDACC.Models.Entities
{
    public class CKFile : IDBRow
    {
        [IdentityDBField]
        public long? id { get; set; }

        public long? mid { get; set; }

        public long? mmsid { get; set; }

        public string filname { get; set; }

        // public string tfilname { get; set; }

        public DBRowTableName GetTableName()
        {
            return DBRowTableName.Instance("ckfile");
        }
    }
}

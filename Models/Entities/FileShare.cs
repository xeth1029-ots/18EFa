using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Turbo.DataLayer;

namespace WDACC.Models.Entities
{
    public class FileShare : IDBRow
    {
        public long? mfid { get; set; }

        public long? mid { get; set; }

        public DBRowTableName GetTableName()
        {
            return DBRowTableName.Instance("fileshare");
        }
    }
}

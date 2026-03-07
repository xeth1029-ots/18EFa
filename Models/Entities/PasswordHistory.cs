using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Turbo.DataLayer;

namespace WDACC.Models.Entities
{
    public class PasswordHistory : IDBRow
    {
        [IdentityDBField]
        public long? id { get; set; }
        public long? mid { get; set; }
        public string password { get; set; }
        public DateTime? modifydate { get; set; }

        public DBRowTableName GetTableName()
        {
            return DBRowTableName.Instance("passwordhistory");
        }
    }
}

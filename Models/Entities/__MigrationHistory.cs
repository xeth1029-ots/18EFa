using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.DataLayer;
using Turbo.MVC.Base3.Commons;

namespace Turbo.MVC.Base3.Models.Entities
{
    public class __MigrationHistory : IDBRow
    {
        public string MigrationId
        {
            get;
            set;
        }
        public string ContextKey
        {
            get;
            set;
        }
        public string Model
        {
            get;
            set;
        }
        public string ProductVersion
        {
            get;
            set;
        }

        public DBRowTableName GetTableName()
        {
            return StaticCodeMap.TableName.__MigrationHistory;
        }
    }
}
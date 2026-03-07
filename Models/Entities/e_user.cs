using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.DataLayer;
using Turbo.MVC.Base3.Commons;

namespace Turbo.MVC.Base3.Models.Entities
{
    public class e_user : IDBRow
    {
        [IdentityDBField]
        public long? user_id
        {
            get;
            set;
        }
        public int? unit_id
        {
            get;
            set;
        }
        public string user_name
        {
            get;
            set;
        }
        public string user_org
        {
            get;
            set;
        }
        public string user_username
        {
            get;
            set;
        }
        public string user_password
        {
            get;
            set;
        }
        public string user_state
        {
            get;
            set;
        }
        public string user_power
        {
            get;
            set;
        }
        public DateTime? indate
        {
            get;
            set;
        }
        public string user_orgcode
        {
            get;
            set;
        }
        public string user_enabled
        {
            get;
            set;
        }
        public string sys_id
        {
            get;
            set;
        }
        public int? muser
        {
            get;
            set;
        }
        public DateTime? mtime
        {
            get;
            set;
        }

        public int? errCnt
        {
            get;
            set;
        }

        public DBRowTableName GetTableName()
        {
            return StaticCodeMap.TableName.e_user;
        }
    }
}

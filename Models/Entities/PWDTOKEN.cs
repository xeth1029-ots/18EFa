using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.DataLayer;
using Turbo.MVC.Base3.Commons;

namespace Turbo.MVC.Base3.Models.Entities
{
    public class PWDTOKEN : IDBRow
    {
        public string user_username
        {
            get;
            set;
        }

        public string token
        {
            get;
            set;
        }
        public DateTime? Validate
        {
            get;
            set;
        }
        public string enable
        {
            get;
            set;
        }
        public string Password
        {
            get;
            set;
        }


        public DBRowTableName GetTableName()
        {
            return StaticCodeMap.TableName.PWDTOKEN;
        }
    }
}
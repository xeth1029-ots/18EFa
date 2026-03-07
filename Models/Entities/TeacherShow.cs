using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using Turbo.DataLayer;

namespace WDACC.Models.Entities
{
    public class TeacherShow : IDBRow
    {
        [IdentityDBField]
        public long? id { get; set; }

        [DisplayName("講師識別碼")]
        public long? mid { get; set; }

        [DisplayName("資料類型")]
        public Int16? @class { get; set; }

        [DisplayName("圖片連結文字")]
        public string content { get; set; }

        [DisplayName("圖片連結說明")]
        public string title { get; set; }

        [DisplayName("備註")]
        public string info { get; set; }

        public DBRowTableName GetTableName()
        {
            return DBRowTableName.Instance("teachershow");
        }
    }
}

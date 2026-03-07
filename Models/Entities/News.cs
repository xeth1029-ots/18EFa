using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Turbo.DataLayer;

namespace WDACC.Models.Entities
{
    public class News : DBRowModel, IDBRow, IDBRowOper
    {
        /// <summary>
        /// 序號
        /// </summary>
        [IdentityDBField]
        public long? id { get; set; }

        public DateTime? sdate { get; set; }

        public DateTime? edate { get; set; }

        public string title { get; set; }

        public string content { get; set; }

        public Int16? @class { get; set; }

        public Byte? fordownload { get; set; }

        public Byte? showoff { get; set; }

        public string ishot { get; set; }

        public DBRowTableName GetTableName()
        {
            return DBRowTableName.Instance("news");
        }

    }
}

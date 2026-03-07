using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Turbo.DataLayer;

namespace WDACC.Models.Entities
{
    public class NewsFile : IDBRow
    {
        /// <summary>
        /// 序號
        /// </summary>
        [IdentityDBField]
        public long? id { get; set; }

        public string filename { get; set; }

        public string comment { get; set; }

        public long? news_id { get; set; }

        public Int16? forder { get; set; }

        public DBRowTableName GetTableName()
        {
            return DBRowTableName.Instance("newsfile");
        }

    }
}

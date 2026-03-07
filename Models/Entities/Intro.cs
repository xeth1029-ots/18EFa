using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Turbo.DataLayer;

namespace WDACC.Models.Entities
{
    public class Intro : IDBRow
    {
        public Int16? id { get; set; }

        public string content { get; set; }

        public DBRowTableName GetTableName()
        {
            return DBRowTableName.Instance("intro");
        }
    }
}

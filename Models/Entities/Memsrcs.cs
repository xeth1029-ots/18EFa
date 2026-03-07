using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using Turbo.DataLayer;

namespace WDACC.Models.Entities
{
    /// <summary>
    /// 取得老師基本資料清單
    /// </summary>
    public class Memsrcs : IDBRow
    {
        [IdentityDBField]
        public long? id { get; set; }

        [DisplayName("講師識別碼")]
        public long? mid { get; set; }

        [DisplayName("訓練單位類別")]
        public byte? tuclass { get; set; }

        [DisplayName("單位名稱")]
        public string tuname { get; set; }

        [DisplayName("課程類別")]
        public byte? tcclass { get; set; }

        [DisplayName("課程名稱")]
        public string tcname { get; set; }

        [DisplayName("開始日期時間")]
        public DateTime? sdate { get; set; }

        [DisplayName("結束日期時間")]
        public DateTime? edate { get; set; }

        [DisplayName("授課天數")]
        public byte? days { get; set; }

        [DisplayName("時數")]
        public byte? hours { get; set; }

        [DisplayName("是否顯示")]
        public byte? showoff { get; set; }

        [DisplayName("審核狀態")]
        public byte? ckeckd { get; set; }

        [DisplayName("審核意見")]
        public string comment { get; set; }

        public string govplan { get; set; }

        public DBRowTableName GetTableName()
        {
            return DBRowTableName.Instance("memsrcs");
        }

    }
}

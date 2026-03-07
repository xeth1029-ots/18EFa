using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using Turbo.DataLayer;

namespace WDACC.Models.Entities
{
    public class TeaSurvey : IDBRow
    {
        [IdentityDBField]
        public long? id { get; set; }
        /// <summary>
        /// 需求單位名稱
        /// </summary>
        [DisplayName("需求單位名稱(必要的)")]
        public string indname { get; set; }
        /// <summary>
        /// 單位聯絡人
        /// </summary>
        [DisplayName("單位聯絡人(必要的)")]
        public string indcontact { get; set; }
        /// <summary>
        /// 單位聯絡電話
        /// </summary>
        [DisplayName("單位聯絡電話(必要的)")]
        public string indphone { get; set; }
        /// <summary>
        /// 單位聯絡電子信箱
        /// </summary>
        [DisplayName("單位聯絡電子信箱(必要的)")]
        public string indemail { get; set; }
        /// <summary>
        /// 講師需求人數
        /// </summary>
        [DisplayName("講師需求人數(必要的，數字)")]
        public Int16? teano { get; set; }
        /// <summary>
        /// 學員人數
        /// </summary>
        [DisplayName("學員人數(必要的，數字)")]
        public long? stuno { get; set; }
        public DateTime? stime { get; set; }
        public DateTime? etime { get; set; }
        /// <summary>
        /// 授課總時數
        /// </summary>
        [DisplayName("授課總時數(必要的，數字)")]
        public Int16? couhours { get; set; }
        /// <summary>
        /// 授課時間說明
        /// </summary>
        [DisplayName("授課時間說明(必要的)")]
        public string coutime { get; set; }
        /// <summary>
        /// 授課地點
        /// </summary>
        [DisplayName("授課地點(必要的)")]
        public string couplace { get; set; }
        /// <summary>
        /// 需求課程類別
        /// </summary>
        [DisplayName("需求課程類別(必要的)")]
        public string couclass { get; set; }
        /// <summary>
        /// 需求課程產業屬性
        /// </summary>
        [DisplayName("需求課程產業屬性(必要的)")]
        public string indclass1 { get; set; }
        /// <summary>
        /// 講師待遇
        /// </summary>
        [DisplayName("講師待遇")]
        public string salary { get; set; }
        public DateTime? fdate { get; set; }
        
        /// <summary>其他補充說明</summary>
        [DisplayName("其他補充說明")]
        public string indinfo { get; set; }
        public Int16? confirm { get; set; }
        public DateTime? aodate { get; set; }
        public Int32? clicks { get; set; }

        public DBRowTableName GetTableName()
        {
            return DBRowTableName.Instance("teasurvey");
        }
    }

}

using Turbo.MVC.Base3.Commons;
using Turbo.DataLayer;
using System;
using System.ComponentModel.DataAnnotations;

namespace Turbo.MVC.Base3.Models.Entities
{

    /// <summary>
    /// 郵遞區號代碼檔
    /// </summary>
    public class TblZIP_CO : IDBRow
    {
        /// <summary>
        /// 郵遞區號代碼
        /// </summary>
        [StringLength(3)]
        public string ZIP_CO { get; set; }

        /// <summary>
        /// 郵遞區號名稱
        /// </summary>
        [StringLength(12)]
        public string ZIP_NM { get; set; }

        /// <summary>
        /// 郵遞分區群組旗標
        /// </summary>
        [StringLength(2)]
        public string ZIPGRP { get; set; }

        public DBRowTableName GetTableName()
        {
            return StaticCodeMap.TableName.ZIP_CO;
        }

    }
}
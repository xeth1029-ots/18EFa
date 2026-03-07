using Turbo.MVC.Base3.Commons;
using Turbo.DataLayer;
using System;
using System.ComponentModel.DataAnnotations;

namespace Turbo.MVC.Base3.Models.Entities
{

    /// <summary>
    /// 附加檔案(實體)
    /// </summary>
    public class TblFILEGRID : IDBRow
	{
        /// <summary>
        /// ID
        /// </summary>
        [Display(Name = "ID")]
        [IdentityDBField]
        public string FILEID { get; set; }

        /// <summary>
        /// 檔案名稱
        /// </summary>
        [Display(Name = "檔案名稱")]
        public string FILENAME { get; set; }

        /// <summary>
        /// 檔案路徑
        /// </summary>
        [Display(Name = "檔案路徑")]
        public string FILEPATH { get; set; }

        /// <summary>
        /// 檔案大小
        /// </summary>
        [Display(Name = "檔案大小")]
        public string FILECAPACTIY { get; set; }

        /// <summary>
        /// Pkey1
        /// </summary>
        [Display(Name = "Pkey1")]
        public string FILEPKEY1 { get; set; }

        /// <summary>
        /// Pkey2
        /// </summary>
        [Display(Name = "Pkey2")]
        public string FILEPKEY2 { get; set; }

        /// <summary>
        /// Pkey3
        /// </summary>
        [Display(Name = "Pkey3")]
        public string FILEPKEY3 { get; set; }

        /// <summary>
        /// MODUSERID 異動者帳號
        /// <summary>
        [Display(Name = "異動者帳號")]
        public string MODUSERID { get; set; }

        /// <summary>
        /// MODUSERNAME 異動者姓名
        /// <summary>
        [Display(Name = "異動者姓名")]
        public string MODUSERNAME { get; set; }

        /// <summary>
        /// MODIP 異動ip
        /// <summary>
        [Display(Name = "異動ip")]
        public string MODIP { get; set; }

        /// <summary>
        /// MODTIME 異動時間
        /// <summary>
        [Display(Name = "異動時間")]
        public string MODTIME { get; set; }

        public DBRowTableName GetTableName()
		{
			return StaticCodeMap.TableName.FILEGRID;
		}

	}
}
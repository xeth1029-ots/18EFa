using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Turbo.MVC.Base3.Models.Entities;

namespace Turbo.MVC.Base3.Models
{
    /// <summary>
    /// 使用者群組
    /// </summary>
    public class ClamUser : e_user
    {
        /// <summary>
        /// 最後變更密碼時間
        /// </summary>
        public DateTime? pwdupdate { get; set; }

        /// <summary>
        /// 使用者角色
        /// </summary>
        public int? role { get; set; }

    }
}

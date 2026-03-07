using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Turbo.MVC.Base3.Models
{
    public class ReturnModel
    {
        /// <summary>
        /// 執行成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 回傳訊息
        /// </summary>
        public string Message { get; set; }
    }
}
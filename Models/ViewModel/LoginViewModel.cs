using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WDACC.Commons;
using WDACC.Models.StoreExt;

namespace WDACC.Models.ViewModel
{
    public class LoginViewModel
    {
        // private LogCollection log;

        public LoginViewModel()
        {
            //this.log = log;
        }
        //public LoginViewModel(LogCollection log)
        //{
        //    this.log = log;
        //}
        // public string PxsswordConfirm { get; set; }

        public string UserName { get; set; }
        public string Pxssword { get; set; }
        public string ValidateCode { get; set; }
        public string ErrorMessage { get; set; }

        public Store Grid { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WDACC.Models.ViewModel.Member
{
    public class PasswordModel
    {
        public string OldPassword { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
    }
}

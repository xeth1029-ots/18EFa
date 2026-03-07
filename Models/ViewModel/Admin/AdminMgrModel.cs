using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WDACC.Models.Entities;
using WDACC.Models.StoreExt;

namespace WDACC.Models.ViewModel.Admin
{
    public class AdminMgrModel
    {
        public WDACC.Models.Entities.Member Mem { get; set; }

        public MemDetail Form { get; set; }

        public string PasswordConfirm { get; set; }

        public IList<Store> Grid { get; set; }

    }
}

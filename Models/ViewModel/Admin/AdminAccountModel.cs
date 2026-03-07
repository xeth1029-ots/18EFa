using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WDACC.Models.Entities;

namespace WDACC.Models.ViewModel.Admin
{
    public class AdminAccountModel
    {
        public WDACC.Models.Entities.Member Mem { get; set; }

        public MemDetail Detail { get; set; }

        public void PrepareAfterLoad()
        {
            // NOTHING
        }

        public void PrepareBeforeSave()
        {
            if (this.Mem != null && this.Detail != null)
            {
                this.Detail.email = this.Mem.email;
            }
        }

    }
}

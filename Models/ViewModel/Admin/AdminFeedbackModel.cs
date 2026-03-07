using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WDACC.Models.Entities;
using WDACC.Models.StoreExt;

namespace WDACC.Models.ViewModel.Admin
{
    public class AdminFeedbackModel
    {

        public Opinion OpinionItem { get; set; }

        public string RealName { get; set; }

        public IList<Store> Grid { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WDACC.Models.Entities;

namespace WDACC.Models.ViewModel.Member
{
    public class FeedbackModel
    {
        public Opinion Opinion { get; set; }
        public bool IsAgree { get; set; }
    }
}

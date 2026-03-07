using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WDACC.Models.Entities;

namespace WDACC.Models.ViewModel.Admin
{
    public class AdminSystemModel
    {
        public IList<Intro> IntroList { get; set; }

        public Intro Detail { get; set; }

    }
}

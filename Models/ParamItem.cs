using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Turbo.MVC.Base3.Models
{
    public class ParamItem
    {

        private string name;
        private string value;

        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

    }
}
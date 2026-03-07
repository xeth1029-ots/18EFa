using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Turbo.Commons;

namespace WDACC.Models
{

    public class TwDate
    {
        private DateTime? date;

        public DateTime? DATE
        {
            get { return this.date; }
            set { this.date = value; }
        }

        public string DATE_TW
        {
            get
            {
                string res = string.Empty;
                if (this.date.HasValue)
                {
                    res = HelperUtil.TransToTwYear(this.date);
                }
                return res;
            }
        }

        public string DATE_AD
        {
            get
            {
                string res = string.Empty;
                if (this.date.HasValue)
                {
                    res = this.date.Value.ToString("yyyy/MM/dd");
                }
                return res;
            }
            set
            {
                DateTime d = DateTime.MaxValue;
                if (DateTime.TryParse(value, out d))
                {
                    this.date = d;
                }
            }
        }

        public string Label { get; set; }

        // public bool IncludeKO { get; set; }

        public void SetDate(DateTime? date)
        {
            this.date = date;
        }

        public DateTime? GetDate()
        {
            return this.date;
        }

    }

}

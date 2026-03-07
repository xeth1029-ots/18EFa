using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WDACC.Models
{
    public class CouClassField
    {
        public bool IsDc { get; set; }
        public bool IsBc { get; set; }
        public bool IsKc { get; set; }

        public void SetValue(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (value.Contains("DC"))
                {
                    IsDc = true;
                }
                if (value.Contains("BC"))
                {
                    IsBc = true;
                }
                if (value.Contains("KC"))
                {
                    IsKc = true;
                }
            }
        }

        public string GetValue()
        {
            List<string> res = new List<string>();
            if (IsDc)
            {
                res.Add("DC");
            }
            if (IsBc)
            {
                res.Add("BC");
            }
            if (IsKc)
            {
                res.Add("KC");
            }
            return string.Join(",", res);
        }

    }
}

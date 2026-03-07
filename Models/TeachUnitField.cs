using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WDACC.Models
{
    public class TeachUnitField
    {
        public bool IsDc { get; set; }
        public bool IsBc { get; set; }
        public bool IsKc { get; set; }

        public void SetValue(string value)
        {
            if (value != null && value.Length == 3)
            {
                if (value[0] == '1')
                {
                    IsDc = true;
                }
                if (value[1] == '1')
                {
                    IsBc = true;
                }
                if (value[2] == '1')
                {
                    IsKc = true;
                }
            }
        }

        public string GetValue()
        {
            string[] res = new string[] { "0", "0", "0" };
            if (IsDc)
            {
                res[0] = "1";
            }
            if (IsBc)
            {
                res[1] = "1";
            }
            if (IsKc)
            {
                res[2] = "1";
            }
            return string.Join(",", res);
        }

    }
}

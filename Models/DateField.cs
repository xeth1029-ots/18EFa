using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WDACC.Models
{
    public class DateField
    {
        public int? Year { get; set; }

        public int? Month { get; set; }

        public int? Day { get; set; }

        public DateTime GetDate()
        {
            return new DateTime(Year.Value, Month.Value, Day.Value);
        }

        public void SetDate(DateTime? date)
        {
            if (date.HasValue)
            {
                this.Year = date.Value.Year;
                this.Month = date.Value.Month;
                this.Day = date.Value.Day;
            }
        }

    }

    public class DateTimeField : DateField
    {
        public int Hour { get; set; }
        public int Minute { get; set; }

        public new DateTime GetDate()
        {
            return new DateTime(Year.Value, Month.Value, Day.Value, Hour, Minute, 0);
        }

        public new void SetDate(DateTime? date)
        {
            if (date.HasValue)
            {
                this.Year = date.Value.Year;
                this.Month = date.Value.Month;
                this.Day = date.Value.Day;
                this.Hour = date.Value.Hour;
                this.Minute = date.Value.Minute;
            }
            else
            {
                this.Year = DateTime.Now.Year;
                this.Month = 1;
                this.Day = 1;
                this.Hour = 0;
                this.Minute = 0;
            }
        }
    }



}

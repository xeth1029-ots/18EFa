using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WDACC.Models
{
    public class TimeField
    {
        public int? Hour { get; set; }
        public int? Minute { get; set; }

        public void SetTime(TimeSpan time)
        {
            this.Hour = time.Hours;
            this.Minute = time.Minutes;
        }

        public void SetTime(DateTime? time)
        {
            if (time.HasValue)
            {
                this.Hour = time.Value.Hour;
                this.Minute = time.Value.Minute;
            }
        }

        public TimeSpan GetTime()
        {
            return new TimeSpan(Hour.Value, Minute.Value, 0);
        }

    }
}

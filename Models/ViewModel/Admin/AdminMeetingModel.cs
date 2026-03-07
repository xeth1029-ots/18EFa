using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WDACC.Models.Entities;
using WDACC.Models.StoreExt;
using WDACC.Commons;
using Turbo.MVC.Base3.Services;

namespace WDACC.Models.ViewModel.Admin
{
    public class AdminMeetingModel
    {
        public AdminMeetingModel()
        {
            this.StartTime = new TimeField();
            this.EndTime = new TimeField();
        }

        public int? Year { get; set; }

        /// <summary>
        /// 參與課程積分 
        /// </summary>
        public long? MeetScore { get; set; }

        /// <summary>
        /// 分享課程積分 
        /// </summary>
        public long? SharedScore { get; set; }

        public Meeting Meet { get; set; }
        public int? MeetYear { get; set; }
        public int? MeetMonth { get; set; }
        public int? MeetDay { get; set; }
        public TimeField StartTime { get; set; }
        public TimeField EndTime { get; set; }

        /// <summary>
        /// 參與人員
        /// </summary>
        public IList<AdminMeetMemberModel> MetDetailList { get; set; }

        /// <summary>
        /// 分享人員 
        /// </summary>
        public IList<AdminMeetMemberModel> SharedList { get; set; }

        /// <summary>
        /// 儲存 參與人員
        /// </summary>
        public IList<string> SaveDetailList { get; set; }

        /// <summary>
        /// 儲存 分享人員
        /// </summary>
        public IList<string> SaveShareList { get; set; }


        public void PrepareAfterLoad()
        {
            if (this.Meet != null)
            {
                if (Meet.date.HasValue)
                {
                    this.MeetYear = Meet.date.Value.Year;
                    this.MeetMonth = Meet.date.Value.Month;
                    this.MeetDay = Meet.date.Value.Day;
                }

                if (Meet.time.HasValue)
                {
                    this.StartTime.SetTime(Meet.time.Value);
                }

                if (Meet.time.HasValue)
                {
                    this.EndTime.SetTime(Meet.etime.Value);
                }

                if (Meet.starttime.HasValue)
                {
                    this.StartTime.SetTime(Meet.starttime);
                }

                if (Meet.endtime.HasValue)
                {
                    this.EndTime.SetTime(Meet.endtime);
                }
            }

            if (this.MetDetailList != null && this.MetDetailList.Count > 0)
            {
                this.MeetScore = this.MetDetailList[0].Score.TOInt64();
            }

            if (this.SharedList != null && this.SharedList.Count > 0)
            {
                this.SharedScore = this.SharedList[0].Score.TOInt64();
            }
        }

        public void PrepareBeforeSave()
        {
            if (this.Meet != null)
            {
                if (this.MeetYear.HasValue && this.MeetMonth.HasValue && this.MeetDay.HasValue)
                {
                    this.Meet.date = new DateTime(this.MeetYear.Value, this.MeetMonth.Value, this.MeetDay.Value);
                }

                Meet.starttime = new DateTime(1900, 1, 1, this.StartTime.Hour.Value, this.StartTime.Minute.Value, 0);   // 年月日欄位無作用
                Meet.endtime = new DateTime(1900, 1, 1, this.EndTime.Hour.Value, this.EndTime.Minute.Value, 0);         // 年月日欄位無作用
            }
        }

    }

    public class AdminMeetMemberModel
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public string Score { get; set; }

    }

}

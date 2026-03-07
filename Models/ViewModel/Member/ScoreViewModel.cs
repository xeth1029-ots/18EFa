using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WDACC.Models.StoreExt;

namespace WDACC.Models.ViewModel.Member
{
    public class ScoreViewModel
    {
        public int Year { get; set; }

        /// <summary>
        /// 授課推廣積分達標
        /// </summary>
        public bool IsTeachReach { get; set; }

        /// <summary>
        /// 社群互動達標
        /// </summary>
        public bool IsCommunityReach { get; set; }

        /// <summary>
        /// 教材分享達標
        /// </summary>
        public bool IsShareReach { get; set; }

        /// <summary>
        /// 是否列入觀察清單
        /// </summary>
        public bool IsInWatch { get; set; }

        /// <summary>
        /// 年度總積分
        /// </summary>
        public long TotalScore { get; set; }

        /// <summary>
        /// 時數:訓練單位
        /// </summary>
        public int TeachHourClass1 { get; set; }

        /// <summary>
        /// 時數:職訓單位
        /// </summary>
        public int TeachHourClass2 { get; set; }

        /// <summary>
        /// 時數:學校
        /// </summary>
        public int TeachHourClass3 { get; set; }

        /// <summary>
        /// 時數:企業
        /// </summary>
        public int TeachHourClass4 { get; set; }

        /// <summary>
        /// 時數:其它
        /// </summary>
        public int TeachHourClass5 { get; set; }

        /// <summary>
        /// 積分: 訓練單位
        /// </summary>
        public int TeachScoreClass1 { get; set; }

        /// <summary>
        /// 積分:職訓機構
        /// </summary>
        public int TeachScoreClass2 { get; set; }

        /// <summary>
        /// 積分:學校
        /// </summary>
        public int TeachScoreClass3 { get; set; }

        /// <summary>
        /// 積分:企業
        /// </summary>
        public int TeachScoreClass4 { get; set; }

        /// <summary>
        /// 積分:其它 
        /// </summary>
        public int TeachScoreClass5 { get; set; }

        /// <summary>
        /// 積分:社群分享講師
        /// </summary>
        public long TeachScoreCommunitySpeaker { get; set; }

        /// <summary>
        /// 積分:發展署會議講師
        /// </summary>
        public long TeachScoreSessionSpeaker { get; set; }

        /// <summary>
        /// 授課推廣積分加總
        /// </summary>
        public long TeachTotalScore { get; set; }

        /// <summary>
        /// 社群互動積分
        /// </summary>
        public IList<Store> CommunityScoreList { get; set; }

        /// <summary>
        /// 社群互動積分加總
        /// </summary>
        public long CommunityScoreTotal { get; set; }

        /// <summary>
        /// 教材分享篇數
        /// </summary>
        public long ShareCount { get; set; }

        /// <summary>
        /// 教材分享積分
        /// </summary>
        public long ShareScore { get; set; }

        /// <summary>
        /// 教材被下載數量
        /// </summary>
        public long DownloadCount { get; set; }

        /// <summary>
        /// 教材被下載積分
        /// </summary>
        public long DownloadScore { get; set; }

    }

}

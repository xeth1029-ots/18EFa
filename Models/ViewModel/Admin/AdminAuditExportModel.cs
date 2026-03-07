using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WDACC.Models.StoreExt;

namespace WDACC.Models.ViewModel.Admin
{
    public class AdminAuditExportModel
    {
        public AdminAuditExportModel()
        {
            this.ScoreAuditGrid = new SqlMapModel
            {
                StatementId = () =>
                {
                    return this.QueryKind == "STATISTIC" ? "Admin.ExportScoreAuditStatistic" : "Admin.ExportScoreAuditList";
                },
                Parameter = () => { return new Store { { "year", this.Year } }; }
            };
            this.FileAuditGrid = new SqlMapModel
            {
                StatementId = () =>
                {
                    return this.QueryKind == "STATISTIC" ? "Admin.ExportFileAuditStatistic" : "Admin.ExportFileAuditList";
                },
                Parameter = () => { return new Store { { "year", this.Year } }; }
            };
        }

        /// <summary>
        /// 查詢年度
        /// </summary>
        public int? Year { get; set; }

        /// <summary>
        /// 查詢類型， 統計 | 清單
        /// </summary>
        public string QueryKind { get; set; }   // STATISTIC | LIST

        public SqlMapModel FileAuditGrid { get; set; }

        public SqlMapModel ScoreAuditGrid { get; set; }

    }
}

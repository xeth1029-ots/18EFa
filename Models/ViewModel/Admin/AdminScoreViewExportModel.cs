using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WDACC.Models.StoreExt;

namespace WDACC.Models.ViewModel.Admin
{
    public class AdminScoreViewExportModel
    {
        public AdminScoreViewExportModel()
        {
            this.Setup();
        }

        public int? Year { get; set; }

        public SqlMapModel Grid { get; set; }

        private void Setup()
        {
            this.Grid = new SqlMapModel
            {
                StatementId = () =>
                {
                    string result = string.Empty;
                    if (this.Year >= 2015)
                    {
                        result = "Admin.GetScoreList2015Export";
                    }
                    else
                    {
                        result = "Admin.GetScoreList2014";
                    }
                    return result;
                },
                Parameter = () =>
                {
                    var store = new Store();
                    store["year"] = this.Year;
                    return store;
                },
            };

        }

    }
}

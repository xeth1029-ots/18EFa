using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Turbo.DataLayer;
using Turbo.MVC.Base3.Areas.SHARE.Models;
using Turbo.MVC.Base3.Models.Entities;
using Omu.ValueInjecter;
using Turbo.MVC.Base3.Services;
using System.Collections;
using Turbo.MVC.Base3.Models;
//using Turbo.MVC.Base3.Areas.SHARE.Models;
using System.Configuration;
using System.Data.SqlClient;

namespace Turbo.MVC.Base3.DataLayers
{
    public class SHAREDAO : BaseDAO
    {
        #region ZIP_CO
        /// <summary>
        /// 查詢 ZIP_CO
        /// </summary>
        /// <param name="detail"></param>
        public IList<ZIP_COGridModel> QueryZIP_CO(ZIP_COFormModel parms)
        {
            return base.QueryForList<ZIP_COGridModel>("SHARE.queryZIP_CO", parms);
        }
        #endregion

        #region e_unit
        /// <summary>
        /// 查詢 e_unit
        /// </summary>
        /// <param name="detail"></param>
        public IList<e_unitGridModel> Querye_unit(e_unitFormModel parms)
        {
            return base.QueryForList<e_unitGridModel>("SHARE.querye_unit", parms);
        }

        #endregion

        #region e_user
        /// <summary>
        /// 查詢 e_unit
        /// </summary>
        /// <param name="detail"></param>
        public IList<e_userGridModel> Querye_user(e_userFormModel parms)
        {
            return base.QueryForList<e_userGridModel>("SHARE.querye_user", parms);
        }

        #endregion
    }
}

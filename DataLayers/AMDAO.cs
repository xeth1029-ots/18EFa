using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Turbo.MVC.Base3.Models;
using Turbo.MVC.Base3.Models.Entities;
using Turbo.MVC.Base3.Services;

namespace Turbo.MVC.Base3.DataLayers
{
    public class AMDAO : BaseDAO
    {
        /// <summary>
        /// 檔案下載
        /// </summary>
        /// <param name="id"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public TblFILEGRID QueryFileid(Int32? id, string func)
        {
            TblFILEGRID where = new TblFILEGRID();
            where.FILEPKEY2 = id.TONotNullString();
            where.FILEPKEY1 = func;
            var data = this.GetRow(where);
            return data;
        }
    }
}
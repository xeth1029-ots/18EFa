using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Turbo.Commons;

namespace Turbo.MVC.Base3.Commons
{
    /// <summary>
    /// 系統代碼及表格名稱列舉
    /// </summary>
    public partial class StaticCodeMap
    {
        /// <summary>
        /// 代碼表類別列舉清單, 叫用 KeyMapDAO.GetCodeMapList() 所需的參數
        /// </summary>
        public class CodeMap : CodeMapType
        {
            #region 私有(隱藏) CodeMap 建構式

            private CodeMap(string codeName) : base(codeName)
            { }

            /// <summary>
            ///
            /// </summary>
            /// <param name="codeName"></param>
            /// <param name="sqlStatementId"></param>
            private CodeMap(string codeName, string sqlStatementId) :
                base(codeName, sqlStatementId)
            { }

            #endregion 私有(隱藏) CodeMap 建構式

            // 單位清單
            //public static CodeMapType e_unit = new CodeMapType("e_unit", "KeyMap.gete_unit");

            //單位清單
            //public static CodeMapType e_unit_AB = new CodeMapType("e_unit_AB", "KeyMap.gete_unit_AB");

            // 功能清單A6 A6/C101M
            //public static CodeMapType amfuncm_a6 = new CodeMapType("amfuncm_a6", "KeyMap.amfuncm_a6");

            // 功能清單 A6/C102M
            //public static CodeMapType functionname = new CodeMapType("functionname", "KeyMap.functionname");

            // 使用方法 A6/C102M
            //public static CodeMapType methodname = new CodeMapType("methodname", "KeyMap.methodname");
        }
    }
}
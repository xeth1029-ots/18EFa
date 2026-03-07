using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Turbo.DataLayer;
using Turbo.MVC.Base3.DataLayers;
using WDACC.Commons;
using WDACC.Models.StoreExt;
using WDACC.Services;

namespace WDACC.DataLayers
{
    public class MyBaseDAO : RowBaseDAO
    {
        private LogCollection logCollection;

        public MyBaseDAO(LogCollection collection)
        {
            this.logCollection = collection;
        }

        public override IList<T> QueryForListAll<T>(String statementId, Object arg, Boolean pagingCount = false)
        {
            return base.QueryForListAll<T>(statementId, arg);
        }

        public override IList<T> QueryForList<T>(string statementId, object arg)
        {
            return base.QueryForList<T>(statementId, arg);
        }

        public new int Update<T>(T newModel, T whereConds, ClearFieldMap clearFields = null, bool disableExecuteTracer = false) where T : IDBRow
        {
            int count = 0;
            try
            {
                count = base.Update(newModel, whereConds, clearFields, disableExecuteTracer);
                if (!disableExecuteTracer)
                {
                    this.logCollection.AddFuncCollection
                        (ActionName.EditMode.UPDATE, ActionName.EditStatus.SUCCESS, newModel, whereConds);
                }
            }
            catch (Exception ex)
            {
                if (!disableExecuteTracer)
                {
                    this.logCollection.AddFuncCollection
                        (ActionName.EditMode.UPDATE, ActionName.EditStatus.FAIL, newModel, whereConds);
                }
                throw ex;
            }

            return count;
        }

        public new Int32 Insert<T>(T param, bool disableExecuteTracer = false) where T : IDBRow
        {
            int sn = 0;

            try
            {
                sn = base.Insert(param, disableExecuteTracer);

                if (!disableExecuteTracer)
                {
                    this.logCollection.AddFuncCollection
                        (ActionName.EditMode.CREATE, ActionName.EditStatus.SUCCESS, param, null);
                }
            }
            catch (Exception ex)
            {
                if (!disableExecuteTracer)
                {
                    this.logCollection.AddFuncCollection
                        (ActionName.EditMode.CREATE, ActionName.EditStatus.FAIL, param, null);
                }
                throw ex;
            }

            return sn;
        }

        public new int Delete<T>(T param, bool disableExecuteTracer = false) where T : IDBRow
        {
            int count = 0;
            try
            {
                count = base.Delete(param, disableExecuteTracer);

                if (!disableExecuteTracer)
                {
                    this.logCollection.AddFuncCollection
                        (ActionName.EditMode.DELETE, ActionName.EditStatus.SUCCESS, param, null);
                }
            }
            catch (Exception ex)
            {
                if (!disableExecuteTracer)
                {
                    this.logCollection.AddFuncCollection
                        (ActionName.EditMode.DELETE, ActionName.EditStatus.FAIL, param, null);
                }
                throw ex;
            }

            return count;
        }

        /// <summary>
        /// 取得網站特定功能項目說明
        /// </summary>
        /// <param name="funID"></param>
        /// <param name="funcItem"></param>
        /// <returns></returns>
        public string GetTbContent(string s_SEQNO)
        {
            int iSEQNO = 0;
            if (!int.TryParse(s_SEQNO,out iSEQNO)) { return null; }
            string funcName = "Facade.GetTbContent";
            Store param = new Store();
            param["SEQNO"] = iSEQNO;
            Store store = this.QueryForObject<Store>(funcName, param);
            if (store == null) { return null; }
            if (store.Get("CONTENT1") == null) { return null; }
            return store.Get("CONTENT1").AsText();
        }


    }
}

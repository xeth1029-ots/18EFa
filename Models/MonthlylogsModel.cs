using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.WebPages;
using WDACC.DataLayers;
using WDACC.Models.StoreExt;

namespace WDACC.Models
{
    public class MonthlylogsModel
    {
        public Func<string> StatementId { get; set; }
        public Func<Store> Parameter { get; set; }
        public IList<Store> Result { get; set; }
        public Action<IList<Store>> AfterLoad { get; set; }

        // public List<string> FieldList { get; set; }
        public List<Func<Store, HelperResult>> ColumnList { get; set; }

        public void GetData(MyBaseDAO dao)
        {
            Store store = null;

            if (this.Parameter != null)
            {
                store = this.Parameter.Invoke();
            }
            else
            {
                store = new Store();
            }

            this.Result = dao.QueryForListAll<Store>(this.StatementId.Invoke(), store);

            if (this.AfterLoad != null)
            {
                this.AfterLoad.Invoke(this.Result);
            }
        }

        public void QueryForListAll(MyBaseDAO dao)
        {
            Store store = null;

            if (this.Parameter != null)
            {
                store = this.Parameter.Invoke();
            }
            else
            {
                store = new Store();
            }

            this.Result = dao.QueryForListAll<Store>(this.StatementId.Invoke(), store);

            if (this.AfterLoad != null)
            {
                this.AfterLoad.Invoke(this.Result);
            }
        }

        /*
        public SqlMapModel SequenceField(string title)
        {
            return this;
        }

        public SqlMapModel TextField(string name, string title)
        {
            return this;
        }

        public SqlMapModel CustomField(string title, Func<Store, HelperResult> template)
        {
            return this;
        }
        */
    }
}

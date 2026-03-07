using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Turbo.MVC.Base3.Commons;

namespace WDACC.Models.StoreExt
{
    public class Store : Hashtable { }

    public class StoreColumn
    {
        public object RawValue { get; set; }
    }

    // public class StoreCastException : Exception { }

    public static class StoreExtension
    {
        public static T ConvertTo<T>(this Store store)
        {
            Exception except = null;
            T model = Activator.CreateInstance<T>();
            IList<string> err = new List<string>();
            foreach (var prop in model.GetType().GetProperties())
            {
                if (store.ContainsKey(prop.Name))
                {
                    try
                    {
                        prop.SetValue(model, store[prop.Name]);
                    }
                    catch (Exception ex)
                    {
                        // throw new Exception(string.Format("{0}[{1}]): 型別應為 {2}", typeof(T).Name, prop.Name, store[prop.Name].GetType().Name), ex);
                        err.Add(string.Format("{0}[{1}]): 型別應為 {2}", typeof(T).Name, prop.Name, store[prop.Name].GetType().Name));
                        except = ex;
                    }
                }
            }

            if (err.Count > 0)
            {
                string message = "轉型錯誤: ";
                foreach (var msg in err)
                {
                    message += (msg + " ");
                }
                throw new Exception(message, except);
            }

            return model;
        }

        public static void Collection(this Store store, FormCollection collection)
        {
            foreach (var key in collection.Keys)
            {
                string keyStr = key.ToString();
                store[keyStr] = collection.Get(keyStr);
            }
        }

        public static void Collection(this Store store, object model)
        {
            if (model != null)
            {
                foreach (var prop in model.GetType().GetProperties())
                {
                    string name = prop.Name;
                    var val = prop.GetValue(model);
                    if (val != null) { store[name] = val; }
                }
            }
        }

        public static Func<string, StoreColumn> GetFinder(this Store store)
        {
            return (string fieldName) => { return store.Get(fieldName); };
        }

        public static StoreColumn Get(this Store store, string field)
        {
            StoreColumn col = new StoreColumn();
            if (!store.ContainsKey(field)) { return col; }
            object f = store[field];
            if (f == null) { return col; }
            col.RawValue = f;
            return col;
        }

        public static string AsText(this StoreColumn store)
        {
            return store.RawValue != null ? store.RawValue.ToString() : null;
        }

        public static Int64? AsInt64(this StoreColumn store)
        {
            return store.RawValue != null ? System.Convert.ToInt64(store.RawValue) : default(Int64);
        }

        public static Int32? AsInt32(this StoreColumn store)
        {
            return store.RawValue != null ? System.Convert.ToInt32(store.RawValue) : default(Int32);
        }

        public static DateTime? AsDateTime(this StoreColumn store)
        {
            return store.RawValue != null ? System.Convert.ToDateTime(store.RawValue) : default(DateTime);
        }

        public static string AsDateTimeText(this StoreColumn store, string format = null)
        {
            string result = null; //string.Empty;
            if (format == null) { return result; }
            if (store == null) { return result; }
            if (store.RawValue == null) { return result; }
            DateTime? dateValue = System.Convert.ToDateTime(store.RawValue);
            if (dateValue.HasValue) { result = dateValue.Value.ToString(format); }
            return result;
        }

        public static string AsDateTimeTextTw(this StoreColumn store)
        {
            string result = null; //string.Empty;
            if (store == null) { return result; }
            if (store.RawValue == null) { return result; }
            DateTime? dateValue = System.Convert.ToDateTime(store.RawValue);
            if (dateValue.HasValue) { result = MyCommonUtil.TransToTwYYYMMDD(dateValue); }
            return result;
        }

        public static string AsCodeText(this StoreColumn store, IList<SelectListItem> items)
        {
            string result = string.Empty;
            if (store.RawValue == null) { return result; }
            var matchs = items.Where(x => x.Value == store.RawValue.ToString().Trim());
            if (matchs == null) { return result; }
            if (matchs.Count() <= 0) { return result; }
            result = matchs.First().Text;
            return result;
        }

        public static string AsHtmlDecodedText(this StoreColumn store)
        {
            string result = string.Empty;
            if (store == null) { return result; }
            if (store.RawValue == null) { return result; }
            result = HttpUtility.HtmlDecode(store.RawValue.ToString());
            return result;
        }

        public static string GetText(this Store store, string field)
        {
            string result = string.Empty;
            if (store.ContainsKey(field))
            {
                object f = store[field];
                result = f != null ? f.ToString() : null;
            }
            return result;
        }

        public static Int64? GetInt64(this Store store, string field)
        {
            Int64? result = default(Int64?);
            if (store.ContainsKey(field))
            {
                object f = store[field];
                result = f != null ? System.Convert.ToInt64(f) : default(Int64);
            }
            return result;
        }

        public static Int32? GetInt32(this Store store, string field)
        {
            Int32? result = default(Int32?);
            if (store.ContainsKey(field))
            {
                object f = store[field];
                result = f != null ? System.Convert.ToInt32(f) : default(Int32);
            }
            return result;
        }

        public static DateTime? GetDateTime(this Store store, string field)
        {
            DateTime? result = default(DateTime?);
            if (store.ContainsKey(field))
            {
                object f = store[field];
                result = f != null ? System.Convert.ToDateTime(f) : default(DateTime);
            }
            return result;
        }

        public static string GetDateTimeText(this Store store, string field, string format)
        {
            DateTime? result = default(DateTime?);
            if (store.ContainsKey(field))
            {
                object f = store[field];
                result = f != null ? System.Convert.ToDateTime(f) : default(DateTime);
            }
            return result.HasValue ? result.Value.ToString(format) : null;
        }

        public static string GetDateTimeTextTw(this Store store, string field)
        {
            DateTime? result = default(DateTime?);
            if (store.ContainsKey(field))
            {
                object f = store[field];
                result = f != null ? System.Convert.ToDateTime(f) : default(DateTime);
            }
            return result.HasValue ?
                MyCommonUtil.TransToTwYYYMMDD(result) : null;
        }

    }

}


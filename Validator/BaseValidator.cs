using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using FluentValidation;
using WDACC.Models.Entities;
using WDACC.Models.ViewModel.Member;
using log4net;

namespace WDACC.Validator
{
    public class BaseValidator<T> : AbstractValidator<T>
    {
        protected static readonly ILog LOG = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public int GetMaxFileSizeLimit()
        {
            return 18874368;
        }

        public bool IsFileExtLegal(string contentType)
        {
            //XLS、DOC、PPT、PDF、JPG、PNG、MSG，ODT、ODF
            //https://developer.mozilla.org/en-US/docs/Web/HTTP/Basics_of_HTTP/MIME_types/Common_types
            //LOG.Debug(string.Concat("#WDACC.Validator,IsFileExtLegal,contentType:", contentType));
            //xls xlsx
            if (contentType == "application/vnd.ms-excel" || contentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" ) { return true; }
            //doc docx
            if (contentType == "application/msword" || contentType == "application/vnd.openxmlformats-officedocument.wordprocessingml.document") { return true; }
            //ppt pptx
            if (contentType == "application/vnd.ms-powerpoint" || contentType == "application/vnd.openxmlformats-officedocument.presentationml.presentation") { return true; }
            //pdf
            if (contentType == "application/pdf") { return true; }
            //jpg jpeg png
            if (contentType == "image/jpeg" || contentType == "image/png") { return true; }
            //msg
            if (contentType == "application/vnd.ms-outlook") { return true; }
            //ODF //odt,ods
            if (contentType == "application/vnd.oasis.opendocument.text" || contentType == "application/vnd.oasis.opendocument.spreadsheet") { return true; }
            if (contentType == "application/x-msdownload") { return false; }
            return false;
        }

        /// <summary>
        /// 檢查是否包含所需字元組合
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public bool CheckPasswordChar(string str)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(str))
            {
                Regex regex1 = new Regex("[A-z]+");
                Regex regex2 = new Regex("[a-z]+");
                Regex regex3 = new Regex("[0-9]+");
                Regex regex4 = new Regex("[@#$_%]+");
                result = regex1.IsMatch(str) && regex2.IsMatch(str) && regex3.IsMatch(str) && regex4.IsMatch(str);
            }
            return result;
        }

    }

}

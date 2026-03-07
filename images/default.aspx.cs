using System;


namespace WDACC.images
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Response.Flush();
            Response.Clear();
            Response.StatusCode = 404;
            Response.Status = "404 Not Found";
            Response.End();
        }
    }
}
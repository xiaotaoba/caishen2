using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
//using LitJson;
using WxPayAPI;

public partial class _WxPayDefault : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Log.Info(this.GetType().ToString(), "page load");
    }

}
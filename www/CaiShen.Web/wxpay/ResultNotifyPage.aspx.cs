using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using WxPayAPI;

public partial class ResultNotifyPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ResultNotify resultNotify = new ResultNotify(this);
        resultNotify.ProcessNotify();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pannet.DAL;
using Pannet.Utility;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace Pannet.Web.Areas.Mobile.Controllers
{
    public class CodeController : Controller
    {
        //
        // GET: /Code/
        public ActionResult Index()
        {
            string VNum = UtilityClass.RndNum(4); 
            Bitmap Img = null;
            Graphics g = null;
            MemoryStream ms = null;

            int gheight = VNum.Length * 16;
            Img = new Bitmap(gheight, 25);
            g = Graphics.FromImage(Img);
            //背景颜色
            g.Clear(Color.White);
            //文字字体
            Font f = new Font("Arial Black", 14);
            //文字颜色
            SolidBrush s = new SolidBrush(Color.FromArgb(253, 115, 004));
            g.DrawString(VNum, f, s, 3, 3);
            ms = new MemoryStream();
            Img.Save(ms, ImageFormat.Jpeg);
            Response.ClearContent();
            Response.ContentType = "image/Jpeg";
            Response.BinaryWrite(ms.ToArray());

            g.Dispose();
            Img.Dispose();
            //Response.End();
            Session["YX_Login_Code"] = VNum;

            return File(ms.ToArray(), "image/jpeg");  
        }
	}
}
using System;
using System.Collections.Specialized;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using ThoughtWorks.QRCode.Codec;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Pannet.Utility
{
    /// <summary>
    /// 生成二维码操作类
    /// Copyright (C) Maticsoft
    /// </summary>
    public sealed class QRCodeHelper2
    {
        /// <summary>
        /// 生成推荐二维码
        /// </summary>
        /// <param name="code">二维码内容(链接)</param>
        /// <param name="userid">用户ID</param>
        /// <param name="fileName">文件名称不含后缀</param>
        /// <returns>返回二维码图片地址</returns>
        public static string CreateQrCode(string code, int userid, string fileName = "qrcode")
        {
            fileName = fileName + ".jpg";
            string codePhotoPath = string.Format("/upload/{0}/qrcode/", userid);
            string codePhotoDirectory = AppDomain.CurrentDomain.BaseDirectory.Trim('/') + codePhotoPath;
            string codePhotoUrl = codePhotoDirectory + fileName;
            if (!System.IO.File.Exists(codePhotoUrl))//不存在创建
            {
                if (!System.IO.Directory.Exists(codePhotoDirectory))
                {
                    System.IO.Directory.CreateDirectory(codePhotoDirectory);
                }

                if (!string.IsNullOrEmpty(code))
                {
                    QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
                    qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                    qrCodeEncoder.QRCodeScale = 4;
                    qrCodeEncoder.QRCodeVersion = 8;
                    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
                    Image image = qrCodeEncoder.Encode(code);

                    System.IO.FileStream fStream = new System.IO.FileStream(codePhotoUrl, System.IO.FileMode.OpenOrCreate);
                    //image.Save(fStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    System.IO.MemoryStream MStream = new System.IO.MemoryStream();
                    image.Save(MStream, System.Drawing.Imaging.ImageFormat.Png);

                    //System.IO.MemoryStream MStream1 = new System.IO.MemoryStream();
                    //CombinImage(image, HttpContext.Current.Server.MapPath("~/Content/images/logo80.png")).Save(MStream1, System.Drawing.Imaging.ImageFormat.Png);
                    CombinImage(image, HttpContext.Current.Server.MapPath("~/Content/images/logo80.png")).Save(fStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    //HttpContext.Current.Response.ClearContent();
                    //HttpContext.Current.Response.ContentType = "image/png";
                    //HttpContext.Current.Response.BinaryWrite(MStream1.ToArray());

                    image.Dispose();
                    fStream.Close();
                    MStream.Dispose();
                    //MStream1.Dispose();
                    HttpContext.Current.Response.Flush();
                    HttpContext.Current.Response.End();
                }
            }
            return codePhotoPath + fileName;
        }
        /// <summary>
        /// 生成推荐二维码
        /// </summary>
        /// <param name="code">二维码内容(链接)</param>
        /// <param name="userid">用户ID</param>
        /// <returns>直接生成二维码图片 二进制</returns>
        public static void CreateQrCodeBinary(string code, int userid)
        {
            if (!string.IsNullOrEmpty(code))
            {
                QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
                qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                qrCodeEncoder.QRCodeScale = 4;
                qrCodeEncoder.QRCodeVersion = 8;
                qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
                Image image = qrCodeEncoder.Encode(code);

                //System.IO.FileStream fStream = new System.IO.FileStream(codePhotoUrl, System.IO.FileMode.OpenOrCreate);
                //image.Save(fStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                System.IO.MemoryStream MStream = new System.IO.MemoryStream();
                image.Save(MStream, System.Drawing.Imaging.ImageFormat.Png);

                System.IO.MemoryStream MStream1 = new System.IO.MemoryStream();
                CombinImage(image, HttpContext.Current.Server.MapPath("~/Content/images/logo80.png")).Save(MStream1, System.Drawing.Imaging.ImageFormat.Png);
                //CombinImage(image, HttpContext.Current.Server.MapPath("~/Content/images/logo80.png")).Save(fStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.ContentType = "image/png";
                HttpContext.Current.Response.BinaryWrite(MStream1.ToArray());

                image.Dispose();
                //fStream.Close();
                MStream.Dispose();
                MStream1.Dispose();
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
        }

        /// <summary>    
        /// 调用此函数后使此两种图片合并，类似相册，有个    
        /// 背景图，中间贴自己的目标图片    
        /// </summary>    
        /// <param name="imgBack">粘贴的源图片</param>    
        /// <param name="destImg">粘贴的目标图片</param>    
        public static System.Drawing.Image CombinImage(System.Drawing.Image imgBack, string destImg)
        {
            System.Drawing.Image img = System.Drawing.Image.FromFile(destImg);        //照片图片      
            if (img.Height != 65 || img.Width != 65)
            {
                img = KiResizeImage(img, 65, 65, 0);
            }
            Graphics g = Graphics.FromImage(imgBack);

            g.DrawImage(imgBack, 0, 0, imgBack.Width, imgBack.Height);      //g.DrawImage(imgBack, 0, 0, 相框宽, 相框高);     

            //g.FillRectangle(System.Drawing.Brushes.White, imgBack.Width / 2 - img.Width / 2 - 1, imgBack.Width / 2 - img.Width / 2 - 1,1,1);//相片四周刷一层黑色边框    

            //g.DrawImage(img, 照片与相框的左边距, 照片与相框的上边距, 照片宽, 照片高);    

            g.DrawImage(img, imgBack.Width / 2 - img.Width / 2, imgBack.Width / 2 - img.Width / 2, img.Width, img.Height);
            GC.Collect();
            return imgBack;
        }


        /// <summary>    
        /// Resize图片    
        /// </summary>    
        /// <param name="bmp">原始Bitmap</param>    
        /// <param name="newW">新的宽度</param>    
        /// <param name="newH">新的高度</param>    
        /// <param name="Mode">保留着，暂时未用</param>    
        /// <returns>处理以后的图片</returns>    
        public static System.Drawing.Image KiResizeImage(System.Drawing.Image bmp, int newW, int newH, int Mode)
        {
            try
            {
                System.Drawing.Image b = new Bitmap(newW, newH);
                Graphics g = Graphics.FromImage(b);
                // 插值算法的质量    
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(bmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
                g.Dispose();
                return b;
            }
            catch
            {
                return null;
            }
        }
    }
}

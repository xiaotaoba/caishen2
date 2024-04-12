using System;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Pannet.Utility
{
    public class UtilityClass
    {
        public static string CutTitle(string str_value, int str_len)
        {
            int p_num = 0;
            string New_Str_value = "";
            if (str_value == "")
            {
                return "";
            }
            int Len_Num = str_value.Length;
            for (int i = 0; i < Len_Num; i++)
            {
                char c;
                if (i < (Len_Num - 1))
                {
                    c = Convert.ToChar(str_value.Substring(i, 1));
                }
                else
                {
                    c = Convert.ToChar(str_value.Substring(Len_Num - 1, 1));
                }
                if ((c > '\x00ff') || (c < '\0'))
                {
                    p_num += 2;
                }
                else
                {
                    p_num++;
                }
                if (p_num >= str_len)
                {
                    return str_value.Substring(0, i + 1);
                }
                New_Str_value = str_value;
            }
            return New_Str_value;
        }

        public static string GetCurrentUrl()
        {
            string strUrl = HttpContext.Current.Request.ServerVariables["Url"];
            if (HttpContext.Current.Request.QueryString.Count == 0)
            {
                return strUrl;
            }
            return (strUrl + "?" + HttpContext.Current.Request.ServerVariables["Query_String"]);
        }

        public static string GetScriptName()
        {
            return HttpContext.Current.Request.ServerVariables["Script_Name"];
        }

        public static string GetUserIp()
        {
            string ip;
            bool isErr = false;
            if (HttpContext.Current.Request.ServerVariables["HTTP_X_ForWARDED_For"] == null)
            {
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            }
            else
            {
                ip = HttpContext.Current.Request.ServerVariables["HTTP_X_ForWARDED_For"].ToString();
            }
            if (ip.Length > 15)
            {
                isErr = true;
            }
            else
            {
                string[] temp = ip.Split(new char[] { '.' });
                if (temp.Length == 4)
                {
                    for (int i = 0; i < temp.Length; i++)
                    {
                        if (temp[i].Length > 3)
                        {
                            isErr = true;
                        }
                    }
                }
                else
                {
                    isErr = true;
                }
            }
            if (isErr)
            {
                return "0.0.0.0";
            }
            return ip;
        }

        public static string HtmlDecode(string str)
        {
            str = str.Replace("<br>", "\n");
            str = str.Replace("&amp;", "&");
            str = str.Replace("&lt;", "<");
            str = str.Replace("&gt;", ">");
            str = str.Replace("&nbsp;", " ");
            str = str.Replace("&acute;", "'");
            str = str.Replace("&quot;", "\"");
            return str;
        }

        public static string HtmlEncode(string str)
        {
            str = str.Replace("&", "&amp;");
            str = str.Replace("<", "&lt;");
            str = str.Replace(">", "&gt;");
            str = str.Replace("\n", "<br>");
            str = str.Replace(" ", "&nbsp;");
            str = str.Replace("'", "&acute;");
            str = str.Replace("\"", "&quot;");
            return str;
        }

        public static string UrlEncode(string str)
        {
            return HttpContext.Current.Server.UrlEncode(str);
        }
        public static string UrlDecode(string str)
        {
            return HttpContext.Current.Server.UrlDecode(str);
        }

        public static string HtmlFilter(string strHtml)
        {
            if (string.IsNullOrEmpty(strHtml))
            {
                return "";
            }
            string[] aryReg = new string[] { "<script[^>]*?>.*?</script>", "<(\\/\\s*)?!?((\\w+:)?\\w+)(\\w+(\\s*=?\\s*(([\"'])(\\\\[\"'tbnr]|[^\\7])*?\\7|\\w+)|.{0})|\\s)*?(\\/\\s*)?>", @"([\r\n])[\s]+", "&(quot|#34);", "&(amp|#38);", "&(lt|#60);", "&(gt|#62);", "&(nbsp|#160);", "&(iexcl|#161);", "&(cent|#162);", "&(pound|#163);", "&(copy|#169);", @"&#(\d+);", "-->", @"<!--.*\n" };
            string[] aryRep = new string[] { "", "", "", "\"", "&", "<", ">", "   ", "\x00a1", "\x00a2", "\x00a3", "\x00a9", "", "\r\n", "" };
            string newReg = aryReg[0];
            string strOutput = strHtml;
            for (int i = 0; i < aryReg.Length; i++)
            {
                strOutput = new Regex(aryReg[i], RegexOptions.IgnoreCase).Replace(strOutput, aryRep[i]);
            }
            strOutput.Replace("<", "");
            strOutput.Replace(">", "");
            strOutput.Replace("\r\n", "");
            return strOutput;
        }

        /// <summary>
        /// 过滤sql中非法字符
        /// </summary>
        /// <param name="value">要过滤的字符串</param>
        /// <returns>string</returns>
        public static string SQLFilter(string value)
        {
            //value = value.ToLower();
            if (string.IsNullOrEmpty(value)) return string.Empty;
            value = Regex.Replace(value, @";", string.Empty);
            value = Regex.Replace(value, @"'", string.Empty);
            value = Regex.Replace(value, "\"", string.Empty);
            value = Regex.Replace(value, @"&", string.Empty);
            value = Regex.Replace(value, @"%20", string.Empty);
            value = Regex.Replace(value, @"--", string.Empty);
            value = Regex.Replace(value, @"==", string.Empty);
            value = Regex.Replace(value, @"<", string.Empty);
            value = Regex.Replace(value, @">", string.Empty);
            value = Regex.Replace(value, @"%", string.Empty);
            value = Regex.Replace(value, @"\(", string.Empty);
            value = Regex.Replace(value, @"\)", string.Empty);
            //value = Regex.Replace(value, @"+", string.Empty);
            value = Regex.Replace(value, @"\\", string.Empty);
            value = Regex.Replace(value, @"$", string.Empty);
            value = Regex.Replace(value, @"|", string.Empty);
            value = Regex.Replace(value, "exec", string.Empty);
            value = Regex.Replace(value, "delete", string.Empty);
            value = Regex.Replace(value, "master", string.Empty);
            value = Regex.Replace(value, "truncate", string.Empty);
            value = Regex.Replace(value, "declare", string.Empty);
            value = Regex.Replace(value, "create", string.Empty);
            value = Regex.Replace(value, "cast", string.Empty);
            value = Regex.Replace(value, "xp_", string.Empty);
            return value;
        }

        public static long IpToLong(string Ip)
        {
            string[] sIP = Ip.Split(new char[] { '.' });
            return ((((((long.Parse(sIP[0]) * 0xffL) * 0xffL) * 0xffL) + ((long.Parse(sIP[1]) * 0xffL) * 0xffL)) + (long.Parse(sIP[2]) * 0xffL)) + long.Parse(sIP[3]));
        }

        public static string Left(string inputString, int len)
        {
            if (inputString.Length < len)
            {
                return inputString;
            }
            return inputString.Substring(0, len);
        }

        public static int Max(int int1, int int2)
        {
            return ((int1 > int2) ? int1 : int2);
        }

        public static string GetMD52(string str)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");
        }
        /// <summary>
        /// ASP MD5加密算法
        /// </summary>
        /// <param name="md5str">要加密的字符串</param>
        /// <param name="type">16还是32位加密</param>
        /// <returns>Asp md5加密结果</returns>
        public static string GetAspMd5(string md5str, int type)
        {
            if (type == 16)
            {
                return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(md5str, "MD5").Substring(8, 16).ToLower();
            }
            else if (type == 32)
            {
                return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(md5str, "MD5").ToLower();
            }
            return "";
        }


        #region ========加密========

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string Encrypt(string Text)
        {
            return Encrypt(Text, "Eyoo");
        }
        /// <summary> 
        /// 加密数据 
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string Encrypt(string Text, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray;
            inputByteArray = Encoding.Default.GetBytes(Text);
            des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();
        }

        #endregion

        #region ========解密========


        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string Decrypt(string Text)
        {
            return Decrypt(Text, "Eyoo");
        }
        /// <summary> 
        /// 解密数据 
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string Decrypt(string Text, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            int len;
            len = Text.Length / 2;
            byte[] inputByteArray = new byte[len];
            int x, i;
            for (x = 0; x < len; x++)
            {
                i = Convert.ToInt32(Text.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }
            des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Encoding.Default.GetString(ms.ToArray());
        }

        #endregion


        public static void MessageBox(string msg)
        {
            ((Page)HttpContext.Current.Handler).ClientScript.RegisterStartupScript(typeof(Page), "", "alert('" + msg + "');", true);
        }

        public static void MessageBox(string msg, bool back)
        {
            ((Page)HttpContext.Current.Handler).ClientScript.RegisterStartupScript(typeof(Page), "", "alert('" + msg + "');history.go(-1);", true);
        }

        public static void MessageBox(string msg, string url)
        {
            ((Page)HttpContext.Current.Handler).ClientScript.RegisterStartupScript(typeof(Page), "", "alert('" + msg + "');window.location.href='" + url + "';", true);
        }

        public static int Min(int int1, int int2)
        {
            return ((int1 < int2) ? int1 : int2);
        }

        public static string RepeatFilter(string _val, char splitChar)
        {
            string _str = string.Empty;
            string[] strArray = _val.Split(new char[] { splitChar });
            for (int i = 0; i < strArray.Length; i++)
            {
                for (int j = i + 1; j < strArray.Length; j++)
                {
                    if (((strArray[i] != string.Empty) && (strArray[j] != string.Empty)) && (strArray[j] == strArray[i]))
                    {
                        strArray[j] = string.Empty;
                    }
                }
                if (strArray[i] != string.Empty)
                {
                    _str = _str + strArray[i] + splitChar.ToString();
                }
            }
            if (_str.LastIndexOf(splitChar.ToString()) > -1)
            {
                _str = _str.Substring(0, _str.Length - 1);
            }
            return _str;
        }

        public static string Right(string inputString, int len)
        {
            if (inputString.Length < len)
            {
                return inputString;
            }
            return inputString.Substring(inputString.Length - len, len);
        }

        public static bool VaildIpv4(string TestIp)
        {
            bool blReturn = false;
            string Reg = @"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$";
            if (Regex.IsMatch(TestIp, Reg))
            {
                blReturn = true;
            }
            return blReturn;
        }

        public static string RndNum(int VcodeNum)
        {
            string Vchar = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z";
            string[] VcArray = Vchar.Split(new Char[] { ',' });
            string VNum = "";
            int temp = -1;

            Random rand = new Random();

            for (int i = 1; i < VcodeNum + 1; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(i * temp * unchecked((int)DateTime.Now.Ticks));
                }

                int t = rand.Next(35);
                if (temp != -1 && temp == t)
                {
                    return RndNum(VcodeNum);
                }
                temp = t;
                VNum += VcArray[t];
            }
            return VNum;
        }

        public static string GetMD5(string str)
        {
            //if (code == 16) //16位MD5加密（取32位加密的9~25字符） 
            //{
            //    return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToLower().Substring(8, 16);
            //}
            //else//32位加密 
            //{
            //    return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToLower();
            //}
            HashAlgorithm hashAlgorithm = MD5.Create();// CryptoAlgorithms.CreateSHA1();//或者 CryptoAlgorithms.CreateMD5()

            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(str)); // 请注意选择你的 Encoding
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        /// <summary>  
        /// 判断手机用户UserAgent  
        /// </summary>  
        /// <returns></returns>  
        public static bool IsMobile()
        {

            HttpContext context = HttpContext.Current;
            if (context != null)
            {
                HttpRequest request = context.Request;
                if (request.Browser.IsMobileDevice)
                    return true;

                string MobileUserAgent = System.Configuration.ConfigurationManager.AppSettings["MobileUserAgent"];
                Regex MOBILE_REGEX = new Regex(MobileUserAgent);
                if (string.IsNullOrEmpty(request.UserAgent) || MOBILE_REGEX.IsMatch(request.UserAgent.ToLower()))
                    return true;
            }
            return false;
        }

        #region  转换成 整型 id数组

        /// <summary>
        /// 逗号隔开的id字符串 转换成 整型 id数组
        /// </summary>
        /// <param name="idStr"></param>
        /// <returns></returns>
        public static Int32[] ConvertIntArr(string idStr)
        {
            idStr = idStr.Trim(',');
            string[] splitStr = { "," };
            string[] idsArr = idStr.Split(splitStr, StringSplitOptions.RemoveEmptyEntries);
            Int32[] cartids = new Int32[idsArr.Length];
            try
            {
                for (int i = 0; i < idsArr.Length; i++)
                {
                    cartids[i] = Convert.ToInt32(idsArr[i]);
                }
            }
            catch
            {
                throw new Exception("参数错误！");
            }
            return cartids;
        }

        #endregion

        #region 内容里面的img src加上域名

        /// <summary>
        /// 内容里面的img src加上域名
        /// </summary>
        /// <param name="content"></param>
        /// <param name="domain">http:// 开头 </param>
        /// <returns></returns>
        public static string ImgAddDomain(string content, string domain)
        {
            ////正则表达式  
            //string RegexString = "(?is)<img.*?src=[\"'](.*?)[\"']+";
            //Regex r = new Regex(RegexString, RegexOptions.None);

            ////把路径替换成绝对路径  
            //Model.G_Desc = r.Replace(Model.G_Desc, "<img src='" + WebSiteConfig.ImgDomain + "$1'");  

            // 定义正则表达式用来匹配 img 标签   
            Regex regImg = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);

            string sHtmlText = content;
            if (!string.IsNullOrEmpty(sHtmlText))
            {
                // 搜索匹配的字符串   
                MatchCollection matches = regImg.Matches(sHtmlText);
                //int i = 0;
                //string[] sUrlList = new string[matches.Count];

                // 取得匹配项列表   
                foreach (Match match in matches)
                {
                    // <span>@match.Groups["imgUrl"].Value </span>
                    string src = match.Groups["imgUrl"].Value;
                    if (!src.ToLower().Contains("http://"))
                    {
                        sHtmlText = sHtmlText.Replace(src, domain + src);
                    }
                }
                //sUrlList[i++] = match.Groups["imgUrl"].Value;
                //return sUrlList;   
            }
            return sHtmlText;
        }

        #endregion

        /// <summary>
        /// 隐藏中间4个字符
        /// </summary>
        /// <param name="title"></param>
        /// <param name="length">开始位置</param>
        /// <returns></returns>
        public static string HiddenWithStar(string title, int length = 4)
        {
            if (title.Length > length)
            {
                char[] titleArr = title.ToCharArray();
                string newStr = "";
                for (int i = 0; i < titleArr.Length; i++)
                {
                    if (i >= length && i < length + 4)
                    {
                        newStr += "*";
                    }
                    else
                    {
                        newStr += titleArr[i];
                    }
                }
                return newStr;
                //return title.Replace(title.Substring(length, (title.Length - length) > 4 ? 4 : (title.Length - length)), "****");
            }
            else
            {
                return "****";
            }
        }
        /// <summary>
        /// 隐藏后几个字符
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public static string HiddenLastWithStar(string title, int length = 4)
        {
            if (title.Length > length)
            {
                return title.Replace(title.Substring(title.Length - length, length), "****");
            }
            else
            {
                return "****";
            }
        }
        // <summary>
        /// 隐藏前几个字符
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public static string HiddenFirstWithStar(string title, int length = 4)
        {
            if (title.Length > length)
            {
                return title.Replace(title.Substring(0, length), "****");
            }
            else
            {
                return "****";
            }
        }
        /// <summary>
        /// 获取字符串中的数字
        /// </summary>
        /// <returns></returns>
        public static int GetNumberFromString(string strList)
        {
            if (string.IsNullOrEmpty(strList))
            {
                return 1;
            }
            Regex reg = new Regex(@"\D"); //找到所有非数字
            strList = reg.Replace(strList, ""); //把所有非数字替换成空
            if (string.IsNullOrEmpty(strList) || strList.Length < 1)
            {
                strList = "1";
            }
            return Convert.ToInt32(strList);
        }

        /// <summary>
        /// 把科学计数的数字转数字字符串
        /// </summary>
        /// <returns></returns>
        public static string ConvertToString(double num)
        {
            if (num.ToString().Contains("E"))
            {
                Decimal dData = 0.0M;
                dData = Convert.ToDecimal(Decimal.Parse(num.ToString(), System.Globalization.NumberStyles.Float));
                return dData.ToString().TrimEnd('0');
            }
            else
            {
                return num.ToString();
            }
        }
        
    }

}
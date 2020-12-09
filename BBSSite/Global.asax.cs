using BBSSite.MyPublic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace BBSSite
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            HttpContext.Current.User = MyFormsAuthentication<MyUserDataPrincipal>.TryParsePrincipal(HttpContext.Current.Request);
        }
        protected void Application_Error(object sender, EventArgs e)
        {
            string HeadStrInfo = "";
            System.Collections.Specialized.NameValueCollection GetHeaders = Request.Headers;
            if (GetHeaders != null)
            {
                foreach (string keys in GetHeaders.AllKeys)
                {
                    HeadStrInfo += "       " + keys + ":\r\n";
                    foreach (string values in GetHeaders.GetValues(keys))
                    {
                        HeadStrInfo += "            " + values + "\r\n";
                    }
                }
            }
            string Params = "";

            System.Collections.Specialized.NameValueCollection param = Request.Form;
            foreach (string key in param.AllKeys)
            {
                Params += "       " + key + ":\r\n            { ";
                string[] values = param.GetValues(key);
                for (int v = 0; v < values.Length; v++)
                {
                    Params += values[v] + (v == (values.Length - 1) ? "" : ",");
                }
                Params += " }\r\n";
            }
            if (Params == "")
            {
                Params = "       {}\r\n";
            }
            Exception objErr = Server.GetLastError().GetBaseException();
            System.IO.FileInfo fi = new System.IO.FileInfo("D:/BBSLog.txt");
            System.IO.FileStream fs = null;
            System.IO.StreamWriter sw = null;
            if (!fi.Exists)
            {
                fs = fi.Create();
            }
            else
            {
                sw = fi.AppendText();
            }
            string log = "   发生异常页: " + Request.Url.ToString() + "\r\n   客户端IP: " + Request.UserHostAddress + "\r\n   客户端DNS: " + Request.UserHostName + "\r\n   客户端平台: " + Request.Browser.Platform + "\r\n   访问者名称: " + Request.Browser.Type.ToString() + "\r\n   客户端类型: " + Request.UserAgent + "\r\n   IsCookies: " + Request.Browser.Cookies.ToString() + "\r\n   传输方式: " + Request.HttpMethod + "\r\n   传输头部: \r\n   {\r\n" + HeadStrInfo + "   }\r\n   Post参数: \r\n   {\r\n" + Params + "   }\r\n   异常信息: " + objErr.ToString();
            if (fs != null)
            {
                byte[] bs = System.Text.Encoding.Default.GetBytes(log);
                int length = bs.Length;
                fs.Write(bs, 0, length);
                fs.Close();
                fs.Dispose();
                fs = null;
            }
            else
            {
                sw.Write(log);
                sw.Close();
                sw.Dispose();
                sw = null;
            }
            fi = null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace BBSSite.MyPublic
{    
    public class MyFormsAuthentication<TUserData> where TUserData : class, new()//身份验证类
    {
        public static void SetAuthCookie(string UserName, TUserData UserData, int ExpiresMinutes)//用户登录成功时设置Cookie
        {
            if (UserData == null) { throw new ArgumentNullException("userData"); }       //如果传入的数据对象为空，则抛出异常
            string Data = (new JavaScriptSerializer()).Serialize(UserData);              //将对象序列化成JSON字符串
            //创建ticket
            var ticket = new FormsAuthenticationTicket(1, UserName, DateTime.Now, DateTime.Now.AddMinutes(ExpiresMinutes), true, Data);            
            var cookieValue = FormsAuthentication.Encrypt(ticket);                       //加密ticket            
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, cookieValue)//创建Cookie
            {
                HttpOnly = true,
                Secure = FormsAuthentication.RequireSSL,
                //Domain = FormsAuthentication.CookieDomain,
                Path = FormsAuthentication.FormsCookiePath,
            };
            if (ExpiresMinutes > 0)                                                      //如果传入的分钟数大于0
            {
                cookie.Expires = DateTime.Now.AddMinutes(ExpiresMinutes);                //在当前时间的基础上增加60分钟
            }            
            HttpContext.Current.Response.Cookies.Remove(cookie.Name);                    //先移除（不管是否存在）
            HttpContext.Current.Response.Cookies.Add(cookie);                            //写入Cookie
        }
        public static MyFormsPrincipal<TUserData> TryParsePrincipal(HttpRequest request) //从Request中解析出Ticket,UserData
        {
            if (request == null) { throw new ArgumentNullException("request"); }//如果如果请求状态对象为空，则抛出异常            
            var cookie = request.Cookies[FormsAuthentication.FormsCookieName];  //读登录Cookie
            if (cookie == null || string.IsNullOrEmpty(cookie.Value)) { return null; }//如果Cookie不存在，则抛出异常
            try
            {                
                var ticket = FormsAuthentication.Decrypt(cookie.Value);      //解密Cookie值，获取FormsAuthenticationTicket对象
                if (ticket != null && !string.IsNullOrEmpty(ticket.UserData))//如果解密后用户数据对象不为空
                {
                    //将用户数据对象的JSON字符串反序列化成实体对象
                    var userData = (new JavaScriptSerializer()).Deserialize<TUserData>(ticket.UserData);
                    if (userData != null)//如果反序列后不为空
                    {
                        return new MyFormsPrincipal<TUserData>(ticket, userData);//返回用于存储用户数据和实现了IPrincipal接口方法的泛型实例对象
                    }
                }
                return null;//如果解密后为空，则返回空
            }
            catch
            {
                return null;//有异常也不要抛出，防止攻击者试探
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace BBSSite.MyPublic
{
    //定义存储用户数据，实现IPrincipal接口方法。解析Cookie用户票证数据时，该类将被存储在HttpContext.Current.User请求信息中
    public class MyFormsPrincipal<TUserData> : IPrincipal where TUserData : class, new()
    {
        public IIdentity Identity { get; private set; }//当前用户实例        
        public TUserData UserData { get; private set; }//用户数据
        public MyFormsPrincipal(FormsAuthenticationTicket ticket, TUserData userData)//构造方法
        {
            if (ticket == null) { throw new ArgumentNullException("ticket"); }//验证票证信息
            if (userData == null) { throw new ArgumentNullException("userData"); }//验证用户信息
            Identity = new FormsIdentity(ticket);//通过票证信息创建身份验证标识
            UserData = userData;//用户数据
        }        
        public bool IsInRole(string Roles)//角色验证
        {
            var userData = UserData as MyUserDataPrincipal;//将用户数据转换成实际类型
            if (userData == null) { throw new NotImplementedException(); }//验证用户信息
            return userData.IsInRole(Roles);//返回调用用户数据类中定义的IsInRole方法，验证逻辑在此方法中实现
        }        
        public bool IsInUser(string user)//用户名验证
        {
            var userData = UserData as MyUserDataPrincipal;//将用户数据转换成实际类型//验证用户信息
            if (userData == null) { throw new NotImplementedException(); }
            return userData.IsInUser(user);//返回调用用户数据类中定义的IsInUser方法，验证逻辑在此方法中实现
        }
    }
}
using BBSSite.Models;
using BBSSite.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BBSSite.MyPublic
{
    /// <summary>
    /// 登录用户信息管理
    /// </summary>
    public interface ILoginStatus
    {
        void LoginSuccess(string UserName, HttpSessionStateBase Session);
        void LoginOut(HttpSessionStateBase Session);
        bool IsLogin { get; }
        void SetBackLink(string SendUrl);
        string GetBackLink { get; }
        void RemoveBackLink();
    }
    /// <summary>
    /// 登录用户信息管理
    /// </summary>
    public class LoginStatus : ILoginStatus
    {
        /// <summary>
        /// 登录或注册成功时,保存的用户信息
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <param name="Session">请求的Session状态对象</param>
        public void LoginSuccess(string UserName, HttpSessionStateBase Session)
        {
            tb_UsersByCustomer MyUsersByCustomer = null;
            tb_ZY_Sex MyZY_Sex = null;
            using (DB_BBSEntities db = new DB_BBSEntities())
            {
                MyUsersByCustomer = db.tb_UsersByCustomer.Where(W => W.UserName == UserName).First();
                MyZY_Sex = db.tb_ZY_Sex.Where(W => W.ID == MyUsersByCustomer.SexID).First();
            }
            if (MyUsersByCustomer != null)
            {
                LoginStatusEntity LSE = new LoginStatusEntity
                {
                    ID = MyUsersByCustomer.ID,
                    UserName = MyUsersByCustomer.UserName,
                    NickName = MyUsersByCustomer.NickName,
                    SexID = MyUsersByCustomer.SexID,
                    Sex = MyZY_Sex.Content,
                    Age = MyUsersByCustomer.Age,
                    PhotoUrl = MyUsersByCustomer.PhotoUrl,
                    Email = MyUsersByCustomer.Email,
                    Fatieshu = MyUsersByCustomer.Fatieshu ?? 0,
                    Huitieshu = MyUsersByCustomer.Huitieshu ?? 0
                };
                Session.Add("LoginSuccess", LSE);
            }
        }
        /// <summary>
        /// 退出登录方法
        /// </summary>
        /// <param name="Session">请求的Session状态对象</param>
        public void LoginOut(HttpSessionStateBase Session)
        {
            Session.Remove("LoginSuccess");
        }
        /// <summary>
        /// 已登录的用户信息
        /// </summary>
        public LoginStatusEntity LoginStatusEntity
        {
            get
            {
                return HttpContext.Current.Session["LoginSuccess"] as LoginStatusEntity;
            }
        }
        /// <summary>
        /// 在登录或注册用户时保存的原页面地址
        /// </summary>
        /// <param name="SendUrl">原页面地址</param>
        public void SetBackLink(string SendUrl)
        {
            if (SendUrl == null) { return; }
            string DeSendUrl = System.Text.Encoding.Default.GetString(Convert.FromBase64String(SendUrl));
            if (DeSendUrl != null && DeSendUrl != "" && DeSendUrl != "\\" && DeSendUrl != "/")
            {
                HttpContext.Current.Session["GoBack"] = DeSendUrl;
            }
        }
        /// <summary>
        /// 登录或注册成功后移除原页面地址
        /// </summary>
        public void RemoveBackLink()
        {
            HttpContext.Current.Session.Remove("GoBack");
        }
        /// <summary>
        /// 返回原页面地址
        /// </summary>
        public string GetBackLink
        {
            get { return HttpContext.Current.Session["GoBack"] as string; }
        }
        /// <summary>
        /// 返回是否为已登录用户
        /// </summary>
        public bool IsLogin
        {
            get { return HttpContext.Current.Session["LoginSuccess"] != null; }
        }
    }

    /// <summary>
    /// 内部用户登录信息管理
    /// </summary>
    public class AdminLoginStatus : ILoginStatus
    {
        /// <summary>
        /// 登录或注册成功时,保存的用户信息
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <param name="Session">请求的Session状态对象</param>
        public void LoginSuccess(string UserName, HttpSessionStateBase Session)
        {
            tb_UsersBySystem MyUsersBySystem = null;
            using (DB_BBSEntities db = new DB_BBSEntities())
            {
                MyUsersBySystem = db.tb_UsersBySystem.Where(W => W.UserName == UserName).First();
            }
            if (MyUsersBySystem != null)
            {
                LoginStatusAdminEntity LSAE = new LoginStatusAdminEntity
                {
                    ID = MyUsersBySystem.ID,
                    RoleID = MyUsersBySystem.RoleID,
                    UserName = MyUsersBySystem.UserName,
                    NickName = MyUsersBySystem.NickName,
                    Email = MyUsersBySystem.Email
                };
                Session.Add("LoginAdminSuccess", LSAE);
            }
        }
        /// <summary>
        /// 退出登录方法
        /// </summary>
        /// <param name="Session">请求的Session状态对象</param>
        public void LoginOut(HttpSessionStateBase Session)
        {
            Session.Remove("LoginAdminSuccess");
        }
        /// <summary>
        /// 已登录的用户信息
        /// </summary>
        public LoginStatusAdminEntity LoginStatusEntity
        {
            get
            {
                return HttpContext.Current.Session["LoginAdminSuccess"] as LoginStatusAdminEntity;
            }
        }
        /// <summary>
        /// 在登录或注册用户时保存的原页面地址
        /// </summary>
        /// <param name="SendUrl">原页面地址</param>
        public void SetBackLink(string SendUrl)
        {
            HttpContext.Current.Session["GoAdminBack"] = "admins/Home/Index";
        }
        /// <summary>
        /// 登录或注册成功后移除原页面地址
        /// </summary>
        public void RemoveBackLink()
        {
            HttpContext.Current.Session.Remove("GoAdminBack");
        }
        /// <summary>
        /// 返回原页面地址
        /// </summary>
        public string GetBackLink
        {
            get { return HttpContext.Current.Session["GoAdminBack"] as string; }
        }
        /// <summary>
        /// 返回是否为已登录用户
        /// </summary>
        public bool IsLogin
        {
            get { return HttpContext.Current.Session["LoginAdminSuccess"] != null; }
        }
    }
}
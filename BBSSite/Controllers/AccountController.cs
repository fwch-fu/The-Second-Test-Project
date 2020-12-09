using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using BBSSite.Models;
using MyPublic;
using BBSSite.ViewModels;
using System.Web.Security;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using BBSSite.MyPublic;

namespace BBSSite.Controllers
{
    /// <summary>
    /// Account控制器 用户处理注册、登录控制器
    /// </summary>
    public class AccountController : Controller
    {
        public AccountController()
        {
        }

        /// <summary>
        /// Login action 用于加载登录页面
        /// </summary>
        /// <param name="id">通过正常访问该页面id应为上一次访问的页面地址,用于注册或登录后返回原页面</param>
        /// <returns>返回Login视图</returns>
        // GET: /Account/Login
        [HttpGet, AllowAnonymous]
        public ActionResult Login(string id)
        {
            PublicFunctions.SetUrls(ViewBag, Url);//构造资源文件路径            
            new MyPublic.LoginStatus().SetBackLink(id);//保存上一次页面地址
            return View();//返回视图
        }
        /// <summary>
        /// 登录时接受处理方法
        /// </summary>
        /// <param name="UserEntity">绑定的强类型视图模型,提交数据通过该实体接受</param>
        /// <returns>登录成功返回登录视图action中保存的URL地址(如果存在)或返回首页,登录失败返回Login视图并发送提示消息</returns>
        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]//3个特性为Post接收,允许匿名,防止CSRF跨站攻击
        public ActionResult DoLogin(LoginUsersByCustomerEntity UserEntity)
        {
            //构造资源路径(js、css、image等)
            PublicFunctions.SetUrls(ViewBag, Url);
            bool IsLoginSuccess = false;
            string GetPassword = PublicFunctions.MD5(UserEntity.UserPassword);//将密码MD5加密
            //验证用户名和密码的表达式
            System.Linq.Expressions.Expression<Func<tb_UsersByCustomer, bool>> Exp = f => f.UserName == UserEntity.UserName && Encoding.Unicode.GetString(f.UserPassword) == GetPassword;
            using (DB_BBSEntities db = new DB_BBSEntities())
            {
                //通过表达式验证该登录是否合法
                IsLoginSuccess = db.tb_UsersByCustomer.Count(Exp.Compile()) > 0;
            }
            if (IsLoginSuccess)
            {
                //FormsAuthentication.SetAuthCookie(UserEntity.UserName, false);
                //如果登录成功保存用户信息到session
                MyPublic.ILoginStatus ILoginStatus = new MyPublic.LoginStatus();
                ILoginStatus.LoginSuccess(UserEntity.UserName, Session);
                string GetBackLink = ILoginStatus.GetBackLink;
                if (GetBackLink != null)
                {
                    //移除存储的url地址
                    ILoginStatus.RemoveBackLink();
                    //跳转到原页面
                    return Redirect(GetBackLink);
                }
                else
                {
                    //返回首页
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                //返回登录页并提示错误消息
                ModelState.AddModelError("LoginError", "用户名或密码错误");
                return View("Login");
            }
        }
        /// <summary>
        /// Register action 用于加载登录页面
        /// </summary>
        /// <returns>返回Login视图</returns>
        [HttpGet, AllowAnonymous]
        public ActionResult Register()
        {
            PublicFunctions.SetUrls(ViewBag, Url); //构造资源文件路径                
            PublicService.RegistSexIDBind(ViewBag);//绑定页面性别下来框
            return View();//返回视图
        }
        /// <summary>
        /// 注册时接受处理方法
        /// </summary>
        /// <param name="UserEntity">绑定的强类型视图模型,提交数据通过该实体接受</param>
        /// <returns>注册成功返回登录视图action中保存的URL地址(如果存在)或返回首页,注册失败返回Login视图并发送提示消息</returns>
        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public ActionResult DoRegister(RegistUsersByCustomerEntity UserEntity)
        {
            PublicFunctions.SetUrls(ViewBag, Url);
            if (ModelState.IsValid)//通过实体类的验证特性判断是否验证通过
            {
                tb_UsersByCustomer ub = new tb_UsersByCustomer()//创建表实体数据对象
                {
                    UserName = UserEntity.UserName,
                    UserPassword = Encoding.Unicode.GetBytes(PublicFunctions.MD5(UserEntity.UserPassword)),
                    NickName = UserEntity.NickName,SexID = UserEntity.SexID,
                    Age = UserEntity.Age,Email = UserEntity.Email
                };
                using (DB_BBSEntities db = new DB_BBSEntities())//实例化数据库上下文类
                {
                    db.tb_UsersByCustomer.Add(ub);//将实体数据追加到上下文集合中
                    if (db.SaveChanges() == 0)    //执行保存用户数据,0为保存失败
                    {
                        ModelState.AddModelError("LoginError", "注册失败");//将错误消息添加到状态字典集合中                        
                        PublicService.RegistSexIDBind(ViewBag);//绑定页面性别下来框（如果注册失败）
                        return View("Register");//返回的还是注册页面
                    }
                    else
                    {
                        //注册成功保存用户信息到session
                        MyPublic.ILoginStatus ILoginStatus = new MyPublic.LoginStatus();
                        ILoginStatus.LoginSuccess(UserEntity.UserName, Session);
                        string GetBackLink = ILoginStatus.GetBackLink;
                        if (GetBackLink != null)
                        {
                            //移除存储的url地址
                            ILoginStatus.RemoveBackLink();
                            return Redirect(GetBackLink);
                        }
                        else
                        {
                            //返回首页
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
            }
            else
            {
                //绑定页面性别下来框（如果参数验证失败）
                PublicService.RegistSexIDBind(ViewBag);
                return View("Register");
            }
        }
        /// <summary>
        /// 退出时接受处理方法
        /// </summary>
        /// <returns>返回原页面(如果存在)或首页</returns>
        public ActionResult LoginOut()
        {
            //判断用心是否存在
            BBSSite.MyPublic.ILoginStatus IStatus = new BBSSite.MyPublic.LoginStatus();
            if (IStatus.IsLogin)
            {
                IStatus.LoginOut(Session);
            }
            //返回原页面(如果存在)或首页
            return PublicFunctions.ToRedirect(this, null, null, (Url) =>
             {
                 return Redirect(Url);
             },
            (Url) =>
            {
                return RedirectToAction("Index", "Home");
            });
        }
    }
}
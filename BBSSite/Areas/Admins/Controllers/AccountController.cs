using BBSSite.Areas.Admins.ViewModels;
using BBSSite.Models;
using BBSSite.MyPublic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BBSSite.Areas.Admins.Controllers
{
    public class AccountController : Controller
    {
        // GET: Admins/Account
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult InnerLogin()
        {
            PublicFunctions.SetUrls(ViewBag, Url);
            return View();
        }

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public ActionResult InnerDoLogin(LoginAdminEntity Entity)
        {
            PublicFunctions.SetUrls(ViewBag, Url);
            if (ModelState.IsValid)
            {
                tb_UsersBySystem UsersByCustomer = null;
                Expression<Func<tb_UsersBySystem, bool>> ex = where => where.UserName == Entity.UserName && Encoding.Unicode.GetString(where.UserPassword) == PublicFunctions.MD5(Entity.UserPassword);
                using (DB_BBSEntities db = new DB_BBSEntities())
                {
                    try
                    {
                        if ((UsersByCustomer = db.tb_UsersBySystem.Where(ex.Compile()).FirstOrDefault()) == null)
                        {
                            //MyPublic.ILoginStatus ILoginStatus = new MyPublic.AdminLoginStatus();
                            //ILoginStatus.LoginSuccess(Entity.UserName, Session);
                            //返回登录页并提示错误消息
                            ModelState.AddModelError("LoginError", "用户名或密码错误");
                            return View("InnerLogin");
                        }
                    }
                    catch
                    {
                        ModelState.AddModelError("LoginError", "用户名或密码错误");
                        return View("InnerLogin");
                    }
                }
                var userData = new MyUserDataPrincipal()
                {
                    UserID = UsersByCustomer.ID,
                    RoleID = UsersByCustomer.RoleID,
                    UserName = UsersByCustomer.UserName,
                    NickName = UsersByCustomer.NickName,
                    Email = UsersByCustomer.Email
                };
                MyFormsAuthentication<MyUserDataPrincipal>.SetAuthCookie(Entity.UserName, userData, 60);
                //跳转到首页
                return RedirectToAction("Index", "Home");
            }
            //返回登录页并提示错误消息
            ModelState.AddModelError("LoginError", "用户名或密码不能为空");
            return View("InnerLogin");
        }
    }
}
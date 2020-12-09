using BBSSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BBSSite.MyPublic
{
    public class MyAuthorizeAttribute : AuthorizeAttribute//对控制器和操作方法实现权限验证的特性类
    {
        public MyAuthorizeAttribute(int ColumnID)//传入栏目ID，构造验证类
        {
            using (DB_BBSEntities db = new DB_BBSEntities())//创建数据库上下文类
            {
                IList<int> UserByRoleJoinColumns = null;//定义用户角色与栏目关联表（权限表）的集合对象
                if (ColumnID > 0)//如果传入的栏目ID大于0
                {                    
                    UserByRoleJoinColumns = db.tb_UserByRoleJoinColumn.Where(W => W.ColumnID == ColumnID)
                        .Select(S => S.RoleID).ToList();//查询拥有该栏目权限的所有角色ID
                }
                else
                {                    
                    UserByRoleJoinColumns = db.tb_UserByRole.Select(S => S.ID).ToList();//查询所有角色ID
                }
                if (UserByRoleJoinColumns != null && UserByRoleJoinColumns.Count > 0)//验证数据是否为空
                {
                    this.Roles = string.Join(",", UserByRoleJoinColumns);//赋值基类的角色字符串数据
                }
            }
        }
        //重写自定义授权检查方法
        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            //将Http请求安全信息对象转换为实际类型
            var user = httpContext.User as MyFormsPrincipal<MyUserDataPrincipal>;
            if (user != null)//验证对象是否为空
            {
                return (user.IsInRole(Roles) || user.IsInUser(Users));//传入角色ID集合和用户ID集合，调用验证角色或用户名的方法
            }
            return false;//返回false
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //验证不通过,直接跳转到相应页面，注意：如果不使用以下跳转，则会继续执行Action方法
            filterContext.Result = new RedirectResult("~/Admins/Account/InnerLogin");
        }
    }
}
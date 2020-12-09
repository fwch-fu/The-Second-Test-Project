using BBSSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Script.Serialization;

namespace BBSSite.MyPublic
{
    //存放数据的用户实体
    public class MyUserDataPrincipal : IPrincipal
    {
        public int UserID { get; set; }
        public int RoleID { get; set; }
        public string UserName { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }

        //当使用Authorize特性时，会调用该方法验证角色 
        public bool IsInRole(string Roles)
        {
            var RolesArray = Roles.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            return RolesArray.Contains(RoleID.ToString());
        }

        //验证用户信息
        public bool IsInUser(string Users)
        {
            var UsersArray = Users.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            return UsersArray.Contains(UserID.ToString());
        }
        [ScriptIgnore]    //在序列化的时候忽略该属性
        public IIdentity Identity { get { throw new NotImplementedException(); } }
    }
}
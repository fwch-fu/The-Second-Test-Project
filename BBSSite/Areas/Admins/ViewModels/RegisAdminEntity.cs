using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using BBSSite.MyPublic;

namespace BBSSite.Areas.Admins.ViewModels
{
    public class RegisAdminEntity
    {
        [Required, Display(Name = "角色ID")]
        public int RoleID { get; set; }
        [Required, Display(Name = "用户名"), MaxLength(16, ErrorMessage = "{0}最大长度为16位"), MinLength(6, ErrorMessage = "{0}最小长度为6位"), DataUnique(ErrorMessage = "{0}已存在", edu = EnumDataUnique.tb_UsersBySystem, MyType = 1)]
        public string UserName { get; set; }
        [Required, Display(Name = "昵称"), MaxLength(16, ErrorMessage = "{0}最大长度为16位"), MinLength(2, ErrorMessage = "{0}最小长度为2位")]
        public string NickName { get; set; }
        [Required, Display(Name = "密码"), DataType(DataType.Password), MaxLength(16, ErrorMessage = "{0}最大长度为16位"), MinLength(6, ErrorMessage = "{0}最小长度为6位")]
        public string UserPassword { get; set; }
        [Required, Display(Name = "确认密码"), DataType(DataType.Password), Compare("UserPassword", ErrorMessage = "2次输入密码不一致")]
        public string OkUserPassword { get; set; }
        [Required, Display(Name = "邮箱"), DataType(DataType.EmailAddress), DataUnique(ErrorMessage = "{0}已存在", edu = EnumDataUnique.tb_UsersBySystem, MyType = 2)]
        public string Email { get; set; }
    }
    public class EditAdminEntity
    {
        public int ID { get; set; }
        [Required, Display(Name = "角色ID")]
        public int RoleID { get; set; }
        public string UserName { get; set; }
        [Required, Display(Name = "昵称"), MaxLength(16, ErrorMessage = "{0}最大长度为16位"), MinLength(2, ErrorMessage = "{0}最小长度为2位")]
        public string NickName { get; set; }
        [Required, Display(Name = "邮箱"), DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
using BBSSite.MyPublic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BBSSite.ViewModels
{
    public class LoginUsersByCustomerEntity
    {
        [Required, Display(Name = "用户名")]
        public string UserName { get; set; }
        [Required, Display(Name = "密码"), DataType(DataType.Password)]
        public string UserPassword { get; set; }
    }
    public class RegistUsersByCustomerEntity
    {
        [Required, Display(Name = "用户名"), MaxLength(20, ErrorMessage = "{0}超出了最大长度"), MinLength(6, ErrorMessage = "{0}没达到最小长度"), DataUnique(ErrorMessage = "{0}已存在", edu = EnumDataUnique.tb_UsersByCustomer, MyType = 1)]
        public string UserName { get; set; }
        [Required, DataType(DataType.Password), MaxLength(20, ErrorMessage = "{0}超出了最大长度"), MinLength(8, ErrorMessage = "{0}没达到最小长度"), Display(Name = "密码")]
        public string UserPassword { get; set; }
        [Required, DataType(DataType.Password), Display(Name = "确认密码"), Compare("UserPassword", ErrorMessage = "2次密码不一致")]
        public string UserRePassword { get; set; }
        [Required, Display(Name = "姓名")]
        public string NickName { get; set; }
        [Required, Display(Name = "性别")]
        public int SexID { get; set; }
        [Required, Display(Name = "年龄")]
        public int Age { get; set; }
        public string PhotoUrl { get; set; }
        [Required, DataType(DataType.EmailAddress), Display(Name = "邮箱"), DataUnique(ErrorMessage = "{0}已存在", edu = EnumDataUnique.tb_UsersByCustomer, MyType = 2)]
        public string Email { get; set; }
    }
}
using BBSSite.Models;
using BBSSite.MyPublic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BBSSite.Areas.Admins.ViewModels
{
    public class AddRoleEntity
    {
        public int ID { get; set; }
        [Required, Display(Name = "角色名称"), DataUnique(ErrorMessage = "{0}已存在", edu = EnumDataUnique.tb_UserByRole, MyType = 1, Key = "ID"), MaxLength(30, ErrorMessage = "{0}最大长度为30位"), MinLength(2, ErrorMessage = "{0}最大长度为2位")]
        public string RoleName { get; set; }
    }
    public class BandingColumn
    {
        public IList<tb_Column> Column { get; set; }
        public string[] Checked { get; set; }
    }
}
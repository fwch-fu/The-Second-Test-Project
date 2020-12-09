using BBSSite.MyPublic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BBSSite.Areas.Admins.ViewModels
{
    public class CreateForumAreaEntity
    {
        public int ID { get; set; }
        [Required, Display(Name = "专区名称"), DataUnique(ErrorMessage = "{0}已存在", edu = EnumDataUnique.tb_ForumArea, MyType = 1, Key = "ID")]
        public string AreaName { get; set; }
        [Required, Display(Name = "版主")]
        public int UserID { get; set; }
    }
}
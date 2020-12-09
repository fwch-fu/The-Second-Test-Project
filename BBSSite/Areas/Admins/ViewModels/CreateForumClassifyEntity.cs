using BBSSite.MyPublic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BBSSite.Areas.Admins.ViewModels
{
    public class CreateForumClassifyEntity
    {
        [Required, Display(Name = "所属专区")]
        public int ForumAreaID { get; set; }
        [Required, Display(Name = "所属版主")]
        public int ForumUserID { get; set; }
        [Required, Display(Name = "专区名称"), DataUnique(ErrorMessage = "{0}已存在", edu = EnumDataUnique.tb_ForumClassify, MyType = 1)]
        public string ClassifyName { get; set; }
        [Required, Display(Name = "专区名称")]
        public string ClassifyLogo { get; set; }
        public string ClassifyInnerLogo { get; set; }
        [Required, Display(Name = "专区排序")]
        public int ClassifyOrder { get; set; }
    }
}
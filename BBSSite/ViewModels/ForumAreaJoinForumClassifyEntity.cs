using BBSSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BBSSite.ViewModels
{
    public class ForumAreaJoinForumClassifyEntity
    {
        public int ID { get; set; }
        public string AreaName { get; set; }
        public ICollection<ChildForumClassify> ChildForumClassify { get; set; }
    }
    public class ChildForumClassify
    {
        public tb_ForumClassify Classifys { get; set; }
        public ICollection<tb_ForumMain> ForumMain { get; set; }
    }
}
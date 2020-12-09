using BBSSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BBSSite.ViewModels
{
    public class ForumMainJoinForumSecondEntity
    {
        public tb_ForumMain ForumMain { get; set; }
        public tb_ForumClassify ForumClassify { get; set; }
        public tb_UsersByCustomer UsersByCustomer { get; set; }
        public tb_ZY_Sex ZY_Sex { get; set; }
        public int ForumSecondCount { get; set; }
        public ICollection<ChildForumSecondByUsersByCustomer> ForumSecond { get; set; }
    }
    public class ChildForumSecondByUsersByCustomer
    {
        public tb_ForumSecond ForumSecond { get; set; }
        public tb_UsersByCustomer UsersByCustomer { get; set; }
        public tb_ZY_Sex ZY_Sex { get; set; }
    }
}
using BBSSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BBSSite.ViewModels
{
    public class ChildReplyEntity
    {
        public tb_ForumSecond ForumSecond { get; set; }
        public tb_UsersByCustomer UsersByCustomer { get; set; }
        public tb_UsersByCustomer ByUsersByCustomer { get; set; }
    }
}
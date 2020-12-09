using BBSSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BBSSite.ViewModels
{
    public class ForumClassifyJoinForumMainEntity
    {
        public int ID { get; set; }
        public string ClassifyInnerLogo { get; set; }
        public string ClassifyName { get; set; }
        public tb_UsersByCustomer UsersByBanzhu { get; set; }
        public ICollection<tb_ForumMain> ForumMain { get; set; }
        public IList<tb_UsersByCustomer> UsersByCustomer { get; set; }
        public IList<int> ReplyNumber { get; set; }
        public IList<int> SeeNumber { get; set; }
        public IList<string> LastReplyUser { get; set; }
        public int TotalForumCount { get; set; }
        public int TotalReplyCount { get; set; }
        public int TotalSeeCount { get; set; }
        public IList<string> ImgUrl { get; set; }
        public IList<string> FMType { get; set; }
    }
}
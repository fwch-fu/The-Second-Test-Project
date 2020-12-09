using BBSSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BBSSite.ViewModels
{
    public class ForumMainByRecommendEntity
    {
        public ICollection<tb_ForumMain> ForumMain { get; set; }
        public IList<tb_UsersByCustomer> UsersByCustomer { get; set; }
        public IList<int> ReplyNumber { get; set; }
        public IList<int> SeeNumber { get; set; }
        public IList<string> LastReplyUser { get; set; }
        public int ForumMainCount { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BBSSite.Areas.Admins.ViewModels
{
    public class CustomerUserListEntity
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string NickName { get; set; }
        public string Sex { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public int Fatieshu { get; set; }
        public int Huitieshu { get; set; }
        public bool IsModerator { get; set; }
        public string IsModeratorStr { get; set; }
    }
}
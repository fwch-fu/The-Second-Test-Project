using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BBSSite.Areas.Admins.ViewModels
{
    public class UserListEntity
    {
        public int ID { get; set; }
        public string RoleName { get; set; }
        public string UserName { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
    }
}
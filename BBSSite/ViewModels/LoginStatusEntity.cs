using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BBSSite.ViewModels
{
    public class LoginStatusEntity
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string NickName { get; set; }
        public int SexID { get; set; }
        public string Sex { get; set; }
        public int Age { get; set; }
        public string PhotoUrl { get; set; }
        public string Email { get; set; }
        public int Fatieshu { get; set; }
        public int Huitieshu { get; set; }
    }

    public class LoginStatusAdminEntity
    {
        public int ID { get; set; }
        public int RoleID { get; set; }
        public string UserName { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BBSSite.Areas.Admins.ViewModels
{
    public class RoleListEntity
    {
        public int ID { get; set; }
        public string RoleName { get; set; }
        public int ColumnCount { get; set; }
    }
}
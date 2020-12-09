using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BBSSite.Areas.Admins.ViewModels
{
    public class RoleJoinColumnEntity
    {
        public int ColumnID { get; set; }
        public string ColumnCode { get; set; }
        public string ColumnName { get; set; }
        public string Url { get; set; }
        public string LogoClassName { get; set; }
        public bool IsClassA { get; set; }
        public bool IsSelectedA { get; set; }
        public bool IsSelected { get; set; }
        public bool IsUrlNull { get; set; }
    }
}
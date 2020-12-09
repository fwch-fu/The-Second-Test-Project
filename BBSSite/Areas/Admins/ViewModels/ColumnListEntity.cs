using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BBSSite.Areas.Admins.ViewModels
{
    public class ColumnListEntity
    {
        public int ID { get; set; }
        public string ColumnCode { get; set; }
        public string ColumnName { get; set; }
        public string Url { get; set; }
        public string LogoClassName { get; set; }
    }
}
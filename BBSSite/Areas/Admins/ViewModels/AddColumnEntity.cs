using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BBSSite.Areas.Admins.ViewModels
{
    public class AddColumnEntity
    {
        public int ID { get; set; }
        public bool IsColumnA { get; set; }
        public int ColumnCodeAID { get; set; }
        [Required, MaxLength(60)]
        public string ColumnName { get; set; }
        [MaxLength(256)]
        public string Url { get; set; }
        [MaxLength(32)]
        public string LogoClassName { get; set; }
    }
}
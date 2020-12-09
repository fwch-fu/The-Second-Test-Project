using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BBSSite.Areas.Admins.ViewModels
{
    public class ForumClassifyListEntity
    {
        public int ID { get; set; }
        public string ForumUser { get; set; }
        public string ClassifyName { get; set; }
        public int ClassifyOrder { get; set; }
        public string ClassifyIsleaf { get; set; }
    }
}
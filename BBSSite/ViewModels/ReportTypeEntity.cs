using BBSSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BBSSite.ViewModels
{
    public class ReportTypeEntity
    {
        public IList<tb_ZY_ReportType> PersonalReportType { get; set; }
        public IList<tb_ZY_ReportType> GarbageReportType { get; set; }
    }
}
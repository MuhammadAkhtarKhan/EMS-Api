using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.Models
{
    public class LeaveTypeViewModel
    {
        public double TRNNO { get; set; }
        public string LDESC { get; set; }
        public Nullable<double> LWEIGHT { get; set; }
        public string STATUS { get; set; }
        public string LABRV { get; set; }
    }
}
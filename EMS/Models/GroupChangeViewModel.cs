using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.Models
{
    public class GroupChangeViewModel
    {
        public double TRNNO { get; set; }
        public Nullable<double> GRPMST_TRNNO { get; set; }
        public Nullable<double> EM_TRNNO { get; set; }
        public Nullable<System.DateTime> CHDATE { get; set; }
    }
}
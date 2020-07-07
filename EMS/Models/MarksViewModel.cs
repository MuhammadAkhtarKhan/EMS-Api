using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.Models
{
    public class MarksViewModel
    {
        public double TRNNO { get; set; }
        public Nullable<System.DateTime> MDT { get; set; }
        public Nullable<double> EXAM_TRNNO { get; set; }
        public Nullable<double> CLASS_TRNNO { get; set; }
        public Nullable<double> GRPMST_TRNNO { get; set; }
        public Nullable<double> EM_TRNNO { get; set; }        
        public virtual IList<MARKSDTL> MARKSDTLs { get; set; }
    }
}
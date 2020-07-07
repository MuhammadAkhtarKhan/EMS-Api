using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.Models
{
    public class TotalMarkDtlViewModel
    {
        public double TRNNO { get; set; }
        public double SR { get; set; }
        public Nullable<double> SUBJECT_TRNNO { get; set; }
        public double TOTMARKS { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.Models
{
    public class PromotionDtlViewModel
    {
        public double TRNNO { get; set; }
        public double SR { get; set; }
        public Nullable<double> EM_TRNNO { get; set; }
        public string STATUS { get; set; }
        public Nullable<double> ADIMNO { get; set; }
        public virtual EM EM { get; set; }
        public virtual PROMMST PROMMST { get; set; }
    }
}
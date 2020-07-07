using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.Models
{
    public class FeeCollectViewModel
    {
        public double TRNNO { get; set; }
        public Nullable<System.DateTime> RDATE { get; set; }
        public Nullable<double> EM_TRNNO { get; set; }
        public Nullable<double> FMONTH { get; set; }
        public Nullable<System.DateTime> DDATE { get; set; }
        public Nullable<System.DateTime> LDATE { get; set; }
        public string FSTATUS { get; set; }
        public Nullable<double> DISCOUNT { get; set; }
        public string DISCOUNTTYPE { get; set; }
        public Nullable<double> CLASS_TRNNO { get; set; }
        public Nullable<double> SECDTL_TRNNO { get; set; }
        public Nullable<double> SECDTL_SR { get; set; }
        public Nullable<System.DateTime> PDATE { get; set; }
        public Nullable<double> ARRFLG { get; set; }
        public virtual EM EM { get; set; }
        public virtual IList<FEECOLLECTDTL> FEECOLLECTDTLs { get; set; }
    }
}
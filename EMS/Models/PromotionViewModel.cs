using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.Models
{
    public class PromotionViewModel
    {
        public double TRNNO { get; set; }
        public Nullable<System.DateTime> PDT { get; set; }
        public Nullable<double> GRPMST_TRNNO { get; set; }
        public Nullable<double> CLASS_TRNNO { get; set; }
        public Nullable<double> FCLASS_TRNNO { get; set; }
        public Nullable<double> SECDTL_TRNNO { get; set; }
        public Nullable<double> SECDTL_SR { get; set; }

        public  GRPMST GRPMST { get; set; }
        public  IList<PROMDTL> PROMDTLs { get; set; }
    }
}
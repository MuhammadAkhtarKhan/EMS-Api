using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.Models
{
    public class LSchoolViewModel
    {
        public double TRNNO { get; set; }
        public Nullable<double> CLASS_TRNNO { get; set; }
        public System.DateTime LDATE { get; set; }
        public  CLASS CLASS { get; set; }        
        public  IList<LSCHOOLDTL> LSCHOOLDTLs { get; set; }
    }
}
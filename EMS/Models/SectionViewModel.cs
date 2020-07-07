using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.Models
{
    public class SectionViewModel
    {
        public double TRNNO { get; set; }
        public Nullable<double> CLASS_TRNNO { get; set; }
        public  CLASS CLASS { get; set; }       
        public  IList<EM> EMs { get; set; }       
        public  IList<SECDTL> SECDTLs { get; set; }
    }
}
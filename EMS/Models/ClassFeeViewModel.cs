using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.Models
{
    public class ClassFeeViewModel
    {
        public double TRNNO { get; set; }
        public Nullable<double> CLASS_TRNNO { get; set; }          
        public virtual IList<CLFEEDTL> CLFEEDTLs { get; set; }
    }
}
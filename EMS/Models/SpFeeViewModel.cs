using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.Models
{
    public class SpFeeViewModel
    {
        public double TRNNO { get; set; }
        public System.DateTime SPDATE { get; set; }
        public double CLASS_TRNNO { get; set; }
        public virtual IList<SPFEEDTL> SPFEEDTLs { get; set; }
    }
}
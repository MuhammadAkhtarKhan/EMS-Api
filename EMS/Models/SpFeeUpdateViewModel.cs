using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.Models
{
    public class SpFeeUpdateViewModel
    {
        public double TRNNO { get; set; }
        public string EMP_NAME { get; set; }
        public string EMP_F_NAME { get; set; }
        public double SPFEE { get; set; }
        public System.DateTime SPDATE { get; set; }
        public double CLASS_TRNNO { get; set; }
        
    }
}
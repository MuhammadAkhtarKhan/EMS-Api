using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.Models
{
    public class EMDTLViewModel
    {
        public double TRNNO { get; set; }
        public double SR { get; set; }
        public string ADDDESC { get; set; }
        public string ADDTYPE { get; set; }

        public virtual EM EM { get; set; }
    }
}
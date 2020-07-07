using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.Models
{
    public class EMDTL1ViewModel
    {
        public double TRNNO { get; set; }
        public double SR { get; set; }
        public string PHNO { get; set; }
        public string PHTYPE { get; set; }
        public Nullable<double> RELTION { get; set; }

        public virtual EM EM { get; set; }
    }
}
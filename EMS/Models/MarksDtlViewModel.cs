using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.Models
{
    public class MarksDtlViewModel
    {
        public double TRNNO { get; set; }
        public double SR { get; set; }
        public Nullable<double> EM_TRNNO { get; set; }
        public  IList<MarksDtl1ViewModel> MARKSDTL1 { get; set; }
    }
}
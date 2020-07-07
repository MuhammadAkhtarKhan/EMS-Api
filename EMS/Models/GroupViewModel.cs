using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.Models
{
    public class GroupViewModel
    {
        public double TRNNO { get; set; }
        public string GRPNAME { get; set; }
        public Nullable<System.DateTime> DT { get; set; }
        public Nullable<double> COMPDTL_TRNNO { get; set; }       
        public  IList<MARKSMSTOLD> MARKSMSTOLDs { get; set; }
         public  IList<PROMMST> PROMMSTs { get; set; }
        public  IList<GRPDTL> GRPDTLs { get; set; }
    }
}
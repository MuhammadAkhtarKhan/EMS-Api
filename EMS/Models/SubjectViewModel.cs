using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.Models
{
    public class SubjectViewModel
    {
        public double TRNNO { get; set; }
        public string SNAME { get; set; }
        public string SCODE { get; set; }
        public double TOTALMARKS { get; set; }
        public  IList<CTTDTL> CTTDTLs { get; set; }
        public  IList<MARKTOTALDTL> MARKTOTALDTLs { get; set; }
        public  IList<GRPMST> GRPMSTs { get; set; }
    }
}
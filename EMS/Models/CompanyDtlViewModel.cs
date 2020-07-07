using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.Models
{
    public class CompanyDtlViewModel
    {
        public double TRNNO { get; set; }
        public string BNAME { get; set; }
        public Nullable<double> C_TRNNO { get; set; }
        public virtual COMPANY COMPANY { get; set; }
    }
}
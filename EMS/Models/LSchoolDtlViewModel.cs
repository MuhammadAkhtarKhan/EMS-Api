using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.Models
{
    public class LSchoolDtlViewModel
    {
        public double TRNNO { get; set; }
        public double SR { get; set; }
        public double EM_TRNNO { get; set; }

        public virtual LSCHOOLMST LSCHOOLMST { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.Models
{
    public class TeacherViewModel
    {
        public double TRNNO { get; set; }
        public string EMP_ID { get; set; }
        public string EMP_NAME { get; set; }
        public System.DateTime DOB { get; set; }
        public Nullable<System.DateTime> DOJ { get; set; }
        public string EMP_F_NAME { get; set; }
        public Nullable<double> BP_TRNNO { get; set; }
        public string ETYPE { get; set; } 
        public string EMAIL { get; set; }
        public string CNIC { get; set; }
        public string SEX { get; set; }
        public Nullable<double> BSAL { get; set; }
        public string FCNIC { get; set; }
        public Nullable<double> CST_TRNNO { get; set; }

        public virtual List<EMDTL> EMDTLs { get; set; }

        public virtual List<EMDTL1> EMDTL1 { get; set; }

        public virtual List<EMDTL2> EMDTL2 { get; set; }
    }
}
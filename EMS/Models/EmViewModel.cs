using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.Models
{
    public class EmViewModel
    {
        public double TRNNO { get; set; }
        public string EMP_ID { get; set; }
        public string EMP_NAME { get; set; }
        public System.DateTime DOB { get; set; }
        public Nullable<System.DateTime> DOJ { get; set; }
        public Nullable<double> CST_TRNNO { get; set; }
        public string EMP_F_NAME { get; set; }
        public Nullable<double> COMP_TRNNO { get; set; }
        public Nullable<double> BP_TRNNO { get; set; }
        public string ETYPE { get; set; }
        public Nullable<double> CL_TRNNO { get; set; }
        public Nullable<double> CL_TRNNO1 { get; set; }
        public string CERTIFICATE { get; set; }
        public Nullable<System.DateTime> PSCHDT { get; set; }
        public string EMAIL { get; set; }
        public Nullable<System.DateTime> ADT { get; set; }
        public string FTYPE { get; set; }
        public Nullable<double> FEE { get; set; }
        public string EMAILST { get; set; }
        public Nullable<double> GRPMST_TRNNO { get; set; }
        public string CNIC { get; set; }
        public Nullable<double> SECMST_TRNNO { get; set; }
        public Nullable<double> SECDTL_SR { get; set; }
        public string SEX { get; set; }
        public Nullable<double> BSAL { get; set; }
        public string FCNIC { get; set; }
        public Nullable<double> ADIMNO { get; set; }
        public string STYPE { get; set; }

        public virtual List<EMDTL> EMDTLs { get; set; }
       
        public virtual List<EMDTL1> EMDTL1 { get; set; }
       
        public virtual List<EMDTL2> EMDTL2 { get; set; }
    }
}
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EMS
{
    using System;
    using System.Collections.Generic;
    
    public partial class LEAVECERTMST
    {
        public double TRNNO { get; set; }
        public Nullable<double> EM_TRNNO { get; set; }
        public System.DateTime LDATE { get; set; }
        public string LCOND { get; set; }
        public string REMARK { get; set; }
        public System.DateTime DPAIDDT { get; set; }
    
        public virtual EM EM { get; set; }
    }
}
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
    
    public partial class SECCHANGE
    {
        public double TRNNO { get; set; }
        public Nullable<double> EM_TRNNO { get; set; }
        public System.DateTime CHDATE { get; set; }
        public Nullable<double> SECDTL_TRNNO { get; set; }
        public Nullable<double> SECDTL_SR { get; set; }
        public Nullable<System.DateTime> EDATE { get; set; }
    
        public virtual EM EM { get; set; }
    }
}
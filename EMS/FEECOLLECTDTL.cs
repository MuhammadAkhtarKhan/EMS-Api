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
    
    public partial class FEECOLLECTDTL
    {
        public double TRNNO { get; set; }
        public string FMONTH { get; set; }
        public Nullable<double> FEETYPE_TRNNO { get; set; }
        public double SR { get; set; }
        public Nullable<double> AMT { get; set; }
        public Nullable<double> FEEMON { get; set; }
    
        public virtual FEECOLLECTMST FEECOLLECTMST { get; set; }
        public virtual FEETYPE FEETYPE { get; set; }
    }
}

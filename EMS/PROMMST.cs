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
    
    public partial class PROMMST
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PROMMST()
        {
            this.PROMDTLs = new HashSet<PROMDTL>();
        }
    
        public double TRNNO { get; set; }
        public Nullable<System.DateTime> PDT { get; set; }
        public Nullable<double> GRPMST_TRNNO { get; set; }
        public Nullable<double> CLASS_TRNNO { get; set; }
        public Nullable<double> FCLASS_TRNNO { get; set; }
        public Nullable<double> SECDTL_TRNNO { get; set; }
        public Nullable<double> SECDTL_SR { get; set; }
    
        public virtual GRPMST GRPMST { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PROMDTL> PROMDTLs { get; set; }
    }
}

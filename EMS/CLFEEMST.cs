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
    
    public partial class CLFEEMST
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CLFEEMST()
        {
            this.CLFEEDTLs = new HashSet<CLFEEDTL>();
        }
    
        public double TRNNO { get; set; }
        public Nullable<double> CLASS_TRNNO { get; set; }
    
        public virtual CLASS CLASS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CLFEEDTL> CLFEEDTLs { get; set; }
    }
}
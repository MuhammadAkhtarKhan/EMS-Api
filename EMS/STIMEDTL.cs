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
    
    public partial class STIMEDTL
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public STIMEDTL()
        {
            this.CTTDTLs = new HashSet<CTTDTL>();
        }
    
        public double TRNNO { get; set; }
        public double SR { get; set; }
        public System.DateTime LECSTIME { get; set; }
        public System.DateTime LECETIME { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CTTDTL> CTTDTLs { get; set; }
        public virtual STIME STIME { get; set; }
    }
}

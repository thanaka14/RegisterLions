//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RegisterLions.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CommitteeOfficer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CommitteeOfficer()
        {
            this.CommitteeDetail = new HashSet<CommitteeDetail>();
        }
    
        public int officer_id { get; set; }
        public string officer_name { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CommitteeDetail> CommitteeDetail { get; set; }
    }
}

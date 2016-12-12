﻿//------------------------------------------------------------------------------
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
    using System.ComponentModel.DataAnnotations;

    public partial class RegionOfficer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RegionOfficer()
        {
            this.ZoneOfficer = new HashSet<ZoneOfficer>();
        }

        public int region_officer_id { get; set; }

        [Display(Name = "ภูมิภาคที่")]
        [Required(ErrorMessage = "บันทึกภูมิภาคที่")]
        public Nullable<int> region_no { get; set; }

        [Display(Name = "ปีบริหาร")]
        [Required(ErrorMessage = "บันทึกปีบริหาร")]
        public Nullable<int> fiscal_year { get; set; }

        [Display(Name = "ชื่อประธานภูมิภาค")]
        [Required(ErrorMessage = "เลือกประธานภูมิภาค")]
        public Nullable<int> member_seq { get; set; }

        [Display(Name = "ปีบริหาร")]
        public string fiscal_year_disp
        {
            get { return fiscal_year + "-" + (fiscal_year + 1); }
        }


        public virtual Member Member { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ZoneOfficer> ZoneOfficer { get; set; }
    }
}

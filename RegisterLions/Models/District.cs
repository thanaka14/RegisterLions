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

    public partial class District
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public District()
        {
            this.Club = new HashSet<Club>();
        }

        [Display(Name = "รหัสภาค")]
        [Required(ErrorMessage = "บันทึกรหัสภาค")]
        public int district_id { get; set; }

        [Display(Name = "ภาค")]
        [Required(ErrorMessage = "บันทึกภาค")]
        public string district_name_thai { get; set; }

        [Display(Name = "ภาค(อังกฤษ)")]
        public string district_name_eng { get; set; }

        [Display(Name = "ภาครวม")]
        [Required(ErrorMessage = "เลือกภาครวม")]
        public Nullable<int> multiple_district_id { get; set; }

        public virtual MultipleDistrict MultipleDistrict { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Club> Club { get; set; }
    }
}
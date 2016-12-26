using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RegisterLions.Models.Metadata
{
    public class RegionOfficerMetadata
    {
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

        
    }
}
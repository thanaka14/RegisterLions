using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RegisterLions.Models.Metadata
{
    public class ZoneOfficerMetadata
    {
        public int zone_officer_id { get; set; }

        [Display(Name = "เขตที่")]
        [Required(ErrorMessage = "บันทึกเขตที่")]
        public Nullable<int> zone_no { get; set; }

        [Display(Name = "ปีบริหาร")]
        [Required(ErrorMessage = "บันทึกปีบริหาร")]
        public Nullable<int> fiscal_year { get; set; }

        [Display(Name = "ประธานภูมิภาค")]
        [Required(ErrorMessage = "เลือกประธานภูมิภาค")]
        public Nullable<int> region_officer_id { get; set; }

        [Display(Name = "ประธานเขต")]
        [Required(ErrorMessage = "เลือกประธานเขต")]
        public Nullable<int> member_seq { get; set; }

        
    }
}
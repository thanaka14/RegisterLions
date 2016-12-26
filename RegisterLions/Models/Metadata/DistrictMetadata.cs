using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RegisterLions.Models.Metadata
{
    public class DistrictMetadata
    {
        [Display(Name = "รหัสภาค")]
        [Required(ErrorMessage = "บันทึกรหัสภาค")]
        public int district_id { get; set; }

        [Display(Name = "ภาค")]
        [Required(ErrorMessage = "บันทึกภาค")]
        [MaxLength(100)]
        public string district_name_thai { get; set; }

        [Display(Name = "ภาค(อังกฤษ)")]
        [MaxLength(100)]
        public string district_name_eng { get; set; }

        [Display(Name = "ภาครวม")]
        [Required(ErrorMessage = "เลือกภาครวม")]
        public Nullable<int> multiple_district_id { get; set; }
    }
}
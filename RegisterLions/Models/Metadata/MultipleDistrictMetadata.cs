using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RegisterLions.Models.Metadata
{
    public class MultipleDistrictMetadata
    {
        [Display(Name = "รหัสภาครวม")]
        [Required(ErrorMessage = "บันทึกรหัสภาครวม")]
        public int multiple_district_id { get; set; }

        [Display(Name = "ภาครวม")]
        [Required(ErrorMessage = "บันทึกภาครวม")]
        [MaxLength(100)]
        public string multiple_district_name_thai { get; set; }

        [Display(Name = "ภาครวม(อังกฤษ)")]
        [MaxLength(100)]
        public string multiple_district_name_eng { get; set; }
    }
}
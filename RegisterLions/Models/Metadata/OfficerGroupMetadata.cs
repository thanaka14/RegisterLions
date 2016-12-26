using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RegisterLions.Models.Metadata
{
    public class OfficerGroupMetadata
    {
        [Display(Name = "รหัสกลุ่ม")]
        public int officer_grp { get; set; }

        [Required(ErrorMessage = "บันทึกกลุ่ม")]
        [MaxLength(100)]
        [Display(Name = "กลุ่ม")]
        public string officer_grp_desc { get; set; }
    }
}
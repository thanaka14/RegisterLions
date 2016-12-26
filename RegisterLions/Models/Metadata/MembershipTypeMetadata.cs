using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RegisterLions.Models.Metadata
{
    public class MembershipTypeMetadata
    {
        [Display(Name = "ประเภทสมาชิก")]
        public int membership_type { get; set; }

        [Display(Name = "ชื่อประเภท")]
        [Required(ErrorMessage = "บันทึกชื่อประเภท")]
        [MaxLength(100)]
        public string membership_desc_thai { get; set; }

        [Display(Name = "ชื่อประเภท(อังกฤษ)")]
        [MaxLength(100)]
        public string membership_desc_eng { get; set; }
    }
}
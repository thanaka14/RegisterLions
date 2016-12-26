using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RegisterLions.Models.Metadata
{
    public class MovementMetadata
    {
        [Display(Name = "รหัสความเคลื่อนไหว")]
        public int move_sts { get; set; }

        [Display(Name = "ความเคลื่อนไหว")]
        [Required(ErrorMessage = "บันทึกความเคลื่อนไหว")]
        [MaxLength(100)]
        public string move_desc { get; set; }
    }
}
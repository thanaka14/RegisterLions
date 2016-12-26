using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RegisterLions.Models.Metadata
{
    public class ClubStatusMetadata
    {
        public int club_sts { get; set; }

        [Display(Name = "สถานะสโมสร")]
        [Required(ErrorMessage = "บันทึกสถานะสโมสร")]
        [MaxLength(50)]
        public string club_status_desc { get; set; }
    }
}
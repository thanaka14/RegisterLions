using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RegisterLions.Models.Metadata
{
    public class ClubOfficerMetadata
    {
        [Display(Name = "ปีบริหาร")]
        [Required(ErrorMessage = "บันทึกปีบริหาร")]
        public Nullable<int> fiscal_year { get; set; }

        [Display(Name = "ชื่อ-นามสกุล")]
        public int member_seq { get; set; }

        [Display(Name = "ตำแหน่ง")]
        public int officer_id { get; set; }
        public int club_officer_id { get; set; }

        [Display(Name = "สโมสร")]
        public Nullable<int> club_id { get; set; }

        [Display(Name = "ลำดับที่")]
        public Nullable<int> seq_no { get; set; }

        
    }
}
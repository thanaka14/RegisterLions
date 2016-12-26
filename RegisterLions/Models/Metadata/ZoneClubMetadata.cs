using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RegisterLions.Models.Metadata
{
    public class ZoneClubMetadata
    {
        public int zone_club_id { get; set; }
        [Display(Name = "ปีบริหาร")]
        [Required(ErrorMessage = "บันทึกปีบริหาร")]
        public Nullable<int> fiscal_year { get; set; }

        [Display(Name = "สโมสร")]
        [Required(ErrorMessage = "เลือกสโมสร")]
        public Nullable<int> club_id { get; set; }

        [Display(Name = "เขตที่")]
        [Required(ErrorMessage = "เลือกเขตที่")]
        public Nullable<int> zone_officer_id { get; set; }

        [Display(Name = "ลำดับที่")]
        [Required(ErrorMessage = "บันทึกลำดับที่")]
        public Nullable<int> club_seq { get; set; }

        
    }
}
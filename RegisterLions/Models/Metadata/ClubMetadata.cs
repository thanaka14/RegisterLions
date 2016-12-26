using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RegisterLions.Models.Metadata
{
    public class ClubMetadata
    {
        [Display(Name = "รหัสสโมสร")]
        [Required(ErrorMessage = "บันทึกรหัสสโมสร")]
        public int club_id { get; set; }

        [Display(Name = "สโมสร")]
        [Required(ErrorMessage = "บันทึกชื่อสโมสร")]
        [MaxLength(100)]
        public string club_name_thai { get; set; }

        [Display(Name = "สโมสร(อังกฤษ)")]
        [MaxLength(100)]
        public string club_name_eng { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d MMM yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "วันที่ก่อตั้ง")]
        //[Required(ErrorMessage = "บันทึกวันที่ก่อตั้ง")]
        public Nullable<System.DateTime> charter_date { get; set; }

        [Display(Name = "สถานที่ประชุม")]
        [MaxLength(400)]
        public string meeting_place { get; set; }



        [Display(Name = "รหัสภาค")]
        [Required(ErrorMessage = "เลือกภาค")]
        public Nullable<int> district_id { get; set; }

        [Display(Name = "สถานะ")]
        [Required(ErrorMessage = "เลือกสถานะ")]
        public Nullable<int> club_sts { get; set; }

        [Display(Name = "รูปภาพ")]
        public byte[] image { get; set; }
    }
}
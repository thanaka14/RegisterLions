using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RegisterLions.Models.Metadata
{
    public class MemberMetadata
    {
        [Display(Name = "รหัสสมาชิก")]
        [Required(ErrorMessage = "บันทึกรหัสสมาชิก")]
        public Nullable<int> member_id { get; set; }

        [Display(Name = "ชื่อ")]
        [MaxLength(50)]
        public string first_name { get; set; }

        [Display(Name = "นามสกุล")]
        [MaxLength(50)]
        public string last_name { get; set; }

        [Display(Name = "เพศ")]
        public string gender { get; set; }

        [Display(Name = "ที่อยู่(อังกฤษ)")]
        [MaxLength(300)]
        public string member_address_eng { get; set; }

        [Display(Name = "อีเมล์")]
        [MaxLength(50)]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string email { get; set; }

        [Display(Name = "เบอร์มือถือ")]
        [MaxLength(50)]
        public string cell_phone { get; set; }

        [Display(Name = "ปีเกิด")]
        public Nullable<int> birth_year { get; set; }

        [Display(Name = "อาชีพ")]
        [MaxLength(100)]
        public string occupation { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d MMM yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "วันที่เป็นสมาชิก")]
        [Required(ErrorMessage = "บันทึกวันที่เป็นสมาชิก")]
        public Nullable<System.DateTime> join_date { get; set; }

        [Display(Name = "สโมสร")]
        [Required(ErrorMessage = "บันทึกสโมสร")]
        public Nullable<int> club_id { get; set; }

        public int member_seq { get; set; }

        [Display(Name = "ประเภทสมาชิก")]
        [Required(ErrorMessage = "บันทึกประเภทสมาชิก")]
        public Nullable<int> membership_type { get; set; }

        [Display(Name = "ชื่อ(อังกฤษ)")]
        [Required(ErrorMessage = "บันทึกชื่ออังกฤษ")]
        [MaxLength(50)]
        public string first_name_eng { get; set; }

        [Display(Name = "นามสกุล(อังกฤษ)")]
        [Required(ErrorMessage = "บันทึกนามสกุลอังกฤษ")]
        [MaxLength(50)]
        public string last_name_eng { get; set; }

        [Display(Name = "ที่อยู่")]
        [MaxLength(300)]
        public string member_address_thai { get; set; }

        [Display(Name = "รูปภาพ")]
        public byte[] image { get; set; }

        [Display(Name = "ผู้แนะนำ")]
        [MaxLength(50)]
        public string sponsor_name { get; set; }
        public Nullable<System.DateTime> upd_date { get; set; }

        [Display(Name = "สมาชิกก่อตั้ง")]
        public string charter_flag { get; set; }

        [Display(Name = "สถานะสมาชิก")]
        public Nullable<int> member_sts { get; set; }

        [Display(Name = "Line ID")]
        [MaxLength(20)]
        public string line_id { get; set; }


    }
}
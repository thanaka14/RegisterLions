using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RegisterLions.Models.Metadata
{
    public class TUserMetadata
    {
        [Display(Name = "รหัสผู้ใช้")]
        [Required(ErrorMessage = "บันทึกรหัสผู้ใช้")]
        [MaxLength(20)]
        public string user_code { get; set; }

        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "รหัสผ่าน")]
        [Required(ErrorMessage = "บันทึกรหัสผ่าน")]
        public string user_pwd { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d MMM yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "วันที่เริ่มต้น")]
        public Nullable<System.DateTime> eff_date { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d MMM yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "วันที่หมดอายุ")]
        public Nullable<System.DateTime> exp_date { get; set; }

        [Display(Name = "ชื่อ-นามสกุล")]
        [Required(ErrorMessage = "เลือกชื่อ-นามสกุลผู้ใช้")]
        public Nullable<int> member_seq { get; set; }


        public Nullable<System.DateTime> upd_date { get; set; }

        [Display(Name = "บทบาท")]
        [Required(ErrorMessage = "เลือกบทบาท")]
        [MaxLength(20)]
        public string role_name { get; set; }

        
    }
}
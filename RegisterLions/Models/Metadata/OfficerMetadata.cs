using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RegisterLions.Models.Metadata
{
    public class OfficerMetadata
    {
        [Display(Name = "รหัสตำแหน่ง")]
        public int officer_id { get; set; }

        [Display(Name = "ตำแหน่ง")]
        [Required(ErrorMessage = "บันทึกตำแหน่ง")]
        [MaxLength(200)]
        public string title { get; set; }

        [Display(Name = "ประเภทตำแหน่ง")]
        [Required(ErrorMessage = "บันทุกประเภทตำแหน่ง")]
        public string officer_type { get; set; }

        [Display(Name = "ตำแหน่ง(อังกฤษ)")]
        [MaxLength(200)]
        public string title_eng { get; set; }

        

        [Display(Name = "กลุ่ม")]
        public Nullable<int> officer_grp { get; set; }

        
    }
}
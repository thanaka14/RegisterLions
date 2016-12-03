﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RegisterLions.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class ClubOfficer
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

        [Display(Name = "ปีบริหาร")]
        public string fiscal_year_disp
        {
            get { return fiscal_year + "-" + (fiscal_year + 1); }
        }

        [Display(Name = "ช่วงวันที่ปีบริหาร")]
        public string fiscal_year_date
        {
            get { return "1/7/" + fiscal_year.ToString() + " 30/6" + (fiscal_year + 1); }
        }

        public virtual Officer Officer { get; set; }
        public virtual Member Member { get; set; }
        public virtual Club Club { get; set; }
    }
}

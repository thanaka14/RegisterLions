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

    public partial class Club
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Club()
        {
            this.CourseDetail = new HashSet<CourseDetail>();
            this.CommitteeDetail = new HashSet<CommitteeDetail>();
            this.MemberMovement = new HashSet<MemberMovement>();
            this.ZoneClub = new HashSet<ZoneClub>();
            this.Member = new HashSet<Member>();
            this.ClubOfficer = new HashSet<ClubOfficer>();
        }

        [Display(Name = "รหัสสโมสร")]
        [Required(ErrorMessage = "บันทึกรหัสสโมสร")]
        public int club_id { get; set; }

        [Display(Name = "สโมสร")]
        [Required(ErrorMessage = "บันทึกชื่อสโมสร")]
        public string club_name_thai { get; set; }

        [Display(Name = "สโมสร(อังกฤษ)")]
        public string club_name_eng { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d MMM yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "วันที่ก่อตั้ง")]
        //[Required(ErrorMessage = "บันทึกวันที่ก่อตั้ง")]
        public Nullable<System.DateTime> charter_date { get; set; }

        [Display(Name = "สถานที่ประชุม")]
        public string meeting_place { get; set; }

        

        [Display(Name = "รหัสภาค")]
        [Required(ErrorMessage = "เลือกภาค")]
        public Nullable<int> district_id { get; set; }

        [Display(Name = "สถานะ")]
        [Required(ErrorMessage = "เลือกสถานะ")]
        public Nullable<int> club_sts { get; set; }
        public byte[] image { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CourseDetail> CourseDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CommitteeDetail> CommitteeDetail { get; set; }
        public virtual District District { get; set; }
        public virtual ClubStatus ClubStatus { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MemberMovement> MemberMovement { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ZoneClub> ZoneClub { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Member> Member { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClubOfficer> ClubOfficer { get; set; }
    }
}
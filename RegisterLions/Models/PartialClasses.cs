using RegisterLions.Models.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RegisterLions.Models
{
    [MetadataType(typeof(ClubMetadata))]
    public partial class Club
    {
    }

    [MetadataType(typeof(ClubOfficerMetadata))]
    public partial class ClubOfficer
    {
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
    }

    [MetadataType(typeof(ClubStatusMetadata))]
    public partial class ClubStatus
    {
    }

    [MetadataType(typeof(DistrictMetadata))]
    public partial class District
    {
    }

    [MetadataType(typeof(MemberMetadata))]
    public partial class Member
    {
        [Display(Name = "ชื่อ นามสกุล")]
        public string full_name
        {
            get
            {
                if (first_name == null) { return "Lion " + first_name_eng + " " + last_name_eng; } else { return "ไลออน " + first_name + " " + last_name; }
            }
        }
        [Display(Name = "ชื่อ นามสกุล")]
        public string full_name_eng
        {
            get { return "Lion " + first_name_eng + " " + last_name_eng; }
        }

        [Display(Name = "ชื่อ นามสกุล")]
        public string full_name_thai
        {
            get { return "ไลออน " + first_name + " " + last_name; }
        }



        public string member_addr
        {
            get
            {
                if (member_address_eng != null) { return member_address_eng; } else { return member_address_thai; }
            }
        }

        public string genderName
        {
            get
            {

                var gName = gender == "M" ? "ชาย"
                : gender == "F" ? "หญิง" : "";
                return gName;
            }
        }
        public string charterName
        {
            get
            {

                var CharterName = charter_flag == "N" ? ""
                : charter_flag == "Y" ? "สมาชิกก่อตั้ง" : "";
                return CharterName;
            }
        }
        public string statusName
        {
            get
            {
                string tMemberSts = null;
                if (member_sts == 1)
                {
                    tMemberSts = "ปกติ";
                }
                else if (member_sts == 2)
                {
                    tMemberSts = "พ้นสภาพ";
                }
                else
                {
                    tMemberSts = "";
                }

                return tMemberSts;

            }
        }
    }

    [MetadataType(typeof(MembershipTypeMetadata))]
    public partial class MembershipType
    {
    }

    [MetadataType(typeof(MovementMetadata))]
    public partial class Movement
    {
    }

    [MetadataType(typeof(MultipleDistrictMetadata))]
    public partial class MultipleDistrict
    {
    }

    [MetadataType(typeof(OfficerMetadata))]
    public partial class Officer
    {
        [Display(Name = "ประเภทตำแหน่ง")]
        public string office_type_desc
        {
            get
            {

                var gName = officer_type == "C" ? "กรรมการสโมสร"
                : officer_type == "D" ? "กรรมการภาค" : "";
                return gName;
            }
        }
    }

    [MetadataType(typeof(OfficerGroupMetadata))]
    public partial class OfficerGroup
    {
    }

    [MetadataType(typeof(RegionOfficerMetadata))]
    public partial class RegionOfficer
    {
        [Display(Name = "ปีบริหาร")]
        public string fiscal_year_disp
        {
            get { return fiscal_year + "-" + (fiscal_year + 1); }
        }
    }

    [MetadataType(typeof(TUserMetadata))]
    public partial class TUser
    {
        [Display(Name = "บทบาท")]
        public string role_name_disp
        {
            get
            {

                var grole_name = role_name == "User" ? "ผู้ใช้งาน"
                : role_name == "Admin" ? "ผู้ดูแลระบบ" : "";
                return grole_name;
            }
        }
    }

    [MetadataType(typeof(ZoneClubMetadata))]
    public partial class ZoneClub
    {
        [Display(Name = "ปีบริหาร")]
        public string fiscal_year_disp
        {
            get { return fiscal_year + "-" + (fiscal_year + 1); }
        }
    }

    [MetadataType(typeof(ZoneOfficerMetadata))]
    public partial class ZoneOfficer
    {
        [Display(Name = "ปีบริหาร")]
        public string fiscal_year_disp
        {
            get { return fiscal_year + "-" + (fiscal_year + 1); }
        }
    }
    [MetadataType(typeof(MemberMovementMetadata))]
    public partial class MemberMovement
    {
        
    }


}
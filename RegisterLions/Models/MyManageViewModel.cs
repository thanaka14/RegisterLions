using System;
using System.ComponentModel.DataAnnotations;

namespace RegisterLions.Models
{
   
    public class ChangePwd

    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "รหัสผ่านปัจจุบัน")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "รหัสผ่านใหม่")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ยืนยันรหัสผ่าน")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
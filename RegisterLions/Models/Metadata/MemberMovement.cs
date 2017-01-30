using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace RegisterLions.Models.Metadata
{
    public class MemberMovementMetadata
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d MMM yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "วันที่ทำรายการ")]        
        public Nullable<System.DateTime> hist_date { get; set; }

        
    }
}
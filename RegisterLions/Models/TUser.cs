//------------------------------------------------------------------------------
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
    
    public partial class TUser
    {
        public string user_code { get; set; }
        public string user_pwd { get; set; }
        public Nullable<System.DateTime> eff_date { get; set; }
        public Nullable<System.DateTime> exp_date { get; set; }
        public Nullable<int> member_seq { get; set; }
        public Nullable<System.DateTime> upd_date { get; set; }
        public string role_name { get; set; }
    
        public virtual Member Member { get; set; }
    }
}

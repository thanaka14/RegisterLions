using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RegisterLions.Models
{
    public class User
    {
        public string user_code { get; set; }

        public string user_pwd { get; set; }
        
        public Nullable<System.DateTime> eff_date { get; set; }
       
        public Nullable<System.DateTime> exp_date { get; set; }
        public Nullable<int> member_seq { get; set; }
        public string first_name_eng { get; set; }

        public string last_name_eng { get; set; }

        public string first_name { get; set; }

        public string last_name { get; set; }

        public string club_name_thai { get; set; }

        public int? club_id { get; set; }

        public int district_id { get; set; }
        public string district_name_thai { get; set; }

        public string full_name { get
            {
                if (first_name != null) { return first_name + " " + last_name; } else { return first_name_eng + " " + last_name_eng; }
            }
        }
        


    }
}
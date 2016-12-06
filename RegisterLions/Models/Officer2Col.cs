using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RegisterLions.Models
{
    public class Officer2Col
    {
        
        public string imgsrc1 { get; set; }
        public string title1 { get; set; }
        public string name1 { get; set; }
        public string imgsrc2 { get; set; }
        public string title2 { get; set; }
        public string name2 { get; set; }

        public Officer2Col(string pimgsrc1,string ptitle1,string pname1, string pimgsrc2, string ptitle2, string pname2)
        {
            
            imgsrc1 = pimgsrc1;
            imgsrc2 = pimgsrc2;
            title1 = ptitle1;
            title2 = ptitle2;
            name1 = pname1;
            name2 = pname2;
        }

    }
}
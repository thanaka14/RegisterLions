using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RegisterLions.Models
{
    public class MemberReport
    {
        [Key]       
        public int? club_id { get; set; }
        public string club_name { get; set; }
        public int newMember { get; set; }
        public int transferMember { get; set; }
        public int reinstallMember { get; set; }
        public int dropMember { get; set; }
        public int totalMember { get; set; }

        public MemberReport(int? pclub_id, string pclub_name, int pnewMemer, int ptransferMemer, int preinstallMember, int pdropMember, int ptotMember)
        {
            
            club_id = pclub_id;
            club_name = pclub_name;
            newMember = pnewMemer;
            transferMember = ptransferMemer;
            reinstallMember = preinstallMember;
            dropMember = pdropMember;
            totalMember = ptotMember;
        }

       
    }
}
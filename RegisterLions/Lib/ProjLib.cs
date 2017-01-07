using RegisterLions.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RegisterLions.Lib
{
    public class ProjLib
    {
        //private RegisterLionsEntities db = new RegisterLionsEntities();
        public  static void writeTransactionLog(int? pMemberSeq,string pViewName,int? pClub_ID)
        {
            ApplicationLog applicationLog = new ApplicationLog();
            applicationLog.log_date = DateTime.Now;
            applicationLog.ip_addr = getIPAddress();
            applicationLog.member_seq = pMemberSeq;
            applicationLog.view_name = pViewName;
            applicationLog.club_id = pClub_ID;
            RegisterLionsEntities db = new RegisterLionsEntities();
            db.ApplicationLogs.Add(applicationLog);
            db.SaveChanges();
            
        }
        public static string getIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }
        public static string chkUserCode(string first_name_eng, string last_name_eng)
        {
            RegisterLionsEntities db = new RegisterLionsEntities();
            string user_code = "";
            int j = 0;
            IEnumerable<TUser> chkuser = new List<TUser>();
            do
            {
                if (j == 0)
                {
                    user_code = first_name_eng.ToLower() + "." + last_name_eng.ToLower().Substring(0, 1);
                }
                else
                {
                    user_code = first_name_eng.ToLower() + "." + last_name_eng.ToLower().Substring(0, 1) + last_name_eng.ToLower().Substring(j, 1);
                }
                chkuser = (from u1 in db.TUsers where u1.user_code == user_code select u1).ToList();
                if (chkuser.Count() != 0)
                {
                    j++;
                }
            } while (chkuser.Count() != 0);
            return user_code;
        }
        public static int getFiscalYear(int pYear, int pMonth)
        {
            int fiscal_year = 0;
            if (pMonth >= 7 && pMonth <= 12)
            {

                fiscal_year = pYear + 543;
            }
            else
            {

                fiscal_year = (pYear + 543 - 1);
            }
            return fiscal_year;
        
    }



    }
    
}
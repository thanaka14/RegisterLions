using RegisterLions.Models;
using System;

namespace RegisterLions.Lib
{
    public class WriteLog
    {
        public  void TransactionLog(int? pMemberSeq,string pViewName,int? pClub_ID)
        {
            ApplicationLog applicationLog = new ApplicationLog();
            applicationLog.log_date = DateTime.Now;
            applicationLog.ip_addr = GetIPAddress();
            applicationLog.member_seq = pMemberSeq;
            applicationLog.view_name = pViewName;
            applicationLog.club_id = pClub_ID;
            RegisterLionsEntities db = new RegisterLionsEntities();
            db.ApplicationLogs.Add(applicationLog);
            db.SaveChanges();
            
        }
        public string GetIPAddress()
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

    }
    
}
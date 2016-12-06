using System.Linq;
using System.Web.Mvc;
using RegisterLions.Models;
using RegisterLions.Lib;
using System;
using System.Collections.Generic;

namespace RegisterLions.Controllers
{
    public class HomeController : Controller
    {
        private RegisterLionsEntities db = new RegisterLionsEntities();

        public ActionResult Index()
        {
            int fiscal_year = (DateTime.Now.Year) + 543;
            var clubofficer = (from co in db.ClubOfficers
                               where co.fiscal_year == fiscal_year 
                               && (co.officer_id >= 13 && co.officer_id <= 18)
                               join o in db.Officers on co.officer_id equals o.officer_id
                               select co
                               ).OrderBy(x=>x.Officer.officer_grp).ThenBy(x=>x.seq_no);
            List<Officer2Col> officer2col = new List<Officer2Col>();
            int i = 0;
            string imgsrc="",title="",name = "";
            foreach (var c1 in clubofficer)
            {
                i++;
                
                string imgsrctmp = "";
                if (c1.Member.image != null)
                {
                    var base64 = Convert.ToBase64String(c1.Member.image);
                    imgsrctmp = String.Format("data:image/gif;base64,{0}", base64);
                }else
                {
                    imgsrctmp = "~/Pictures/login.jpg";
                }
                if (i % 2 == 1)
                {
                    imgsrc = imgsrctmp;
                    title = c1.Officer.title;
                    name = c1.Member.full_name;
                }else
                {

                    officer2col.Add(new Officer2Col(imgsrc, title, name, imgsrctmp, c1.Officer.title, c1.Member.full_name));
                    imgsrc = "";
                    title = "";
                    name = "";
                }
            }
            // Write log to table TransactionLog
            WriteLog writeLog = new WriteLog();
            writeLog.TransactionLog(0, "Index", 0);
            ViewBag.officer = officer2col;
            return View();
            
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            // Write log to table TransactionLog
            WriteLog writeLog = new WriteLog();
            writeLog.TransactionLog(0, "About", 0);
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            // Write log to table TransactionLog
            WriteLog writeLog = new WriteLog();
            writeLog.TransactionLog(0, "Contact", 0);

            return View();
        }
        [Authorize]
        public ActionResult Welcome()
        {
            var identity = (System.Web.HttpContext.Current.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
            //var Tuser = (from u in db.TUsers
            //             join m in db.Members on u.member_seq equals m.member_seq
            //             join c in db.Clubs on m.club_id equals c.club_id
            //             join d in db.MembershipTypes on m.membership_type equals d.membership_type
            //             join e in db.Districts on c.district_id equals e.district_id
            //             where u.user_code.Equals(HttpContext.User.Identity.Name)
            //             select u).FirstOrDefault();
            var member = (from m in db.Members where m.member_seq == identity.User.member_seq select m).FirstOrDefault(); 
            ViewBag.member_seq = identity.User.member_seq;


            return View(member);
           
        }
    }
}
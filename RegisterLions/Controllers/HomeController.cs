﻿using System.Linq;
using System.Web.Mvc;
using RegisterLions.Models;
using RegisterLions.Lib;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace RegisterLions.Controllers
{
    public class HomeController : Controller
    {
        private RegisterLionsEntities db = new RegisterLionsEntities();

        public ActionResult Index()
        {
            int fiscal_year = ProjLib.getFiscalYear(DateTime.Now.Year, DateTime.Now.Month);

            var clubofficer = (from co in db.ClubOfficers
                               where co.fiscal_year == fiscal_year 
                               && (co.officer_id >= 13 && co.officer_id <= 18)
                               join o in db.Officers on co.officer_id equals o.officer_id
                               select co
                               ).OrderBy(x=>x.Officer.officer_grp).ThenBy(x=>x.seq_no);
            List<Officer2Col> officer2col = new List<Officer2Col>();
            int i = 0;
            string imgsrc="",title="",name = "";
            if (clubofficer.ToList().Count() != 0)
            {
                foreach (var c1 in clubofficer)
                {
                    i++;

                    string imgsrctmp = "";
                    if (c1.Member.image != null)
                    {
                        var base64 = Convert.ToBase64String(c1.Member.image);
                        imgsrctmp = String.Format("data:image/gif;base64,{0}", base64);
                    }
                    else
                    {
                        imgsrctmp = "~/Pictures/login.jpg";
                    }
                    if (i % 2 == 1)
                    {
                        imgsrc = imgsrctmp;
                        title = c1.Officer.title;
                        name = c1.Member.full_name;
                    }
                    else
                    {

                        officer2col.Add(new Officer2Col(imgsrc, title, name, imgsrctmp, c1.Officer.title, c1.Member.full_name));
                        imgsrc = "";
                        title = "";
                        name = "";
                    }
                }
            }
            // Write log to table TransactionLog
            //ProjLib projlib = new ProjLib();
            ProjLib.writeTransactionLog(0, "Index", 0);
            ViewBag.officer = officer2col;
            // Statistic for Member
            DateTime startDate = DateTime.ParseExact(ProjLib.getBegFiscalYear(DateTime.Now.Year, DateTime.Now.Month), "yyyy/MM/dd", CultureInfo.InvariantCulture);
            DateTime endDate = DateTime.ParseExact(ProjLib.getEndFiscalYear(DateTime.Now.Year, DateTime.Now.Month), "yyyy/MM/dd", CultureInfo.InvariantCulture);
            var newMember = (from t in db.MemberMovements
                                  where (t.move_sts != 4) && (t.hist_date >= startDate && t.hist_date <= endDate)                                 
                                  select t
                                  );
            
            var dropMember = (from t in db.MemberMovements
                                  where t.move_sts == 4 && (t.hist_date >= startDate && t.hist_date <= endDate)                                  
                                  select t
                                  );
            var totMember = (from m in db.Members
                          where m.member_sts == 1                          
                          select m);

            ViewBag.newMember = newMember.ToList().Count();
            ViewBag.dropMember = dropMember.ToList().Count();
            ViewBag.netMember = newMember.ToList().Count() - dropMember.ToList().Count();
            ViewBag.totMember = totMember.ToList().Count();
            ViewBag.fiscal_year = "ปีบริหาร " + ProjLib.displayFiscalYear(DateTime.Now.Year, DateTime.Now.Month);

            //club status

            return View();
            
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            // Write log to table TransactionLog
            //ProjLib projlib = new ProjLib();
            ProjLib.writeTransactionLog(0, "About", 0);
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            // Write log to table TransactionLog
            //ProjLib projlib = new ProjLib();
            ProjLib.writeTransactionLog(0, "Contact", 0);

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
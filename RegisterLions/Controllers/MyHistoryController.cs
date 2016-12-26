using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RegisterLions.Models;
using RegisterLions.Lib;

namespace RegisterLions.Controllers
{
    [Authorize]
    public class MyHistoryController : Controller
    {
        private RegisterLionsEntities db = new RegisterLionsEntities();
        // GET: MyHistory
        public ActionResult OfficerHistory()
        {
            var identity = (System.Web.HttpContext.Current.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
            //identity.User.member_seq
            var clubOfficer = (from co in db.ClubOfficers
                               where co.member_seq == identity.User.member_seq
                               join c in db.Clubs on co.club_id equals c.club_id                               
                               join o in db.Officers on co.officer_id equals o.officer_id
                               
                               select co).OrderBy(x=>x.fiscal_year).ThenBy(x=>x.officer_id);
            // Write log to table TransactionLog
            //ProjLib projlib = new ProjLib();
            ProjLib.TransactionLog(identity.User.member_seq, "OfficerHistory", identity.User.club_id);
            return View(clubOfficer);
        }
        public ActionResult DistrictOfficerHistory(int? member_seq)
        {
            var identity = (System.Web.HttpContext.Current.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
            //identity.User.member_seq
            if (member_seq == null)
            {
                member_seq = identity.User.member_seq;
            }
            var clubOfficer = (from co in db.ClubOfficers
                               where co.member_seq == member_seq
                               join c in db.Clubs on co.club_id equals c.club_id
                               join o in db.Officers on co.officer_id equals o.officer_id

                               select co).OrderBy(x => x.fiscal_year).ThenBy(x => x.officer_id);
            var c_member = (from c1 in db.ClubOfficers
                            join c2 in db.Members on c1.member_seq equals c2.member_seq
                                 select new
                                 {
                                     member_seq = c1.member_seq,
                                     first_name = c2.first_name,
                                     last_name = c2.last_name,
                                     first_name_eng = c2.first_name_eng,
                                     last_name_eng = c2.last_name_eng
                                 }).Distinct().ToList();
            var c_member2 = from m in c_member
                            select new
                              {
                                  m.member_seq,
                                  full_name = string.Format("{0} {1}", m.first_name_eng, m.last_name_eng)
                              };
            ViewBag.member_seq = new SelectList(c_member2.OrderBy(x=>x.full_name), "member_seq", "full_name", member_seq);
            // Write log to table TransactionLog
            //ProjLib projlib = new ProjLib();
            ProjLib.TransactionLog(identity.User.member_seq, "DistrictOfficerHistory", identity.User.club_id);
            //ViewBag.member_seq = c_member2;
            return View(clubOfficer);
        }
    }
}
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
    public class MyLionsClubsController : Controller
    {
        
        private RegisterLionsEntities db = new RegisterLionsEntities();
        // GET: MyLionsClubs        
        //public ActionResult Membership(string searchString, int? memberSts)
            public ActionResult Membership(string searchString)
        {

            #region sub query
            //var Club = (from u in db.TUsers
            //            where u.user_code.Equals(HttpContext.User.Identity.Name)
            //            join m in db.Members on u.member_seq equals m.member_seq
            //            select m.club_id);



            //var member = (from m in db.Members
            //              where Club.Contains(m.club_id)
            //              join c in db.Clubs on m.club_id equals c.club_id
            //              join mt in db.MembershipTypes on m.membership_type equals mt.membership_type
            //              select m);

            #endregion
            var identity = (System.Web.HttpContext.Current.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
            var tClub_id = identity.User.club_id;
            var member = (from m in db.Members
                          where m.club_id == tClub_id
                          join c in db.Clubs on m.club_id equals c.club_id
                          join mt in db.MembershipTypes on m.membership_type equals mt.membership_type
                          select m);
            //List<SelectListItem> lstMemberSts = new List<SelectListItem>();


            //lstMemberSts.Add(new SelectListItem() { Text = "สมาชิกสถานะปกติ", Value = "1" });
            //lstMemberSts.Add(new SelectListItem() { Text = "สมาชิกสถานะพ้นสภาพ", Value = "2" });
            //this.ViewBag.memberSts = new SelectList(lstMemberSts, "Value", "Text");

            if (!String.IsNullOrEmpty(searchString))
            {                
                member = member.Where(x => x.first_name.Contains(searchString) || x.last_name.Contains(searchString)
                    || x.first_name_eng.Contains(searchString) || x.last_name_eng.Contains(searchString));

            }
            //if ((memberSts != null))
            //{               
            //    member = member.Where(x => x.member_sts == memberSts);
            //}else
            //{
            //    this.ViewBag.memberSts = new SelectList(lstMemberSts, "Value", "Text","1");                               
            //}
            member = member.Where(x => x.member_sts == 1);
            ViewBag.MemberCount = member.Count();
            // Write log to table TransactionLog
            WriteLog writeLog = new WriteLog();
            writeLog.TransactionLog(identity.User.member_seq, "Membership", identity.User.club_id);
            return View(member.ToList());
        }

        // GET: ClubOfficers/Details/5
        public ActionResult MembershipDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            var identity = (System.Web.HttpContext.Current.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
            // Write log to table TransactionLog
            WriteLog writeLog = new WriteLog();
            writeLog.TransactionLog(identity.User.member_seq, "MembershipDetails", identity.User.club_id);
            return View(member);
        }
        // GET: ClubInfo
        public ActionResult ClubInfo()
        {
            #region sub query
            //var Club = (from u in db.TUsers
            //            where u.user_code.Equals(HttpContext.User.Identity.Name)
            //            join m in db.Members on u.member_seq equals m.member_seq
            //            select m.club_id);
            //ViewBag.ClubID = null;


            //var clubs = (from c in db.Clubs where Club.Contains(c.club_id) select c);
            #endregion
            var identity = (System.Web.HttpContext.Current.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
            var tClub_id = identity.User.club_id;
            Club club = db.Clubs.Find(tClub_id);
            if (club == null)
            {
                return HttpNotFound();
            }

            // Write log to table TransactionLog
            WriteLog writeLog = new WriteLog();
            writeLog.TransactionLog(identity.User.member_seq, "ClubInfo", identity.User.club_id);

            return View(club);
        }
        //GET: Officer
        public ActionResult Officer(int? searchFisicalYear,int? club_id)
        {
            var identity = (System.Web.HttpContext.Current.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
            var tClub_id = identity.User.club_id;
            var FiscalYear = db.ClubOfficers.Select(x => new { x.fiscal_year }).Distinct();
            //ViewBag.searchString = null;
            List<SelectListItem> lstFiscalYear = new List<SelectListItem>();
            foreach (var d in FiscalYear)
            {
                lstFiscalYear.Add(new SelectListItem() { Text = "ปีบริหาร "+d.fiscal_year + "-" + (d.fiscal_year + 1), Value = d.fiscal_year.ToString()  });  
            }
            
            var clubOfficer = (from a in db.ClubOfficers
                               //where a.club_id == tClub_id
                               join b in db.Members on a.member_seq equals b.member_seq                                                              
                               join d in db.Officers on a.officer_id equals d.officer_id
                               where d.officer_type.Equals("C") //&& a.fiscal_year == fiscal_year
                               orderby a.officer_id
                               select a);
            var tClub = (from c in db.Clubs                         
                         join zc in db.ZoneClubs on c.club_id equals zc.club_id                         
                         select new
                         {
                             club_id = c.club_id,
                             club_name_thai = c.club_name_thai,
                             fiscal_year= zc.fiscal_year
                         }
                           ).Distinct();

            if (searchFisicalYear == null)
            { searchFisicalYear = (DateTime.Now.Year) + 543; }
            if (club_id == null)
            {
                club_id = tClub_id;
               
            }

            clubOfficer = clubOfficer.Where(x => x.club_id == club_id);
            ViewBag.searchFisicalYear = new SelectList(lstFiscalYear, "Value", "Text", searchFisicalYear);
            SelectList listClub = new SelectList(tClub.OrderBy(x => x.club_name_thai), "club_id", "club_name_thai");
            ViewBag.club_id = new SelectList(tClub.Where(X => X.fiscal_year == searchFisicalYear).OrderBy(x => x.club_name_thai), "club_id", "club_name_thai", club_id);
            clubOfficer = clubOfficer.Where(x=>x.fiscal_year== searchFisicalYear);
           
            // Write log to table TransactionLog
            WriteLog writeLog = new WriteLog();
            writeLog.TransactionLog(identity.User.member_seq, "Officer", identity.User.club_id);
            return View(clubOfficer.ToList());
        }
    }
}
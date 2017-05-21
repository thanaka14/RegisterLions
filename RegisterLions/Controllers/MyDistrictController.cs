using RegisterLions.Models;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Web.Mvc;
using System.Globalization;
using RegisterLions.Lib;

namespace RegisterLions.Controllers
{
    
    public class MyDistrictController : Controller
    {
        private RegisterLionsEntities db = new RegisterLionsEntities();

        // GET: MyDistrict
        [Authorize]
        public ActionResult Membership(string searchString, int? club_id)
        {    
            var identity = (System.Web.HttpContext.Current.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
            var tDistrict_id = identity.User.district_id;
            var member = (from m in db.Members
                          where m.member_sts==1                      
                          join c in db.Clubs on m.club_id equals c.club_id                          
                          join d in db.Districts on c.district_id equals d.district_id                          
                          where d.district_id == tDistrict_id
                          select m);
            if (!String.IsNullOrEmpty(searchString))
            {
                member = member.Where(x => x.first_name.Contains(searchString) || x.last_name.Contains(searchString)
                    || x.first_name_eng.Contains(searchString) || x.last_name_eng.Contains(searchString));

            }
            
            ViewBag.club_id = new SelectList(db.Clubs.OrderBy(x=>x.club_name_thai).Where(x => x.club_sts == 1 && x.district_id == identity.User.district_id).OrderBy(x => x.club_name_thai), "club_id", "club_name_thai");
            ViewBag.clubName = null;
            if (club_id != null)
            {  
                member = member.Where(x => x.club_id == club_id);
                var clubName = (from c in db.Clubs where c.club_id == club_id select c);
                foreach (var c in clubName)
                {

                    ViewBag.clubName = c.club_name_thai;                    
                }
               
            }
            if (String.IsNullOrEmpty(searchString)&& club_id == null)
            {
                member = member.Where(x => x.first_name.Contains("-"));

            }
            else
            {
                member = member.OrderBy(x => x.first_name_eng).ThenBy(x => x.last_name_eng);

                // Write log to table TransactionLog
                //ProjLib projlib = new ProjLib();
                ProjLib.writeTransactionLog(identity.User.member_seq, "Membership", identity.User.club_id);

            }
            ViewBag.MemberCount = member.Count();
            return View(member.ToList());
        }
        public ActionResult ClubList(int? club_id,int? searchFisicalYear,int? club_sts)
        {
            #region Part1
            //var tDistrict = (from d in db.Districts
            //                 join c in db.Clubs on d.district_id equals c.district_id
            //                 join m in db.Members on c.club_id equals m.club_id
            //                 join u in db.TUsers on m.member_seq equals u.member_seq
            //                 where u.user_code.Equals(HttpContext.User.Identity.Name)
            //                 select d.district_id);


            //var club = (from c in db.Clubs
            //            where c.club_sts != 3
            //            join d in db.Districts on c.district_id equals d.district_id
            //            where tDistrict.Contains(d.district_id)                        
            //            select c);
            #endregion
            //--var identity = (System.Web.HttpContext.Current.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
            //--var tDistrict_id = identity.User.district_id;
            #region part2
            //var club = (from c in db.Clubs
            //            where c.club_sts != 3
            //            //join d in db.Districts on c.district_id equals d.district_id
            //            //where d.district_id == tDistrict_id
            //            select c);
            ////--ViewBag.club_id = new SelectList(db.Clubs.OrderBy(x=>x.club_name_thai).Where(x => x.club_sts == 1 && x.district_id == identity.User.district_id).OrderBy(x => x.club_name_thai), "club_id", "club_name_thai");
            //ViewBag.club_id = new SelectList(db.Clubs.Where(x=>x.club_sts!=3).OrderBy(x => x.club_name_thai), "club_id", "club_name_thai");
            //if (club_id != null)
            //{
            //    club = club.Where(x => x.club_id == club_id);
            //}
            //club = club.OrderBy(x => x.club_name_thai);
            //ViewBag.ClubCount =club.Count();
            //// Write log to table TransactionLog
            //projlib projlib = new projlib();
            ////--projlib.TransactionLog(identity.User.member_seq, "ClubList", identity.User.club_id);
            //projlib.TransactionLog(0, "ClubList", 0);
            #endregion           
            var zoneClub = (from c1 in db.ZoneClubs
                            join c2 in db.ZoneOfficers on c1.zone_officer_id equals c2.zone_officer_id
                            join c3 in db.Clubs on c1.club_id equals c3.club_id
                            select c1
                            );
            var c_fiscal_year = (from c1 in db.ZoneClubs
                                 select new
                                 {
                                     fiscal_year = c1.fiscal_year
                                 }).Distinct().OrderByDescending(x=>x.fiscal_year).ToList();

            int fiscal_year_default = 0;
            var flag = true;

            foreach (var c in c_fiscal_year)
            {
                if (flag)
                {
                    fiscal_year_default = (int)c.fiscal_year;
                    flag = false;
                }
            }
            var fiscal_year = from m in c_fiscal_year
                              select new
                              {
                                  m.fiscal_year,
                                  fiscal_year_disp = string.Format("ปีบริหาร {0}-{1}", m.fiscal_year, m.fiscal_year + 1)
                              };
            if (searchFisicalYear == null)
            {
                //searchFisicalYear = ProjLib.getFiscalYear(DateTime.Now.Year, DateTime.Now.Month);
                searchFisicalYear = fiscal_year_default;
            }
            if (club_id != null)
            {
                zoneClub = zoneClub.Where(x => x.club_id == club_id);
            }
            if(club_sts == null)
            {
                zoneClub = zoneClub.Where(x => x.Club.club_sts == 1);
            }
            else { zoneClub = zoneClub.Where(x => x.Club.club_sts == club_sts); }

            //fiscal_year = fiscal_year.OrderByDescending(x => x.fiscal_year);

            ViewBag.searchFisicalYear = new SelectList(fiscal_year, "fiscal_year", "fiscal_year_disp", searchFisicalYear);            
            zoneClub = zoneClub.Where(x => x.fiscal_year == searchFisicalYear);

            
            zoneClub = zoneClub.OrderBy(x => x.ZoneOfficer.zone_no).ThenBy(x => x.club_seq);
           
            ViewBag.zoneClub = zoneClub.ToList();
            ViewBag.club_id = new SelectList(db.Clubs.Where(x => x.club_sts != 3).OrderBy(x => x.club_name_thai), "club_id", "club_name_thai");
            //ProjLib projlib = new ProjLib();
            //projlib.TransactionLog(identity.User.member_seq, "RegionZone", identity.User.club_id);
            ProjLib.writeTransactionLog(0, "ClubList", 0);
            // return View(regionOfficer.ToList());
            return View();
        }

        
        public ActionResult DroppedMember(int? club_id,int? fiscal_year)
        {
            //var identity = (System.Web.HttpContext.Current.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
            //var tDistrict_id = identity.User.district_id;
            var intYear = 0;
            var intMth = 0;
            if (fiscal_year == null)
            {
                intYear = DateTime.Now.Year;
                intMth = DateTime.Now.Month;
                fiscal_year = ProjLib.getFiscalYear(DateTime.Now.Year, DateTime.Now.Month);
            }
            else
            {
                intYear = (int)fiscal_year-543;
                intMth = 7;
            }
            //ViewBag.fiscal_year = "ปีบริหาร " + displayFiscalYear(DateTime.Now.Year, DateTime.Now.Month);
            //ViewBag.fiscal_year = "ปีบริหาร " + displayFiscalYear(intYear, intMth);
            DateTime startDate = DateTime.ParseExact(ProjLib.getBegFiscalYear(intYear, intMth), "yyyy/MM/dd", CultureInfo.InvariantCulture);
            DateTime endDate = DateTime.ParseExact(ProjLib.getEndFiscalYear(intYear, intMth), "yyyy/MM/dd", CultureInfo.InvariantCulture);


            

            
             var memberMovement = (from t in db.MemberMovements
                                  where t.move_sts==4 && (t.hist_date >= startDate && t.hist_date <= endDate)                                  
                                  join mm in db.Movements on t.move_sts equals mm.move_sts
                                  join m in db.Members on t.member_seq equals m.member_seq
                                  join c in db.Clubs on t.club_id equals c.club_id
                                  //join d in db.Districts on c.district_id equals d.district_id
                                  //where d.district_id == tDistrict_id
                                  select t
                                  );
            if (club_id != null)
            {
                memberMovement = memberMovement.Where(x => x.club_id == club_id);
            }
            var tClub = (from c in db.Clubs
                        // where c.district_id == identity.User.district_id
                         join t in db.MemberMovements on c.club_id equals t.club_id
                         where t.move_sts == 4 && (t.hist_date >= startDate && t.hist_date <= endDate)
                         select new
                         {
                             club_id = c.club_id,
                             club_name_thai = c.club_name_thai
                         }
                           ).Distinct() ;            
            SelectList listClub = new SelectList(tClub.OrderBy(x=>x.club_name_thai), "club_id", "club_name_thai");
            ViewBag.club_id = listClub;
            ViewBag.MemberCount = memberMovement.Count();
            var c_fiscal_year = (from m1 in db.MemberMovements
                                 where m1.move_sts==4 && (m1.hist_date.Value.Year >= 2016 && m1.hist_date.Value.Month>=7)
                                 select new
                                 {
                                     fiscal_year = m1.hist_date.Value.Year+543
                                 }).Distinct().ToList();
            var c_fiscal_year2 = from m in c_fiscal_year
                                 select new
                                 {
                                     m.fiscal_year,
                                     fiscal_year_disp = string.Format("ปีบริหาร {0}-{1}", m.fiscal_year, (m.fiscal_year) + 1)
                                 };
            ViewBag.fiscal_year = new SelectList(c_fiscal_year2.OrderByDescending(x => x.fiscal_year), "fiscal_year", "fiscal_year_disp", fiscal_year);
            // Write log to table TransactionLog
            //ProjLib projlib = new ProjLib();
            //ProjLib.writeTransactionLog(identity.User.member_seq, "DroppedMember", identity.User.club_id);
            ProjLib.writeTransactionLog(0, "DroppedMember", 0);
            return View(memberMovement);
        }

        
        public ActionResult NewMember(int? club_id, int? fiscal_year)
        {
            //var identity = (System.Web.HttpContext.Current.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
            //var tDistrict_id = identity.User.district_id;
            var intYear = 0;
            var intMth = 0;
            if (fiscal_year == null)
            {
                intYear = DateTime.Now.Year;
                intMth = DateTime.Now.Month;
                fiscal_year = ProjLib.getFiscalYear(DateTime.Now.Year, DateTime.Now.Month);
            }
            else
            {
                intYear = (int)fiscal_year-543 ;
                intMth = 7;
            }

            //ViewBag.fiscal_year = "ปีบริหาร " + displayFiscalYear(DateTime.Now.Year, DateTime.Now.Month);
            //ViewBag.fiscal_year = "ปีบริหาร " + displayFiscalYear(intYear, intMth);
            DateTime startDate = DateTime.ParseExact(ProjLib.getBegFiscalYear(intYear, intMth), "yyyy/MM/dd", CultureInfo.InvariantCulture);
            DateTime endDate = DateTime.ParseExact(ProjLib.getEndFiscalYear(intYear,intMth), "yyyy/MM/dd", CultureInfo.InvariantCulture);
            var memberMovement = (from t in db.MemberMovements
                                  where (t.move_sts != 4) && (t.hist_date >= startDate && t.hist_date <= endDate)
                                  join m in db.Members on t.member_seq equals m.member_seq
                                  join c in db.Clubs on t.club_id equals c.club_id
                                  //join d in db.Districts on c.district_id equals d.district_id
                                  //where d.district_id == tDistrict_id
                                  select t
                                  );
            
           
            
            if (club_id != null)
            {
                memberMovement = memberMovement.Where(x => x.club_id == club_id);
            }

            var tClub = (from c in db.Clubs
                         //where c.district_id== identity.User.district_id
                         join t in db.MemberMovements on c.club_id equals t.club_id
                         where (t.move_sts != 4) && (t.hist_date >= startDate && t.hist_date <= endDate)
                         select new
                         {
                             club_id = c.club_id,
                             club_name_thai = c.club_name_thai
                         }
                           ).Distinct();

            SelectList listClub = new SelectList(tClub.OrderBy(x=>x.club_name_thai), "club_id", "club_name_thai");
                ViewBag.club_id = listClub;

            var c_fiscal_year = (from m1 in db.MemberMovements
                                 where m1.move_sts != 4 && (m1.hist_date.Value.Year >= 2016 && m1.hist_date.Value.Month>=7)
                                 select new
                                 {
                                     fiscal_year = m1.hist_date.Value.Year+543
                                 }).Distinct().ToList();
            var c_fiscal_year2 = from m in c_fiscal_year
                                 select new
                                 {
                                     m.fiscal_year,
                                     fiscal_year_disp = string.Format("ปีบริหาร {0}-{1}", m.fiscal_year, (m.fiscal_year) + 1)
                                 };
            ViewBag.fiscal_year = new SelectList(c_fiscal_year2.OrderByDescending(x => x.fiscal_year), "fiscal_year", "fiscal_year_disp", fiscal_year);

            ViewBag.MemberCount = memberMovement.Count();
            // Write log to table TransactionLog
            //ProjLib projlib = new ProjLib();
            //ProjLib.writeTransactionLog(identity.User.member_seq, "NewMember", identity.User.club_id);
            ProjLib.writeTransactionLog(0, "NewMember", 0);
            return View(memberMovement);
        }

        
        public ActionResult MemberReport()
        {
            //var identity = (System.Web.HttpContext.Current.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
            ViewBag.fiscal_year = "ปีบริหาร " + ProjLib.displayFiscalYear(DateTime.Now.Year, DateTime.Now.Month);
            DateTime startDate = DateTime.ParseExact(ProjLib.getBegFiscalYear(DateTime.Now.Year, DateTime.Now.Month), "yyyy/MM/dd", CultureInfo.InvariantCulture);
            DateTime endDate = DateTime.ParseExact(ProjLib.getEndFiscalYear(DateTime.Now.Year,DateTime.Now.Month), "yyyy/MM/dd", CultureInfo.InvariantCulture);

           
            var totMemberList = (from m in db.Members
                              where (m.member_sts == 1)
                              join c in db.Clubs on m.club_id equals c.club_id
                             // where c.district_id == identity.User.district_id
                                 select m)
                              .GroupBy(t => t.club_id).Select(grp => new {club_id = grp.Key, totMember = grp.Count()}                              
                            ).OrderBy(x => x.club_id);
            var newMemberList = (from mm in db.MemberMovements
                                 where (mm.hist_date >= startDate && mm.hist_date <= endDate && mm.move_sts == 1)
                                 join c in db.Clubs on mm.club_id equals c.club_id
                                // where c.district_id == identity.User.district_id
                                 select mm)
                                 .GroupBy(t => t.club_id).Select(grp => new { club_id = grp.Key, newMember = grp.Count() })
                                 .OrderBy(x => x.club_id).ToList();
            var transferMemberList = (from mm in db.MemberMovements
                                 where (mm.hist_date >= startDate && mm.hist_date <= endDate && mm.move_sts == 2)
                                      join c in db.Clubs on mm.club_id equals c.club_id
                                     // where c.district_id == identity.User.district_id
                                      select mm)
                                .GroupBy(t => t.club_id).Select(grp => new { club_id = grp.Key, transferMember = grp.Count() })
                                .OrderBy(x => x.club_id).ToList();
            var reinstallMemberList = (from mm in db.MemberMovements
                                      where (mm.hist_date >= startDate && mm.hist_date <= endDate && mm.move_sts == 3)
                                       join c in db.Clubs on mm.club_id equals c.club_id
                                      // where c.district_id == identity.User.district_id
                                       select mm)
                                .GroupBy(t => t.club_id).Select(grp => new { club_id = grp.Key, reinstallMember = grp.Count() })
                                .OrderBy(x => x.club_id).ToList();
            var dropMemberList = (from mm in db.MemberMovements
                                       where (mm.hist_date >= startDate && mm.hist_date <= endDate && mm.move_sts == 4)
                                  join c in db.Clubs on mm.club_id equals c.club_id
                                  //where c.district_id == identity.User.district_id
                                  select mm)
                                .GroupBy(t => t.club_id).Select(grp => new { club_id = grp.Key, dropMember = grp.Count() })
                                .OrderBy(x => x.club_id).ToList();
            List<MemberReport> memberReport = new List<MemberReport>();
           // memberReport.Add(new MemberReport(2559, 1, 1, 1, 1, 1, 1));
            //MemberReport memberReport = new MemberReport();
            foreach (var t in totMemberList)
            {
                
                var newMemer = 0;           
                if (newMemberList.Find(x => x.club_id == t.club_id) != null)
                {
                    newMemer = newMemberList.Find(x => x.club_id == t.club_id).newMember;
                }
                var transferMemer = 0;
                if (transferMemberList.Find(x => x.club_id == t.club_id) != null)
                {
                    transferMemer = transferMemberList.Find(x => x.club_id == t.club_id).transferMember;
                }
                var reinstallMember = 0;
                if (reinstallMemberList.Find(x => x.club_id == t.club_id) != null)
                {
                    reinstallMember = reinstallMemberList.Find(x => x.club_id == t.club_id).reinstallMember;
                }
                var dropMember = 0;
                if (dropMemberList.Find(x => x.club_id == t.club_id) != null)
                {
                    dropMember = dropMemberList.Find(x => x.club_id == t.club_id).dropMember;
                }
                Club club = db.Clubs.Find(t.club_id);
                var tClubName="";
                if (club != null)
                {
                    tClubName = club.club_name_thai;
                    if (club.club_sts == 2)
                    {
                        tClubName = tClubName+"("+club.ClubStatus.club_status_desc+")";
                    }
                    
                }                
                memberReport.Add(new MemberReport(t.club_id, tClubName,newMemer, transferMemer, reinstallMember, dropMember, t.totMember,0,0));
            }
            
            ViewBag.MemberReport = memberReport.OrderBy(x => x.club_name);

            var zoneClub = (from c1 in db.ZoneClubs
                            join c2 in db.ZoneOfficers on c1.zone_officer_id equals c2.zone_officer_id
                            select c1
                            );
            var searchFisicalYear = ProjLib.getFiscalYear(DateTime.Now.Year, DateTime.Now.Month);
            zoneClub = zoneClub.Where(x => x.fiscal_year == searchFisicalYear);


            zoneClub = zoneClub.OrderBy(x => x.ZoneOfficer.zone_no).ThenBy(x => x.club_seq);

            ViewBag.zoneClub = zoneClub.ToList();

            // Write log to table TransactionLog
            //ProjLib projlib = new ProjLib();
            //ProjLib.writeTransactionLog(identity.User.member_seq, "MemberReport", identity.User.club_id);
            ProjLib.writeTransactionLog(0, "MemberReport", 0);
            var member = (from m in db.Members where m.member_sts == 1 select m);
            ViewBag.MemberCount = member.Count();
            return View();
        }
        public ActionResult RegionZone(int? searchFisicalYear)
        {
            //var identity = (System.Web.HttpContext.Current.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
            //var regionOfficer = (from r in db.RegionOfficers
            //                     //join m in db.Members on r.member_seq equals m.member_seq
            //                     //join c in db.Clubs on m.club_id equals c.club_id
            //                     //where c.district_id == identity.User.district_id
            //                     select r);
            //var zoneOfficer = (from z in db.ZoneOfficers
            //                   join r in db.RegionOfficers on z.region_officer_id equals r.region_officer_id
            //                   //join m in db.Members on z.member_seq equals m.member_seq
            //                   //join c in db.Clubs on m.club_id equals c.club_id
            //                   //where c.district_id == identity.User.district_id
            //                   select z);
            var zoneClub = (from c1 in db.ZoneClubs
                            join c2 in db.ZoneOfficers on c1.zone_officer_id equals c2.zone_officer_id
                            //join c3 in db.Clubs on c1.club_id equals c3.club_id
                            //where c3.district_id == identity.User.district_id
                            select c1
                            );

            var c_fiscal_year = (from c1 in db.ZoneClubs                              
                                select new
                                {                                    
                                    fiscal_year = c1.fiscal_year
                                }).Distinct().OrderByDescending(x=>x.fiscal_year).ToList();
            int fiscal_year_default = 0;
            var flag = true;

            foreach (var c in c_fiscal_year)
            {
                if (flag)
                {
                    fiscal_year_default = (int)c.fiscal_year;
                    flag = false;
                }
            }

            var fiscal_year = from m in c_fiscal_year
                             select new
                             {
                                 m.fiscal_year,
                                 fiscal_year_disp = string.Format("ปีบริหาร {0}-{1}", m.fiscal_year, m.fiscal_year + 1)
                             };
            if (searchFisicalYear == null)
            {
                //searchFisicalYear = ProjLib.getFiscalYear(DateTime.Now.Year, DateTime.Now.Month);
                searchFisicalYear = fiscal_year_default;
            }
            var clubOfficer = (from c in db.ClubOfficers                               
                               where c.fiscal_year == searchFisicalYear
                               join o in db.Officers on c.officer_id equals o.officer_id
                               where o.officer_type == "C" && o.officer_id >= 1 && o.officer_id <= 3
                               select c);

            fiscal_year = fiscal_year.OrderByDescending(x => x.fiscal_year);

            ViewBag.searchFisicalYear = new SelectList(fiscal_year, "fiscal_year", "fiscal_year_disp", searchFisicalYear);
            //    regionOfficer = regionOfficer.Where(x => x.fiscal_year == searchFisicalYear);
            //    zoneOfficer = zoneOfficer.Where(x => x.fiscal_year == searchFisicalYear);
            zoneClub = zoneClub.Where(x => x.fiscal_year == searchFisicalYear);

            //regionOfficer = regionOfficer.OrderBy(x => x.fiscal_year).ThenBy(x => x.region_no);
            //zoneOfficer= zoneOfficer.OrderBy(x => x.fiscal_year).ThenBy(x => x.zone_no);
            zoneClub = zoneClub.OrderBy(x => x.fiscal_year).ThenBy(x => x.ZoneOfficer.zone_no).ThenBy(x => x.club_seq);
            //ViewBag.zoneOfficer = zoneOfficer.ToList();
            ViewBag.zoneClub = zoneClub.ToList();
            ViewBag.clubOfficer = clubOfficer.OrderBy(x=>x.officer_id).ToList();
            //ProjLib projlib = new ProjLib();
            //projlib.TransactionLog(identity.User.member_seq, "RegionZone", identity.User.club_id);
            ProjLib.writeTransactionLog(0, "RegionZone", 0);
            return View();
        }
        public ActionResult DistrictOfficer(int? fiscal_year, string searchString,int? officer_grp)
        {
            //var identity = (System.Web.HttpContext.Current.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
            //identity.User.member_seq
            
            var clubOfficer = (from c1 in db.ClubOfficers
                               where c1.fiscal_year == fiscal_year
                               join m in db.Members on c1.member_seq equals m.member_seq
                               join o in db.Officers on c1.officer_id equals o.officer_id
                               where o.officer_type=="D" 
                               select c1);
            if (officer_grp != null)
            {
                clubOfficer = clubOfficer.Where(x => x.Officer.officer_grp == officer_grp);
            }
            var regionOfficer = (from r in db.RegionOfficers
                                 where r.fiscal_year == fiscal_year
                                 select r);
            var zoneOfficer = (from r in db.ZoneOfficers
                                 where r.fiscal_year == fiscal_year
                                 select r);
            if (!String.IsNullOrEmpty(searchString))
            {
                clubOfficer = clubOfficer.Where(x => x.Member.first_name.Contains(searchString) || x.Member.last_name.Contains(searchString)
                    || x.Member.first_name_eng.Contains(searchString) || x.Member.last_name_eng.Contains(searchString));

            }
            clubOfficer = clubOfficer.OrderBy(x => x.Officer.officer_grp).ThenBy(x=>x.seq_no);

            var c_fiscal_year = (from c1 in db.ClubOfficers
                                 join c2 in db.Officers on c1.officer_id equals c2.officer_id
                                 where c2.officer_type=="D"
                            select new
                            {
                                fiscal_year=c1.fiscal_year                                
                            }).Distinct().OrderByDescending(x=>x.fiscal_year).ToList();

            int fiscal_year_default = 0;
            var flag = true;

            foreach (var c in c_fiscal_year)
            {
                if (flag)
                {
                    fiscal_year_default = (int)c.fiscal_year;
                    flag = false;
                }
            }

            if (fiscal_year == null)
            {
                //fiscal_year = ProjLib.getFiscalYear(DateTime.Now.Year, DateTime.Now.Month);
                fiscal_year = fiscal_year_default;
            }

            var c_fiscal_year2 = from m in c_fiscal_year
                            select new
                            {
                                m.fiscal_year,
                                fiscal_year_disp = string.Format("ปีบริหาร {0}-{1}", m.fiscal_year, (m.fiscal_year)+1)
                            };
            ViewBag.fiscal_year = new SelectList(c_fiscal_year2.OrderByDescending(x => x.fiscal_year), "fiscal_year", "fiscal_year_disp", fiscal_year);
            ViewBag.officer_grp = new SelectList(db.OfficerGroups.OrderBy(x => x.officer_grp), "officer_grp", "officer_grp_desc", officer_grp);
            ViewBag.zoneOfficer = zoneOfficer;
            ViewBag.regionOfficer = regionOfficer;
            // Write log to table TransactionLog
            //ProjLib projlib = new ProjLib();
            //projlib.TransactionLog(identity.User.member_seq, "DistrictOfficer", identity.User.club_id);
            ProjLib.writeTransactionLog(0, "DistrictOfficer", 0);
            //ViewBag.member_seq = c_member2;
            return View(clubOfficer);
        }
        
        

    }
}
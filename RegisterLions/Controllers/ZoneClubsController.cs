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
    [Authorize(Roles = "Admin")]
    public class ZoneClubsController : Controller
    {
        private RegisterLionsEntities db = new RegisterLionsEntities();

        // GET: ZoneClubs
        public ActionResult Index(string searchString)
        {
            //var zoneClub = db.ZoneClubs.Include(z => z.ZoneOfficer).Include(z => z.Club).Include(z => z.Member);
            var zoneClub = db.ZoneClubs.Include(z => z.ZoneOfficer).Include(z => z.Club);
            if (!String.IsNullOrEmpty(searchString))
            {
                zoneClub = zoneClub.Where(x => x.Club.club_name_eng.Contains(searchString) || x.Club.club_name_thai.Contains(searchString)               
                );

            }
            return View(zoneClub.OrderBy(x=>x.fiscal_year).ThenBy(x=>x.ZoneOfficer.zone_no).ThenBy(x=>x.club_seq).ToList());
        }

        
        // GET: ZoneClubs/Create
        public ActionResult Create()
        {
            var identity = (System.Web.HttpContext.Current.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
            var c_memberZone = (from m in db.Members
                                  join r in db.ZoneOfficers on m.member_seq equals r.member_seq
                                join c in db.Clubs on m.club_id equals c.club_id
                                where c.district_id == identity.User.district_id
                                select new
                                  {
                                      zone_officer_id = r.zone_officer_id,
                                      first_name = m.first_name,
                                      last_name = m.last_name,
                                      zone_no = r.zone_no,
                                      fiscal_year = r.fiscal_year
                                  }).ToList();
            var memberZone = from m in c_memberZone
                             select new
                               {
                                   m.zone_officer_id,
                                   full_name = string.Format("{0} {1} เขตที่ {2} ปีบริหาร {3}-{4}", m.first_name, m.last_name, m.zone_no, m.fiscal_year, m.fiscal_year+1)
                               };
            ViewBag.zone_officer_id = new SelectList(memberZone.OrderBy(x=>x.zone_officer_id), "zone_officer_id", "full_name");
            ViewBag.club_id = new SelectList(db.Clubs.OrderBy(x=>x.club_name_thai), "club_id", "club_name_thai");
            //ViewBag.member_seq = new SelectList(db.Members, "member_seq", "first_name");
            return View();
        }

        // POST: ZoneClubs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "zone_club_id,fiscal_year,club_id,zone_officer_id,club_seq,member_seq")] ZoneClub zoneClub)
        {
            var identity = (System.Web.HttpContext.Current.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
            if (ModelState.IsValid)
            {
                db.ZoneClubs.Add(zoneClub);
                db.SaveChanges();
                // Write log to table TransactionLog
                WriteLog writeLog = new WriteLog();
                writeLog.TransactionLog(identity.User.member_seq, "CreateZoneClub", identity.User.club_id);
                return RedirectToAction("Index");
            }

            var c_memberZone = (from m in db.Members
                                join r in db.ZoneOfficers on m.member_seq equals r.member_seq
                                join c in db.Clubs on m.club_id equals c.club_id
                                where c.district_id == identity.User.district_id
                                select new
                                {
                                    zone_officer_id = r.zone_officer_id,
                                    first_name = m.first_name,
                                    last_name = m.last_name,
                                    zone_no = r.zone_no,
                                    fiscal_year = r.fiscal_year
                                }).ToList();
            var memberZone = from m in c_memberZone
                             select new
                             {
                                 m.zone_officer_id,
                                 full_name = string.Format("{0} {1} เขตที่ {2} ปีบริหาร {3}-{4}", m.first_name, m.last_name, m.zone_no, m.fiscal_year, m.fiscal_year + 1)
                             };            
            ViewBag.zone_officer_id = new SelectList(memberZone.OrderBy(x => x.zone_officer_id), "zone_officer_id", "full_name", zoneClub.zone_officer_id);
            ViewBag.club_id = new SelectList(db.Clubs.OrderBy(x => x.club_name_thai), "club_id", "club_name_thai", zoneClub.club_id);

            return View(zoneClub);
        }

        // GET: ZoneClubs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ZoneClub zoneClub = db.ZoneClubs.Find(id);
            if (zoneClub == null)
            {
                return HttpNotFound();
            }
            var identity = (System.Web.HttpContext.Current.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
            var c_memberZone = (from m in db.Members
                                join r in db.ZoneOfficers on m.member_seq equals r.member_seq
                                join c in db.Clubs on m.club_id equals c.club_id
                                where c.district_id == identity.User.district_id
                                select new
                                {
                                    zone_officer_id = r.zone_officer_id,
                                    first_name = m.first_name,
                                    last_name = m.last_name,
                                    zone_no = r.zone_no,
                                    fiscal_year = r.fiscal_year
                                }).ToList();
            var memberZone = from m in c_memberZone
                             select new
                             {
                                 m.zone_officer_id,
                                 full_name = string.Format("{0} {1} เขตที่ {2} ปีบริหาร {3}-{4}", m.first_name, m.last_name, m.zone_no, m.fiscal_year, m.fiscal_year + 1)
                             };
            ViewBag.zone_officer_id = new SelectList(memberZone.OrderBy(x => x.zone_officer_id), "zone_officer_id", "full_name", zoneClub.zone_officer_id);
            ViewBag.club_id = new SelectList(db.Clubs.OrderBy(x => x.club_name_thai), "club_id", "club_name_thai", zoneClub.club_id);
            return View(zoneClub);
        }

        // POST: ZoneClubs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "zone_club_id,fiscal_year,club_id,zone_officer_id,club_seq,member_seq")] ZoneClub zoneClub)
        {
            var identity = (System.Web.HttpContext.Current.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
            if (ModelState.IsValid)
            {
                db.Entry(zoneClub).State = EntityState.Modified;
                db.SaveChanges();
                // Write log to table TransactionLog
                WriteLog writeLog = new WriteLog();
                writeLog.TransactionLog(identity.User.member_seq, "EditZoneClub", identity.User.club_id);
                return RedirectToAction("Index");
            }
            
            var c_memberZone = (from m in db.Members
                                join r in db.ZoneOfficers on m.member_seq equals r.member_seq
                                join c in db.Clubs on m.club_id equals c.club_id
                                where c.district_id == identity.User.district_id
                                select new
                                {
                                    zone_officer_id = r.zone_officer_id,
                                    first_name = m.first_name,
                                    last_name = m.last_name,
                                    zone_no = r.zone_no,
                                    fiscal_year = r.fiscal_year
                                }).ToList();
            var memberZone = from m in c_memberZone
                             select new
                             {
                                 m.zone_officer_id,
                                 full_name = string.Format("{0} {1} เขตที่ {2} ปีบริหาร {3}-{4}", m.first_name, m.last_name, m.zone_no, m.fiscal_year, m.fiscal_year + 1)
                             };
           

            ViewBag.zone_officer_id = new SelectList(memberZone.OrderBy(x => x.zone_officer_id), "zone_officer_id", "full_name", zoneClub.zone_officer_id);
            ViewBag.club_id = new SelectList(db.Clubs.OrderBy(x => x.club_name_thai), "club_id", "club_name_thai", zoneClub.club_id);
            return View(zoneClub);
        }

        // GET: ZoneClubs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ZoneClub zoneClub = db.ZoneClubs.Find(id);
            if (zoneClub == null)
            {
                return HttpNotFound();
            }
            return View(zoneClub);
        }

        // POST: ZoneClubs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var identity = (System.Web.HttpContext.Current.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
            ZoneClub zoneClub = db.ZoneClubs.Find(id);
            db.ZoneClubs.Remove(zoneClub);
            db.SaveChanges();
            // Write log to table TransactionLog
            WriteLog writeLog = new WriteLog();
            writeLog.TransactionLog(identity.User.member_seq, "DeleteZoneClub", identity.User.club_id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

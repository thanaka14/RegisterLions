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
    public class ZoneOfficersController : Controller
    {
        private RegisterLionsEntities db = new RegisterLionsEntities();

        // GET: ZoneOfficers
        public ActionResult Index(string searchString)
        {
            var zoneOfficer = db.ZoneOfficers.Include(z => z.Member).Include(z => z.RegionOfficer);
            if (!String.IsNullOrEmpty(searchString))
            {
                zoneOfficer = zoneOfficer.Where(x => x.Member.first_name_eng.Contains(searchString) || x.Member.last_name_eng.Contains(searchString)
                || x.Member.first_name.Contains(searchString) || x.Member.last_name.Contains(searchString)
                );

            }
            return View(zoneOfficer.OrderBy(x=>x.fiscal_year).ThenBy(x=>x.RegionOfficer.region_no).ThenBy(x=>x.zone_no).ToList());
        }

        

        // GET: ZoneOfficers/Create
        public ActionResult Create()
        {
            var identity = (System.Web.HttpContext.Current.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
            var memberZone = (from m in db.Members
                          join co in db.ClubOfficers on m.member_seq equals co.member_seq
                          where co.officer_id == 22
                            join c in db.Clubs on m.club_id equals c.club_id
                            where c.district_id == identity.User.district_id
                            select m

                             ).OrderBy(x => x.first_name);

            var c_memberRegion = (from m in db.Members
                                join r in db.RegionOfficers on m.member_seq equals r.member_seq
                                  join c in db.Clubs on m.club_id equals c.club_id
                                  where c.district_id == identity.User.district_id
                                  select new
                                {
                                    region_officer_id = r.region_officer_id,
                                    first_name = m.first_name,
                                    last_name = m.last_name  ,
                                    region_no=r.region_no                                 
                                }).ToList();
            var memberRegion = from m in c_memberRegion
                                 select new
                                 {
                                     m.region_officer_id,
                                     full_name = string.Format("{0} {1} ภาคที่ {2}", m.first_name, m.last_name,m.region_no)
                                 };

                             

            ViewBag.member_seq = new SelectList(memberZone, "member_seq", "full_name");
            ViewBag.region_officer_id = new SelectList(memberRegion, "region_officer_id", "full_name");
            return View();
        }

        // POST: ZoneOfficers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "zone_officer_id,zone_no,fiscal_year,region_officer_id,member_seq")] ZoneOfficer zoneOfficer)
        {
            var identity = (System.Web.HttpContext.Current.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
            if (ModelState.IsValid)
            {
                db.ZoneOfficers.Add(zoneOfficer);
                db.SaveChanges();
                // Write log to table TransactionLog
                // ProjLib projlib = new ProjLib();
                ProjLib.writeTransactionLog(identity.User.member_seq, "CreateZoneOfficer", identity.User.club_id);
                return RedirectToAction("Index");
            }
            var memberZone = (from m in db.Members
                              join co in db.ClubOfficers on m.member_seq equals co.member_seq
                              where co.officer_id == 22
                              join c in db.Clubs on m.club_id equals c.club_id
                              where c.district_id == identity.User.district_id
                              select m

                             ).OrderBy(x => x.first_name);

            var c_memberRegion = (from m in db.Members
                                  join r in db.RegionOfficers on m.member_seq equals r.member_seq
                                  join c in db.Clubs on m.club_id equals c.club_id
                                  where c.district_id == identity.User.district_id
                                  select new
                                  {
                                      region_officer_id = r.region_officer_id,
                                      first_name = m.first_name,
                                      last_name = m.last_name,
                                      region_no = r.region_no
                                  }).ToList();
            var memberRegion = from m in c_memberRegion
                               select new
                               {
                                   m.region_officer_id,
                                   full_name = string.Format("{0} {1} ภาคที่ {2}", m.first_name, m.last_name, m.region_no)
                               };


            ViewBag.member_seq = new SelectList(memberZone, "member_seq", "full_name", zoneOfficer.member_seq);
            ViewBag.region_officer_id = new SelectList(memberRegion, "region_officer_id", "full_name", zoneOfficer.region_officer_id);
            return View(zoneOfficer);
        }

        // GET: ZoneOfficers/Edit/5
        public ActionResult Edit(int? id)
        {
            var identity = (System.Web.HttpContext.Current.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ZoneOfficer zoneOfficer = db.ZoneOfficers.Find(id);
            if (zoneOfficer == null)
            {
                return HttpNotFound();
            }
            var memberZone = (from m in db.Members
                              join co in db.ClubOfficers on m.member_seq equals co.member_seq
                              where co.officer_id == 22
                              join c in db.Clubs on m.club_id equals c.club_id
                              where c.district_id == identity.User.district_id
                              select m

                             ).OrderBy(x => x.first_name);

            var c_memberRegion = (from m in db.Members
                                  join r in db.RegionOfficers on m.member_seq equals r.member_seq
                                  join c in db.Clubs on m.club_id equals c.club_id
                                  where c.district_id == identity.User.district_id
                                  select new
                                  {
                                      region_officer_id = r.region_officer_id,
                                      first_name = m.first_name,
                                      last_name = m.last_name,
                                      region_no = r.region_no
                                  }).ToList();
            var memberRegion = from m in c_memberRegion
                               select new
                               {
                                   m.region_officer_id,
                                   full_name = string.Format("{0} {1} ภาคที่ {2}", m.first_name, m.last_name, m.region_no)
                               };
            ViewBag.member_seq = new SelectList(memberZone, "member_seq", "full_name", zoneOfficer.member_seq);
            ViewBag.region_officer_id = new SelectList(memberRegion, "region_officer_id", "full_name", zoneOfficer.region_officer_id);
            return View(zoneOfficer);
        }

        // POST: ZoneOfficers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "zone_officer_id,zone_no,fiscal_year,region_officer_id,member_seq")] ZoneOfficer zoneOfficer)
        {
            var identity = (System.Web.HttpContext.Current.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
            if (ModelState.IsValid)
            {
                db.Entry(zoneOfficer).State = EntityState.Modified;
                db.SaveChanges();
                // Write log to table TransactionLog
                //ProjLib projlib = new ProjLib();
                ProjLib.writeTransactionLog(identity.User.member_seq, "EditZoneOfficer", identity.User.club_id);
                return RedirectToAction("Index");
            }
            var memberZone = (from m in db.Members
                              join co in db.ClubOfficers on m.member_seq equals co.member_seq
                              where co.officer_id == 22
                              join c in db.Clubs on m.club_id equals c.club_id
                              where c.district_id == identity.User.district_id
                              select m

                            ).OrderBy(x => x.first_name);

            var c_memberRegion = (from m in db.Members
                                  join r in db.RegionOfficers on m.member_seq equals r.member_seq
                                  join c in db.Clubs on m.club_id equals c.club_id
                                  where c.district_id == identity.User.district_id
                                  select new
                                  {
                                      region_officer_id = r.region_officer_id,
                                      first_name = m.first_name,
                                      last_name = m.last_name,
                                      region_no = r.region_no
                                  }).ToList();
            var memberRegion = from m in c_memberRegion
                               select new
                               {
                                   m.region_officer_id,
                                   full_name = string.Format("{0} {1} ภาคที่ {2}", m.first_name, m.last_name, m.region_no)
                               };
            ViewBag.member_seq = new SelectList(memberZone, "member_seq", "full_name", zoneOfficer.member_seq);
            ViewBag.region_officer_id = new SelectList(memberRegion, "region_officer_id", "full_name", zoneOfficer.region_officer_id);
            return View(zoneOfficer);
        }

        // GET: ZoneOfficers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ZoneOfficer zoneOfficer = db.ZoneOfficers.Find(id);
            if (zoneOfficer == null)
            {
                return HttpNotFound();
            }
            return View(zoneOfficer);
        }

        // POST: ZoneOfficers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var identity = (System.Web.HttpContext.Current.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
            ZoneOfficer zoneOfficer = db.ZoneOfficers.Find(id);
            db.ZoneOfficers.Remove(zoneOfficer);
            db.SaveChanges();
            // Write log to table TransactionLog
            //ProjLib projlib = new ProjLib();
            ProjLib.writeTransactionLog(identity.User.member_seq, "DeleteZoneOfficer", identity.User.club_id);
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

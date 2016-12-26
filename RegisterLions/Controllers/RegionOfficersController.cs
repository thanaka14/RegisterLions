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
    public class RegionOfficersController : Controller
    {
        private RegisterLionsEntities db = new RegisterLionsEntities();

        // GET: RegionOfficers
        public ActionResult Index(string searchString)
        {
            var regionOfficer = db.RegionOfficers.Include(r => r.Member);
            if (!String.IsNullOrEmpty(searchString))
            {
                regionOfficer = regionOfficer.Where(x => x.Member.first_name_eng.Contains(searchString) || x.Member.last_name_eng.Contains(searchString)
                || x.Member.first_name.Contains(searchString) || x.Member.last_name.Contains(searchString)
                );

            }
            return View(regionOfficer.OrderBy(x=>x.fiscal_year).ThenBy(x=>x.region_no).ToList());
        }

        // GET: RegionOfficers/Create
        public ActionResult Create()
        {
            var identity = (System.Web.HttpContext.Current.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
            var member = (from m in db.Members
                              join co in db.ClubOfficers on m.member_seq equals co.member_seq
                          where co.officer_id == 21
                          join c in db.Clubs on m.club_id equals c.club_id
                              where c.district_id == identity.User.district_id
                          select m

                              ).OrderBy(x=>x.first_name);
            ViewBag.member_seq = new SelectList(member, "member_seq", "full_name");
            return View();
        }

        // POST: RegionOfficers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "region_officer_id,region_no,fiscal_year,member_seq")] RegionOfficer regionOfficer)
        {
            var identity = (System.Web.HttpContext.Current.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
            if (ModelState.IsValid)
            {
                db.RegionOfficers.Add(regionOfficer);
                db.SaveChanges();
                // Write log to table TransactionLog
                //ProjLib projlib = new ProjLib();
                ProjLib.TransactionLog(identity.User.member_seq, "CreateRegionOfficer", identity.User.club_id);
                return RedirectToAction("Index");
            }

            var member = (from m in db.Members
                          join co in db.ClubOfficers on m.member_seq equals co.member_seq
                          where co.officer_id == 21
                          join c in db.Clubs on m.club_id equals c.club_id
                          where c.district_id == identity.User.district_id
                          select m

                               ).OrderBy(x => x.first_name);

            ViewBag.member_seq = new SelectList(member, "member_seq", "full_name", regionOfficer.member_seq);
            return View(regionOfficer);
        }

        // GET: RegionOfficers/Edit/5
        public ActionResult Edit(int? id)
        {
            var identity = (System.Web.HttpContext.Current.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegionOfficer regionOfficer = db.RegionOfficers.Find(id);
            if (regionOfficer == null)
            {
                return HttpNotFound();
            }
            var member = (from m in db.Members
                          join co in db.ClubOfficers on m.member_seq equals co.member_seq
                          where co.officer_id == 21
                          join c in db.Clubs on m.club_id equals c.club_id
                          where c.district_id == identity.User.district_id
                          select m

                              ).OrderBy(x => x.first_name);
            ViewBag.member_seq = new SelectList(member, "member_seq", "full_name", regionOfficer.member_seq);
            return View(regionOfficer);
        }

        // POST: RegionOfficers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "region_officer_id,region_no,fiscal_year,member_seq")] RegionOfficer regionOfficer)
        {
            var identity = (System.Web.HttpContext.Current.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
            if (ModelState.IsValid)
            {
                db.Entry(regionOfficer).State = EntityState.Modified;
                db.SaveChanges();
                // Write log to table TransactionLog
                //ProjLib projlib = new ProjLib();
                ProjLib.TransactionLog(identity.User.member_seq, "EditRegionOfficer", identity.User.club_id);
                return RedirectToAction("Index");
            }
            var member = (from m in db.Members
                          join co in db.ClubOfficers on m.member_seq equals co.member_seq
                          where co.officer_id == 21
                          join c in db.Clubs on m.club_id equals c.club_id
                          where c.district_id == identity.User.district_id
                          select m

                              ).OrderBy(x => x.first_name);
            ViewBag.member_seq = new SelectList(member, "member_seq", "full_name", regionOfficer.member_seq);
            return View(regionOfficer);
        }

        // GET: RegionOfficers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegionOfficer regionOfficer = db.RegionOfficers.Find(id);
            if (regionOfficer == null)
            {
                return HttpNotFound();
            }
            return View(regionOfficer);
        }

        // POST: RegionOfficers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var identity = (System.Web.HttpContext.Current.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
            RegionOfficer regionOfficer = db.RegionOfficers.Find(id);
            db.RegionOfficers.Remove(regionOfficer);
            db.SaveChanges();
            // Write log to table TransactionLog
            //ProjLib projlib = new ProjLib();
            ProjLib.TransactionLog(identity.User.member_seq, "DeleteRegionOfficer", identity.User.club_id);
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

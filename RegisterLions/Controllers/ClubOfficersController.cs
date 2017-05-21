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
    [Authorize(Roles ="Admin")]
    public class ClubOfficersController : Controller
    {
        private RegisterLionsEntities db = new RegisterLionsEntities();

        // GET: ClubOfficers
        public ActionResult Index(string searchString)
        {
            var clubOfficer = (from a in db.ClubOfficers
                               join b in db.Members on a.member_seq equals b.member_seq
                               join c in db.Clubs on a.club_id equals c.club_id
                               join d in db.Officers on a.officer_id equals d.officer_id
                               orderby a.fiscal_year, c.club_name_thai, a.officer_id
                               select a);
            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    clubOfficer = clubOfficer.Where(x => x.Member.first_name_eng.Contains(searchString) || x.Member.last_name_eng.Contains(searchString) ||
            //             x.Member.last_name.Contains(searchString) || x.Member.first_name.Contains(searchString) || x.Member.Club.club_name_thai.Contains(searchString) ||
            //             x.Officer.title.Contains(searchString) || x.Officer.title_eng.Contains(searchString)||x.fiscal_year.ToString().Contains(searchString));
            //}
            //else
            //{
            //    clubOfficer = clubOfficer.Where(x => x.Member.first_name_eng.Contains(searchString));
            //}

            clubOfficer = clubOfficer.Where(x => x.Member.first_name_eng.Contains(searchString) || x.Member.last_name_eng.Contains(searchString) ||
                        x.Member.last_name.Contains(searchString) || x.Member.first_name.Contains(searchString) || x.Member.Club.club_name_thai.Contains(searchString) ||
                        x.Officer.title.Contains(searchString) || x.Officer.title_eng.Contains(searchString) || x.fiscal_year.ToString().Contains(searchString));

            return View(clubOfficer.ToList());
        }

        

        // GET: ClubOfficers/Create
        public ActionResult Create()
        {

            var c_member = from m in db.Members
                           select new
                           {
                               full_name = m.first_name + " " + m.last_name,
                               m.member_seq,
                               m.first_name,
                               m.last_name
                           };
            ViewBag.member_seq = new SelectList(c_member.OrderBy(x => x.first_name).ThenBy(x=>x.last_name), "member_seq", "full_name");
            ViewBag.officer_id = new SelectList(db.Officers.OrderBy(x=>x.officer_id), "officer_id", "title");
            ViewBag.club_id = new SelectList(db.Clubs.OrderBy(x => x.club_name_thai), "club_id", "club_name_thai");
            return View();
        }

        // POST: ClubOfficers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "fiscal_year,member_seq,officer_id,club_officer_id,club_id,seq_no")] ClubOfficer clubOfficer)
        {
            if (ModelState.IsValid)
            {
                db.ClubOfficers.Add(clubOfficer);
                db.SaveChanges();
                var identity = (HttpContext.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
                // Write log to table TransactionLog
                //ProjLib projlib = new ProjLib();
                ProjLib.writeTransactionLog(identity.User.member_seq, "CreateClubOfficer", identity.User.club_id);
                return RedirectToAction("Index");
            }
            var c_member = from m in db.Members
                           select new
                           {
                               full_name=  m.first_name+" "+m.last_name,
                               m.member_seq,
                               m.first_name,
                               m.last_name
                           };
            ViewBag.member_seq = new SelectList(c_member.OrderBy(x => x.first_name).ThenBy(x => x.last_name), "member_seq", "full_name", clubOfficer.member_seq);
            
            ViewBag.officer_id = new SelectList(db.Officers.OrderBy(x => x.officer_id), "officer_id", "title", clubOfficer.officer_id);
            ViewBag.club_id = new SelectList(db.Clubs.OrderBy(x => x.club_name_thai), "club_id", "club_name_thai", clubOfficer.club_id);            
            return View(clubOfficer);
        }

        // GET: ClubOfficers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClubOfficer clubOfficer = db.ClubOfficers.Find(id);
            if (clubOfficer == null)
            {
                return HttpNotFound();
            }
            var c_member = from m in db.Members
                           select new
                           {
                               full_name = m.first_name + " " + m.last_name,
                               m.member_seq,
                               m.first_name,
                               m.last_name
                           };
            ViewBag.member_seq = new SelectList(c_member.OrderBy(x => x.first_name).ThenBy(x => x.last_name), "member_seq", "full_name", clubOfficer.member_seq);
            
            ViewBag.officer_id = new SelectList(db.Officers.OrderBy(x => x.officer_id), "officer_id", "title", clubOfficer.officer_id);
            ViewBag.club_id = new SelectList(db.Clubs.OrderBy(x => x.club_name_thai), "club_id", "club_name_thai", clubOfficer.club_id);
            return View(clubOfficer);
        }

        // POST: ClubOfficers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "fiscal_year,member_seq,officer_id,club_officer_id,club_id,seq_no")] ClubOfficer clubOfficer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(clubOfficer).State = EntityState.Modified;
                db.SaveChanges();
                var identity = (HttpContext.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
                // Write log to table TransactionLog
                //ProjLib projlib = new ProjLib();
                ProjLib.writeTransactionLog(identity.User.member_seq, "EditClubOfficer", identity.User.club_id);
                return RedirectToAction("Index");
            }

            var c_member = from m in db.Members
                           select new
                           {
                               full_name = m.first_name + " " + m.last_name,
                               m.member_seq,
                               m.first_name,
                               m.last_name
                           };
            ViewBag.member_seq = new SelectList(c_member.OrderBy(x => x.first_name).ThenBy(x => x.last_name), "member_seq", "full_name", clubOfficer.member_seq);

            
            ViewBag.officer_id = new SelectList(db.Officers.OrderBy(x => x.officer_id), "officer_id", "title", clubOfficer.officer_id);
            ViewBag.club_id = new SelectList(db.Clubs.OrderBy(x => x.club_name_thai), "club_id", "club_name_thai", clubOfficer.club_id);
            return View(clubOfficer);
        }

        // GET: ClubOfficers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClubOfficer clubOfficer = db.ClubOfficers.Find(id);
            if (clubOfficer == null)
            {
                return HttpNotFound();
            }
            return View(clubOfficer);
        }

        // POST: ClubOfficers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ClubOfficer clubOfficer = db.ClubOfficers.Find(id);
            db.ClubOfficers.Remove(clubOfficer);
            db.SaveChanges();
            var identity = (HttpContext.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
            // Write log to table TransactionLog
            //ProjLib projlib = new ProjLib();
            ProjLib.writeTransactionLog(identity.User.member_seq, "DeleteClubOfficer", identity.User.club_id);
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

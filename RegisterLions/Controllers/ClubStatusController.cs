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
    public class ClubStatusController : Controller
    {
        private RegisterLionsEntities db = new RegisterLionsEntities();

        // GET: ClubStatus
        public ActionResult Index(string searchString)
        {
            var clubstatus = (from c in db.ClubStatus select c);
            if (!String.IsNullOrEmpty(searchString))
            {
                clubstatus = clubstatus.Where(x => x.club_status_desc.Contains(searchString));

            }
            
            return View(clubstatus.ToList());
        }

        

        // GET: ClubStatus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ClubStatus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "club_sts,club_status_desc")] ClubStatus clubStatus)
        {
            if (ModelState.IsValid)
            {
                db.ClubStatus.Add(clubStatus);
                db.SaveChanges();
                var identity = (HttpContext.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
                // Write log to table TransactionLog
                //ProjLib projlib = new ProjLib();
                ProjLib.TransactionLog(identity.User.member_seq, "CreateClubStatus", identity.User.club_id);
                return RedirectToAction("Index");
            }

            return View(clubStatus);
        }

        // GET: ClubStatus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClubStatus clubStatus = db.ClubStatus.Find(id);
            if (clubStatus == null)
            {
                return HttpNotFound();
            }
            return View(clubStatus);
        }

        // POST: ClubStatus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "club_sts,club_status_desc")] ClubStatus clubStatus)
        {
            if (ModelState.IsValid)
            {
                db.Entry(clubStatus).State = EntityState.Modified;
                var identity = (HttpContext.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
                // Write log to table TransactionLog
                //ProjLib projlib = new ProjLib();
                ProjLib.TransactionLog(identity.User.member_seq, "EditClubStatus", identity.User.club_id);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(clubStatus);
        }

        // GET: ClubStatus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClubStatus clubStatus = db.ClubStatus.Find(id);
            if (clubStatus == null)
            {
                return HttpNotFound();
            }
            return View(clubStatus);
        }

        // POST: ClubStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ClubStatus clubStatus = db.ClubStatus.Find(id);
            db.ClubStatus.Remove(clubStatus);
            db.SaveChanges();
            var identity = (HttpContext.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
            // Write log to table TransactionLog
            //ProjLib projlib = new ProjLib();
            ProjLib.TransactionLog(identity.User.member_seq, "DeleteClubStatus", identity.User.club_id);
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

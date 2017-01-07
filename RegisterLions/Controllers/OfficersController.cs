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
    public class OfficersController : Controller
    {
        private RegisterLionsEntities db = new RegisterLionsEntities();

        // GET: Officers
        public ActionResult Index(string searchString)
        {
            var officer = (from o in db.Officers select o);
            if (!String.IsNullOrEmpty(searchString))
            {
                officer = officer.Where(x => x.title.Contains(searchString) || x.title_eng.Contains(searchString) || x.OfficerGroup.officer_grp_desc.Contains(searchString));

            }

            

            return View(officer.ToList());
        }

        

        // GET: Officers/Create
        public ActionResult Create()
        {
            ViewBag.officer_grp = new SelectList(db.OfficerGroups.OrderBy(x=>x.officer_grp), "officer_grp", "officer_grp_desc");
            return View();
        }

        // POST: Officers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "officer_id,title,title_eng,officer_type,officer_grp")] Officer officer)
        {
            if (ModelState.IsValid)
            {
                db.Officers.Add(officer);
                db.SaveChanges();
                var identity = (HttpContext.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
                // Write log to table TransactionLog
                //ProjLib projlib = new ProjLib();
                ProjLib.writeTransactionLog(identity.User.member_seq, "CreateOfficer", identity.User.club_id);
                return RedirectToAction("Index");
            }
            ViewBag.officer_grp = new SelectList(db.OfficerGroups.OrderBy(x => x.officer_grp), "officer_grp", "officer_grp_desc", officer.officer_grp);
            return View(officer);
        }

        // GET: Officers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Officer officer = db.Officers.Find(id);
            if (officer == null)
            {
                return HttpNotFound();
            }
            ViewBag.officer_grp = new SelectList(db.OfficerGroups.OrderBy(x => x.officer_grp), "officer_grp", "officer_grp_desc", officer.officer_grp);
            return View(officer);
        }

        // POST: Officers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "officer_id,title,title_eng,officer_type,officer_grp")] Officer officer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(officer).State = EntityState.Modified;
                db.SaveChanges();
                var identity = (HttpContext.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
                // Write log to table TransactionLog
                //ProjLib projlib = new ProjLib();
                ProjLib.writeTransactionLog(identity.User.member_seq, "EditOfficer", identity.User.club_id);
                return RedirectToAction("Index");
            }
            ViewBag.officer_grp = new SelectList(db.OfficerGroups.OrderBy(x => x.officer_grp), "officer_grp", "officer_grp_desc", officer.officer_grp);
            return View(officer);
        }

        // GET: Officers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Officer officer = db.Officers.Find(id);
            if (officer == null)
            {
                return HttpNotFound();
            }
            return View(officer);
        }

        // POST: Officers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Officer officer = db.Officers.Find(id);
            db.Officers.Remove(officer);
            db.SaveChanges();
            var identity = (HttpContext.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
            // Write log to table TransactionLog
            //ProjLib projlib = new ProjLib();
            ProjLib.writeTransactionLog(identity.User.member_seq, "DeleteOfficer", identity.User.club_id);
            return RedirectToAction("Index");
        }
        public ActionResult ListOfficer(string searchString)
        {
            ViewBag.searchString = "ค้นหา...";
            var officer = (from c in db.Officers
                        select c);
            if (!String.IsNullOrEmpty(searchString))
            {
                officer = officer.Where(x => x.title.Contains(searchString)||x.title_eng.Contains(searchString))
                .OrderBy(x => x.title);
                ViewBag.searchString = searchString;

            }
            else
            {
                officer = officer.Where(x => x.title.Contains("X"));
            }

            return View(officer.ToList());

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

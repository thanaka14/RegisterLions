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
    public class MembershipTypesController : Controller
    {
        private RegisterLionsEntities db = new RegisterLionsEntities();

        // GET: MembershipTypes
        public ActionResult Index(string searchString)
        {
            var membershipTypes = (from m in db.MembershipTypes select m);
            if (!String.IsNullOrEmpty(searchString))
            {
                membershipTypes = membershipTypes.Where(x => x.membership_desc_thai.Contains(searchString) ||
                x.membership_desc_eng.Contains(searchString));
            }
           
            return View(membershipTypes.ToList());
        }

        

        // GET: MembershipTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MembershipTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "membership_type,membership_desc_thai,membership_desc_eng")] MembershipType membershipType)
        {
            if (ModelState.IsValid)
            {
                db.MembershipTypes.Add(membershipType);
                var identity = (HttpContext.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
                // Write log to table TransactionLog
                //ProjLib projlib = new ProjLib();
                ProjLib.writeTransactionLog(identity.User.member_seq, "CreateMembershipType", identity.User.club_id);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(membershipType);
        }

        // GET: MembershipTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MembershipType membershipType = db.MembershipTypes.Find(id);
            if (membershipType == null)
            {
                return HttpNotFound();
            }
            return View(membershipType);
        }

        // POST: MembershipTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "membership_type,membership_desc_thai,membership_desc_eng")] MembershipType membershipType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(membershipType).State = EntityState.Modified;
                db.SaveChanges();
                var identity = (HttpContext.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
                // Write log to table TransactionLog
                //ProjLib projlib = new ProjLib();
                ProjLib.writeTransactionLog(identity.User.member_seq, "EditMembershipType", identity.User.club_id);
                return RedirectToAction("Index");
            }
            return View(membershipType);
        }

        // GET: MembershipTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MembershipType membershipType = db.MembershipTypes.Find(id);
            if (membershipType == null)
            {
                return HttpNotFound();
            }
            return View(membershipType);
        }

        // POST: MembershipTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MembershipType membershipType = db.MembershipTypes.Find(id);
            db.MembershipTypes.Remove(membershipType);
            var identity = (HttpContext.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
            // Write log to table TransactionLog
            //ProjLib projlib = new ProjLib();
            ProjLib.writeTransactionLog(identity.User.member_seq, "DeleteMembershipType", identity.User.club_id);
            db.SaveChanges();
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

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
    public class MultipleDistrictsController : Controller
    {
        private RegisterLionsEntities db = new RegisterLionsEntities();

        // GET: MultipleDistricts
        public ActionResult Index(string searchString)
        {
            var multipledistrict = (from m in db.MultipleDistricts select m);
            if (!String.IsNullOrEmpty(searchString))
            {
                multipledistrict= multipledistrict.Where(x => x.multiple_district_name_thai.Contains(searchString)
                ||
                x.multiple_district_name_eng.Contains(searchString));
            }
           
            return View(multipledistrict.ToList());
        }

       

        // GET: MultipleDistricts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MultipleDistricts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "multiple_district_id,multiple_district_name_thai,multiple_district_name_eng")] MultipleDistrict multipleDistrict)
        {
            if (ModelState.IsValid)
            {
                db.MultipleDistricts.Add(multipleDistrict);
                db.SaveChanges();
                var identity = (HttpContext.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
                // Write log to table TransactionLog
                WriteLog writeLog = new WriteLog();
                writeLog.TransactionLog(identity.User.member_seq, "CreateMultipleDistrict", identity.User.club_id);
                return RedirectToAction("Index");
            }

            return View(multipleDistrict);
        }

        // GET: MultipleDistricts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MultipleDistrict multipleDistrict = db.MultipleDistricts.Find(id);
            if (multipleDistrict == null)
            {
                return HttpNotFound();
            }
            return View(multipleDistrict);
        }

        // POST: MultipleDistricts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "multiple_district_id,multiple_district_name_thai,multiple_district_name_eng")] MultipleDistrict multipleDistrict)
        {
            if (ModelState.IsValid)
            {
                db.Entry(multipleDistrict).State = EntityState.Modified;
                db.SaveChanges();
                var identity = (HttpContext.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
                // Write log to table TransactionLog
                WriteLog writeLog = new WriteLog();
                writeLog.TransactionLog(identity.User.member_seq, "EditMultipleDistrict", identity.User.club_id);
                return RedirectToAction("Index");
            }
            return View(multipleDistrict);
        }

        // GET: MultipleDistricts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MultipleDistrict multipleDistrict = db.MultipleDistricts.Find(id);
            if (multipleDistrict == null)
            {
                return HttpNotFound();
            }
            return View(multipleDistrict);
        }

        // POST: MultipleDistricts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MultipleDistrict multipleDistrict = db.MultipleDistricts.Find(id);
            db.MultipleDistricts.Remove(multipleDistrict);
            db.SaveChanges();
            var identity = (HttpContext.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
            // Write log to table TransactionLog
            WriteLog writeLog = new WriteLog();
            writeLog.TransactionLog(identity.User.member_seq, "DeleteMultipleDistrict", identity.User.club_id);
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

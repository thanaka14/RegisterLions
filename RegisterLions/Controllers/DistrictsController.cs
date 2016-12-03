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
    public class DistrictsController : Controller
    {
        private RegisterLionsEntities db = new RegisterLionsEntities();

        // GET: Districts
        public ActionResult Index(string searchString)
        {
            var districts = db.Districts.Include(d => d.MultipleDistrict);
            if (!String.IsNullOrEmpty(searchString))
            {
                districts = districts.Where(x => x.district_name_thai.Contains(searchString) ||
                 x.district_name_eng.Contains(searchString));
            }
            
            return View(districts.ToList());
        }

        

        // GET: Districts/Create
        public ActionResult Create()
        {
            ViewBag.multiple_district_id = new SelectList(db.MultipleDistricts, "multiple_district_id", "multiple_district_name_thai");
            return View();
        }

        // POST: Districts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "district_id,district_name_thai,district_name_eng,multiple_district_id")] District district)
        {
            if (ModelState.IsValid)
            {
                string dt_name = district.district_name_thai;
                db.Districts.Add(district);
                db.SaveChanges();
                var identity = (HttpContext.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
                // Write log to table TransactionLog
                WriteLog writeLog = new WriteLog();
                writeLog.TransactionLog(identity.User.member_seq, "CreateDistrict", identity.User.club_id);
                return RedirectToAction("Index");
            }

            ViewBag.multiple_district_id = new SelectList(db.MultipleDistricts, "multiple_district_id", "multiple_district_name_thai", district.multiple_district_id);
            return View(district);
        }

        // GET: Districts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            District district = db.Districts.Find(id);
            if (district == null)
            {
                return HttpNotFound();
            }
            ViewBag.multiple_district_id = new SelectList(db.MultipleDistricts, "multiple_district_id", "multiple_district_name_thai", district.multiple_district_id);
            return View(district);
        }

        // POST: Districts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "district_id,district_name_thai,district_name_eng,multiple_district_id")] District district)
        {
            if (ModelState.IsValid)
            {
                db.Entry(district).State = EntityState.Modified;
                db.SaveChanges();
                var identity = (HttpContext.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
                // Write log to table TransactionLog
                WriteLog writeLog = new WriteLog();
                writeLog.TransactionLog(identity.User.member_seq, "EditDistrict", identity.User.club_id);
                return RedirectToAction("Index");
            }
            ViewBag.multiple_district_id = new SelectList(db.MultipleDistricts, "multiple_district_id", "multiple_district_name_thai", district.multiple_district_id);
            return View(district);
        }

        // GET: Districts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            District district = db.Districts.Find(id);
            if (district == null)
            {
                return HttpNotFound();
            }
            return View(district);
        }

        // POST: Districts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            District district = db.Districts.Find(id);
            db.Districts.Remove(district);
            db.SaveChanges();
            var identity = (HttpContext.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
            // Write log to table TransactionLog
            WriteLog writeLog = new WriteLog();
            writeLog.TransactionLog(identity.User.member_seq, "DeleteDistrict", identity.User.club_id);
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

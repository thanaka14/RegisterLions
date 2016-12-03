using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using RegisterLions.Models;
using RegisterLions.Lib;

namespace RegisterLions.Controllers
{
    [Authorize(Roles ="Admin")]
    public class ClubsController : Controller
    {
        private RegisterLionsEntities db = new RegisterLionsEntities();
        [Authorize]
        // GET: Clubs
        public ActionResult Index(string searchString)
        {
            var clubs = db.Clubs.Include(c => c.District).Include(c => c.ClubStatus);
            if (!String.IsNullOrEmpty(searchString))
            {
                clubs = clubs.Where(x => x.club_name_thai.Contains(searchString) ||
                 x.club_name_eng.Contains(searchString));
            }
            
            return View(clubs.ToList());
        }

       

        // GET: Clubs/Create
        public ActionResult Create()
        {
            ViewBag.district_id = new SelectList(db.Districts, "district_id", "district_name_thai");
            
            ViewBag.club_sts = new SelectList(db.ClubStatus, "club_sts", "club_status_desc");
            //ViewBag.province_id = new SelectList(db.Provinces, "province_id", "province_name");
            return View();
        }

        // POST: Clubs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "club_id,club_name_thai,club_name_eng,charter_date,meeting_place,district_id,club_sts")] Club club)
        {
            if (ModelState.IsValid)
            {
                db.Clubs.Add(club);
                db.SaveChanges();
                var identity = (HttpContext.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
                // Write log to table TransactionLog
                WriteLog writeLog = new WriteLog();
                writeLog.TransactionLog(identity.User.member_seq, "CreateClub", identity.User.club_id);
                return RedirectToAction("Index");
            }

            ViewBag.district_id = new SelectList(db.Districts, "district_id", "district_name_thai", club.district_id);
            ViewBag.club_sts = new SelectList(db.ClubStatus, "club_sts", "club_status_desc", club.club_sts);
           // ViewBag.province_id = new SelectList(db.Provinces, "province_id", "province_name", club.province_id);
            return View(club);
        }

        // GET: Clubs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Club club = db.Clubs.Find(id);
            if (club == null)
            {
                return HttpNotFound();
            }
            ViewBag.district_id = new SelectList(db.Districts, "district_id", "district_name_thai", club.district_id);
            ViewBag.club_sts = new SelectList(db.ClubStatus, "club_sts", "club_status_desc", club.club_sts);
            //ViewBag.province_id = new SelectList(db.Provinces, "province_id", "province_name", club.province_id);
            return View(club);
        }

        // POST: Clubs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "club_id,club_name_thai,club_name_eng,charter_date,meeting_place,district_id,club_sts")] Club club)
        {
            if (ModelState.IsValid)
            {
                db.Entry(club).State = EntityState.Modified;
                db.SaveChanges();
                var identity = (HttpContext.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
                // Write log to table TransactionLog
                WriteLog writeLog = new WriteLog();
                writeLog.TransactionLog(identity.User.member_seq, "EditClub", identity.User.club_id);
                return RedirectToAction("Index");
            }
            ViewBag.district_id = new SelectList(db.Districts, "district_id", "district_name_thai", club.district_id);
            ViewBag.club_sts = new SelectList(db.ClubStatus, "club_sts", "club_status_desc", club.club_sts);
           // ViewBag.province_id = new SelectList(db.Provinces, "province_id", "province_name", club.province_id);
            return View(club);
        }

        // GET: Clubs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Club club = db.Clubs.Find(id);
            if (club == null)
            {
                return HttpNotFound();
            }
            return View(club);
        }

        // POST: Clubs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Club club = db.Clubs.Find(id);
            db.Clubs.Remove(club);
            db.SaveChanges();
            var identity = (HttpContext.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
            // Write log to table TransactionLog
            WriteLog writeLog = new WriteLog();
            writeLog.TransactionLog(identity.User.member_seq, "DeleteClub", identity.User.club_id);
            return RedirectToAction("Index");
        }
        public ActionResult ListClub(string searchString)
        {
            var identity = (System.Web.HttpContext.Current.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
            //var tDistrict_id = identity.User.district_id;
            ViewBag.searchString = "ค้นหา...";
            var club = (from c in db.Clubs
                        where c.district_id == identity.User.district_id
                        select c);
            if (!String.IsNullOrEmpty(searchString))
            {
                club = club.Where(x => x.club_name_thai.Contains(searchString) ||
                  x.club_name_eng.Contains(searchString)).OrderBy(x => x.club_name_thai);
                ViewBag.searchString = searchString;

            }
            else
            {
                club = club.Where(x => x.club_name_thai.Contains("X"));
            }            
            
            return View(club.ToList());

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

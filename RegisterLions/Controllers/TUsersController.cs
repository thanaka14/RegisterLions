using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using RegisterLions.Models;
using System;
using System.Collections.Generic;
using RegisterLions.Lib;
using System.Web.Security;

namespace RegisterLions.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TUsersController : Controller
    {
        private RegisterLionsEntities db = new RegisterLionsEntities();

        // GET: TUsers
        public ActionResult Index(string searchString)
        {
            var tUser = db.TUsers.Include(t => t.Member);
//            var dt_name = db.Districts.Where(x=>x.district_id.Equals())
            if (!String.IsNullOrEmpty(searchString))
            {
                tUser = tUser.Where(x => x.user_code.Contains(searchString) ||
                x.Member.first_name_eng.Contains(searchString) || x.Member.last_name_eng.Contains(searchString) ||
                x.Member.first_name.Contains(searchString) || x.Member.last_name.Contains(searchString));
            }
            var identity = (HttpContext.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
            // Write log to table TransactionLog
            WriteLog writeLog = new WriteLog();
            writeLog.TransactionLog(identity.User.member_seq, "ListTUser", identity.User.club_id);
            return View(tUser.OrderBy(x => x.user_code).ToList());
        }

        
        // GET: TUsers/Create
        public ActionResult Create()
        {
            List<SelectListItem> lstRoleName = new List<SelectListItem>();
            lstRoleName.Add(new SelectListItem() { Text = "ผู้ดูแลระบบ", Value = "Admin" });
            lstRoleName.Add(new SelectListItem() { Text = "ผู้ใช้งาน", Value = "User" });
            ViewBag.role_name = new SelectList(lstRoleName, "Value", "Text");

            ViewBag.member_seq = new SelectList(db.Members.OrderBy(x => x.first_name_eng).ThenBy(x => x.last_name_eng), "member_seq", "full_name_eng");
            ViewBag.club_id = new SelectList(db.Clubs.OrderBy(x => x.club_name_thai), "club_id", "club_name_thai");
            return View();
        }

        // POST: TUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "user_code,user_pwd,eff_date,exp_date,member_seq,role_name")] TUser tUser)
        {
            if (ModelState.IsValid)
            {
                TUser result = db.TUsers.Find(tUser.user_code);
                if (result == null)
                {
                   
                        tUser.upd_date = DateTime.Now;
                        tUser.user_pwd= FormsAuthentication.HashPasswordForStoringInConfigFile(tUser.user_pwd, "SHA1");
                    db.TUsers.Add(tUser);
                        db.SaveChanges();
                        var identity = (HttpContext.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
                        // Write log to table TransactionLog
                        WriteLog writeLog = new WriteLog();
                        writeLog.TransactionLog(identity.User.member_seq, "CreateTUser", identity.User.club_id);
                        return RedirectToAction("Index");
                    
                }else
                {
                    ViewBag.errorMessage = "รหัสผู้ใช้ซ้ำ";
                }
            }
            List<SelectListItem> lstRoleName = new List<SelectListItem>();
            lstRoleName.Add(new SelectListItem() { Text = "ผู้ดูแลระบบ", Value = "Admin" });
            lstRoleName.Add(new SelectListItem() { Text = "ผู้ใช้งาน", Value = "User" });
            ViewBag.role_name = new SelectList(lstRoleName, "Value", "Text",tUser.role_name);
            ViewBag.member_seq = new SelectList(db.Members.OrderBy(x => x.first_name_eng).ThenBy(x => x.last_name_eng), "member_seq", "full_name_eng", tUser.member_seq);
            ViewBag.club_id = new SelectList(db.Clubs.OrderBy(x => x.club_name_thai), "club_id", "club_name_thai");
            return View(tUser);
        }

        // GET: TUsers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TUser tUser = db.TUsers.Find(id);            
            if (tUser == null)
            {
                return HttpNotFound();
            }
            
            ViewBag.member_seq = new SelectList(db.Members.OrderBy(x => x.first_name_eng).ThenBy(x => x.last_name_eng), "member_seq", "full_name_eng", tUser.member_seq);
            ViewBag.club_id = new SelectList(db.Clubs.OrderBy(x => x.club_name_thai), "club_id", "club_name_thai",tUser.Member.club_id);
            List<SelectListItem> lstRoleName = new List<SelectListItem>();
            lstRoleName.Add(new SelectListItem() { Text = "ผู้ดูแลระบบ", Value = "Admin" });
            lstRoleName.Add(new SelectListItem() { Text = "ผู้ใช้งาน", Value = "User" });
            ViewBag.role_name = new SelectList(lstRoleName, "Value", "Text", tUser.role_name);
            return View(tUser);
        }

        // POST: TUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "user_code,user_pwd,eff_date,exp_date,member_seq,role_name")] TUser tUser)
        {
            if (ModelState.IsValid)
            {
                tUser.upd_date = DateTime.Now;
                db.Entry(tUser).State = EntityState.Modified;
                tUser.user_pwd = FormsAuthentication.HashPasswordForStoringInConfigFile(tUser.user_pwd, "SHA1");
                db.SaveChanges();
                var identity = (HttpContext.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
                // Write log to table TransactionLog
                WriteLog writeLog = new WriteLog();
                writeLog.TransactionLog(identity.User.member_seq, "EditTUser", identity.User.club_id);
                return RedirectToAction("Index");
            }
            ViewBag.member_seq = new SelectList(db.Members.OrderBy(x => x.first_name_eng).ThenBy(x => x.last_name_eng), "member_seq", "full_name_eng", tUser.member_seq);
            ViewBag.club_id = new SelectList(db.Clubs.OrderBy(x => x.club_name_thai), "club_id", "club_name_thai", tUser.Member.club_id);
            List<SelectListItem> lstRoleName = new List<SelectListItem>();
            lstRoleName.Add(new SelectListItem() { Text = "ผู้ดูแลระบบ", Value = "Admin" });
            lstRoleName.Add(new SelectListItem() { Text = "ผู้ใช้งาน", Value = "User" });
            ViewBag.role_name = new SelectList(lstRoleName, "Value", "Text", tUser.role_name);
            return View(tUser);
        }


        // GET: TUsers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TUser tUser = db.TUsers.Find(id);
            if (tUser == null)
            {
                return HttpNotFound();
            }
            return View(tUser);
        }

        // POST: TUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            TUser tUser = db.TUsers.Find(id);
            db.TUsers.Remove(tUser);
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

﻿using System.Data.Entity;
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
    [Authorize(Roles="Admin")]
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
            //ProjLib projlib = new ProjLib();
            ProjLib.writeTransactionLog(identity.User.member_seq, "ListTUser", identity.User.club_id);
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
                    //ProjLib projlib = new ProjLib();
                    ProjLib.writeTransactionLog(identity.User.member_seq, "CreateTUser", identity.User.club_id);
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
                var user_pwd_2 = Request.Form["user_pwd_edit"];
                if (user_pwd_2 != null)
                {
                    tUser.user_pwd = FormsAuthentication.HashPasswordForStoringInConfigFile(user_pwd_2, "SHA1");
                }
                
                db.Entry(tUser).State = EntityState.Modified;
                
                db.SaveChanges();
                var identity = (HttpContext.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
                // Write log to table TransactionLog
                //ProjLib projlib = new ProjLib();
                ProjLib.writeTransactionLog(identity.User.member_seq, "EditTUser", identity.User.club_id);
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

        public ActionResult GenerateUser()
        {
            var member = (from m in db.Members
                          where m.member_sts==1 
                          select m
                         
                        
                      ).ToList();
            foreach(var t in member)
            {
                if (t.member_seq != 326 && t.member_seq != 356 && t.member_seq != 357 && t.member_seq != 396
                    && t.member_seq != 474 && t.member_seq != 708 && t.member_seq != 932 && t.member_seq != 1111
                    && t.member_seq != 1168 && t.member_seq != 1194 && t.member_seq != 1263 && t.member_seq != 1265
                    && t.member_seq != 1316)
                { 
                    var user = (from u in db.TUsers where u.member_seq == t.member_seq select u).ToList();
                    if (user.Count() == 0)
                    {
                        //ProjLib projlib = new ProjLib();
                        string user_code= ProjLib.chkUserCode(t.first_name_eng, t.last_name_eng);                        
                        TUser tuser = new TUser();
                        tuser.user_code = user_code;                     
                        tuser.user_pwd = FormsAuthentication.HashPasswordForStoringInConfigFile(user_code + "@123", "SHA1");
                        tuser.role_name = "User";
                        tuser.member_seq = t.member_seq;
                        tuser.upd_date = DateTime.Now;
                        db.TUsers.Add(tuser);
                        db.SaveChanges();
                    }
                }
            }
            return View();
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

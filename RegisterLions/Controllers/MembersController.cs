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
using System.Web.Security;

namespace RegisterLions.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MembersController : Controller
    {
        private RegisterLionsEntities db = new RegisterLionsEntities();

        // GET: Members
        public ActionResult Index(string searchString, int? memberSts)
        {
            ViewBag.searchString = "ค้นหา...";
            var members = db.Members.Include(m => m.Club).Include(m => m.MembershipType);
            //var members = db.Members.Include(m => m.Club).Include(m => m.MembershipType);
            List<SelectListItem> lstMemberSts = new List<SelectListItem>();


            lstMemberSts.Add(new SelectListItem() { Text = "สถานะทั้งหมด", Value = "0" });
            lstMemberSts.Add(new SelectListItem() { Text = "สมาชิกสถานะปกติ", Value = "1" });
            lstMemberSts.Add(new SelectListItem() { Text = "สมาชิกสถานะพ้นสภาพ", Value = "2" });
            this.ViewBag.memberSts = new SelectList(lstMemberSts, "Value", "Text");
            if (!String.IsNullOrEmpty(searchString))
            {
                members=members.Where(x => x.first_name_eng.Contains(searchString) ||
                x.last_name_eng.Contains(searchString)|| x.first_name.Contains(searchString)||
                x.last_name.Contains(searchString) || x.Club.club_name_thai.Contains(searchString)||
                x.Club.club_name_eng.Contains(searchString) || x.member_id.ToString().Contains(searchString));
                ViewBag.searchString = searchString;
            }
            if (memberSts== null)
            {
                memberSts = 1;
                this.ViewBag.memberSts = new SelectList(lstMemberSts, "Value", "Text", "1");
            }
           
            if ((memberSts == 1 || memberSts == 2 ))
            {
                members = members.Where(x => x.member_sts == memberSts);
                
            }
            else
            {
                this.ViewBag.memberSts = new SelectList(lstMemberSts, "Value", "Text", "0");
            }
           // members = members.OrderBy(x => x.Club.club_name_thai).ThenBy(x => x.first_name_eng).ThenBy(x => x.last_name_eng);
            ViewBag.MemberCount = members.Count();
           
            return View(members.ToList());
        }

       

        // GET: Members/Create
        public ActionResult Create()
        {
            var identity = (HttpContext.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
            ViewBag.club_id = new SelectList(db.Clubs.Where(x=>x.district_id== identity.User.district_id).OrderBy(x => x.club_name_thai), "club_id", "club_name_thai");
            ViewBag.membership_type = new SelectList(db.MembershipTypes.OrderBy(x => x.membership_desc_thai), "membership_type", "membership_desc_thai");
            ViewBag.movement = new SelectList(db.Movements.OrderBy(x => x.move_desc), "move_sts", "move_desc");
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "member_id,first_name,last_name,gender,member_address_eng,post_code,email,cell_phone,birth_year,occupation,Join_Date,club_id,member_seq,membership_type,first_name_eng,last_name_eng,member_address_thai,sponsor_name,charter_flag,member_sts,line_id")] Member member, HttpPostedFileBase image)
        //public ActionResult Create([Bind(Include = "member_id,first_name,last_name,gender,member_address_eng,email,cell_phone,birth_year,occupation,Join_Date,club_id,member_seq,membership_type,first_name_eng,last_name_eng,member_address_thai")] Member member)
        {
            var identity = (HttpContext.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
            if (ModelState.IsValid)
            {
                //Member result  = db.Members.Find(member.member_id);
                var result = (from m in db.Members
                              where m.member_id== member.member_id
                              select m
                              
                                );

                if (result.Count()==0)
                {

                    if (image != null && image.ContentLength > 0)
                    {
                        using (var reader = new System.IO.BinaryReader(image.InputStream))
                        {
                            member.image = reader.ReadBytes(image.ContentLength);
                        }

                    }
                    member.upd_date = DateTime.Now;
                    db.Members.Add(member);
                    string tMovement = Request.Form["movement"];
                    string tMoveDate = Request.Form["movedate"];
                    MemberMovement memberMovement = new MemberMovement();
                    if (tMovement != "1" && Convert.ToDateTime(tMoveDate) != memberMovement.hist_date)
                    {
                        MemberMovement memberMovement1 = new MemberMovement();
                        memberMovement1.member_seq = member.member_seq;
                        memberMovement1.hist_date = member.join_date;
                        memberMovement1.club_id = null;
                        memberMovement1.move_sts = 1;
                        db.MemberMovements.Add(memberMovement1);
                    }
                    if (tMovement == "1")
                    {
                        memberMovement.hist_date = member.join_date;
                    }
                    else
                    {
                        memberMovement.hist_date = Convert.ToDateTime(tMoveDate);

                    }

                    memberMovement.member_seq = member.member_seq;
                    memberMovement.club_id = member.club_id;
                    memberMovement.move_sts = Int32.Parse(tMovement);
                    db.MemberMovements.Add(memberMovement);

                    //ProjLib projlib = new ProjLib();
                    var user_code = ProjLib.ChkUserCode(member.first_name_eng, member.last_name_eng);
                    TUser tuser = new TUser();
                    //tuser.user_code = member.first_name_eng.ToLower() + "." + member.last_name_eng.ToLower().Substring(0, 1);
                    tuser.user_code = user_code;
                    //tuser.user_pwd = FormsAuthentication.HashPasswordForStoringInConfigFile(member.first_name_eng.ToLower() + "." + member.last_name_eng.ToLower().Substring(0, 1) + "@123", "SHA1");
                    tuser.user_pwd = FormsAuthentication.HashPasswordForStoringInConfigFile(user_code + "@123", "SHA1");
                    tuser.role_name = "User";
                    tuser.member_seq = member.member_seq;
                    tuser.upd_date = DateTime.Now;
                    db.TUsers.Add(tuser);
                    db.SaveChanges();


                    // Write log to table TransactionLog

                    ProjLib.TransactionLog(identity.User.member_seq, "CreateMember", identity.User.club_id);
                    return RedirectToAction("Index");
                }
                else { ViewBag.errorMessage = "รหัสสมาชิกซ้ำ"; }
            }
           
            ViewBag.club_id = new SelectList(db.Clubs.Where(x => x.district_id == identity.User.district_id).OrderBy(x => x.club_name_thai), "club_id", "club_name_thai", member.club_id);
            ViewBag.membership_type = new SelectList(db.MembershipTypes.OrderBy(x => x.membership_desc_thai), "membership_type", "membership_desc_thai", member.membership_type);            
            ViewBag.movement = new SelectList(db.Movements.OrderBy(x => x.move_desc), "move_sts", "move_desc");


            return View(member);
        }

        // GET: Members/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            var identity = (HttpContext.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
            ViewBag.club_id = new SelectList(db.Clubs.Where(x => x.district_id == identity.User.district_id).OrderBy(x => x.club_name_thai), "club_id", "club_name_thai", member.club_id);
            ViewBag.membership_type = new SelectList(db.MembershipTypes.OrderBy(x => x.membership_desc_thai), "membership_type", "membership_desc_thai", member.membership_type);
            ViewBag.movement = new SelectList(db.Movements.OrderBy(x => x.move_desc), "move_sts", "move_desc");


            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "member_id,first_name,first_name,last_name,gender,member_address_eng,post_code,email,cell_phone,birth_year,occupation,Join_Date,club_id,member_seq,membership_type,first_name_eng,last_name_eng,member_address_thai,sponsor_name,charter_flag,member_sts,line_id")] Member member, HttpPostedFileBase image, Member m)
        //public ActionResult Edit([Bind(Include = "member_id,first_name,first_name,last_name,gender,member_address_eng,email,cell_phone,birth_year,occupation,Join_Date,club_id,member_seq,membership_type,first_name_eng,last_name_eng,member_address_thai")] Member member)
        {
            var identity = (HttpContext.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
            if (ModelState.IsValid)
            {
                if (image != null && image.ContentLength > 0)
                {
                    using (var reader = new System.IO.BinaryReader(image.InputStream))
                    {
                        member.image = reader.ReadBytes(image.ContentLength);
                    }

                }else if (m.image != null)
                {
                    member.image = m.image;
                }
                member.upd_date = DateTime.Now;
                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();
               
                // Write log to table TransactionLog
                //ProjLib projlib = new ProjLib();
                ProjLib.TransactionLog(identity.User.member_seq, "EditMember", identity.User.club_id);
                return RedirectToAction("Index");
            }

            ViewBag.club_id = new SelectList(db.Clubs.Where(x => x.district_id == identity.User.district_id).OrderBy(x => x.club_name_thai), "club_id", "club_name_thai", member.club_id);           
            ViewBag.membership_type = new SelectList(db.MembershipTypes.OrderBy(x => x.membership_desc_thai), "membership_type", "membership_desc_thai", member.membership_type);
            ViewBag.movement = new SelectList(db.Movements.OrderBy(x => x.move_desc), "move_sts", "move_desc");


            return View(member);
        }

        // GET: Members/Delete/5
        public ActionResult Movement(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            var identity = (HttpContext.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
            ViewBag.club_id = new SelectList(db.Clubs.Where(x => x.district_id == identity.User.district_id).OrderBy(x => x.club_name_thai), "club_id", "club_name_thai", member.club_id);
            ViewBag.movement = new SelectList(db.Movements.Where(x => x.move_sts!=1).OrderBy(x => x.move_desc), "move_sts", "move_desc");
            return View(member);
        }

        // POST: Members/Movement/5
        [HttpPost, ActionName("Movement")]
        [ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(Bind(Include = "member_id,first_name,last_name,gender,member_address_eng,post_code,email,cell_phone,birth_year,occupation,Join_Date,club_id,member_seq,membership_type,first_name_eng,last_name_eng,member_address_thai,sponsor_name,charter_flag,member_sts")] Member member, Member m)
        public ActionResult Movement([Bind(Include = "member_id,first_name,last_name,gender,member_address_eng,post_code,email,cell_phone,birth_year,occupation,Join_Date,club_id,member_seq,membership_type,first_name_eng,last_name_eng,member_address_thai,sponsor_name,charter_flag,member_sts")] Member member,Member m)
        {
            //Member member = db.Members.Find(id);
            //db.Members.Remove(member);
            //db.SaveChanges();
            if (ModelState.IsValid)
            {
                string tComment = Request.Form["comment"];
                string tMoveDate = Request.Form["movedate"];
                string tMovement = Request.Form["movement"];

                if (tMovement == "4")
                {
                    member.member_sts = 2;
                }
                else { member.member_sts = 1; }
                
                member.image = m.image;
                member.upd_date = DateTime.Now;
                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();

                MemberMovement memberMovement = new MemberMovement();
                memberMovement.member_seq = member.member_seq;
                memberMovement.club_id = m.club_id;
                memberMovement.move_sts = Int32.Parse(tMovement);
                memberMovement.comment = tComment;
                memberMovement.hist_date = Convert.ToDateTime(tMoveDate);
                db.MemberMovements.Add(memberMovement);
                db.SaveChanges();
                var identity = (HttpContext.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
                // Write log to table TransactionLog
                //ProjLib projlib = new ProjLib();
                ProjLib.TransactionLog(identity.User.member_seq, "MovementMember", identity.User.club_id);
            }
            return RedirectToAction("Index");
        }

        public ActionResult ListMember(string searchString)
        {
            ViewBag.searchString = "ค้นหา...";
            var members = db.Members.Include(m => m.Club).Include(m => m.MembershipType);
            if (!String.IsNullOrEmpty(searchString))
            {
                members = members.Where(x => x.first_name_eng.Contains(searchString) ||
                  x.last_name_eng.Contains(searchString) || x.first_name.Contains(searchString) ||
                  x.last_name.Contains(searchString) || x.Club.club_name_thai.Contains(searchString) ||
                  x.Club.club_name_eng.Contains(searchString) || x.member_id.ToString().Contains(searchString)).OrderBy(x => x.first_name_eng);
                ViewBag.searchString = searchString;

            }else
            {
                members = members.Where(x => x.first_name.Contains("X"));
            }

            // return View(members.OrderBy(x => x.Club.club_name_thai).ThenBy(x => x.first_name_eng).ThenBy(x => x.last_name_eng).ToList());

            //var identity = (HttpContext.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
            // Write log to table TransactionLog
           // projlib projlib = new projlib();
           // projlib.TransactionLog(identity.User.member_seq, "ListMember", identity.User.club_id);
            return View(members.ToList());

        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Member member = db.Members.Find(id);
            db.Members.Remove(member);
            db.TUsers.RemoveRange(db.TUsers.Where(x => x.member_seq == id));        
            db.MemberMovements.RemoveRange(db.MemberMovements.Where(x => x.member_seq == id));

            db.SaveChanges();
            var identity = (HttpContext.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
            // Write log to table TransactionLog
            //ProjLib projlib = new ProjLib();
            ProjLib.TransactionLog(identity.User.member_seq, "DeleteMember", identity.User.club_id);
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

using RegisterLions.Lib;
using RegisterLions.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace RegisterLions.Controllers
{
    public class ManageController : Controller
    {
        private RegisterLionsEntities db = new RegisterLionsEntities();
        // GET: Manage
        [Authorize]
        public new ActionResult Profile(string message, string returnURL="")
        {
            var identity = (System.Web.HttpContext.Current.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
            ViewBag.StatusMessage = message== "ChangePasswordSuccess" ? "เปลี่ยนรหัสผ่านเรียบร้อยแล้ว"
                : message == "ChangeInformationSuccess" ? "แก้ไขข้อมูลส่วนตัวเรียบร้อยแล้ว" : "";
            var Tuser = (from u in db.TUsers
                           join m in db.Members on u.member_seq equals m.member_seq
                           join c in db.Clubs on m.club_id equals c.club_id
                           join d in db.MembershipTypes on m.membership_type equals d.membership_type
                           join e in db.Districts on c.district_id equals e.district_id
                           where u.user_code.Equals(HttpContext.User.Identity.Name)
                           select u).FirstOrDefault();
            ViewBag.member_seq = Tuser.member_seq;

            // Write log to table TransactionLog
            //ProjLib projlib = new ProjLib();
            ProjLib.writeTransactionLog(identity.User.member_seq, "Profile", identity.User.club_id);
            return View(Tuser);
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }
        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePwd model)
        {
            var identity = (System.Web.HttpContext.Current.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
            #region
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            string Username = HttpContext.User.Identity.Name;
            if (!System.Web.Security.Membership.ValidateUser(Username, model.OldPassword))
            {
                ModelState.AddModelError("", "รหัสผ่านปัจจุบันไม่ถูกต้อง");
                return View(model);
            }           
            var u = (from c in db.TUsers
                        where c.user_code == Username
                        select c).First();
            u.user_pwd = FormsAuthentication.HashPasswordForStoringInConfigFile(model.NewPassword, "SHA1");
            u.upd_date = DateTime.Now;           
            db.SaveChanges();
            // Write log to table TransactionLog
            //ProjLib projlib = new ProjLib();
            ProjLib.writeTransactionLog(identity.User.member_seq, "ChangePassword", identity.User.club_id);
            return RedirectToAction("Profile", new { Message = "ChangePasswordSuccess" });
           


            #endregion
        }
        //
        // GET: /Manage/Profile
        public ActionResult EditProfile(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var identity = (HttpContext.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
            if (id != identity.User.member_seq) return RedirectToAction("Profile");
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            ViewBag.club_id = new SelectList(db.Clubs, "club_id", "club_name_thai", member.club_id);
            ViewBag.membership_type = new SelectList(db.MembershipTypes, "membership_type", "membership_desc_thai", member.membership_type);

           
            return View(member);
        }

        // POST: Manage/EditProfile/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile([Bind(Include = "member_id,full_name,first_name,last_name,gender,member_address_eng,post_code,email,cell_phone,birth_year,occupation,Join_Date,club_id,member_seq,membership_type,first_name_eng,last_name_eng,member_address_thai,sponsor_name,charter_flag,member_sts")] Member member, HttpPostedFileBase image, Member m)
        {
            if (ModelState.IsValid)
            {
                if (image != null && image.ContentLength > 0)
                {
                    using (var reader = new System.IO.BinaryReader(image.InputStream))
                    {
                        member.image = reader.ReadBytes(image.ContentLength);
                    }

                }
                else if (m.image != null)
                {
                    member.image = m.image;
                }
                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();
                var identity = (HttpContext.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
                // Write log to table TransactionLog
                //ProjLib projlib = new ProjLib();
                ProjLib.writeTransactionLog(identity.User.member_seq, "EditProfile", identity.User.club_id);
                return RedirectToAction("Profile", new { Message = "ChangeInformationSuccess" });
            }
            //ViewBag.club_id = new SelectList(db.Clubs, "club_id", "club_name_thai", member.club_id);
            // ViewBag.membership_type = new SelectList(db.MembershipTypes, "membership_type", "membership_desc_thai", member.membership_type);

           
            return View(member);
        }

        // GET: Members/Delete/5

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");

        }
    }
}
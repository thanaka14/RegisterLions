using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Data;
using RegisterLions.Models;
using RegisterLions.Lib;

namespace RegisterLions.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login(string returnUrl)
        {
            return View();
        }



        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login login, string ReturnUrl = "")
        {

            #region Part 2 Code
            //if (ModelState.IsValid)
            //{
            //    var isValidUser = Membership.ValidateUser(loing.Username, loing.Password);
            //    if (isValidUser)
            //    {
            //        FormsAuthentication.SetAuthCookie(loing.Username, loing.RememberMe);
            //        if (Url.IsLocalUrl(ReturnUrl))
            //        {
            //            return Redirect(ReturnUrl);
            //        }
            //        else
            //        {
            //            return RedirectToAction("Profile", "Home");
            //        }
            //    }
            //}
            #endregion
            #region Part 4 Code
            if (ModelState.IsValid)
            {
                // ecncrypt before Validate or not
                
                if (!Membership.ValidateUser(login.Username, login.Password))
                {
                    ModelState.AddModelError("", "รหัสผู้ใช้/รหัสผ่าน ไม่ถูกต้อง");
                    return View(login);
                }

                //TUser user = null;
                //List<TUser> user;
                // string[][] baColl = new string[][] { };
                RegisterLionsEntities db = new RegisterLionsEntities();
                var user = (from u in db.TUsers 
                            join m in db.Members on u.member_seq equals m.member_seq
                            join c in db.Clubs on m.club_id equals c.club_id
                            join d in db.Districts on c.district_id equals d.district_id
                           where u.user_code.Equals(login.Username)
                           select new User
                           {
                              user_code=u.user_code,
                              user_pwd=u.user_pwd,
                               eff_date=u.eff_date,
                               exp_date=u.exp_date,
                               upd_date=u.upd_date,
                               first_name=m.first_name,
                               last_name=m.last_name,
                               first_name_eng=m.first_name_eng,
                               last_name_eng=m.last_name_eng, 
                               club_name_thai=c.club_name_thai,                              
                               member_seq =u.member_seq,
                               club_id = m.club_id,
                               district_id = d.district_id,
                               district_name_thai = d.district_name_thai

                           }).FirstOrDefault();


                    //user = dc.TUsers.Where(a => a.user_code.Equals(login.Username)).FirstOrDefault();
                   

                    
                    //var user1 = (from c in dc.TUsers
                    //        where c.user_code.Equals(loing.Username)).FirstOrDefault();


               

                if (user != null)
                {
                    //var image = (from m in db.Members where m.member_seq.Equals(user.member_seq)
                    //                select Image.FromStream(new MemoryStream(m.image.ToArray() ))).FirstOrDefault();


                    // Write log to table TransactionLog
                    //ProjLib projlib = new ProjLib();
                    ProjLib.TransactionLog(user.member_seq,"Login",user.club_id);

                    JavaScriptSerializer js = new JavaScriptSerializer();
                    string data = js.Serialize(user);
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, user.user_code, DateTime.Now, DateTime.Now.AddMinutes(30), login.RememberMe, data);
                    string encToken = FormsAuthentication.Encrypt(ticket);
                    HttpCookie authoCookies = new HttpCookie(FormsAuthentication.FormsCookieName, encToken);
                    Response.Cookies.Add(authoCookies);
                    if (Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else if (user.upd_date != null)
                    {
                        return RedirectToAction("Welcome", "Home");
                    }else { return RedirectToAction("ChangePassword", "Manage"); }
                    
                }
                
            }
            #endregion
            ModelState.Remove("Password");
            return View();
        }
        
    }

   
}
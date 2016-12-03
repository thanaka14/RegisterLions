using System.Linq;
using System.Web.Mvc;
using RegisterLions.Models;
using RegisterLions.Lib;

namespace RegisterLions.Controllers
{
    public class HomeController : Controller
    {
        private RegisterLionsEntities db = new RegisterLionsEntities();

        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                var Tuser = (from u in db.TUsers
                             join m in db.Members on u.member_seq equals m.member_seq
                             join c in db.Clubs on m.club_id equals c.club_id
                             join d in db.MembershipTypes on m.membership_type equals d.membership_type
                             join e in db.Districts on c.district_id equals e.district_id
                             where u.user_code.Equals(HttpContext.User.Identity.Name)
                             select u).FirstOrDefault();
                ViewBag.member_seq = Tuser.member_seq;
                return View(Tuser);
            }else
            {
                // Write log to table TransactionLog
                WriteLog writeLog = new WriteLog();
                writeLog.TransactionLog(0, "Index", 0);
            }
            return View();
            
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            // Write log to table TransactionLog
            WriteLog writeLog = new WriteLog();
            writeLog.TransactionLog(0, "About", 0);
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            // Write log to table TransactionLog
            WriteLog writeLog = new WriteLog();
            writeLog.TransactionLog(0, "Contact", 0);

            return View();
        }
        [Authorize]
        public ActionResult Welcome()
        {
            var identity = (System.Web.HttpContext.Current.User as RegisterLions.MyPrincipal).Identity as RegisterLions.MyIdentity;
            //var Tuser = (from u in db.TUsers
            //             join m in db.Members on u.member_seq equals m.member_seq
            //             join c in db.Clubs on m.club_id equals c.club_id
            //             join d in db.MembershipTypes on m.membership_type equals d.membership_type
            //             join e in db.Districts on c.district_id equals e.district_id
            //             where u.user_code.Equals(HttpContext.User.Identity.Name)
            //             select u).FirstOrDefault();
            var member = (from m in db.Members where m.member_seq == identity.User.member_seq select m).FirstOrDefault(); 
            ViewBag.member_seq = identity.User.member_seq;


            return View(member);
           
        }
    }
}
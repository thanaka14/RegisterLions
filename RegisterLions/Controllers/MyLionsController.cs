using RegisterLions.Models;
using System.Linq;
using System.Web.Mvc;

namespace RegisterLions.Controllers
{
    [Authorize]
    public class MyLionsController : Controller
    {
        private RegisterLionsEntities db = new RegisterLionsEntities();
        // GET: MyLions
        public ActionResult Officers()
        {
            
            var Club = (from u in db.TUsers
                        where u.user_code.Equals(HttpContext.User.Identity.Name)
                        join m in db.Members on u.member_seq equals m.member_seq
                        select m.club_id);

            ViewBag.ClubID = null;
            ViewBag.ClubName = null;
            var club = (from c in db.Clubs where Club.Contains(c.club_id) select c);
            foreach (var x in club)
            {
                ViewBag.ClubName = x.club_name_thai;
                ViewBag.ClubID = x.club_id;
            }
            var District = (from d in db.Districts
                            join c in db.Clubs on d.district_id equals c.district_id
                            where Club.Contains(c.club_id)
                            select d);
            ViewBag.DistrictName = null;
            ViewBag.DistrictID = null;
            foreach (var d in District)
            {
                ViewBag.DistrictName = d.district_name_thai;
                ViewBag.DistrictID = d.district_id;
            }

            
            var clubOfficer = (from a in db.ClubOfficers
                               join b in db.Members on a.member_seq equals b.member_seq                               
                               join c in db.Clubs on b.club_id equals c.club_id
                               join e in db.Districts on c.district_id equals e.district_id
                               join d in db.Officers on a.officer_id equals d.officer_id
                               join u in db.TUsers on b.member_seq equals u.member_seq
                               where u.user_code.Equals(HttpContext.User.Identity.Name)
                               orderby a.fiscal_year
                               select a);
            return View(clubOfficer);
           
        
    }
    }
}
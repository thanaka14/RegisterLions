﻿using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.Security;
using RegisterLions.Models;
using System.Security.Cryptography;

namespace RegisterLions
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_PostAuthenticateRequest()
        {
            HttpCookie authoCookies = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authoCookies != null)
            {
                try { 
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authoCookies.Value);
                JavaScriptSerializer js = new JavaScriptSerializer();
                User user = js.Deserialize<User>(ticket.UserData);
                MyIdentity myIdentity = new MyIdentity(user);
                MyPrincipal myPrincipal = new MyPrincipal(myIdentity);
                HttpContext.Current.User = myPrincipal;
                }
                catch (CryptographicException cex)
                {
                    FormsAuthentication.SignOut();
                }
            }
        }
    }
    
}

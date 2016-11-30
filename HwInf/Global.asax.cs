using HwInf.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Caching;
using System.Web.Http;
using System.Web.Routing;

namespace HwInf
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public override void Init()
        {
            base.Init();
            this.AuthorizeRequest += MvcApplication_AuthorizeRequest;
        }

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        void MvcApplication_AuthorizeRequest(object sender, EventArgs e)
        {
            var user = HttpContext.Current.User.Identity;
            if (user.IsAuthenticated)
            {
                IPrincipal principal = null;

                if (user.Name.EndsWith(",admin"))
                {
                    // Handle impersonation
                    principal = new GenericPrincipal(new GenericIdentity(user.Name.Split(',').First()), new string[] { "Admin" });
                }
                else
                {
                    var key = string.Format("roles-{0}", user.Name);
                    var roles = HttpRuntime.Cache.Get(key) as string[];
                    if (roles == null)
                    {
                        var userModel = LDAPAuthenticator.GetUserParameter(user.Name);
                        if (userModel.PersonalType == "Teacher")
                        {
                            roles = new string[] { "Admin" };
                        }
                        else
                        {
                            roles = new string[] { };
                        }
                        HttpRuntime.Cache.Insert(key, roles, null, Cache.NoAbsoluteExpiration, TimeSpan.FromHours(1));
                    }
                    principal = new GenericPrincipal(user, roles);
                }

                System.Threading.Thread.CurrentPrincipal = principal;
                HttpContext.Current.User = principal;
            }
        }
    }
}

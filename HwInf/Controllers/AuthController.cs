using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HwInf.Common;
using System.Web.Security;

namespace HwInf.Controllers
{

    public class AuthController : ApiController
    {

        [HttpPost]
        public IHttpActionResult Login([FromBody]string user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string uid = user.Split(',')[0];
            string pass = user.Split(',')[1];

            if (LDAPAuthenticator.Authenticate(uid, pass).IsAuthenticated)
            {
                FormsAuthentication.SetAuthCookie(uid, true);
                return Ok(LDAPAuthenticator.Authenticate(uid, pass));
            }
            else
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return Unauthorized();
            }
        }

        [HttpPost]
        public IHttpActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return Ok("LogOff successful.");
        }



    }
}

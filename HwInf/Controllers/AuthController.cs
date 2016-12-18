using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HwInf.Common;
using System.Web.Security;
using HwInf.ViewModels;
using JWT;
using System.Security.Cryptography;
using System.Text;
using HwInf.Common.DAL;

namespace HwInf.Controllers
{
    [RoutePrefix("api/auth")]
    public class AuthController : ApiController
    {

        private HwInfContext db = new HwInfContext();

        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        public IHttpActionResult SignIn(UserViewModel vmdl)
        {
            
            if (ModelState.IsValid)
            {
                if (LDAPAuthenticator.Authenticate(vmdl.Uid, vmdl.Password).IsAuthenticated)
                {
                    Person p;

                    if(db.Persons.Where(i => i.uid == vmdl.Uid).Count() > 0)
                    {
                        p = db.Persons.Single(i => i.uid == vmdl.Uid);
                    } else
                    {
                        //Noch nix. LDAP/SOAP UserDaten.
                        p = new Person();
                    }

                    vmdl.Refresh(p);
                    var token = CreateToken(p);

                    return Ok(new { token });
                } else
                {
                    return Unauthorized();
                }

            }

            return BadRequest(ModelState);
        }


        private static string CreateToken(Person p)
        {
            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var expiry = Math.Round((DateTime.UtcNow.AddHours(2) - unixEpoch).TotalSeconds);
            var issuedAt = Math.Round((DateTime.UtcNow - unixEpoch).TotalSeconds);
            var notBefore = Math.Round((DateTime.UtcNow.AddMonths(6) - unixEpoch).TotalSeconds);


            var payload = new Dictionary<string, object>
            {
                {"uid", p.uid},
                {"role", p.Role.Name  },
                {"nbf", notBefore},
                {"iat", issuedAt},
                {"exp", expiry}
            };

            //var secret = ConfigurationManager.AppSettings.Get("jwtKey");
            const string apikey = "secretKey";

            var token = JsonWebToken.Encode(payload, apikey, JwtHashAlgorithm.HS256);

            return token;
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

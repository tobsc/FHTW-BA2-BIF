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
using System.Web.Http.Description;

namespace HwInf.Controllers
{
    [RoutePrefix("api/auth")]
    public class AuthControllerA : ApiController
    {

        private HwInfContext db = new HwInfContext();

        [AllowAnonymous]
        [ResponseType(typeof(string))]
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
                        
                        p = new Person();
                        var ldapUser = LDAPAuthenticator.Authenticate(vmdl.Uid, vmdl.Password);
                        vmdl.Refresh(ldapUser);
                        vmdl.ApplyChanges(p, db);
                        db.Persons.Add(p);
                        db.SaveChanges();
                    }

                    vmdl.Refresh(p);
                    var token = CreateToken(p);

                    return Ok(new { token });
                } else if(vmdl.Uid.Equals("admin"))
                {
                    Person p = new Person { uid = vmdl.Uid, Name = "Admin", Role = db.Roles.Single(i => i.Name == "Admin") };
                    var token = CreateToken(p);

                    return Ok(new { token });
                }
                {
                    return Unauthorized();
                }

            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Returns a test token.
        /// </summary>
        /// <param name="minutes">Minutes</param>
        /// <param name="role">Admin, User, Verwalter</param>
        /// <param name="uid">UID</param>
        /// <returns></returns>
        [Route("testToken/{minutes}/{role}")]
        public IHttpActionResult CreateTestToken(int minutes = 1, string role = "User", string uid = "test")
        {
            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var expiry = Math.Round((DateTime.UtcNow.AddMinutes(minutes) - unixEpoch).TotalSeconds);
            var issuedAt = Math.Round((DateTime.UtcNow - unixEpoch).TotalSeconds);
            var notBefore = Math.Round((DateTime.UtcNow.AddMonths(6) - unixEpoch).TotalSeconds);


            var payload = new Dictionary<string, object>
            {
                {"uid", uid},
                {"role", role  },
                {"nbf", notBefore},
                {"iat", issuedAt},
                {"exp", expiry}
            };

            //var secret = ConfigurationManager.AppSettings.Get("jwtKey");
            const string apikey = "secretKey";

            var token = JsonWebToken.Encode(payload, apikey, JwtHashAlgorithm.HS256);

            return Ok(new { token });
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using HwInf.Common;
using JWT;
using HwInf.Common.DAL;
using System.Web.Http.Description;
using HwInf.Common.BL;
using HwInf.Common.Models;
using HwInf.ViewModels;

namespace HwInf.Controllers
{
    [RoutePrefix("api/auth")]
    public class AuthController : ApiController
    {

        private readonly HwInfContext _db = new HwInfContext();
        private readonly BL _bl;

        public AuthController()
        {
            _bl = new BL(_db);
        }

        [AllowAnonymous]
        [ResponseType(typeof(string))]
        [Route("login")]
        [HttpPost]
        public IHttpActionResult SignIn(UserViewModel vmdl)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!LDAPAuthenticator.Authenticate(vmdl.Uid, vmdl.Password).IsAuthenticated) return Unauthorized();

            var p = new Person();

            if (_bl.GetUsers(vmdl.Uid) != null)
            {
                p = _bl.GetUsers(vmdl.Uid);
                _bl.UpdateUser(p);
            }
            else
            {
                p = _bl.CreateUser();
            }

            // Load user data from LDAP and save them into DB
            var ldapUser = LDAPAuthenticator.Authenticate(vmdl.Uid, vmdl.Password);
            vmdl.Refresh(ldapUser);
            bool isAdmin = _bl.IsAdminRole(vmdl.Uid);
            vmdl.ApplyChanges(p, _bl);
            if (isAdmin) p.Role = _bl.GetRole("Admin");
            if (_bl.IsVerwalterRole(vmdl.Uid)) p.Role = _bl.GetRole("Owner");
            _db.SaveChanges();

            // Create new token from user
            var token = _bl.CreateToken(p);

            return Ok(new { token });
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }


    }
}

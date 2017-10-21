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
using log4net;

namespace HwInf.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// Controller managing User Login and Impersonation
    /// </summary>
    [RoutePrefix("api/auth")]
    public class AuthController : ApiController
    {

        private readonly IDAL _db;
        private readonly BL _bl;
        private readonly ILog _log = LogManager.GetLogger(typeof(AuthController).Name);

        public AuthController()
        {
            _db = new HwInfContext();
            _bl = new BL(_db);
        }

        public AuthController(IDAL db)
        {
            _db = db;
            _bl = new BL(db);
        }

        /// <summary>
        /// Sign in to HW-INF
        /// </summary>
        /// <remarks>Signs a user in.</remarks>
        /// <param name="vmdl">User as &#x60;UserViewModel&#x60;</param>
        /// <response code="401">An error occured, unauthorized</response>
        /// <response code="500">An error occured, please read log files</response>
        /// <response code="200"></response>
        [AllowAnonymous]
        [ResponseType(typeof(string))]
        [Route("login")]
        [HttpPost]
        public IHttpActionResult SignIn(UserViewModel vmdl)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!LDAPAuthenticator.Authenticate(vmdl.Uid, vmdl.Password).IsAuthenticated)
            {
                _log.WarnFormat("Failed login attempt for '{0}'", vmdl.Uid);
                return Unauthorized();
            }

            Person p;

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
            vmdl.ApplyChanges(p, _bl);
            _db.SaveChanges();

            // Create new token from user
            var token = _bl.CreateToken(p);

            return Ok(new { token });
        }

        /// <summary>
        /// Impersonate User
        /// </summary>
        /// <remarks>Lets an Admin log into another users Account and returns a new JWT</remarks>
        /// <param name="uid">UID of user one want to login to</param>
        /// <response code="500">An error occured, please read log files</response>
        /// <response code="200"></response>
        /// <produces>application/json</produces>
        [ResponseType(typeof(JsonWebToken))]
        [Route("impersonate/{uid}")]
        [Authorize(Roles = "Admin, Verwalter")]
        public IHttpActionResult GetImpersonate(string uid)
        {

            var p = _bl.GetUsers(uid);

            // Create new token from user
            var token = _bl.CreateToken(p);

            return Ok(new { token });
        }

        ///// <summary>
        ///// Returns a test token.
        ///// </summary>
        ///// <param name="minutes">Minutes</param>
        ///// <param name="role">Admin, User, Verwalter</param>
        ///// <param name="uid">UID</param>
        ///// <returns></returns>

        //[Route("testToken/{minutes}/{role}")]
        //public IHttpActionResult CreateTestToken(int minutes = 1, string role = "User", string uid = "test")
        //{
        //    var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        //    var expiry = Math.Round((DateTime.UtcNow.AddMinutes(minutes) - unixEpoch).TotalSeconds);
        //    var issuedAt = Math.Round((DateTime.UtcNow - unixEpoch).TotalSeconds);
        //    var notBefore = Math.Round((DateTime.UtcNow.AddMonths(6) - unixEpoch).TotalSeconds);


        //    var payload = new Dictionary<string, object>
        //    {
        //        {"uid", uid},
        //        {"role", role  },
        //        {"nbf", notBefore},
        //        {"iat", issuedAt},
        //        {"exp", expiry}
        //    };

        //    //var secret = ConfigurationManager.AppSettings.Get("jwtKey");
        //    const string apikey = "secretKey";

        //    var token = JsonWebToken.Encode(payload, apikey, JwtHashAlgorithm.HS256);

        //    return Ok(new { token });
        //}

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

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

namespace HwInf.Controllers
{

    public class AuthController : ApiController
    {
    
        [AllowAnonymous]
        [HttpPost]
        public IHttpActionResult SignIn(LoginViewModel model)
        {
            
            if (ModelState.IsValid)
            {


                if (LDAPAuthenticator.Authenticate(model.Uid, model.Password).IsAuthenticated)
                {
                    object dbUser;
                    var token = CreateToken(model, out dbUser);
                    return Ok(new { token });
                } else
                {
                    return Unauthorized();
                }

            }

            return BadRequest(ModelState);
        }


        private static string CreateToken(LoginViewModel vmdl, out object dbUser)
        {
            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var expiry = Math.Round((DateTime.UtcNow.AddHours(2) - unixEpoch).TotalSeconds);
            var issuedAt = Math.Round((DateTime.UtcNow - unixEpoch).TotalSeconds);
            var notBefore = Math.Round((DateTime.UtcNow.AddMonths(6) - unixEpoch).TotalSeconds);


            var payload = new Dictionary<string, object>
            {
                {"uid", vmdl.Uid},
                {"role", "User"  },
                {"nbf", notBefore},
                {"iat", issuedAt},
                {"exp", expiry}
            };

            //var secret = ConfigurationManager.AppSettings.Get("jwtKey");
            const string apikey = "secretKey";

            var token = JsonWebToken.Encode(payload, apikey, JwtHashAlgorithm.HS256);

            dbUser = new { vmdl.Uid };
            return token;
        }

        /// <summary>
        ///     Creates a random salt to be used for encrypting a password
        /// </summary>
        /// <returns></returns>
        public static string CreateSalt()
        {
            var data = new byte[0x10];
            using (var cryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                cryptoServiceProvider.GetBytes(data);
                return Convert.ToBase64String(data);
            }
        }

        /// <summary>
        ///     Encrypts a password using the given salt
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string EncryptPassword(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var saltedPassword = string.Format("{0}{1}", salt, password);
                var saltedPasswordAsBytes = Encoding.UTF8.GetBytes(saltedPassword);
                return Convert.ToBase64String(sha256.ComputeHash(saltedPasswordAsBytes));
            }
        }


    }
}

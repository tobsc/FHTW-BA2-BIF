using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Claims;
using System.Text;
using HwInf.BusinessLogic.Config;
using HwInf.BusinessLogic.Interfaces;
using HwInf.Common.Models;
using HwInf.DataAccess.Interfaces;
using log4net;
using log4net.Appender;
using Microsoft.IdentityModel.Tokens;

namespace HwInf.BusinessLogic
{
    public class BusinessLogic : IBusinessLogic
    {
        private readonly IDataAccessLayer _dal;
        private readonly IBusinessLogicPrincipal _principal;

        public BusinessLogic(IDataAccessLayer dal, IBusinessLogicPrincipal principal)
        {
            _dal = dal;
            _principal = principal;
        }

        public bool IsAdmin => _principal.IsAdmin;
        public bool IsVerwalter => _principal.IsVerwalter;
        public string[] GetLog()
        {
            if (!_principal.IsAllowed) throw new SecurityException();

            var h = (log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository("log4net-default-repository");
            var appender = h.GetAppenders().SingleOrDefault(i => i.Name.Equals("FileRollingFileAppender")) as FileAppender;

            string[] lines = { };
            if (appender == null) return lines;
            var file = appender.File;
            lines = File.ReadAllLines(@file, Encoding.UTF8);

            return lines;
        }

        public bool IsAdminUid(string uid)
        {
            return _dal.Persons.Any(i => i.Uid == uid && i.Role.Name.Equals("Admin"));
        }


        public string GetCurrentUid()
        {
            return _principal.CurrentUid;
        }

        public JwtSecurityToken CreateToken(Person p)
        {
            var claims = new[]
            {
                new Claim("Uid", p.Uid),
                new Claim("Name", p.Name),
                new Claim("LastName", p.LastName),
                new Claim("DisplayName", p.Name + " " + p.LastName),
                new Claim("Role", p.Role.Name),
                new Claim(ClaimTypes.Role, p.Role.Name),
                new Claim(ClaimTypes.Name, p.Uid), 
                new Claim("isLoggedInAs", IsAdminUid(p.Uid) || _principal.IsAdmin ? "1" : "0"), 
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtConfig.Current.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: JwtConfig.Current.Issuer,
                audience: JwtConfig.Current.Audience,
                claims: claims,
                expires: DateTime.Now.AddHours(JwtConfig.Current.HoursValid),
                signingCredentials: creds);

            return token;
        }
    }
}
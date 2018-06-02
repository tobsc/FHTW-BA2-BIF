using System.Globalization;
using System.Security.Claims;
using System.Threading;
using HwInf.Common.BL;
using HwInf.Common.Interfaces;
using HwInf.UnitTests.DAL;

namespace HwInf.UnitTests.Controllers
{
    public abstract class ControllerTests
    {

        protected readonly IDataAccessLayer _dal = new MockDAL();
        protected readonly IBusinessLayer _bl;


        protected ControllerTests()
        {
            _bl = new BusinessLayer(_dal);

            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            var subject = new ClaimsIdentity("Federation", ClaimTypes.Name, ClaimTypes.Role);
            subject.AddClaim(new Claim(ClaimTypes.Role, "Admin", ClaimValueTypes.String));
            subject.AddClaim(new Claim(ClaimTypes.Name, "if15b032", ClaimValueTypes.String));
            Thread.CurrentPrincipal = new ClaimsPrincipal(subject);
        }
    }
}
using System.Globalization;
using System.Security.Claims;
using System.Threading;
using HwInf.Common.DAL;
using HwInf.Controllers;
using HwInf.Tests.DAL;

namespace HwInf.Tests.Controllers
{
    public abstract class ControllerTests
    {

        protected readonly IDAL _dal = new MockDAL();
        protected readonly Common.BL.BL _bl;


        protected ControllerTests()
        {
            _bl = new Common.BL.BL(_dal);

            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            var subject = new ClaimsIdentity("Federation", ClaimTypes.Name, ClaimTypes.Role);
            subject.AddClaim(new Claim(ClaimTypes.Role, "Admin", ClaimValueTypes.String));
            Thread.CurrentPrincipal = new ClaimsPrincipal(subject);
        }
    }
}
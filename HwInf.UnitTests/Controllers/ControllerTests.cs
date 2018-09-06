using System.Globalization;
using System.Security.Claims;
using System.Threading;
using HwInf.BusinessLogic;
using HwInf.BusinessLogic.Interfaces;
using HwInf.DataAccess.Interfaces;
using HwInf.UnitTests.DAL;
using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace HwInf.UnitTests.Controllers
{
    public abstract class ControllerTests
    {

        protected readonly IDataAccessLayer Dal = new MockDAL();
        protected readonly IBusinessLogicFacade Bl;
        protected readonly ControllerContext _controllerContext;


        protected ControllerTests()
        {
            var mockPrincipal = new Mock<IBusinessLogicPrincipal>();
            mockPrincipal.Setup(x => x.IsAllowed).Returns(true);
            mockPrincipal.Setup(x => x.IsAdmin).Returns(true);
            mockPrincipal.Setup(x => x.IsVerwalter).Returns(false);
            mockPrincipal.Setup(x => x.CurrentUid).Returns("if15b032");
            var abl = new AccessoryBusinessLogic(Dal, mockPrincipal.Object);
            var cbl = new CustomFieldsBusinessLogic(Dal, mockPrincipal.Object);
            var dbl = new DamageBusinessLogic(Dal, mockPrincipal.Object);
            var debl = new DeviceBusinessLogic(Dal, mockPrincipal.Object);
            var blbl = new BusinessLogic.BusinessLogic(Dal, mockPrincipal.Object);
            var obl = new OrderBusinessLogic(Dal, mockPrincipal.Object);
            var sbl = new SettingBusinessLogic(Dal, mockPrincipal.Object);
            var ubl = new UserBusinessLogic(Dal, mockPrincipal.Object);
            Bl = new BusinessLogicFacade(Dal, blbl, abl, cbl, dbl, debl, obl, sbl, ubl);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            var subject = new ClaimsIdentity("Federation", ClaimTypes.Name, ClaimTypes.Role);
            subject.AddClaim(new Claim(ClaimTypes.Role, "Admin", ClaimValueTypes.String));
            subject.AddClaim(new Claim(ClaimTypes.Name, "if15b032", ClaimValueTypes.String));
            _controllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(subject)
                }
            };

        }
    }
}
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Results;
using HwInf.Common.BL;
using HwInf.Common.DAL;
using HwInf.Controllers;
using HwInf.ViewModels;
using NUnit.Framework;
using HwInf.Common.Models;
using HwInf.Tests.DAL;

namespace HwInf.Tests.Controllers
{
    [TestFixture]
    public class DeviceControllerTests
    {
        private readonly IDAL _dal = new MockDAL();
        private readonly Common.BL.BL _bl;
        private readonly DevicesController ctr;


        public DeviceControllerTests()
        {
            _bl = new Common.BL.BL(_dal);
            ctr = new DevicesController(_dal);

            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");           
            var subject = new ClaimsIdentity("Federation", ClaimTypes.Name, ClaimTypes.Role);
            subject.AddClaim(new Claim(ClaimTypes.Role, "Admin", ClaimValueTypes.String));
            Thread.CurrentPrincipal = new ClaimsPrincipal(subject);
        }

        [Test]
        public void HelloWorld()
        {
        }

        [Test]
        public void ctr_should_return_single_device_by_id()
        {
            var c = _bl.GetDevices().FirstOrDefault();
            var fg = _bl.GetFieldGroups("anschluesse");
            var d = _bl.CreateDevice();
            d = c;
            d.DeviceId = 1;
            d.Type.FieldGroups.Add(fg);

            var obj = ctr.GetDevice(1);
            var res = obj as OkNegotiatedContentResult<DeviceViewModel>;
            Assert.NotNull(res);
            Assert.NotNull(res.Content);
        }
    }
}

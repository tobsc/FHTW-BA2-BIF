using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HwInf.Common.BL;
using HwInf.Common.DAL;
using HwInf.Controllers;
using HwInf.ViewModels;
using NUnit.Framework;
using HwInf.Common.Models;

namespace HwInf.Tests
{
    [TestFixture]
    public class HwInfTests
    {
        private readonly BL _bl = new BL();
        private readonly DevicesController _ctrl = new DevicesController();
        private readonly HwInfContext _dal = new HwInfContext();

        public HwInfTests()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");


            var subject = new ClaimsIdentity("Federation", ClaimTypes.Name, ClaimTypes.Role);
           
            subject.AddClaim(new Claim(ClaimTypes.Role, "Admin", ClaimValueTypes.String));

            Thread.CurrentPrincipal = new ClaimsPrincipal(subject);
        }

        [Test]
        public void HelloWorld()
        {
            Console.WriteLine("test");
            Console.WriteLine(System.Threading.Thread.CurrentPrincipal.Identity.Name);
        }

        [Test]
        public void bl_should_create_device()
        {
            
            var device = _bl.CreateDevice();
            Assert.NotNull(device);
        }

        [Test]
        public void bl_should_create_deviceType()
        {

            var obj = _bl.CreateDeviceType();
            Assert.NotNull(obj);
        }


        [Test]
        public void bl_should_create_field()
        {
            var obj = _bl.CreateField();
            Assert.NotNull(obj);
        }


        [Test]
        public void bl_should_create_fieldGroup()
        {
            var obj = _bl.CreateFieldGroup();
            Assert.NotNull(obj);
        }

        [Test]
        public void bl_should_return_deviceType_by_slug()
        {
            var obj = _bl.CreateField();
            obj.Slug = "testtype";

            var getObj = _bl.GetFields("testtype");

            Assert.True(obj.Equals(getObj));

        }

    }
}

using System;
using HwInf.Web.Controllers;
using HwInf.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace HwInf.UnitTests.Controllers
{
    [TestFixture]
    public class SettingsControllerTests : ControllerTests
    {
        private readonly SettingsController ctr;


        public SettingsControllerTests()
        {
            var log = new Mock<ILogger<SettingsController>>();
            ctr = new SettingsController(Bl, log.Object) {ControllerContext = _controllerContext};
        }

        [Test]
        public void HelloWorld()
        {
        }

        [Test]
        public void ctr_should_create_setting()
        {
            var key = Guid.NewGuid().ToString();

            var vmdl = new SettingViewModel
            {
                Key = key,
                Value =
                    "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet."
            };

            var obj = (ctr.PostSetting(vmdl) as OkObjectResult)?.Value as SettingViewModel;

            Assert.NotNull(obj);

            var setting = Bl.GetSetting(key);
            Assert.NotNull(setting);
        }

        [Test]
        public void ctr_should_not_create_duplicate_settings()
        {
            var key = Guid.NewGuid().ToString();
            var vmdl = new SettingViewModel
            {
                Key = key,
                Value =
                    "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet."
            };

            var obj = (ctr.PostSetting(vmdl) as OkObjectResult)?.Value as SettingViewModel;

            Assert.NotNull(obj);

            var setting = Bl.GetSetting(key);
            Assert.NotNull(setting);

            var badObj = ctr.PostSetting(vmdl) as BadRequestObjectResult;
            Assert.NotNull(badObj);
            Assert.AreEqual(400, badObj.StatusCode);
        }

        [Test]
        public void ctr_should_delete_setting()
        {
            var key = Guid.NewGuid().ToString();
            var vmdl = new SettingViewModel
            {
                Key = key,
                Value =
                    "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet."
            };

            var obj = (ctr.PostSetting(vmdl) as OkObjectResult)?.Value as SettingViewModel;
            Assert.NotNull(obj);
            var setting = Bl.GetSetting(key);
            Assert.NotNull(setting);

            ctr.DeleteSetting(key);

            var notFoundResult = ctr.GetSetting(key) as NotFoundResult;
            Assert.NotNull(notFoundResult);
        }

    }
}
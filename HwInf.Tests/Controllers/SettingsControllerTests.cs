using System;
using System.Web.Http.Results;
using HwInf.Common.BL;
using HwInf.Controllers;
using HwInf.ViewModels;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace HwInf.Tests.Controllers
{
    [TestFixture]
    public class SettingsControllerTests : ControllerTests
    {
        private readonly SettingsController ctr;


        public SettingsControllerTests()
        {
            ctr = new SettingsController(_bl);
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

            var obj = ctr.PostSetting(vmdl) as OkNegotiatedContentResult<SettingViewModel>;

            Assert.NotNull(obj);
            Assert.NotNull(obj.Content);

            var setting = _bl.GetSetting(key);
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

            var obj = ctr.PostSetting(vmdl) as OkNegotiatedContentResult<SettingViewModel>;

            Assert.NotNull(obj);
            Assert.NotNull(obj.Content);

            var setting = _bl.GetSetting(key);
            Assert.NotNull(setting);

            var badObj = ctr.PostSetting(vmdl) as BadRequestErrorMessageResult;
            Assert.NotNull(badObj);
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

            var obj = ctr.PostSetting(vmdl) as OkNegotiatedContentResult<SettingViewModel>;

            Assert.NotNull(obj);
            Assert.NotNull(obj.Content);

            var setting = _bl.GetSetting(key);
            Assert.NotNull(setting);

            ctr.DeleteSetting(key);

            var notFoundResult = ctr.GetSetting(key) as NotFoundResult;
            Assert.NotNull(notFoundResult);
        }

    }
}
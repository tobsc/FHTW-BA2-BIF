using System;
using System.Collections.Generic;
using System.Linq;
using HwInf.Web.Controllers;
using HwInf.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace HwInf.UnitTests.Controllers
{
    [TestFixture]
    public class DeviceControllerTests : ControllerTests
    {
        private readonly DevicesController ctr;

        public DeviceControllerTests()
        {
            ctr = new DevicesController(Bl);
            ctr.ControllerContext = _controllerContext;

        }
        [Test]
        public void HelloWorld()
        {
        }

        [Test]
        public void ctr_should_return_single_device_by_id()
        {
            var obj = ctr.GetDevice(1) as OkObjectResult;
            var res = obj?.Value as DeviceViewModel;
            Assert.NotNull(res);
        }

        [Test]
        public void ctr_should_return_devices()
        {
            var obj = ctr.GetAll() as OkObjectResult;
            var res = obj?.Value as DeviceListViewModel;
            Assert.NotNull(res);
        }

        [Test]
        public void ctr_should_return_device_by_invNum()
        {
            var obj = ctr.GetDevice("a5123") as OkObjectResult;
            var res = obj?.Value as DeviceViewModel;
            Assert.NotNull(res);
        }

        [Test]
        public void ctr_should_return_filtered_list_by_type()
        {
            var vmdl = ControllerHelper.GetValidFilterViewModel("pc");

            var obj = ctr.PostFilter(vmdl) as OkObjectResult;
            var res = obj?.Value as DeviceListViewModel;
            Assert.NotNull(res);
            Assert.True(res.Devices.Count() == 3);
        }

        [Test]
        public void ctr_should_return_filtered_list()
        {
            var vmdl = ControllerHelper.GetValidFilterViewModel();

            var obj = ctr.PostFilter(vmdl) as OkObjectResult;
            var res = obj?.Value as DeviceListViewModel;
            Assert.NotNull(res);
            Assert.True(res.Devices.Count() == 6);
        }

        [Test]
        public void ctr_should_return_filtered_list_by_fieldgroup_przessoren()
        {
            var vmdl = ControllerHelper.GetValidFilterViewModel("","prozessoren", "intel-i5");

            var obj = ctr.PostFilter(vmdl) as OkObjectResult;
            var res = obj?.Value as DeviceListViewModel;
            Assert.NotNull(res);
            Assert.True(res.Devices.Count() == 3);
        }

        [Test]
        public void ctr_should_return_filtered_list_by_fieldgroup_hdmi()
        {
            var vmdl = ControllerHelper.GetValidFilterViewModel("", "anschluesse", "hdmi", "8");

            var obj = ctr.PostFilter(vmdl) as OkObjectResult;
            var res = obj?.Value as DeviceListViewModel;
            Assert.NotNull(res);
            Assert.True(res.Devices.Count() == 1);
        }

        [Test]
        public void ctr_should_return_deviceTypes()
        {

            var obj = ctr.GetDeviceTypes() as OkObjectResult;
            var res = obj?.Value as List<DeviceTypeViewModel>;
            Assert.NotNull(res);
            Assert.True(res.Count == 4);
        }

        [Test]
        public void ctr_should_create_device()
        {
            var vmdl = ControllerHelper.GetValidDeviceViewModel();
            vmdl.InvNum = Guid.NewGuid().ToString();
            var obj = ctr.PostDevice(vmdl) as OkObjectResult;
            var res = obj?.Value as List<DeviceViewModel>;
            Assert.NotNull(res);

            var d = Bl.GetDevices();
            Assert.True(d.Any(i => i.InvNum.Equals(vmdl.InvNum)));
        }

        [Test]
        public void ctr_should_create_multitple_devices()
        {
            var invNum1 = Guid.NewGuid().ToString();
            var invNum2 = Guid.NewGuid().ToString();
            var vmdl = ControllerHelper.GetValidDeviceViewModel();
            vmdl.InvNum = invNum1;
            vmdl.AdditionalInvNums = new List<AdditionalInvNumViewModel>
            {
                new AdditionalInvNumViewModel {InvNum = invNum2}
            };
            var obj = ctr.PostDevice(vmdl) as OkObjectResult;
            var res = obj?.Value as List<DeviceViewModel>;
            Assert.NotNull(res);

            var d = Bl.GetDevices();
            Assert.True(d.Any(i => i.InvNum.Equals(invNum1)));
            Assert.True(d.Any(i => i.InvNum.Equals(invNum2)));
        }

        [Test]
        public void ctr_should_not_create_device_when_invalid()
        {
            var vmdl = ControllerHelper.GetInValidDeviceViewModel();
            vmdl.InvNum = Guid.NewGuid().ToString();
            var obj = ctr.PostDevice(vmdl);
            var res = obj as StatusCodeResult;
            Assert.AreEqual(500, res?.StatusCode);
        }

        [Test]
        public void ctr_should_delete_device()
        {
            var vmdl = ControllerHelper.GetValidDeviceViewModel();
            vmdl.InvNum = Guid.NewGuid().ToString();
            var obj = ctr.PostDevice(vmdl) as OkObjectResult;
            var res = obj?.Value as List<DeviceViewModel>;
            Assert.NotNull(res);

            var d = Bl.GetSingleDevice(vmdl.InvNum);
            Assert.True(d.InvNum.Equals(vmdl.InvNum));

            var deviceToDelete = Bl.GetSingleDevice(vmdl.InvNum);
            deviceToDelete.DeviceId = 1010;
            ctr.DeleteDevice(deviceToDelete.DeviceId);
            var deletedDevice = Bl.GetSingleDevice(vmdl.InvNum);
            Assert.True(deletedDevice.IsActive.Equals(false));
        }

        [Test]
        public void ctr_should_delete_deviceType()
        {
            var s = "delete-me";
            var obj = Bl.CreateDeviceType();
            obj.Slug = s;
            var getObj = Bl.GetDeviceType(s);
            Assert.NotNull(getObj);

            ctr.DeleteDeviceType("delete-me");
            var getDel = Bl.GetDeviceType("delete-me");
            Assert.Null(getDel);
        }

        [Test]
        public void ctr_should_update_device()
        {
            var obj = ctr.GetDevice("a5123") as OkObjectResult;
            var res = obj?.Value as DeviceViewModel;
            Assert.NotNull(res);
            var device = res;
            device.DeviceId = 1011;
            device.Name = "Neuer Name";
            ctr.PutDevice(1011, device);

            Assert.NotNull(res);
            var device2 = res;

            Assert.True(device2.Name.Equals("Neuer Name"));

        }

        //[Test]
        //public void ctr_should_update_deviceType()
        //{
        //    var dt = _bl.GetDeviceType("pc");

        //    var dtvmdl = new DeviceTypeViewModel();
        //    dtvmdl.Refresh(dt);
        //    var oldName = dtvmdl.Name;
        //    dtvmdl.Name = "PCs";

        //    var obj = ctr.PutDeviceType(dtvmdl.Slug, dtvmdl);
        //    var res = obj as OkNegotiatedContentResult<DeviceTypeViewModel>;
        //    Assert.NotNull(res);
        //    Assert.False(oldName.Equals(res.Content.Name));


        //}
    }
}

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
    public class DeviceControllerTests : ControllerTests
    {
        private readonly DevicesController ctr;

        public DeviceControllerTests()
        {
            ctr = new DevicesController(_bl);

        }
        [Test]
        public void HelloWorld()
        {
        }

        [Test]
        public void ctr_should_return_single_device_by_id()
        {
            var obj = ctr.GetDevice(1);
            var res = obj as OkNegotiatedContentResult<DeviceViewModel>;
            Assert.NotNull(res);
            Assert.NotNull(res.Content);
        }

        [Test]
        public void ctr_should_return_devices()
        {
            var obj = ctr.GetAll();
            var res = obj as OkNegotiatedContentResult<DeviceListViewModel>;
            Assert.NotNull(res);
            Assert.NotNull(res.Content);
        }

        [Test]
        public void ctr_should_return_device_by_invNum()
        {
            var obj = ctr.GetDevice("a5123");
            var res = obj as OkNegotiatedContentResult<DeviceViewModel>;
            Assert.NotNull(res);
            Assert.NotNull(res.Content);
        }

        [Test]
        public void ctr_should_return_filtered_list_by_type()
        {
            var vmdl = ControllerHelper.GetValidFilterViewModel("pc");

            var obj = ctr.PostFilter(vmdl);
            var res = obj as OkNegotiatedContentResult<DeviceListViewModel>;
            Assert.NotNull(res);
            Assert.True(res.Content.Devices.Count() == 3);
        }

        [Test]
        public void ctr_should_return_filtered_list()
        {
            var vmdl = ControllerHelper.GetValidFilterViewModel();

            var obj = ctr.PostFilter(vmdl);
            var res = obj as OkNegotiatedContentResult<DeviceListViewModel>;
            Assert.NotNull(res);
            Assert.True(res.Content.Devices.Count() == 6);
        }

        [Test]
        public void ctr_should_return_filtered_list_by_fieldgroup_przessoren()
        {
            var vmdl = ControllerHelper.GetValidFilterViewModel("","prozessoren", "intel-i5");

            var obj = ctr.PostFilter(vmdl);
            var res = obj as OkNegotiatedContentResult<DeviceListViewModel>;
            Assert.NotNull(res);
            Assert.True(res.Content.Devices.Count() == 3);
        }

        [Test]
        public void ctr_should_return_filtered_list_by_fieldgroup_hdmi()
        {
            var vmdl = ControllerHelper.GetValidFilterViewModel("", "anschluesse", "hdmi", "8");

            var obj = ctr.PostFilter(vmdl);
            var res = obj as OkNegotiatedContentResult<DeviceListViewModel>;
            Assert.NotNull(res);
            Assert.True(res.Content.Devices.Count() == 1);
        }

        [Test]
        public void ctr_should_return_deviceTypes()
        {

            var obj = ctr.GetDeviceTypes();
            var res = obj as OkNegotiatedContentResult<List<DeviceTypeViewModel>>;
            Assert.NotNull(res);
            Assert.True(res.Content.Count == 4);
        }

        [Test]
        public void ctr_should_create_device()
        {
            var vmdl = ControllerHelper.GetValidDeviceViewModel();
            vmdl.InvNum = Guid.NewGuid().ToString();
            var obj = ctr.PostDevice(vmdl);
            var res = obj as OkNegotiatedContentResult<List<DeviceViewModel>>;
            Assert.NotNull(res);
            Assert.NotNull(res.Content);

            var d = _bl.GetDevices();
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
            var obj = ctr.PostDevice(vmdl);
            var res = obj as OkNegotiatedContentResult<List<DeviceViewModel>>;
            Assert.NotNull(res);
            Assert.NotNull(res.Content);

            var d = _bl.GetDevices();
            Assert.True(d.Any(i => i.InvNum.Equals(invNum1)));
            Assert.True(d.Any(i => i.InvNum.Equals(invNum2)));
        }

        [Test]
        public void ctr_should_not_create_device_when_invalid()
        {
            var vmdl = ControllerHelper.GetInValidDeviceViewModel();
            vmdl.InvNum = Guid.NewGuid().ToString();
            var obj = ctr.PostDevice(vmdl);
            var res = obj as InternalServerErrorResult;
            Assert.NotNull(res);
        }

        [Test]
        public void ctr_should_delete_device()
        {
            var vmdl = ControllerHelper.GetValidDeviceViewModel();
            vmdl.InvNum = Guid.NewGuid().ToString();
            var obj = ctr.PostDevice(vmdl);
            var res = obj as OkNegotiatedContentResult<List<DeviceViewModel>>;
            Assert.NotNull(res);
            Assert.NotNull(res.Content);

            var d = _bl.GetSingleDevice(vmdl.InvNum);
            Assert.True(d.InvNum.Equals(vmdl.InvNum));

            var deviceToDelete = _bl.GetSingleDevice(vmdl.InvNum);
            deviceToDelete.DeviceId = 1010;
            ctr.DeleteDevice(deviceToDelete.DeviceId);
            var deletedDevice = _bl.GetSingleDevice(vmdl.InvNum);
            Assert.True(deletedDevice.IsActive.Equals(false));
        }

        [Test]
        public void ctr_should_delete_deviceType()
        {
            var s = "delete-me";
            var obj = _bl.CreateDeviceType();
            obj.Slug = s;
            var getObj = _bl.GetDeviceType(s);
            Assert.NotNull(getObj);

            ctr.DeleteDeviceType("delete-me");
            var getDel = _bl.GetDeviceType("delete-me");
            Assert.Null(getDel);
        }

        [Test]
        public void ctr_should_update_device()
        {
            var obj = ctr.GetDevice("a5123");
            var res = obj as OkNegotiatedContentResult<DeviceViewModel>;
            Assert.NotNull(res);
            Assert.NotNull(res.Content);
            var device = res.Content;
            device.DeviceId = 1011;
            device.Name = "Neuer Name";
            ctr.PutDevice(1011, device);

            Assert.NotNull(res);
            Assert.NotNull(res.Content);
            var device2 = res.Content;

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

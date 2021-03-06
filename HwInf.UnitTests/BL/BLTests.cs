﻿using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using HwInf.BusinessLogic;
using HwInf.BusinessLogic.Interfaces;
using HwInf.Common.Models;
using HwInf.DataAccess.Interfaces;
using HwInf.UnitTests.DAL;
using Moq;
using NUnit.Framework;

namespace HwInf.UnitTests.BL
{
    [TestFixture]
    public class BLTests
    {
        private readonly IDataAccessLayer _dal = new MockDAL();
        private readonly IBusinessLogicFacade _bl;
        private readonly IAccessoryBusinessLogic _abl;
        private readonly ICustomFieldsBusinessLogic _cbl;
        private readonly IDamageBusinessLogic _dbl;
        private readonly IDeviceBusinessLogic _debl;
        private readonly IBusinessLogic _blbl;
        private readonly IOrderBusinessLogic _obl;
        private readonly ISettingBusinessLogic _sbl;
        private readonly IUserBusinessLogic _ubl;


        public BLTests()
        {
            var mockPrincipal = new Mock<IBusinessLogicPrincipal>();
            mockPrincipal.Setup(x => x.IsAllowed).Returns(true);
            mockPrincipal.Setup(x => x.IsAdmin).Returns(true);
            mockPrincipal.Setup(x => x.IsVerwalter).Returns(true);
            mockPrincipal.Setup(x => x.CurrentUid).Returns("if15b032");
            _abl = new AccessoryBusinessLogic(_dal, mockPrincipal.Object);
            _cbl = new CustomFieldsBusinessLogic(_dal, mockPrincipal.Object);
            _dbl = new DamageBusinessLogic(_dal, mockPrincipal.Object);
            _debl = new DeviceBusinessLogic(_dal, mockPrincipal.Object);
            _blbl = new BusinessLogic.BusinessLogic(_dal, mockPrincipal.Object);
            _obl = new OrderBusinessLogic(_dal, mockPrincipal.Object);
            _sbl = new SettingBusinessLogic(_dal, mockPrincipal.Object);
            _ubl = new UserBusinessLogic(_dal, mockPrincipal.Object);
            _bl = new BusinessLogicFacade(_dal, _blbl, _abl, _cbl, _dbl, _debl, _obl, _sbl, _ubl);
        }

        [Test]
        public void HelloWorld()
        {
        }

        [Test]
        public void bl_should_create_device()
        {
            var device = _bl.CreateDevice();
            device.Name = "test";
            device.InvNum = Guid.NewGuid().ToString();
            Assert.NotNull(device);
        }

        [Test]
        public void bl_should_return_device_by_invNum()
        {
            var invNum = "IN123";
            var obj = _bl.CreateDevice();
            obj.InvNum = invNum;
            var getObj = _bl.GetSingleDevice(invNum);
            Assert.True(obj.Equals(getObj));
        }

        [Test]
        public void bl_should_return_device_by_id()
        {
            var id = 11;
            var obj = _bl.CreateDevice();
            obj.InvNum = Guid.NewGuid().ToString();
            obj.DeviceId = id;
            var getObj = _bl.GetSingleDevice(id);
            Assert.True(obj.Equals(getObj));
        }

        [Test]
        public void bl_should_return_null_when_no_device_found_by_id()
        {
            var id = 1;
            var obj = _bl.CreateDevice();
            obj.DeviceId = id;
            obj.InvNum = Guid.NewGuid().ToString();
            var getObj = _bl.GetSingleDevice(id+1);
            Assert.Null(getObj);
        }

        [Test]
        public void bl_should_return_null_when_no_device_found_by_invNum()
        {
            var invNum = Guid.NewGuid().ToString();
            var obj = _bl.CreateDevice();
            obj.InvNum = invNum;
            var getObj = _bl.GetSingleDevice(invNum + "bla");
            Assert.Null(getObj);
        }

        [Test]
        public void bl_should_return_devices()
        {
            var obj = _bl.GetDevices();
            Assert.NotNull(obj);
        }

        [Test]
        public void bl_should_delete_device()
        {
            var invNum = Guid.NewGuid().ToString();
            var obj = _bl.CreateDevice();
            obj.InvNum = invNum;
            obj.IsActive = true;
            var getObj = _bl.GetSingleDevice(invNum);
            Assert.NotNull(getObj);
            _bl.DeleteDevice(getObj);
            getObj.IsActive = false;
            var getDel = _bl.GetSingleDevice(invNum);
            Assert.False(getDel.IsActive);
        }

        [Test]
        public void bl_should_update_device()
        {
            var invNum = Guid.NewGuid().ToString();
            var name = "Muh";
            var obj = _bl.CreateDevice();
            obj.InvNum = invNum;
            obj.Name = name;
            var getObj = _bl.GetSingleDevice(invNum);
            Assert.NotNull(getObj);
            _bl.UpdateDevice(getObj);
            getObj.Name = "Kuh";
            var getEdit = _bl.GetSingleDevice(invNum);

            Assert.False(getEdit.Name.Equals(name));
        }

        [Test]
        public void bl_should_return_if_device_exists()
        {

            var obj = _bl.CreateDevice();
            obj.DeviceId = 87;
            obj.InvNum = Guid.NewGuid().ToString();
            var doesExist = _bl.DeviceExists(obj.DeviceId);
            Assert.True(doesExist);
        }

        [Test]
        public void bl_should_create_deviceType()
        {
            var obj = _bl.CreateDeviceType();
            obj.Slug = "test";
            obj.TypeId = 23;
            Assert.NotNull(obj);
        }

        public void bl_should_return_deviceTypes()
        {
            var obj = _bl.GetDeviceTypes();
            Assert.NotNull(obj);
        }

        [Test]
        public void bl_should_return_deviceType_by_slug()
        {
            var t = "type";
            var obj = _bl.CreateDeviceType();
            obj.Slug = t;
            var getObj = _bl.GetDeviceType(t);
            Assert.True(obj.Equals(getObj));
        }

        [Test]
        public void bl_should_return_deviceType_by_id()
        {
            var t = 1;
            var obj = _bl.CreateDeviceType();
            obj.TypeId = t;
            obj.Slug = "test";
            var getObj = _bl.GetDeviceType(t);
            Assert.True(obj.Equals(getObj));
        }

        [Test]
        public void bl_should_return_null_when_no_deviceType_found()
        {
            var t = 7;
            var x = "typee";
            var obj = _bl.CreateDeviceType();
            obj.TypeId = t;
            obj.Slug = x;
            var getId = _bl.GetDeviceType(t+10);
            Assert.Null(getId);
            var getSlug = _bl.GetDeviceType(x + "bla");
            Assert.Null(getSlug);
        }

        [Test]
        public void bl_should_delete_deviceType()
        {
            var s = "delete-me";
            var obj = _bl.CreateDeviceType();
            obj.Slug = s;
            var getObj = _bl.GetDeviceType(s);
            Assert.NotNull(getObj);

            _bl.DeleteDeviceType(getObj);
            getObj.IsActive = false;
            var getDel = _bl.GetDeviceType(s);
            Assert.Null(getDel);
        }

        [Test]
        public void bl_should_update_deviceType()
        {
            var id = 213457;
            var name = "Muh";
            var obj = _bl.CreateDeviceType();
            obj.TypeId = id;
            obj.Name = name;
            var getObj = _bl.GetDeviceType(id);
            Assert.NotNull(getObj);
            _bl.UpdateDeviceType(getObj);
            getObj.Name = "Kuh";
            var getEdit = _bl.GetDeviceType(id);

            Assert.False(getEdit.Name.Equals(name));
        }

        [Test]
        public void bl_should_create_field()
        {
            var obj = _bl.CreateField();
            obj.Slug = "test";
            Assert.NotNull(obj);
        }

        [Test]
        public void bl_should_create_fieldGroup()
        {
            var obj = _bl.CreateFieldGroup();
            obj.Slug = "test";
            Assert.NotNull(obj);
        }

        [Test]
        public void bl_should_update_fieldGroup()
        {
            var obj = _bl.GetFieldGroup("prozessoren");
            Assert.NotNull(obj);
            _bl.UpdateFieldGroup(obj);
            obj.Fields.Add(new Field {Slug = "neu"});
            var objEdit = _bl.GetFieldGroup("prozessoren");
            Assert.NotNull(objEdit);
            var newField = objEdit.Fields.SingleOrDefault(i => i.Slug.Equals("neu"));
            Assert.NotNull(newField);
        }

        [Test]
        public void bl_should_return_field_by_slug()
        {
            var s = "field";
            var obj = _bl.CreateField();
            obj.Slug = s;
            var getObj = _bl.GetField(s);
            Assert.True(obj.Equals(getObj));
        }

        [Test]
        public void bl_should_return_fields()
        {
            var obj = _bl.GetFields();
            Assert.NotNull(obj);
        }

        [Test]
        public void bl_should_delete_field()
        {
            var s = "delete-me";
            var obj = _bl.CreateField();
            obj.Slug = s;
            var getObj = _bl.GetField(s);
            Assert.True(obj.Equals(getObj));
            _bl.DeleteField(getObj);
            var getDel = _bl.GetField(s);
            Assert.Null(getDel);
        }

        [Test]
        public void bl_should_return_fieldGroup_by_slug()
        {
            var s = "fieldgroup";
            var obj = _bl.CreateFieldGroup();
            obj.Slug = s;
            var getObj = _bl.GetFieldGroup(s);
            Assert.True(obj.Equals(getObj));
        }

        [Test]
        public void bl_should_return_fieldGroups()
        {
            var obj = _bl.GetFieldGroups();
            Assert.NotNull(obj);
        }

        [Test]
        public void bl_should_return_if_fieldGroup_exists()
        {
            var obj = _bl.FieldGroupExists("prozessoren");
            Assert.True(obj);
        }

        [Test]
        public void bl_should_return_deviceMeta()
        {
            var obj = _bl.GetDeviceMeta();
            Assert.NotNull(obj);
        }

        [Test]
        public void bl_should_create_deviceMeta()
        {
            var obj = _bl.CreateDeviceMeta();
            obj.MetaId = 21345;
            Assert.NotNull(obj);
        }

        [Test]
        public void bl_should_delete_deviceMeta()
        {
            var id = 2134;
            var obj = _bl.CreateDeviceMeta();
            obj.MetaId = id;
            Assert.NotNull(obj);
            var getObj = _bl.GetDeviceMeta();
            var objToDel = getObj.SingleOrDefault(i => i.MetaId == id);
            _bl.DeleteMeta(objToDel);
            var getDel = getObj.SingleOrDefault(i => i.MetaId == id);
            Assert.Null(getDel);
        }

        [Test]
        public void bl_should_create_deviceStatus()
        {
            var obj = _bl.CreateDeviceStatus();
            obj.StatusId = 10;
            Assert.NotNull(obj);
        }

        [Test]
        public void bl_should_return_deviceStatus()
        {
            var obj = _bl.GetDeviceStatuses();
            Assert.NotNull(obj);
        }

        [Test]
        public void bl_should_return_deviceStatus_by_id()
        {
            var id = 11;
            var obj = _bl.CreateDeviceStatus();
            obj.StatusId = id;
            Assert.NotNull(obj);
            var getObj = _bl.GetDeviceStatus(id);
            Assert.True(obj.Equals(getObj));
        }

        [Test]
        public void bl_should_return_null_when_deviceStatus_not_found()
        {
            var obj = _bl.GetDeviceStatus(100);
            Assert.Null(obj);
        }

        [Test]
        public void bl_should_return_persons()
        {
            var obj = _bl.GetUsers();
            Assert.NotNull(obj);
        }

        [Test]
        public void bl_should_return_person_by_uid()
        {
            var obj = _bl.GetUsers("if15b032");
            Assert.NotNull(obj);
        }

        [Test]
        public void bl_should_return_null_when_person_by_uid_not_found()
        {
            var obj = _bl.GetUsers("if15b038");
            Assert.Null(obj);
        }

        [Test]
        public void bl_should_create_person()
        {
            var obj = _bl.CreateUser();
            obj.Uid = "12345";
            Assert.NotNull(obj);
        }

        [Test]
        public void bl_should_return_role()
        {
            var obj = _bl.GetRole("Admin");
            Assert.NotNull(obj);
        }

        [Test]
        public void bl_should_return_null_when_role_not_found()
        {
            var obj = _bl.GetRole("NotFound");
            Assert.Null(obj);
        }

        [Test]
        public void bl_should_update_person()
        {
            var obj = _bl.GetUsers("if15b032");
            var name = obj.Name;

            _bl.UpdateUser(obj);
            obj.Name = "Anders";
            Assert.NotNull(obj);
            var objEdit = _bl.GetUsers("if15b032");
            Assert.False(objEdit.Name.Equals(name));
        }

        [Test]
        public void bl_should_return_isAdmin_true_when_admin()
        {
            var isAdmin = _bl.IsAdmin;
            Assert.True(isAdmin);
        }

        [Test]
        public void bl_should_return_isAdmin_false_when_not_admin()
        {
            var isAdmin = _bl.IsAdminUid("if15b032");
            Assert.False(isAdmin);
        }

        [Test]
        public void bl_should_return_setting_by_key()
        {
            Assert.NotNull(_bl.GetSetting("email_benachrichtigung"));
        }


        [Test]
        public void bl_should_create_setting()
        {
            var obj = _bl.CreateSetting();
            obj.Key = "random";
            Assert.NotNull(obj);
            Assert.True( obj.GetType().Name.Equals("Setting") );
        }

        [Test]
        public void bl_should_update_setting()
        {
            var obj = _bl.GetSetting("email_benachrichtigung");
            var randomString = "Anders";
            obj.Value = randomString;

            _bl.UpdateSetting(obj);
            Assert.NotNull(obj);
            var objEdit = _bl.GetSetting("email_benachrichtigung");
            Assert.True(objEdit.Value.Equals(randomString));
        }

        [Test]
        public void bl_should_return_null_when_no_setting_was_found()
        {
            var key = Guid.NewGuid().ToString();

            var obj = _bl.CreateSetting();
            obj.Key = key;
            _bl.DeleteSetting(obj);
            var result = _bl.GetSetting(key);
            Assert.Null(result);
        }

        [Test]
        public void bl_should_create_order()
        {
            var obj = _bl.CreateOrder();
            Assert.NotNull(obj);
            obj.OrderId = 1;
            obj.OrderGuid = Guid.NewGuid();
        }

        [Test]
        public void bl_should_return_order_by_id()
        {
            var id = 2;
            var obj = _bl.CreateOrder();
            Assert.NotNull(obj);
            obj.OrderId = id;
            obj.OrderGuid = Guid.NewGuid();

            var getObj = _bl.GetOrders(id);
            Assert.NotNull(getObj);
            Assert.True(getObj.OrderId.Equals(id));
        }

        [Test]
        public void bl_should_return_order_by_guid()
        {
            var id = 3;
            var guid = Guid.NewGuid();
            var obj = _bl.CreateOrder();
            Assert.NotNull(obj);
            obj.OrderId = id;
            obj.OrderGuid = guid;

            var getObj = _bl.GetOrders(guid);
            Assert.NotNull(getObj);
            Assert.True(getObj.OrderGuid.Equals(guid));
        }

        [Test]
        public void bl_should_return_orders()
        {
            var id = 4;
            var guid = Guid.NewGuid();
            var obj = _bl.CreateOrder();
            Assert.NotNull(obj);
            obj.OrderId = id;
            obj.OrderGuid = guid;

            var getObj = _bl.GetOrders();
            Assert.NotNull(getObj);
        }

        [Test]
        public void bl_should_return_null_when_no_order_found_by_id()
        {
            var id = 5;
            var guid = Guid.NewGuid();
            var obj = _bl.CreateOrder();
            Assert.NotNull(obj);
            obj.OrderId = id;
            obj.OrderGuid = guid;

            var getObj = _bl.GetOrders(55);
            Assert.Null(getObj);
        }

        [Test]
        public void bl_should_return_null_when_no_order_found_by_guid()
        {
            var id = 6;
            var guid = Guid.NewGuid();
            var obj = _bl.CreateOrder();
            Assert.NotNull(obj);
            obj.OrderId = id;
            obj.OrderGuid = guid;

            var getObj = _bl.GetOrders(Guid.NewGuid());
            Assert.Null(getObj);
        }

        [Test]
        public void bl_should_return_order_status()
        {
            var getObj = _bl.GetOrderStatus();
            Assert.NotNull(getObj);
        }

        [Test]
        public void bl_should_return_order_status_by_slug()
        {
            var getObj = _bl.GetOrderStatus("offen");
            Assert.NotNull(getObj);
            Assert.True(getObj.Name.Equals("Offen"));
        }

        [Test]
        public void bl_should_return_null_when_no_order_status_found_by_slug()
        {
            var getObj = _bl.GetOrderStatus("test");
            Assert.Null(getObj);
        }

        [Test]
        public void bl_should_create_order_item()
        {
            var obj = _bl.CreateOrderItem();
            Assert.NotNull(obj);
            obj.ItemId = 1;

        }

        [Test]
        public void bl_should_return_order_items()
        {
            var obj = _bl.CreateOrderItem();
            Assert.NotNull(obj);
            obj.ItemId = 2;

            var getObj = _bl.GetOrderItems();
            Assert.NotNull(getObj);

        }

        [Test]
        public void bl_should_return_order_item_by_id()
        {
            var id = 3;
            var obj = _bl.CreateOrderItem();
            Assert.NotNull(obj);
            obj.ItemId = id;

            var getObj = _bl.GetOrderItem(id);
            Assert.NotNull(getObj);
        }

        [Test]
        public void bl_should_return_null_when_order_item_not_found_by_id()
        {
            var id = 4;
            var obj = _bl.CreateOrderItem();
            Assert.NotNull(obj);
            obj.ItemId = id;

            var getObj = _bl.GetOrderItem(44);
            Assert.Null(getObj);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Security.Claims;
using System.Threading;
using HwInf.BusinessLogic;
using HwInf.BusinessLogic.Interfaces;
using HwInf.Common.Models;
using HwInf.DataAccess.Interfaces;
using HwInf.UnitTests.DAL;
using HwInf.Web.ViewModels;
using Moq;

namespace HwInf.UnitTests.Controllers
{
    public static class ControllerHelper
    {
        private static readonly IDataAccessLayer _dal = new MockDAL();
        private static readonly IBusinessLogicFacade _bl;
        private static IAccessoryBusinessLogic _abl;
        private static ICustomFieldsBusinessLogic _cbl;
        private static IDamageBusinessLogic _dbl;
        private static IDeviceBusinessLogic _debl;
        private static IBusinessLogic _blbl;
        private static IOrderBusinessLogic _obl;
        private static ISettingBusinessLogic _sbl;
        private static IUserBusinessLogic _ubl;

        static ControllerHelper()
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

            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            var subject = new ClaimsIdentity("Federation", ClaimTypes.Name, ClaimTypes.Role);
            subject.AddClaim(new Claim(ClaimTypes.Role, "Admin", ClaimValueTypes.String));
            subject.AddClaim(new Claim(ClaimTypes.Name, "if15b032", ClaimValueTypes.String));
            Thread.CurrentPrincipal = new ClaimsPrincipal(subject);
        }

        public static FilterViewModel GetValidFilterViewModel(string deviceType = "", string fieldgroup = "anschluesse", string field = "vga", string metaValue = "1", string order = "ASC", int limit = 10, string orderBy = "Name")
        {
            var vmdl = new FilterViewModel
            {
                DeviceType = deviceType,
                Limit = limit,
                Offset = 0,
                MetaQuery = new List<DeviceMetaViewModel>
                {
                    new DeviceMetaViewModel {FieldGroupSlug = fieldgroup, FieldSlug = field, Value = metaValue}
                },
                OrderBy = orderBy,
                Order = order
            };


            return vmdl;
        }

        public static DeviceViewModel GetValidDeviceViewModel()
        {
            var p = _bl.GetUsers("if15b032");
            var uvmdl = new UserViewModel(p);
            var dt = _bl.GetDeviceType("festplatte");
            var dtvmdl = new DeviceTypeViewModel(dt);
            var ds = _bl.GetDeviceStatus(1);
            var dsvmdl = new DeviceStatusViewModel(ds);
            var vmdl = new DeviceViewModel
            {
                CreateDate = DateTime.Now.ToShortDateString(),
                Marke = "Test",
                IsActive = true,
                Verwalter = uvmdl,
                DeviceType = dtvmdl,
                Status = dsvmdl,
                Name =  "Test Device,",
            };

            return vmdl;
        }

        public static DeviceViewModel GetInValidDeviceViewModel()
        {
            var vmdl = new DeviceViewModel
            {
            };

            return vmdl;
        }

        public static OrderViewModel GetValidOrderViewModel()
        {

            var oi = new OrderItem
            {
                Device = _bl.GetSingleDevice("a5123"),
                Accessories = "Maus,Tastatur"
            };

            var vmdl = new OrderViewModel
            {
                Date = DateTime.Now,
                From = DateTime.Now,
                To = DateTime.Now,
                OrderItems = new List<OrderItemViewModel>
                {
                    new OrderItemViewModel(oi)
                },
                OrderGuid = Guid.NewGuid(),
                OrderReason = "Unit Test",
                Entleiher = new UserViewModel
                {
                    Uid = "if15b032"
                }
            };

            return vmdl;
        }

        public static OrderSearchViewModel GetValidOrderSearchViewModel(string searchQuery, bool isAdminView)
        {

            var vmdl = new OrderSearchViewModel
            {
                SearchQuery = searchQuery,
                IsAdminView = isAdminView
            };

            return vmdl;
        }

        public static Order GetValidOrderModel()
        {
            var obj = new Order
            {
                Date = DateTime.Now,
                From = DateTime.Now,
                To =  DateTime.Now,
                Entleiher = _bl.GetUsers(_bl.GetCurrentUid()),
                Verwalter = _bl.GetUsers(_bl.GetCurrentUid()),
                OrderReason = "Unit Test",
                OrderGuid = Guid.NewGuid(),
                OrderStatus = _bl.GetOrderStatus("offen"),
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { CreateDate = DateTime.Now, Device = _bl.GetSingleDevice("a5123")}
                }
            };
            return obj;
        }

        public static FieldGroupViewModel GetvalidFieldGroupViewModel()
        {
            var fvmdl = new FieldViewModel
            {
                Name = "TestField",
                Slug = SlugGenerator.GenerateSlug(_bl, "TestField", "field")
            };

            var vmdl = new FieldGroupViewModel
            {
                Name = "Test",
                Slug = SlugGenerator.GenerateSlug(_bl, "Test", "fieldGroup"),
                Fields = new List<FieldViewModel>
                {
                    fvmdl
                },               
            };

            return vmdl;
        }

        public static FieldViewModel GetValidFieldViewModel()
        {
            var vmdl = new FieldViewModel
            {
                Name = "TestField"
            };

            return vmdl;
        }
    }
}
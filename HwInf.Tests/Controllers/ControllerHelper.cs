using System;
using System.Collections.Generic;
using HwInf.Common.DAL;
using HwInf.Common.Models;
using HwInf.Tests.DAL;
using HwInf.ViewModels;

namespace HwInf.Tests.Controllers
{
    public static class ControllerHelper
    {
        private static readonly IDAL _dal = new MockDAL();
        private static readonly Common.BL.BL _bl;

        static ControllerHelper()
        {
            _bl = new Common.BL.BL(_dal);
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
                OrderReason = "Unit Test"
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
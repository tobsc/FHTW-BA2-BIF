using System;
using System.Collections.Generic;
using HwInf.Common.DAL;
using HwInf.Common.Interfaces;
using HwInf.Common.Models;
using HwInf.Tests.DAL;
using HwInf.ViewModels;

namespace HwInf.Tests.Controllers
{
    public static class ControllerHelper
    {
        private static readonly IDataAccessLayer _dal = new MockDAL();
        private static readonly Common.BL.BusinessLayer BusinessLayer;

        static ControllerHelper()
        {
            BusinessLayer = new Common.BL.BusinessLayer(_dal);
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
            var p = BusinessLayer.GetUsers("if15b032");
            var uvmdl = new UserViewModel(p);
            var dt = BusinessLayer.GetDeviceType("festplatte");
            var dtvmdl = new DeviceTypeViewModel(dt);
            var ds = BusinessLayer.GetDeviceStatus(1);
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
                Device = BusinessLayer.GetSingleDevice("a5123"),
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
                Entleiher = BusinessLayer.GetUsers(BusinessLayer.GetCurrentUid()),
                Verwalter = BusinessLayer.GetUsers(BusinessLayer.GetCurrentUid()),
                OrderReason = "Unit Test",
                OrderGuid = Guid.NewGuid(),
                OrderStatus = BusinessLayer.GetOrderStatus("offen"),
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { CreateDate = DateTime.Now, Device = BusinessLayer.GetSingleDevice("a5123")}
                }
            };
            return obj;
        }

        public static FieldGroupViewModel GetvalidFieldGroupViewModel()
        {
            var fvmdl = new FieldViewModel
            {
                Name = "TestField",
                Slug = SlugGenerator.GenerateSlug(BusinessLayer, "TestField", "field")
            };

            var vmdl = new FieldGroupViewModel
            {
                Name = "Test",
                Slug = SlugGenerator.GenerateSlug(BusinessLayer, "Test", "fieldGroup"),
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
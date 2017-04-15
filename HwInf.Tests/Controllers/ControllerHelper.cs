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
                MetaQuery = new List<DeviceMeta>
                {
                    new DeviceMeta {FieldGroupSlug = fieldgroup, FieldSlug = field, MetaValue = metaValue}
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
            var p = _bl.GetUsers("if15b032");
            var uvmdl = new UserViewModel(p);
            var dt = _bl.GetDeviceType("festplatte");
            var dtvmdl = new DeviceTypeViewModel(dt);
            var ds = _bl.GetDeviceStatus(1);
            var dsvmdl = new DeviceStatusViewModel(ds);
            var vmdl = new DeviceViewModel
            {
            };

            return vmdl;
        }
    }
}
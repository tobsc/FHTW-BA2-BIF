using System.Collections.Generic;
using System.Linq;
using HwInf.BusinessLogic.Interfaces;
using HwInf.Common.Models;

namespace HwInf.Web.ViewModels
{
    public class FilterViewModel
    {

        public string DeviceType { get; set; }
        public string Order { get; set; } = "ASC";
        public string OrderBy { get; set; } = "InvNum";
        public int Offset { get; set; } = 0;
        public int Limit { get; set; } = 100;
        public ICollection<DeviceMetaViewModel> MetaQuery { get; set; } = new List<DeviceMetaViewModel>();
        public bool IsVerwalterView { get; set; } = false;
        public bool OnlyActive { get; set; } = true;


        public FilterViewModel()
        {
            
        }


        public ICollection<Device> FilteredList(IBusinessLogicFacade bl)
        {
            var metaViewModel = MetaQuery.Select(i =>
            {
                var deviceMeta = new DeviceMeta();
                i.ApplyValues(deviceMeta);
                return deviceMeta;

            }).ToList();

            return bl.GetFilteredDevices(metaViewModel, DeviceType, Order, OrderBy, OnlyActive, IsVerwalterView)
                .ToList();
        }

        public ICollection<Device> FilteredListUser(IBusinessLogicFacade bl)
        {
            var metaViewModel = MetaQuery.Select(i =>
            {
                var deviceMeta = new DeviceMeta();
                i.ApplyValues(deviceMeta);
                return deviceMeta;

            }).ToList();

            return bl.GetFilteredDevicesUser(metaViewModel, DeviceType, Order, OrderBy, OnlyActive, IsVerwalterView)
                .ToList();
        }
    }
}
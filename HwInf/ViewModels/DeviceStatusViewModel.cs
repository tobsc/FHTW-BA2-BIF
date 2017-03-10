using HwInf.Common.Models;

namespace HwInf.ViewModels
{
    public class DeviceStatusViewModel
    {
        public int StatusId { get; set; }
        public string Description { get; set; }

        public static implicit operator DeviceStatus(DeviceStatusViewModel vmdl)
        {
            return new DeviceStatus {
                StatusId = vmdl.StatusId,
                Description = vmdl.Description
            };
        }

        public static implicit operator DeviceStatusViewModel(DeviceStatus ds)
        {
            return new DeviceStatusViewModel
            {
                StatusId = ds.StatusId,
                Description = ds.Description
            };
        }
    }
}
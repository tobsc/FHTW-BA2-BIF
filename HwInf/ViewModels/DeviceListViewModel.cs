using System.Collections.Generic;

namespace HwInf.ViewModels
{
    public class DeviceListViewModel
    {

        public int CurrentPage { get; set; }

        public int MaxPages { get; set; }

        public IEnumerable<DeviceViewModel> Devices { get; set; }

    }
}
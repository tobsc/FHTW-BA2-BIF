using System;
using System.Collections.Generic;
using System.Linq;
using HwInf.Common.BL;

namespace HwInf.ViewModels
{
    public class DeviceListViewModel
    {
        public int MaxPages { get; set; }

        public IEnumerable<DeviceViewModel> Devices { get; set; }


        public DeviceListViewModel()
        {

        }

        public DeviceListViewModel(IEnumerable<DeviceViewModel> obj, int offset, int limit, BL bl, int count)
        {
            Devices = obj.ToList();
            MaxPages = limit == 0 ? 0 : (int)Math.Ceiling((double)count / limit);
        }


    }
}
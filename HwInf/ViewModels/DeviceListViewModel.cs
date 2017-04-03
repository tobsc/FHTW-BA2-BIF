using System;
using System.Collections.Generic;
using System.Linq;
using HwInf.Common.BL;

namespace HwInf.ViewModels
{
    public class DeviceListViewModel
    {

        public int CurrentPage { get; set; }

        public int MaxPages { get; set; }

        public IEnumerable<DeviceViewModel> Devices { get; set; }


        public DeviceListViewModel()
        {
            
        }

        public DeviceListViewModel(IEnumerable<DeviceViewModel> obj, int offset, int limit, BL bl)
        {
            Devices = obj.ToList();
            CurrentPage = offset + 1;
            MaxPages = (int)Math.Ceiling((double)bl.DeviceCount() / (limit == 0 ? 1 : limit));
        }


    }
}
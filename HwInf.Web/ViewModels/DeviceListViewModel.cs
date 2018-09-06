using System;
using System.Collections.Generic;
using System.Linq;

namespace HwInf.Web.ViewModels
{
    public class DeviceListViewModel
    {
        public int MaxPages { get; set; }
        public int TotalItems { get; set; }

        public IEnumerable<DeviceViewModel> Devices { get; set; }


        public DeviceListViewModel()
        {

        }

        public DeviceListViewModel(IEnumerable<DeviceViewModel> obj, int limit, int count)
        {
            TotalItems = count;
            Devices = obj.ToList();
            MaxPages = limit == 0 ? 0 : (int)Math.Ceiling((double)count / limit);
        }


    }
}
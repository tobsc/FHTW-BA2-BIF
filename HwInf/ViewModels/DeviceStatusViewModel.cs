using System.ComponentModel;
using HwInf.Common.BL;
using HwInf.Common.Models;

namespace HwInf.ViewModels
{
    public class DeviceStatusViewModel
    {

        public int StatusId { get; set; }
        public string Description { get; set; }



        public DeviceStatusViewModel()
        {
            
        }

        public DeviceStatusViewModel(DeviceStatus ds)
        {
            Refresh(ds);
        }

        public void Refresh(DeviceStatus ds)
        {
            var target = this;
            var source = ds;

            target.Description = source.Description;
            target.StatusId = source.StatusId;
        }

        public void ApplyChanges(DeviceStatus ds, BusinessLayer bl)
        {
            var target = ds;
            var source = this;

            target.Description = source.Description;

        }
    }
}
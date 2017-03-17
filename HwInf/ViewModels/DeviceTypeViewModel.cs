using System.Collections.Generic;
using System.Linq;
using System.Web;
using HwInf.Common.BL;
using HwInf.Common.Models;
using WebGrease.Css.Extensions;

namespace HwInf.ViewModels
{
    public class DeviceTypeViewModel
    {
        public int DeviceTypeId { get; set; }
        public string Slug { get; set; }
        public string Name { get; set; }

        public string PermaLink
        {
            get { return "geraete/typ/" + Slug; }
        }

        public DeviceTypeViewModel()
        {

        }

        public DeviceTypeViewModel(DeviceType obj)
        {
            Refresh(obj);
        }

        public void Refresh(DeviceType obj)
        {
            var target = this;
            var source = obj;

            Slug = obj.Slug;
            Name = obj.Name;

        }

        public void ApplyChanges(DeviceType obj, BL bl)
        {
            var target = obj;
            var source = this;

        }

        public DeviceTypeViewModel LoadComponents(DeviceType dt)
        {



            return this; // fluent interface
        }

        public static implicit operator DeviceType(DeviceTypeViewModel vmdl)
        {
            return new DeviceType
            {
                TypeId = vmdl.DeviceTypeId,
            };
        }

    }
}
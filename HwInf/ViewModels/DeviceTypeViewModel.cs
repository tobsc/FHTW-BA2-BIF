using System.Collections.Generic;
using System.Linq;
using HwInf.Common.BL;
using HwInf.Common.Models;
using WebGrease.Css.Extensions;

namespace HwInf.ViewModels
{
    public class DeviceTypeViewModel
    {
        public int DeviceTypeId { get; set; }
        public string TypeName { get; set; }
        public IEnumerable<ComponentViewModel> Components { get; set; }


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

            target.DeviceTypeId = source.TypeId;
            target.TypeName = source.Description;
            target.Components = new List<ComponentViewModel>();
            source.Components.ForEach(i => target.Components.ToList().Add(i));

        }

        public void ApplyChanges(DeviceType obj, BL bl)
        {
            var target = obj;
            var source = this;

            target.Description = source.TypeName;
            target.Components = new List<Component>();
            source.Components.ToList().ForEach(i => target.Components.Add(i));
            target.Components.ForEach(i => i.ComponentType = bl.GetComponentType(i.ComponentType.Name));
        }

        public DeviceTypeViewModel LoadComponents(DeviceType dt)
        {

            Components = dt.Components
                .Select(i => new ComponentViewModel(i));


            return this; // fluent interface
        }

        public static implicit operator DeviceType(DeviceTypeViewModel vmdl)
        {
            return new DeviceType
            {
                TypeId = vmdl.DeviceTypeId,
                Description = vmdl.TypeName
            };
        }

        public static implicit operator DeviceTypeViewModel(DeviceType dt)
        {
            return new DeviceTypeViewModel
            {
                DeviceTypeId = dt.TypeId,
                TypeName = dt.Description
            };
        }
    }
}
using HwInf.Common.BL;
using HwInf.Common.Models;

namespace HwInf.ViewModels
{
    public class DeviceMetaViewModel
    {

        public string Field { get; set; }
        public string Group { get; set; }
        public string Value { get; set; }


        public DeviceMetaViewModel()
        {

        }

        public DeviceMetaViewModel(DeviceMeta dm)
        {
            Refresh(dm);
        }


        public void Refresh(DeviceMeta dm)
        {
            var target = this;
            var source = dm;

            target.Value = source.MetaValue;
            target.Group = source.FieldGroup.Name;
            target.Field = source.Field.Name;
        }

        public void ApplyChanges(DeviceMeta dm, BL bl)
        {
            var target = dm;
            var source = this;

            target.MetaValue = source.Value;
            
            
        }


        public static implicit operator DeviceMeta(DeviceMetaViewModel vmdl)
        {
            return new DeviceMeta
            {
                MetaValue = vmdl.Value               
            };
        }
    }
}

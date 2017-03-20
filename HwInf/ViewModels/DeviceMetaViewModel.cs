using System.Linq;
using HwInf.Common.BL;
using HwInf.Common.Models;

namespace HwInf.ViewModels
{
    public class DeviceMetaViewModel
    {

        public string Field { get; set; }
        public string FieldSlug { get; set; }
        public string FieldGroup { get; set; }
        public string FieldGroupSlug { get; set; }
        public string Value { get; set; }
        private Field F { get; set; }
        private FieldGroup Fg { get; set; }


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
            target.FieldGroup = source.FieldGroup.Name;
            target.FieldGroupSlug = source.FieldGroup.Slug;
            target.Field = source.Field.Name;
            target.FieldSlug = source.Field.Slug;
            target.F = source.Field;
            target.Fg = source.FieldGroup;
        }

        public void ApplyChanges(DeviceMetaViewModel vmdl, BL bl)
        {
            var target = vmdl;

            target.Fg = bl.GetFieldGroups(target.FieldGroupSlug);
            target.F = bl.GetFields(FieldSlug);
        }

        public void ApplyChanges(DeviceMeta dm, BL bl)
        {
            var target = dm;
            var source = this;

            target.MetaValue = source.Value;
            target.FieldGroup = bl.GetFieldGroups(source.FieldGroupSlug);
            target.Field = bl.GetFields().SingleOrDefault(i => i.Slug == source.FieldSlug);
        }


        public static implicit operator DeviceMeta(DeviceMetaViewModel vmdl)
        {
            return new DeviceMeta
            {                
                MetaValue = vmdl.Value,
                Field = vmdl.F,
                FieldGroup = vmdl.Fg                    
            };
        }
    }
}

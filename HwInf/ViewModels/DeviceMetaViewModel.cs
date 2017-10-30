using System.Linq;
using HwInf.Common.BL;
using HwInf.Common.Interfaces;
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
            target.FieldGroup = source.FieldGroupName;
            target.FieldGroupSlug = source.FieldGroupSlug;
            target.Field = source.FieldName;
            target.FieldSlug = source.FieldSlug;
        }

        public void ApplyChanges(DeviceMetaViewModel vmdl, IBusinessLayer bl)
        {
            var target = vmdl;

            target.Fg = bl.GetFieldGroups(target.FieldGroupSlug);
            target.F = target.Fg.Fields.SingleOrDefault(i => i.Name.Equals(target.Field));

            target.FieldGroup = bl.GetFieldGroups(target.FieldGroupSlug).Name;
            target.FieldSlug = target.Fg.Fields.SingleOrDefault(i => i.Name.Equals(target.Field))?.Slug;
        }

        public void ApplyChanges(DeviceMeta dm, BusinessLayer bl)
        {
            var target = dm;
            var source = this;

            target.MetaValue = source.Value;
            target.FieldGroupName = source.Fg.Name;
            target.FieldName = source.Field;
            target.FieldGroupSlug = source.FieldGroupSlug;
            target.FieldSlug = source.F.Slug;
        }

        public void ApplyValues(DeviceMeta dm)
        {
            var target = dm;
            var source = this;

            target.MetaValue = source.Value;
            target.FieldGroupSlug = source.FieldGroupSlug;
            target.FieldSlug = source.FieldSlug;
        }


        public static implicit operator DeviceMeta(DeviceMetaViewModel vmdl)
        {
            return new DeviceMeta
            {                
                MetaValue = vmdl.Value,
                FieldSlug = vmdl.FieldSlug,
                FieldGroupSlug = vmdl.FieldGroupSlug,
                FieldGroupName = vmdl.FieldGroup,
                FieldName = vmdl.Field
                       
            };
        }
    }
}

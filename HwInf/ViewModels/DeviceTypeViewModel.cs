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
        public ICollection<FieldGroupViewModel> FieldGroups { get; set; }
        public bool IsActive { get; set; } = true;


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

            target.Slug = source.Slug;
            target.Name = source.Name;
            target.DeviceTypeId = source.TypeId;
            target.IsActive = source.IsActive;

        }

        public void ApplyChanges(DeviceType obj, BL bl)
        {
            var target = obj;
            var source = this;

            target.Name = source.Name;
            target.Slug = SlugGenerator.GenerateSlug(bl, source.Name, "deviceType");
            target.FieldGroups = new List<FieldGroup>();


            if (source.FieldGroups != null)
            {
                var fgs = source.FieldGroups.Select(i => bl.GetFieldGroups(i.Slug)).ToList();
                fgs.RemoveAll(i => i == null);
                fgs.ForEach(i => target.FieldGroups.Add(i));
                target.FieldGroups.Add(bl.GetFieldGroups("zubehor"));
            }

            target.IsActive = source.IsActive;

        }

        public DeviceTypeViewModel LoadFieldGroups(DeviceType obj)
        {
            if (obj.FieldGroups == null) return this;

            FieldGroups = obj.FieldGroups
                .Select(i => new FieldGroupViewModel(i))
                .ToList();

            return this;
        }

    }
}
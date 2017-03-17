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
        public ICollection<FieldGroup> FieldGroups { get; set; }

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

            target.Slug = source.Slug;
            target.Name = source.Name;
            target.DeviceTypeId = source.TypeId;

        }

        public void ApplyChanges(DeviceType obj, BL bl)
        {
            var target = obj;
            var source = this;

            target.Name = source.Name;
            target.Slug = SlugGenerator.GenerateSlug(source.Name);
            target.FieldGroups = new List<FieldGroup>();
            //source.FieldGroups.ForEach(i => target.FieldGroups.Add(bl.GetFieldGroups(i.Slug)));

        }

    }
}
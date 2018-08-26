using System.Collections.Generic;
using System.Linq;
using HwInf.BusinessLogic;
using HwInf.BusinessLogic.Interfaces;
using HwInf.Common.Models;

namespace HwInf.Web.ViewModels
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

        public void ApplyChanges(DeviceType obj, IBusinessLogicFacade bl)
        {
            var target = obj;
            var source = this;

            target.Name = source.Name;
            target.Slug = SlugGenerator.GenerateSlug(bl, source.Name, "deviceType");
            target.DeviceTypesFieldGroups = new List<DeviceTypeFieldGroup>();


            if (source.FieldGroups != null)
            {
                var fgs = source.FieldGroups.Select(i => bl.GetFieldGroup(i.Slug)).ToList();
                fgs.RemoveAll(i => i == null);
                foreach (var fieldGroup in fgs)
                {
                    var dtfg = new DeviceTypeFieldGroup()
                    {
                        DeviceType = obj,
                        DeviceTypeId = obj.TypeId,
                        FieldGroup = fieldGroup,
                        FieldGroupId = fieldGroup.GroupId
                    };
                    target.DeviceTypesFieldGroups.Add(dtfg);
                    target.FieldGroups.Add(fieldGroup);
                    
                }
                var afg = bl.GetFieldGroup("zubehor");
                if (afg != null)
                {
                    var adtfg = new DeviceTypeFieldGroup
                    {
                        DeviceType = obj,
                        DeviceTypeId = obj.TypeId,
                        FieldGroup = afg,
                        FieldGroupId = afg.GroupId
                    };
                    target.FieldGroups.Add(afg);
                    target.DeviceTypesFieldGroups.Add(adtfg);   
                }
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
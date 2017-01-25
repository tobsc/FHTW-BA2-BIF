using HwInf.Common.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HwInf.Models
{
    public class DeviceTypeViewModel
    {
        public int DeviceTypeId { get; set; }
        public string typeName { get; set; }
        public IDictionary<string,string> DeviceMetaData { get; set; }

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
            target.typeName = source.Description;
        }

        public void ApplyChanges(DeviceType obj, HwInfContext db)
        {
            var target = obj;
            var source = this;

            target.Description = source.typeName;
        }

        public void CreateDeviceType(DeviceType obj, HwInfContext db)
        {
            var target = obj;
            var source = this;

            target.Description = source.typeName;



            if (source.DeviceMetaData.Count != 0)
            {
                for(int i = 0; i<(source.DeviceMetaData.Count()/2); i++)
                {
                    db.Components.Add(new Component
                    {
                        Name = this.DeviceMetaData["key"+(i + 1)],
                        FieldType = this.DeviceMetaData["value" + (i + 1)],
                        DeviceType = target
                    });
                }
            }
        }

        public DeviceTypeViewModel loadComponents(HwInfContext db)
        {
            var typeComponents = db.Components;
            DeviceMetaData = new Dictionary<string, string>();

            foreach (Component m in typeComponents.Include("DeviceType").Where(i => i.DeviceType.TypeId == DeviceTypeId))
            {
                DeviceMetaData.Add(m.Name, m.FieldType);
            }

            return this; // fluent interface
        }
    }
}
using HwInf.Common.BL;
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
        public IDictionary<string, string> DeviceMetaData { get; set; } //  TODO: Change to DeviceTypeMetaData

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

        public void ApplyChanges(DeviceType obj)
        {
            var target = obj;
            var source = this;

            target.Description = source.typeName;
        }

        public DeviceType CreateDeviceType(BL bl)
        {
            DeviceType deviceType = new DeviceType();
            ApplyChanges(deviceType);

            // Create new deviceType
            bl.CreateDeviceType(deviceType);

            // Check if new Type has MetaData and create Components of it
            if (DeviceMetaData.Count != 0)
            {
                for(int i = 0; i<(DeviceMetaData.Count()/2); i++)
                {
                    Component component = new Component {
                        Name = this.DeviceMetaData["key" + (i + 1)],
                        FieldType = this.DeviceMetaData["value" + (i + 1)],
                        DeviceType = deviceType
                    };

                    bl.CreateComponent(component);
                }
            }

            return deviceType;
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
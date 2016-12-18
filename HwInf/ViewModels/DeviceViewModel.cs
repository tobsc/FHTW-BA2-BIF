using HwInf.Common.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HwInf.Models
{
    public class DeviceViewModel
    {
        public int DeviceId { get; set; }
        public string Name { get; set; }
        public string InvNum { get; set; }
        public string Marke { get; set; }
        public string Status { get; set; }
        public int StatusId { get; set; }
        public string Type { get; set; }
        public int TypeId { get; set; }
        public IDictionary<string,string> DeviceMetaData { get; set; }

        public DeviceViewModel()
        {

        }

        public DeviceViewModel(Device obj)
        {
            Refresh(obj);
        }

        public void Refresh(Device obj)
        {
            var target = this;
            var source = obj;

            target.DeviceId = source.DeviceId;
            target.Name = source.Name;
            target.InvNum = source.InvNum;
            target.Marke = source.Brand;
            target.Status = source.Status.Description;
            target.StatusId = source.Status.StatusId;
            target.TypeId = source.Type.TypeId;
            target.Type= source.Type.Description;
        }

        public void ApplyChanges(Device obj, HwInfContext db)
        {
            var target = obj;
            var source = this;

            target.Name = source.Name;
            target.InvNum = source.InvNum;
            target.Brand = source.Marke;
            target.Status.Description = source.Status;
            target.Status.StatusId = source.StatusId;
            target.Type = db.DeviceTypes.Single(i => i.TypeId == source.TypeId);
        }

        public DeviceViewModel loadMeta(HwInfContext db)
        {
            var deviceMeta = db.DeviceMeta;
            DeviceMetaData = new Dictionary<string, string>();

            foreach (DeviceMeta m in deviceMeta.Include("Device").Where(i => i.Device.DeviceId == DeviceId))
            {
                    DeviceMetaData.Add(m.MetaKey, m.MetaValue);
            }

            return this; // fluent interface
        }
    }
}
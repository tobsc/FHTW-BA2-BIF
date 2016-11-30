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
        public int Status { get; set; }
        public string TypeName { get; set; }
        public int TypeId { get; set; }
        public IDictionary<string,string> DeviceMetaData { get; set; }

        public DeviceViewModel()
        {

        }

        public DeviceViewModel(DBDevice obj)
        {
            Refresh(obj);
        }

        public void Refresh(DBDevice obj)
        {
            var target = this;
            var source = obj;

            target.DeviceId = source.DeviceId;
            target.Name = source.Name;
            target.InvNum = source.InvNum;
            target.Status = source.Status;
            target.TypeId = source.Type.TypeId;
            target.TypeName = source.Type.Name;
        }

        public void ApplyChanges(DBDevice obj, HwInfContext db)
        {
            var target = obj;
            var source = this;

            target.Name = source.Name;
            target.InvNum = source.InvNum;
            target.Status = source.Status;
            target.Type = db.DeviceTypes.Single(i => i.TypeId == source.TypeId);
        }

        public DeviceViewModel loadMeta(HwInfContext db)
        {
            var deviceMeta = db.DeviceMeta;
            DeviceMetaData = new Dictionary<string, string>();

            foreach (DBDeviceMeta m in deviceMeta.Include("Device").Where(i => i.Device.DeviceId == DeviceId))
            {
                    DeviceMetaData.Add(m.MetaKey, m.MetaValue);
            }

            return this; // fluent interface
        }

        public void createDevice(HwInfContext db)
        {
            DBDevice dev = new DBDevice();
            dev.Name = Name;
            dev.InvNum = InvNum;
            dev.Status = Status;
            dev.Type = db.DeviceTypes.Single(i => i.TypeId == TypeId);

            db.Devices.Add(dev);

            foreach(var m in DeviceMetaData)
            {
                db.DeviceMeta.Add(new DBDeviceMeta{
                    MetaKey = m.Key,
                    MetaValue = m.Value,
                    Device = dev,
                    DeviceType = dev.Type
                });
            }

            db.SaveChanges();
        }
    }
}
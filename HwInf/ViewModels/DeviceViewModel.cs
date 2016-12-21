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
        public string Room { get; set; }
        public string Owner { get; set; }
        public string OwnerUid { get; set; }
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
            target.Room = source.Room;
            target.Owner = source.Person.Name + " " + source.Person.LastName;
            target.OwnerUid = source.Person.uid;
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
            target.Person = db.Persons.Single(i => i.uid == source.OwnerUid);
            target.Room = source.Room;
        }

        public void CreateDevice(Device obj, HwInfContext db)
        {
            var target = obj;
            var source = this;

            target.Name = source.Name;
            target.InvNum = source.InvNum;
            target.Brand = source.Marke;
            target.Type = db.DeviceTypes.Single(i => i.TypeId == source.TypeId);
            target.Status = db.DeviceStatus.Single(i => i.StatusId == source.StatusId);
            target.CreateDate = DateTime.Now;
            target.Room = source.Room;
            target.Person = db.Persons.Single(i => i.uid == source.OwnerUid);


            if (source.DeviceMetaData.Count != 0)
            {
                foreach (var m in source.DeviceMetaData)
                {
                    db.DeviceMeta.Add(new DeviceMeta
                    {
                        MetaKey = m.Key,
                        MetaValue = m.Value,
                        Device = target,
                        DeviceType = target.Type
                    });
                }
            }
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
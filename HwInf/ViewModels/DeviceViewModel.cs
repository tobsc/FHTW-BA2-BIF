using HwInf.Common.BL;
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
        public bool IsActive { get; set; } = true;

        public DeviceViewModel()
        {
            Status = "Verfügbar";
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
            target.IsActive = source.IsActive;
        }

        public void ApplyChanges(Device obj, BL bl)
        {
            var target = obj;
            var source = this;

            target.Name = source.Name;
            target.InvNum = source.InvNum;
            target.Brand = source.Marke;
            target.Status = bl.GetDeviceStatus(source.StatusId);
            target.Type = bl.GetDeviceType(source.TypeId);
            target.Person = bl.GetPerson(source.OwnerUid);
            target.Room = source.Room;
            target.IsActive = source.IsActive;
        }

        public Device CreateDevice(BL bl)
        {

            // Create Device from VMDL
            Device device = new Device();
            ApplyChanges(device, bl);

            // Create Device
            bl.CreateDevice(device);

            // Check if there are any MetaData and create them
            if (DeviceMetaData.Count != 0)
            {
                foreach (var meta in DeviceMetaData)
                {
                    bl.CreateDeviceMeta(new DeviceMeta
                    {
                        Component = bl.GetComponent(device.Type.TypeId, meta.Key),
                        MetaValue = meta.Value,
                        Device = device
                    });
                }
            }


            return device;
        }

        public DeviceViewModel loadMeta(BL bl)
        {
            DeviceMetaData = new Dictionary<string, string>();

            var deviceMeta = bl.LoadDeviceMeta(DeviceId);

            foreach (DeviceMeta m in deviceMeta)
            {
                    DeviceMetaData.Add(m.Component.Name, m.MetaValue);
            }
            return this; // fluent interface
        }
    }
}
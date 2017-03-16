using HwInf.Common.DAL;
using System.Linq;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using HwInf.Common.Models;

namespace HwInf.Common.BL
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class BL
    {
        private readonly HwInfContext _dal;

        public BL() {
            _dal = new HwInfContext();
        }
        public BL(HwInfContext dal)
        {
            _dal = dal;
        }

        public IQueryable<Device> GetDevices(int limit = 25, int offset = 0, bool onlyActive = true, int type = 0, bool isSearch = false)
        {

            if (type == 0)
            {
                return _dal.Devices.Include(x => x.Type.Components.Select(y => y.ComponentType))
                    .Where(i => i.IsActive)
                    .Where(i => i.DeviceId > offset)
                    .Take(limit);
            } else if(isSearch)
            {
                return _dal.Devices.Include(x => x.Type)
                    .Where(i => i.IsActive)
                    .Where(i => i.Type.TypeId== type);
            } else
            {
                return _dal.Devices.Include(x => x.Type)
                    .Where(i => i.IsActive)
                    .Where(i => i.Type.TypeId == type)
                    .Where(i => i.DeviceId > offset)
                    .Take(limit);
            }

        }

        public Device GetSingleDevice(int deviceId)
        {
            return _dal.Devices.Include(x => x.DeviceMeta.Select(y => y.FieldGroup).Select(y => y.Fields)).Single(i => i.DeviceId == deviceId);
        }

        public DeviceType GetDeviceType(int typeId)
        {
            return _dal.DeviceTypes.Include(x => x.Components.Select(y => y.ComponentType)).Single(i => i.TypeId == typeId);
        }

        public DeviceType GetDeviceType(string typeName)
        {
            return _dal.DeviceTypes.Include(x => x.Components.Select(y => y.ComponentType)).Single(i => i.Description.ToLower().Equals(typeName.ToLower()));
        }

        public IQueryable<DeviceMeta> GetDeviceMeta()
        {
            return _dal.DeviceMeta.Include(x => x.FieldGroup);
        }

        public IQueryable<DeviceType> GetDeviceTypes()
        {
            return _dal.DeviceTypes.Include(x => x.Components.Select(y => y.ComponentType));
        }

        public Person GetPerson(string uid)
        {
            return _dal.Persons.Single(i => i.uid == uid);
        }

        public DeviceStatus GetDeviceStatus(int statusId)
        {
            return _dal.DeviceStatus.Single(i => i.StatusId == statusId);
        }

        public IQueryable<DeviceStatus> GetDeviceStatus()
        {
            return _dal.DeviceStatus;
        }

        public DeviceMeta CreateDeviceMeta(DeviceMeta dm)
        {
            _dal.DeviceMeta.Add(dm);
            return dm;
        }

        public void UpdateDeviceMeta(DeviceMeta deviceMeta)
        {
            _dal.Entry(deviceMeta).State = EntityState.Modified;
        }


        public Device CreateDevice()
        {
            var dev = new Device();
            _dal.Devices.Add(dev);
            return dev;

        }

        public DeviceType CreateDeviceType()
        {
            var dt = new DeviceType();
            _dal.DeviceTypes.Add(dt);
            return dt;
        }

        public Component CreateComponent()
        {
            var c = new Component();
            _dal.Components.Add(c);
            return c;
        }

        public ComponentType GetComponentType(int compTypeId)
        {
           return _dal.ComponentTypes.Single(i => i.CompTypeId == compTypeId);
        }

        public ComponentType GetComponentType(string compTypeName)
        {
            return _dal.ComponentTypes.Single(i => i.Name == compTypeName);
        }

        public Component GetComponent(int id)
        {
            return _dal.Components.Include(x => x.ComponentType).Single(i => i.CompId == id);
        }

        public void UpdateDevice(Device device)
        {
            _dal.Entry(device).State = EntityState.Modified;
        }

        public bool DeviceExists(int deviceId)
        {
            return _dal.Devices.Count(i => i.DeviceId == deviceId) > 0;
        }

        public void DeleteDevice(int deviceId)
        {
            var device = _dal.Devices.Find(deviceId);
            UpdateDevice(device);
            if (device != null) device.IsActive = false;
        }

        public void SaveChanges()
        {
            _dal.SaveChanges();
        }
    }

}

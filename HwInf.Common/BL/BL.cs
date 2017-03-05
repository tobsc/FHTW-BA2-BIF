using HwInf.Common.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace HwInf.Common.BL
{
    public class BL
    {
        private HwInfContext _dal;

        public BL() {
            _dal = new HwInfContext();
        }
        public BL(HwInfContext dal)
        {
            _dal = dal;
        }

        public IQueryable<Device> GetDevices(bool onlyActive = true, string type = null)
        {

            if(type == null)
            {
                return _dal.Devices.Include(x => x.Type)
                    .Where(i => i.IsActive == true)
                    .Take(10000);
            } else
            {
                return _dal.Devices.Include(x => x.Type)
                    .Where(i => i.IsActive == true)
                    .Where(i => i .Type.Description.ToLower() == type)
                    .Take(10000);
            }

        }

        public Device GetSingleDevice(int deviceId)
        {
            return _dal.Devices.Single(i => i.DeviceId == deviceId);
        }

        public DeviceType GetDeviceType(int typeId)
        {
            return _dal.DeviceTypes.Single(i => i.TypeId == typeId);
        }

        public Person GetPerson(string uid)
        {
            return _dal.Persons.Single(i => i.uid == uid);
        }

        public DeviceStatus GetDeviceStatus(int statusId)
        {
            return _dal.DeviceStatus.Single(i => i.StatusId == statusId);
        }

        public Component GetComponent(int typeId, string componentName)
        {
            return _dal.Components.Single(i => i.DeviceType.TypeId == typeId && i.Name == componentName);
        }

        public void CreateDeviceMeta(DeviceMeta deviceMeta)
        {
            _dal.DeviceMeta.Add(deviceMeta);
            SaveChanges();
        }

        public IQueryable<DeviceMeta> LoadDeviceMeta(int deviceId)
        {
            return _dal.DeviceMeta.Include("Component").Include("Device").Where(i => i.Device.DeviceId == deviceId);
        }

        public void CreateDevice(Device device)
        {
            _dal.Devices.Add(device);
            SaveChanges();
        }

        public void CreateDeviceType(DeviceType deviceType)
        {
            _dal.DeviceTypes.Add(deviceType);
            SaveChanges();
        }

        public void CreateComponent(Component component)
        {
            _dal.Components.Add(component);
            SaveChanges();
        }

        public void UpdateDevice(Device device)
        {
            _dal.Entry(device).State = EntityState.Modified;
            SaveChanges();
        }

        public bool DeviceExists(int deviceId)
        {
            return _dal.Devices.Count(i => i.DeviceId == deviceId) > 0;
        }

        public void DeleteDevice(int deviceId)
        {
            Device device = _dal.Devices.Find(deviceId);
            _dal.Devices.Remove(device);
            SaveChanges();
        }

        private void SaveChanges()
        {
            _dal.SaveChanges();
        }
    }

}

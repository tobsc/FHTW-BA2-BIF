using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security;
using HwInf.BusinessLogic.Interfaces;
using HwInf.Common.Models;
using HwInf.DataAccess.Interfaces;
using MoreLinq;

namespace HwInf.BusinessLogic
{
    public class DeviceBusinessLogic : IDeviceBusinessLogic
    {
        private readonly IDataAccessLayer _dal;
        private readonly IBusinessLogicPrincipal _principal;

        public DeviceBusinessLogic(IDataAccessLayer dal, IBusinessLogicPrincipal principal)
        {
            _dal = dal;
            _principal = principal;
        }
        public IEnumerable<Device> GetDevices(bool onlyActive = true, string typeSlug = "")
        {
            var obj = _dal.Devices;
            obj = onlyActive ? obj.Where(i => i.IsActive) : obj;
            obj = !string.IsNullOrWhiteSpace(typeSlug)
                ? obj.Where(i => i.Type.Slug.Equals(typeSlug))
                : obj;
            return obj;
        }

        public Device GetSingleDevice(int deviceId)
        {
            var obj = _dal.Devices
                    .SingleOrDefault(i => i.DeviceId == deviceId);
            return obj;

        }

        public Device GetSingleDevice(string deviceInvNum)
        {
            var obj = _dal.Devices
                .SingleOrDefault(i => i.InvNum.Equals(deviceInvNum));
            return obj;
        }

        public DeviceType GetDeviceType(int typeId)
        {
            var obj = _dal.DeviceTypes
                    .SingleOrDefault(i => i.TypeId == typeId);
            return obj;

        }

        public DeviceType GetDeviceType(string typeSlug)
        {
            var obj = _dal.DeviceTypes
                .SingleOrDefault(i => i.Slug == typeSlug);
            return obj;
        }

        public IEnumerable<DeviceType> GetDeviceTypes()
        {
            var obj = _dal.DeviceTypes;
            return obj;
        }

        public IEnumerable<DeviceMeta> GetDeviceMeta()
        {
            var obj = _dal.DeviceMeta;
            return obj;
        }

        public DeviceStatus GetDeviceStatus(int statusId)
        {
            var obj = _dal.DeviceStatus.FirstOrDefault(s => s.StatusId == statusId);
            return obj;
        }

        public IEnumerable<DeviceStatus> GetDeviceStatuses()
        {
            var obj = _dal.DeviceStatus;
            return obj;
        }

        public bool DeviceExists(int deviceId)
        {
            return _dal.Devices.Any(i => i.DeviceId == deviceId);
        }

        public Device CreateDevice()
        {
            if (!_principal.IsAllowed) throw new SecurityException();
            var device = _dal.CreateDevice();
            return device;
        }

        public DeviceType CreateDeviceType()
        {
            if (!_principal.IsAllowed) throw new SecurityException();
            var deviceType = _dal.CreateDeviceType();
            return deviceType;
        }

        public DeviceStatus CreateDeviceStatus()
        {
            if (!_principal.IsAllowed) throw new SecurityException();
            var deviceStatus = _dal.CreateDeviceStatus();
            return deviceStatus;
        }

        public DeviceMeta CreateDeviceMeta()
        {
            var deviceMeta = _dal.CreateDeviceMeta();
            return deviceMeta;
        }

        public void DeleteMeta(DeviceMeta dm)
        {
            _dal.DeleteDeviceMeta(dm);
        }

        public void UpdateDevice(Device device)
        {
            if (!_principal.IsAllowed) throw new SecurityException();
            _dal.UpdateObject(device);
        }

        public void UpdateDeviceType(DeviceType dt)
        {
            if (!_principal.IsAllowed) throw new SecurityException();
            _dal.UpdateObject(dt);
        }

        public void DeleteDevice(Device d)
        {
            if (!_principal.IsAllowed) throw new SecurityException();
            var device = _dal.Devices.FirstOrDefault(i => d.DeviceId.Equals(i.DeviceId));
            UpdateDevice(device);
            if (device != null) device.IsActive = false;
        }

        public void DeleteDeviceType(DeviceType dt)
        {
            if (!_principal.IsAllowed) throw new SecurityException();

            if (!GetDevices(true, dt.Slug).Any())
            {
                _dal.DeleteDeviceType(dt);

            }
            else
            {
                UpdateDeviceType(dt);
                dt.IsActive = false;
            }
        }

        public int DeviceCount()
        {
            return _dal.Devices.Count(d => d.IsActive);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="meta">Collection of device meta which should be filtered</param>
        /// <param name="type">Device Type</param>
        /// <param name="order">Order DESC or ASC</param>
        /// <param name="orderBy">Order by property</param>
        /// <param name="onlyActive">Show all or only active devices</param>
        /// <param name="isVerwalterView">Verwalter only gets his own devices in device management view</param>
        /// <param name="isUserView">In user view items with no invnum are only shown as one device</param>
        /// <returns></returns>
        public ICollection<Device> GetFilteredDevices
        (
            ICollection<DeviceMeta> meta,
            string type = null,
            string order = "DESC",
            string orderBy = "Name",
            bool onlyActive = true,
            bool isVerwalterView = false,
            bool isUserView = false
        )
        {

            if (isVerwalterView && !_principal.IsAllowed)
            {
                throw new SecurityException();
            }

            var dt = GetDeviceType(type);
            var devices = GetDevices(onlyActive, string.IsNullOrWhiteSpace(type) ? "" : dt.Slug).ToList();
            List<Device> result;

            // Merge Devices with no invnum to single device
            if (isUserView)
            {
                var nullDeviceGroup = devices
                    .Where(i => string.IsNullOrWhiteSpace(i.DeviceGroupSlug));

                var distinctDevictByDeviceGroup = devices
                    .Where(i => !string.IsNullOrWhiteSpace(i.DeviceGroupSlug))
                    .Where(i => i.Status.Description == "Verfügbar")
                    .AsEnumerable()
                    .DistinctBy(i => i.DeviceGroupSlug);

                result = nullDeviceGroup.Concat(distinctDevictByDeviceGroup).ToList();
            }
            else
            {
                result = devices;
            }

            // If meta is not null filter by provided device meta
            if (meta != null)
            {
                List<List<DeviceMeta>> deviceMetaGroupedByFieldGroup = meta
                    .Where(i => !string.IsNullOrWhiteSpace(i.FieldGroupSlug)
                             && !string.IsNullOrWhiteSpace(i.FieldSlug)
                             && !string.IsNullOrWhiteSpace(i.MetaValue)
                             )
                    .GroupBy(i => i.FieldGroupSlug, i => i)
                    .ToList()
                    .Select(i => i.ToList())
                    .ToList();

                deviceMetaGroupedByFieldGroup.ForEach(i =>
                {
                    result = result
                        .Where(j => j.DeviceMeta.Intersect(i, new DeviceMetaComparer()).Count() == i.Count)
                        .ToList();
                });
            }

            // Order by provided property
            result = order.Equals("ASC")
                ? result.OrderBy(i => i.GetType().GetProperty(orderBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)?.GetValue(i, null))
                .ToList()
                : result
                    .OrderByDescending(i => i.GetType().GetProperty(orderBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)?.GetValue(i, null))
                    .ToList();

            result = result
                .Distinct()
                .ToList();


            return !isVerwalterView || _principal.IsAdmin ?
                result
                : result.Where(i => i.Person.Uid.Equals(_principal.CurrentUid)).ToList();
        }

        public ICollection<Device> GetFilteredDevicesUser
        (
            ICollection<DeviceMeta> meta,
            string type = null,
            string order = "DESC",
            string orderBy = "Name",
            bool onlyActive = true,
            bool isVerwalterView = false
        )
        {

            return GetFilteredDevices(meta, type, order, orderBy, onlyActive, isVerwalterView, true);
        }
    }
}
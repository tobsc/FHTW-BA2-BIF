using System.Collections.Generic;
using System.Linq;
using HwInf.Common.Models;

namespace HwInf.BusinessLogic.Interfaces
{
    public interface IDeviceBusinessLogic
    {
        IEnumerable<Device> GetDevices(bool onlyActive = true, string typeSlug = "");
        Device GetSingleDevice(int deviceId);
        Device GetSingleDevice(string deviceInvNum);
        DeviceType GetDeviceType(int typeId);
        DeviceType GetDeviceType(string typeSlug);
        IEnumerable<DeviceType> GetDeviceTypes();
        IEnumerable<DeviceMeta> GetDeviceMeta();
        DeviceStatus GetDeviceStatus(int statusId);
        IEnumerable<DeviceStatus> GetDeviceStatuses();
        bool DeviceExists(int deviceId);
        Device CreateDevice();
        DeviceType CreateDeviceType();
        DeviceStatus CreateDeviceStatus();
        DeviceMeta CreateDeviceMeta();
        void DeleteMeta(DeviceMeta dm);
        void UpdateDevice(Device device);
        void UpdateDeviceType(DeviceType dt);
        void DeleteDevice(Device d);
        void DeleteDeviceType(DeviceType dt);
        int DeviceCount();
        ICollection<Device> GetFilteredDevices
        (
            ICollection<DeviceMeta> meta,
            string type = null,
            string order = "DESC",
            string orderBy = "Name",
            bool onlyActive = true,
            bool isVerwalterView = false,
            bool isUserView = false
        );
        ICollection<Device> GetFilteredDevicesUser
        (
            ICollection<DeviceMeta> meta,
            string type = null,
            string order = "DESC",
            string orderBy = "Name",
            bool onlyActive = true,
            bool isVerwalterView = false
        );
    }
}
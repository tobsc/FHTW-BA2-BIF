using System;
using System.Collections.Generic;
using System.Linq;
using HwInf.Common.Models;

namespace HwInf.Common.Interfaces
{
    public interface IBusinessLayer
    {
        bool IsAdmin { get; }
        bool IsVerwalter { get; }
        IEnumerable<Device> GetDevices(bool onlyActive = true, string typeSlug = "");
        Device GetSingleDevice(int deviceId);
        Device GetSingleDevice(string deviceInvNum);
        DeviceType GetDeviceType(int typeId);
        DeviceType GetDeviceType(string typeSlug);
        IQueryable<DeviceType> GetDeviceTypes();
        IQueryable<DeviceMeta> GetDeviceMeta();
        DeviceStatus GetDeviceStatus(int statusId);
        IQueryable<DeviceStatus> GetDeviceStatuses();
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
        IQueryable<FieldGroup> GetFieldGroups();
        FieldGroup GetFieldGroups(string groupSlug);
        IQueryable<Field> GetFields();
        Field GetFields(string slug);
        bool FieldGroupExists(string slug);
        FieldGroup CreateFieldGroup();
        Field CreateField();
        void UpdateFieldGroup(FieldGroup obj);
        void DeleteField(Field field);
        void DeleteFieldGroup(FieldGroup obj);
        IQueryable<Accessory> GetAccessories();
        Accessory GetAccessory(string slug);
        Accessory GetAccessory(int id);
        Accessory CreateAccessory();
        void DeleteAccessory(Accessory a);
        void UpdateAccessory(Accessory a);
        IQueryable<Person> GetUsers();
        Person GetUsers(string uid);
        Role GetRole(string name);
        Person CreateUser();
        void UpdateUser(Person obj);
        IEnumerable<Order> GetOrders();
        Order GetOrders(int orderId);
        Order GetOrders(Guid guid);
        IEnumerable<OrderStatus> GetOrderStatus();
        OrderStatus GetOrderStatus(string slug);
        IEnumerable<OrderItem> GetOrderItems();
        OrderItem GetOrderItem(int id);
        Order CreateOrder();
        OrderItem CreateOrderItem();
        void UpdateOrderItem(OrderItem obj);
        void SaveChanges();

        ICollection<Order> GetFilteredOrders(
            ICollection<string> statusSlugs,
            string order,
            string orderBy,
            string orderByFallback,
            bool isAdminView
        );

        ICollection<Order> SearchOrders(
            string searchQuery,
            string order,
            string orderBy,
            string orderByFallback, 
            bool isAdminView );

        ICollection<Device> GetFilteredDevices
        (
            ICollection<DeviceMeta> meta,
            string type = null,
            string order = "DESC",
            string orderBy = "Name",
            bool onlyActive = true,
            bool isVerwalterView = false
        );

        Setting GetSetting(string key);
        IEnumerable<Setting> GetSettings();
        Setting CreateSetting();
        void DeleteSetting(Setting s);
        void UpdateSetting(Setting s);
        string[] GetLog();
        bool IsAdminUid(string uid);
        void SetAdmin(Person obj);
        string GetCurrentUid();
        string CreateToken(Person p);
        Damage GetDamage(int id);
        IEnumerable<Damage> GetDamages(string invNum);
        IEnumerable<Damage> GetDamages();
        Damage CreateDamage();
        void DeleteDamage(Damage s);
        void UpdateDamage(Damage s);
        IEnumerable<DamageStatus> GetDamageStatus();
        DamageStatus GetDamageStatus(string slug);
        bool DamageExists(int damageId);
    }
}
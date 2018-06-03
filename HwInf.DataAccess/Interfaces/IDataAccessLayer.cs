using System;
using System.Linq;
using HwInf.DataAccess.Context;
using HwInf.DataAccess.Entities;

namespace HwInf.DataAccess.Interfaces
{
    public interface IDataAccessLayer
    {
        IQueryable<Device> Devices { get; }
        IQueryable<DeviceMeta> DeviceMeta { get; }
        IQueryable<DeviceType> DeviceTypes { get; }
        IQueryable<DeviceStatus> DeviceStatus { get; }
        IQueryable<OrderStatus> OrderStatus { get; }
        IQueryable<Person> Persons { get; }
        IQueryable<Role> Roles { get; }
        IQueryable<Order> Orders { get; }
        IQueryable<OrderItem> OrderItems { get; }
        IQueryable<Field> Fields { get; }
        IQueryable<FieldGroup> FieldGroups { get; }
        IQueryable<Setting> Settings { get; }
        IQueryable<Damage> Damages { get; }
        IQueryable<DamageStatus> DamageStatus { get; }
        IQueryable<Accessory> Accessories { get; }

        Device CreateDevice();
        DeviceType CreateDeviceType();
        DeviceStatus CreatDeviceStatus();
        OrderStatus CreateOrderStatus();
        Person CreatePerson();
        Order CreateOrder();
        Field CreateField();
        FieldGroup CreateFieldGroup();
        DeviceStatus CreateDeviceStatus();
        DeviceMeta CreateDeviceMeta();
        OrderItem CreateOrderItem();
        Setting CreateSetting();
        Damage CreateDamage();
        DamageStatus CreateDamageStatus();
        Accessory CreateAccessory();

        void SaveChanges();
        void DeleteDeviceType(DeviceType deviceType);
        void DeleteDeviceMeta(DeviceMeta deviceMeta);
        void DeleteField(Field field);
        void DeleteFieldGroup(FieldGroup fieldGroup);
        void DeleteSetting(Setting setting);
        void DeleteDamage(Damage damage);
        void DeleteAccessory(Accessory accessory);
        void UpdateObject(object obj);

        Device AddDeviceMetaToDevice(DeviceMeta deviceMeta, HwInfContext context);
        void AddFieldToFieldGroup(FieldGroup fieldGroup, HwInfContext context);
        void AddDeviceTypeToFieldGroup(DeviceType deviceType, HwInfContext context);
        void AddFieldGroupToDeviceType(FieldGroup fieldGroup, HwInfContext context);
    }
}
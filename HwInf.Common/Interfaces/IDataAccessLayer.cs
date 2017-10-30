using System;
using System.Linq;
using HwInf.Common.Models;

namespace HwInf.Common.Interfaces
{
    public interface IDataAccessLayer : IDisposable
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
        void DeleteDeviceType(DeviceType dt);
        void DeleteDeviceMeta(DeviceMeta dm);
        void DeleteField(Field f);
        void DeleteFieldGroup(FieldGroup fg);
        void DeleteSetting(Setting s);
        void DeleteDamage(Damage d);
        void DeleteAccessory(Accessory a);
        void UpdateObject(object obj);
    }
}
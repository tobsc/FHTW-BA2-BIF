using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using HwInf.BusinessLogic.Interfaces;
using HwInf.Common.Models;
using HwInf.DataAccess.Interfaces;

namespace HwInf.BusinessLogic
{

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class BusinessLogicFacade : IBusinessLogicFacade
    {
        private readonly IDataAccessLayer _dal;
        private readonly IBusinessLogic _bl;
        private readonly IAccessoryBusinessLogic _accessoryBl;
        private readonly ICustomFieldsBusinessLogic _cfBl;
        private readonly IDamageBusinessLogic _damageBl;
        private readonly IDeviceBusinessLogic _deviceBl;
        private readonly IOrderBusinessLogic _orderBl;
        private readonly ISettingBusinessLogic _settingBl;
        private readonly IUserBusinessLogic _userBl;

        public BusinessLogicFacade(
            IDataAccessLayer dal, 
            IBusinessLogic bl, 
            IAccessoryBusinessLogic accessoryBl, 
            ICustomFieldsBusinessLogic cfBl, 
            IDamageBusinessLogic damageBl, 
            IDeviceBusinessLogic deviceBl, 
            IOrderBusinessLogic orderBl, 
            ISettingBusinessLogic settingBl, 
            IUserBusinessLogic userBl)
        {
            _dal = dal;
            _bl = bl;
            _accessoryBl = accessoryBl;
            _cfBl = cfBl;
            _damageBl = damageBl;
            _deviceBl = deviceBl;
            _orderBl = orderBl;
            _settingBl = settingBl;
            _userBl = userBl;
        }

        public bool IsAdmin => _bl.IsAdmin;
        public bool IsVerwalter => _bl.IsVerwalter;
        public string[] GetLog()
        {
            return _bl.GetLog();
        }

        public bool IsAdminUid(string uid)
        {
            return _bl.IsAdminUid(uid);
        }

        public string GetCurrentUid()
        {
            return _bl.GetCurrentUid();
        }

        public JwtSecurityToken CreateToken(Person p)
        {
            return _bl.CreateToken(p);
        }

        public IEnumerable<Device> GetDevices(bool onlyActive = true, string typeSlug = "")
        {
            return _deviceBl.GetDevices(onlyActive, typeSlug);
        }

        public Device GetSingleDevice(int deviceId)
        {
            return _deviceBl.GetSingleDevice(deviceId);
        }

        public Device GetSingleDevice(string deviceInvNum)
        {
            return _deviceBl.GetSingleDevice(deviceInvNum);
        }

        public DeviceType GetDeviceType(int typeId)
        {
            return _deviceBl.GetDeviceType(typeId);
        }

        public DeviceType GetDeviceType(string typeSlug)
        {
            return _deviceBl.GetDeviceType(typeSlug);
        }

        public IEnumerable<DeviceType> GetDeviceTypes()
        {
            return _deviceBl.GetDeviceTypes();
        }

        public IEnumerable<DeviceMeta> GetDeviceMeta()
        {
            return _deviceBl.GetDeviceMeta();
        }

        public DeviceStatus GetDeviceStatus(int statusId)
        {
            return _deviceBl.GetDeviceStatus(statusId);
        }

        public IEnumerable<DeviceStatus> GetDeviceStatuses()
        {
            return _deviceBl.GetDeviceStatuses();
        }

        public bool DeviceExists(int deviceId)
        {
            return _deviceBl.DeviceExists(deviceId);
        }

        public Device CreateDevice()
        {
            return _deviceBl.CreateDevice();
        }

        public DeviceType CreateDeviceType()
        {
            return _deviceBl.CreateDeviceType();
        }

        public DeviceStatus CreateDeviceStatus()
        {
            return _deviceBl.CreateDeviceStatus();
        }

        public DeviceMeta CreateDeviceMeta()
        {
            return _deviceBl.CreateDeviceMeta();
        }

        public void DeleteMeta(DeviceMeta dm)
        {
            _deviceBl.DeleteMeta(dm);
        }

        public void UpdateDevice(Device device)
        {
            _deviceBl.UpdateDevice(device);
        }

        public void UpdateDeviceType(DeviceType dt)
        {
            _deviceBl.UpdateDeviceType(dt);
        }

        public void DeleteDevice(Device d)
        {
            _deviceBl.DeleteDevice(d);
        }

        public void DeleteDeviceType(DeviceType dt)
        {
            _deviceBl.DeleteDeviceType(dt);
        }

        public int DeviceCount()
        {
            return _deviceBl.DeviceCount();
        }

        public ICollection<Device> GetFilteredDevices(ICollection<DeviceMeta> meta, string type = null, string order = "DESC", string orderBy = "Name",
            bool onlyActive = true, bool isVerwalterView = false, bool isUserView = false)
        {
            return _deviceBl.GetFilteredDevices(meta, type, order, orderBy, onlyActive, isVerwalterView, isUserView);
        }

        public ICollection<Device> GetFilteredDevicesUser(ICollection<DeviceMeta> meta, string type = null, string order = "DESC", string orderBy = "Name",
            bool onlyActive = true, bool isVerwalterView = false)
        {
            return _deviceBl.GetFilteredDevicesUser(meta, type, order, orderBy, onlyActive, isVerwalterView);
        }

        public IEnumerable<FieldGroup> GetFieldGroups()
        {
            return _cfBl.GetFieldGroups();
        }

        public FieldGroup GetFieldGroup(string groupSlug)
        {
            return _cfBl.GetFieldGroup(groupSlug);
        }

        public IEnumerable<Field> GetFields()
        {
            return _cfBl.GetFields();
        }

        public Field GetField(string slug)
        {
            return _cfBl.GetField(slug);
        }

        public bool FieldGroupExists(string slug)
        {
            return _cfBl.FieldGroupExists(slug);
        }

        public FieldGroup CreateFieldGroup()
        {
            return _cfBl.CreateFieldGroup();
        }

        public Field CreateField()
        {
            return _cfBl.CreateField();
        }

        public void UpdateFieldGroup(FieldGroup obj)
        {
            _cfBl.UpdateFieldGroup(obj);
        }

        public void DeleteField(Field field)
        {
            _cfBl.DeleteField(field);
        }

        public void DeleteFieldGroup(FieldGroup obj)
        {
            _cfBl.DeleteFieldGroup(obj);
        }

        public IEnumerable<Accessory> GetAccessories()
        {
            return _accessoryBl.GetAccessories();
        }

        public Accessory GetAccessory(string slug)
        {
            return _accessoryBl.GetAccessory(slug);
        }

        public Accessory GetAccessory(int id)
        {
            return _accessoryBl.GetAccessory(id);
        }

        public Accessory CreateAccessory()
        {
            return _accessoryBl.CreateAccessory();
        }

        public void DeleteAccessory(Accessory a)
        {
            _accessoryBl.DeleteAccessory(a);
        }

        public void UpdateAccessory(Accessory a)
        {
            _accessoryBl.UpdateAccessory(a);
        }

        public IEnumerable<Person> GetUsers()
        {
            return _userBl.GetUsers();
        }

        public Person GetUsers(string uid)
        {
            return _userBl.GetUsers(uid);
        }

        public Role GetRole(string name)
        {
            return _userBl.GetRole(name);
        }

        public Person CreateUser()
        {
            return _userBl.CreateUser();
        }

        public void UpdateUser(Person obj)
        {
            _userBl.UpdateUser(obj);
        }

        public void SetAdmin(Person obj)
        {
            _userBl.SetAdmin(obj);
        }

        public IEnumerable<Order> GetOrders()
        {
            return _orderBl.GetOrders();
        }

        public Order GetOrders(int orderId)
        {
            return _orderBl.GetOrders(orderId);
        }

        public Order GetOrders(Guid guid)
        {
            return _orderBl.GetOrders(guid);
        }

        public IEnumerable<OrderStatus> GetOrderStatus()
        {
            return _orderBl.GetOrderStatus();
        }

        public OrderStatus GetOrderStatus(string slug)
        {
            return _orderBl.GetOrderStatus(slug);
        }

        public IEnumerable<OrderItem> GetOrderItems()
        {
            return _orderBl.GetOrderItems();
        }

        public OrderItem GetOrderItem(int id)
        {
            return _orderBl.GetOrderItem(id);
        }

        public Order CreateOrder()
        {
            return _orderBl.CreateOrder();
        }

        public OrderItem CreateOrderItem()
        {
            return _orderBl.CreateOrderItem();
        }

        public void UpdateOrderItem(OrderItem obj)
        {
            _orderBl.UpdateOrderItem(obj);
        }

        public ICollection<Order> GetFilteredOrders(ICollection<string> statusSlugs, string order, string orderBy, string orderByFallback,
            bool isAdminView)
        {
            return _orderBl.GetFilteredOrders(statusSlugs, order, orderBy, orderByFallback, isAdminView);
        }

        public ICollection<Order> SearchOrders(string searchQuery, string order, string orderBy, string orderByFallback, bool isAdminView)
        {
            return _orderBl.SearchOrders(searchQuery, order, orderBy, orderByFallback, isAdminView);
        }

        public Setting GetSetting(string key)
        {
            return _settingBl.GetSetting(key);
        }

        public IEnumerable<Setting> GetSettings()
        {
            return _settingBl.GetSettings();
        }

        public Setting CreateSetting()
        {
            return _settingBl.CreateSetting();
        }

        public void DeleteSetting(Setting s)
        {
            _settingBl.DeleteSetting(s);
        }

        public void UpdateSetting(Setting s)
        {
            _settingBl.UpdateSetting(s);
        }

        public Damage GetDamage(int id)
        {
            return _damageBl.GetDamage(id);
        }

        public IEnumerable<Damage> GetDamages(string invNum)
        {
            return _damageBl.GetDamages(invNum);
        }

        public IEnumerable<Damage> GetDamages(int deviceId)
        {
            return _damageBl.GetDamages(deviceId);
        }

        public IEnumerable<Damage> GetDamages()
        {
            return _damageBl.GetDamages();
        }

        public Damage CreateDamage()
        {
            return _damageBl.CreateDamage();
        }

        public void DeleteDamage(Damage s)
        {
            _damageBl.DeleteDamage(s);
        }

        public void UpdateDamage(Damage s)
        {
            _damageBl.UpdateDamage(s);
        }

        public IEnumerable<DamageStatus> GetDamageStatus()
        {
            return _damageBl.GetDamageStatus();
        }

        public DamageStatus GetDamageStatus(string slug)
        {
            return _damageBl.GetDamageStatus(slug);
        }

        public bool DamageExists(int damageId)
        {
            return _damageBl.DamageExists(damageId);
        }

        public void SaveChanges()
        {
            _dal.SaveChanges();
        }
    }




}

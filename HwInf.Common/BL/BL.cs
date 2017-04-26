using System;
using System.Collections.Generic;
using System.ComponentModel;
using HwInf.Common.DAL;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Validation;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Security;
using System.Text;
using HwInf.Common.Models;
using JWT;
using log4net;
using log4net.Appender;

namespace HwInf.Common.BL
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class BL
    {
        private readonly IDAL _dal;

        public bool IsAdmin => System.Threading.Thread.CurrentPrincipal.IsInRole("Admin");
        public bool IsVerwalter => System.Threading.Thread.CurrentPrincipal.IsInRole("Verwalter");

        public BL()
        {
            _dal = new HwInfContext();
        }

        public BL(IDAL dal)
        {
            _dal = dal;
        }

        #region DAL


        #region Devices

        // Read
        public IEnumerable<Device> GetDevices(bool onlyActive = true, string typeSlug = "")
        {
            var obj = _dal.Devices;

            obj = onlyActive ? obj.Where(i => i.IsActive) : obj;
            obj = !String.IsNullOrWhiteSpace(typeSlug)
                ? obj.Where(i => i.Type.Slug.Equals(typeSlug))
                : obj;

            return obj;
        }

        public Device GetSingleDevice(int deviceId)
        {
            return
                _dal.Devices
                    .SingleOrDefault(i => i.DeviceId == deviceId);
        }

        public Device GetSingleDevice(string deviceInvNum)
        {
            return _dal.Devices
                .SingleOrDefault(i => i.InvNum.Equals(deviceInvNum));
        }

        public DeviceType GetDeviceType(int typeId)
        {
            return
                _dal.DeviceTypes
                    .SingleOrDefault(i => i.TypeId == typeId);
        }

        public DeviceType GetDeviceType(string typeSlug)
        {
            return
                _dal.DeviceTypes
                    .SingleOrDefault(i => i.Slug.ToLower().Equals(typeSlug.ToLower()));
        }

        public IQueryable<DeviceType> GetDeviceTypes()
        {
            return _dal.DeviceTypes;
        }

        public IQueryable<DeviceMeta> GetDeviceMeta()
        {
            return _dal.DeviceMeta;
        }

        public DeviceStatus GetDeviceStatus(int statusId)
        {
            return _dal.DeviceStatus
                .SingleOrDefault(i => i.StatusId == statusId);
        }

        public IQueryable<DeviceStatus> GetDeviceStatus()
        {
            return _dal.DeviceStatus;
        }

        public bool DeviceExists(int deviceId)
        {
            return _dal.Devices.Any(i => i.DeviceId == deviceId);
        }

        // Create
        public Device CreateDevice()
        {
            if (!IsAdmin && !IsVerwalter) throw new SecurityException();
 
            return _dal.CreateDevice();
        }

        public DeviceType CreateDeviceType()
        {
            if (!IsAdmin && !IsVerwalter) throw new SecurityException();

            return _dal.CreateDeviceType();
        }

        public DeviceStatus CreateDeviceStatus()
        {
            if (!IsAdmin && !IsVerwalter) throw new SecurityException();

            return _dal.CreateDeviceStatus();
        }

        public DeviceMeta CreateDeviceMeta()
        {
            return _dal.CreateDeviceMeta();
        }

        public void DeleteMeta(DeviceMeta dm)
        {
            _dal.DeleteDeviceMeta(dm);
        }


        // Update
        public void UpdateDevice(Device device)
        {
            if (!IsAdmin && !IsVerwalter) throw new SecurityException();

            _dal.UpdateObject(device);
        }

        public void UpdateDeviceType(DeviceType dt)
        {
            if (!IsAdmin && !IsVerwalter) throw new SecurityException();

            _dal.UpdateObject(dt);
        }

        public void DeleteDevice(Device d)
        {
            if (!IsAdmin && !IsVerwalter) throw new SecurityException();

            var device = _dal.Devices.FirstOrDefault(i => d.InvNum.Equals(i.InvNum));
            UpdateDevice(device);
            if (device != null) device.IsActive = false;
        }

        public void DeleteDeviceType(DeviceType dt)
        {
            if (!IsAdmin && !IsVerwalter) throw new SecurityException();

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
            return _dal.Devices.Count(i => i.IsActive);
        }

        #endregion

        #region CustomFields

        // Read
        public IQueryable<FieldGroup> GetFieldGroups()
        {
            return _dal.FieldGroups;
        }

        public FieldGroup GetFieldGroups(string groupSlug)
        {
            return _dal.FieldGroups.SingleOrDefault(i => i.Slug.Equals(groupSlug));
        }

        public IQueryable<Field> GetFields()
        {
            return _dal.Fields;
        }

        public Field GetFields(string slug)
        {
            return _dal.Fields.SingleOrDefault(i => i.Slug.Equals(slug));
        }

        public bool FieldGroupExists(string slug)
        {
            return _dal.FieldGroups.Any(i => i.Slug.Equals(slug));
        }

        // Create
        public FieldGroup CreateFieldGroup()
        {
            if (!IsAdmin && !IsVerwalter) throw new SecurityException();

            return _dal.CreteFieldGroup();
        }

        public Field CreateField()
        {
            if (!IsAdmin && !IsVerwalter) throw new SecurityException();

            return _dal.CreaField();
        }

        // Update
        public void UpdateFieldGroup(FieldGroup obj)
        {
            if (!IsAdmin && !IsVerwalter) return;

            _dal.UpdateObject(obj);
        }

        public void DeleteField(Field field)
        {
            if (!IsAdmin && !IsVerwalter) return;
            _dal.DeleteField(field);
        }

        public void DeleteFieldGroup(FieldGroup obj)
        {
            if(!IsAdmin && !IsVerwalter) return;

            var fg = _dal.DeviceTypes.SelectMany(i => i.FieldGroups).ToList();
            if (!fg.Any(i => i.Slug.Equals(obj.Slug)))
            {
                obj.Fields.ToList().ForEach(i => _dal.DeleteField(i));
                _dal.DeleteFieldGroup(obj);
                
            }
            else
            {
                UpdateFieldGroup(obj);
                obj.IsActive = false;
            }
        }

        #endregion


        #region Users

        // Read
        public IQueryable<Person> GetUsers()
        {
            return _dal.Persons;
        }

        public Person GetUsers(string uid)
        {
            return _dal.Persons.SingleOrDefault(i => i.Uid == uid);
        }

        public Role GetRole(string name)
        {
            return _dal.Roles.SingleOrDefault(i => i.Name.Equals(name));
        }

        // Create
        public Person CreateUser()
        {
            return _dal.CreatePerson();
        }

        // Update
        public void UpdateUser(Person obj)
        {
            _dal.UpdateObject(obj);
        }

        #endregion

        #region Orders

        // Read

        public IEnumerable<Order> GetOrders()
        {
            return _dal.Orders;
        }


        public Order GetOrders(int orderId)
        {
            if (!IsAdmin && !IsVerwalter)
            {
               return _dal.Orders
                    .SingleOrDefault(i => i.OrderId.Equals(orderId) && i.Entleiher.Uid.Equals(GetCurrentUid()));
            }

            return _dal.Orders
                    .SingleOrDefault(i => i.OrderId.Equals(orderId));
        }

        public Order GetOrders(Guid guid)
        {
            return _dal.Orders.SingleOrDefault(i => i.OrderGuid.Equals(guid));
        }

        public IEnumerable<OrderStatus> GetOrderStatus()
        {
            return _dal.OrderStatus;
        }

        public OrderStatus GetOrderStatus(string slug)
        {
            return _dal.OrderStatus.FirstOrDefault(i => i.Slug.Equals(slug));
        }

        public IEnumerable<OrderItem> GetOrderItems()
        {
            return _dal.OrderItems;
        }

        public OrderItem GetOrderItem(int id)
        {
            return _dal.OrderItems.FirstOrDefault(i => i.ItemId.Equals(id));
        }

        public Order CreateOrder()
        {
            return _dal.CreateOrder();
        }

        public OrderItem CreateOrderItem()
        {
            return _dal.CreateOrderItem();
        }

        public void UpdateOrderItem(OrderItem obj)
        {
            if (!IsAdmin && !IsVerwalter) throw new SecurityException();
            _dal.UpdateObject(obj);
        }

        #endregion

        public void SaveChanges()
        {
            _dal.SaveChanges();
        }


        #endregion


        public ICollection<Order> GetFileteredOrders(
            string statusSlug,
            string order,
            string orderBy,
            string orderByFallback,
            bool isAdminView
        )
        {
            if (isAdminView && !IsAdmin && !IsVerwalter)
            {
                throw new SecurityException();
            }

            var result = GetOrders()
                .Where(i => String.IsNullOrWhiteSpace(statusSlug) || i.OrderStatus.Slug.Equals(statusSlug))
                .ToList();


            result = order.Equals("ASC")
                ? result.OrderBy(i => i.GetType().GetProperty(orderBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance).GetValue(i, null))
                    .ThenBy(i => i.GetType().GetProperty(orderByFallback, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance).GetValue(i, null))
                    .ToList()
                : result.OrderByDescending(i => i.GetType().GetProperty(orderBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance).GetValue(i, null))
                    .ThenByDescending(i => i.GetType().GetProperty(orderByFallback, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance).GetValue(i, null))
                    .ToList();

            return !isAdminView ?
                result.Where(i => i.Entleiher.Uid.Equals(GetCurrentUid())).ToList()
                : IsAdmin ?
                    result
                    : result.Where(i => i.Verwalter.Uid.Equals(GetCurrentUid())).ToList();
        }

        public ICollection<Order> SearchOrders(
            string searchQuery,
            string order,
            string orderBy,
            string orderByFallback, 
            bool isAdminView )
        {

            if (isAdminView && !IsAdmin && !IsVerwalter)
            {
                throw new SecurityException();
            }

            searchQuery = searchQuery.ToLower();
            var result = GetOrders().ToList();
            if (isAdminView)
            {
                if (IsVerwalter) result = result
                        .Where(i => i.Verwalter.Uid.Equals(GetCurrentUid()))
                        .ToList();

                result = result
                    .Where(i => i.Entleiher.Uid.ToLower().Contains(searchQuery)
                                || i.Entleiher.Name.ToLower().Contains(searchQuery)
                                || i.Entleiher.LastName.ToLower().Contains(searchQuery)
                                || i.OrderId.ToString().ToLower().Contains(searchQuery)
                                || i.OrderItems
                                    .ToList()
                                    .Any(x => x.Device.Name.ToLower().Contains(searchQuery)
                                              || x.Device.InvNum.ToLower().Contains(searchQuery)))
                    .ToList();                
            }
            else
            {
                result = result
                    .Where(i => i.Entleiher.Uid.Equals(GetCurrentUid()))
                    .Where(i => i.Verwalter.Uid.ToLower().Contains(searchQuery)
                        || i.Verwalter.Name.ToLower().Contains(searchQuery)
                        || i.Verwalter.LastName.ToLower().Contains(searchQuery)
                        || i.OrderId.ToString().ToLower().Contains(searchQuery)
                        || i.OrderItems
                            .ToList()
                            .Any(x => x.Device.Name.ToLower().Contains(searchQuery)
                                        || x.Device.InvNum.ToLower().Contains(searchQuery)))
                    .ToList();
                    
            }

            result = order.Equals("ASC")
                ? result.OrderBy(i => i.GetType().GetProperty(orderBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance).GetValue(i, null))
                    .ThenBy(i => i.GetType().GetProperty(orderByFallback, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance).GetValue(i, null))
                    .ToList()
                : result.OrderByDescending(i => i.GetType().GetProperty(orderBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance).GetValue(i, null))
                    .ThenByDescending(i => i.GetType().GetProperty(orderByFallback, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance).GetValue(i, null))
                    .ToList();

            return result;
        }

        public ICollection<Device> GetFilteredDevices
        (
            ICollection<DeviceMeta> meta,
            string type = null,
            string order = "DESC",
            string orderBy = "Name",
            bool onlyActive = true,
            bool isVerwalterView = false
        )
        {

            if (isVerwalterView && !IsAdmin && !IsVerwalter)
            {
                throw new SecurityException();
            }

            var dt = GetDeviceType(type);
            var devices = GetDevices(onlyActive, (string.IsNullOrWhiteSpace(type)) ? "" : dt.Slug).ToList();
            var result = devices;

            if (meta != null)
            {
                List<List<DeviceMeta>> deviceMetaGroupedByFieldGroup = meta
                    .Where(i => !String.IsNullOrWhiteSpace(i.FieldGroupSlug)
                             && !String.IsNullOrWhiteSpace(i.FieldSlug)
                             && !String.IsNullOrWhiteSpace(i.MetaValue)
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


            result = order.Equals("ASC")
                ? result.OrderBy(i =>
                {
                    var prop = i.GetType().GetProperty(orderBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance).GetValue(i, null);
                    return prop;

                }).ToList()
                : result.OrderByDescending(i => i.GetType().GetProperty(orderBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance).GetValue(i, null)).ToList();

            result = result
                .Distinct()
                .ToList();


                return !isVerwalterView || IsAdmin ? 
                    result
                   : result.Where(i => i.Person.Uid.Equals(GetCurrentUid())).ToList();
        }

        #region Settings

        public Setting GetSetting(string key)
        {
            return _dal.Settings.FirstOrDefault(i => i.Key.Equals(key));
        }

        public IEnumerable<Setting> GetSettings()
        {
            return _dal.Settings;
        }

        public Setting CreateSetting()
        {
            if (!IsAdmin && !IsVerwalter) throw new SecurityException();

            return _dal.CreateSetting();
        }

        public void DeleteSetting(Setting s)
        {
            _dal.DeleteSetting(s);
        }

        public void UpdateSetting(Setting s)
        {
            if (!IsAdmin && !IsVerwalter) return;

            _dal.UpdateObject(s);
        }

        public string[] GetLog()
        {
            if(!IsAdmin && !IsVerwalter) throw new SecurityException();

            var h = (log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository();
            var appender = h.GetAppenders().SingleOrDefault(i => i.Name.Equals("FileRollingFileAppender")) as FileAppender;

            string[] lines = {};
            if (appender == null) return lines;
            var file = appender.File;
            lines = File.ReadAllLines(@file, Encoding.UTF8);

            return lines;

        }


        #endregion

        #region Auth


        public bool IsAdminUid(string uid)
        {
            return _dal.Persons.Any(i => i.Uid == uid && i.Role.Name.Equals("Admin"));
        }

        public void SetAdmin(Person obj)
        {
            if (!IsAdmin) throw new SecurityException();
            obj.Role = GetRole("Admin");
        }

        public string GetCurrentUid()
        {
            return System.Threading.Thread.CurrentPrincipal.Identity.Name;
        }

        public string CreateToken(Person p)
        {
            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var expiry = Math.Round((DateTime.UtcNow.AddHours(2) - unixEpoch).TotalSeconds);
            var issuedAt = Math.Round((DateTime.UtcNow - unixEpoch).TotalSeconds);
            var notBefore = Math.Round((DateTime.UtcNow.AddMonths(6) - unixEpoch).TotalSeconds);


            var payload = new Dictionary<string, object>
            {
                {"uid", p.Uid},
                {"role", p.Role.Name },
                {"lastName", p.LastName },
                {"name", p.Name },
                {"displayName", p.Name + " " +p.LastName},
                {"nbf", notBefore},
                {"iat", issuedAt},
                {"exp", expiry}
            };

            //var secret = ConfigurationManager.AppSettings.Get("jwtKey");
            const string apikey = "secretKey";

            var token = JsonWebToken.Encode(payload, apikey, JwtHashAlgorithm.HS256);

            return token;
        }

        #endregion
    }


    

}

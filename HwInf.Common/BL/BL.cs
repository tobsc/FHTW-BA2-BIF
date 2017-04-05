using System;
using System.Collections.Generic;
using HwInf.Common.DAL;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics.CodeAnalysis;
using System.Security;
using HwInf.Common.Models;
using JWT;

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


        #region DAL


        #region Devices

        // Read
        public IQueryable<Device> GetDevices(int limit = 25, int offset = 0, bool onlyActive = true, int type = 0, bool isSearch = false)
        {
            if (type == 0)
            {
                return _dal.Devices.Include(x => x.DeviceMeta).Include(x => x.Type.FieldGroups.Select(y => y.Fields))
                    .Where(i => i.IsActive)
                    .OrderBy(i => i.Name)
                    .ThenBy(i => i.CreateDate)
                    .ThenBy(i => i.InvNum)
                    .Skip(offset)
                    .Take(limit);
            } else if(isSearch)
            {
                return _dal.Devices.Include(x => x.Type).Include(x => x.DeviceMeta).Include(x => x.Type.FieldGroups.Select(y => y.Fields))
                    .Where(i => i.IsActive)
                    .Where(i => i.Type.TypeId== type);
            } else
            {
                return _dal.Devices.Include(x => x.Type).Include(x => x.DeviceMeta).Include(x => x.Type.FieldGroups.Select(y => y.Fields))
                    .Where(i => i.IsActive)
                    .Where(i => i.Type.TypeId == type)
                    .OrderBy(i => i.Name)
                    .ThenBy(i => i.CreateDate)
                    .ThenBy(i => i.InvNum)
                    .Skip(offset)
                    .Take(limit);
            }

        }

        public Device GetSingleDevice(int deviceId)
        {
            return _dal.Devices.Include(x => x.DeviceMeta).Include(x => x.Type.FieldGroups.Select(y => y.Fields)).Single(i => i.DeviceId == deviceId);
        }
        public Device GetSingleDevice(string deviceInvNum)
        {
            return _dal.Devices
                .Include(x => x.DeviceMeta)
                .Include(x => x.Type.FieldGroups.Select(y => y.Fields))
                .OrderByDescending(i => i.CreateDate)
                .FirstOrDefault(i => i.InvNum == deviceInvNum);
        }

        public DeviceType GetDeviceType(int typeId)
        {
            return _dal.DeviceTypes.Include(x => x.FieldGroups.Select(y => y.DeviceTypes)).SingleOrDefault(i => i.TypeId == typeId);
        }

        public DeviceType GetDeviceType(string typeSlug)
        {
            return _dal.DeviceTypes.Include(x => x.FieldGroups.Select(y => y.DeviceTypes)).Single(i => i.Slug.ToLower().Equals(typeSlug.ToLower()));
        }

        public IQueryable<DeviceType> GetDeviceTypes()
        {
            return _dal.DeviceTypes.Include(x => x.FieldGroups.Select(y => y.DeviceTypes));
        }

        public IQueryable<DeviceMeta> GetDeviceMeta()
        {
            return _dal.DeviceMeta;
        }

        public DeviceStatus GetDeviceStatus(int statusId)
        {
            return _dal.DeviceStatus.Single(i => i.StatusId == statusId);
        }

        public IQueryable<DeviceStatus> GetDeviceStatus()
        {
            return _dal.DeviceStatus;
        }

        public bool DeviceExists(int deviceId)
        {
            return _dal.Devices.Count(i => i.DeviceId == deviceId) > 0;
        }

        // Create
        public Device CreateDevice()
        {
            if (!IsAdmin() && !IsVerwalter()) return null;

            var dev = new Device();
            _dal.Devices.Add(dev);
            return dev;
        }

        public DeviceMeta CreateDeviceMeta(DeviceMeta dm)
        {
            if (!IsAdmin() && !IsVerwalter()) return null;

            _dal.DeviceMeta.Add(dm);
            return dm;
        }

        public DeviceType CreateDeviceType()
        {
            if (!IsAdmin() && !IsVerwalter()) return null;

            var dt = new DeviceType();
            _dal.DeviceTypes.Add(dt);
            return dt;
        }

        public void DeleteMeta(DeviceMeta dm)
        {
            _dal.DeviceMeta.Remove(dm);
        }


        // Update
        public void UpdateDevice(Device device)
        {
            if (!IsAdmin() && !IsVerwalter()) return;

            _dal.Entry(device).State = EntityState.Modified;
        }

        public void UpdateDeviceMeta(DeviceMeta deviceMeta)
        {
            if (!IsAdmin() && !IsVerwalter()) return;

            _dal.Entry(deviceMeta).State = EntityState.Modified;
        }

        public void UpdateDeviceType(DeviceType dt)
        {
            if (!IsAdmin() && !IsVerwalter()) return;

            _dal.Entry(dt).State = EntityState.Modified;
        }

        public void DeleteDevice(int deviceId)
        {
            if (!IsAdmin() && !IsVerwalter()) return;

            var device = _dal.Devices.Find(deviceId);
            UpdateDevice(device);
            if (device != null) device.IsActive = false;
        }

        public void DeleteDeviceType(DeviceType dt)
        {
            if (!IsAdmin() && !IsVerwalter()) return;

            if (!GetDevices(0, 0, false, dt.TypeId, true).Any())
            {
                _dal.DeviceTypes.Remove(dt);

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
            return _dal.FieldGroups.Include(x => x.Fields);
        }

        public FieldGroup GetFieldGroups(string groupSlug)
        {
            if (!_dal.FieldGroups.Include(x => x.Fields).Any(i => i.Slug.Equals(groupSlug)))
            {
                return null;
            }

            return _dal.FieldGroups.Include(x => x.Fields).Single(i => i.Slug.Equals(groupSlug));
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
            if (!IsAdmin() && !IsVerwalter()) return null;

            var fg = new FieldGroup();
            _dal.FieldGroups.Add(fg);
            return fg;
        }

        public Field CreateField()
        {
            if (!IsAdmin() && !IsVerwalter()) return null;

            var obj = new Field();
            _dal.Fields.Add(obj);

            return obj;
        }

        // Update
        public void UpdateFieldGroup(FieldGroup obj)
        {
            if (!IsAdmin() && !IsVerwalter()) return;

            _dal.Entry(obj).State = EntityState.Modified;
        }

        public void DeleteField(Field field)
        {
            if (!IsAdmin() && !IsVerwalter()) return;
            _dal.Fields.Remove(field);
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
            return _dal.Roles.Single(i => i.Name.Equals(name));
        }

        // Create
        public Person CreateUser()
        {
            var obj = new Person();
            _dal.Persons.Add(obj);
            return obj;
        }

        // Update
        public void UpdateUser(Person obj)
        {
            _dal.Entry(obj).State = EntityState.Modified;
        }

        #endregion

        public void SaveChanges()
        {
            try
            {
                _dal.SaveChanges();
            }
            catch(DbEntityValidationException ex)
            {

                var a = ex.EntityValidationErrors;
            }
            
        }


        #endregion


        #region Auth

        public bool IsAdmin()
        {
            return System.Threading.Thread.CurrentPrincipal.IsInRole("Admin");
        }

        public bool IsAdmin(string uid)
        {
            return _dal.Persons.Any(i => i.Uid == uid && i.Role.Name.Equals("Admin"));
        }

        public bool IsVerwalter()
        {
            return System.Threading.Thread.CurrentPrincipal.IsInRole("Verwalter");
        }

        public bool IsVerwalter(string uid)
        {
            return _dal.Persons.Any(i => i.Uid == uid && i.Role.Name.Equals("Verwalter"));
        }

        public void SetAdmin(Person obj)
        {
            if (!IsAdmin()) return;
            obj.Role = GetRole("Admin");
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

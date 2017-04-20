using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HwInf.Common.DAL;
using HwInf.Common.Models;

namespace HwInf.Tests.DAL
{
    public class MockDAL: IDAL
    {

        private List<Device> _devices;
        private List<DeviceType> _deviceTypes;
        private List<DeviceStatus> _deviceStatus;
        private List<Person> _persons;
        private List<FieldGroup> _fieldGroups;
        private List<DeviceMeta> _deviceMeta;

        private List<Field> _prozessorenFields;
        private List<Field> _anschluesseFields;
        private List<Field> _aufloesungFields;
        private List<Field> _fields;
        private List<Role> _roles;

        private List<Order> _orders;
        private List<OrderItem> _orderItems;
        private List<OrderStatus> _orderStatus;

        private List<Setting> _settings;

        public MockDAL()
        {
            Init();
        }

        public void Init()
        {

            _orders = new List<Order>();
            _orderItems = new List<OrderItem>();
            _orderStatus = new List<OrderStatus>
            {
                new OrderStatus {Name = "Offen", Slug = "offen"},
                new OrderStatus {Name = "Abgelehnt", Slug = "abgelehnt"},
                new OrderStatus {Name = "Akzeptiert", Slug = "akzeptiert"},
            };

            _settings = new List<Setting>
            {
                new Setting { Key = "email_benachrichtigung", Value = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet."},
                new Setting { Key = "ws_anfang", Value = "1. September"},
                new Setting { Key = "ws_ende", Value = "31. Jänner"},
                new Setting { Key = "ss_anfang", Value = "1. September"},
                new Setting { Key = "ss_ende", Value = "31. Jänner"},
            };


            _deviceStatus = new List<DeviceStatus>
                {
                    new DeviceStatus { Description = "Verfügbar", StatusId = 1},
                    new DeviceStatus { Description = "Ausgeliehen", StatusId = 2},
                    new DeviceStatus { Description = "In Reparatur", StatusId = 3},
                };

            _roles = new List<Role>
                {
                    new Role { Name = "Admin" },
                    new Role { Name = "User" },
                    new Role { Name = "Verwalter" }
                };

            _persons = new List<Person>
            {
                new Person { Name = "Jan", LastName = "Calanog", Email = "jan.calanog@technikum-wien.at", Role = _roles.Single(i => i.Name == "Admin"), Uid = "if15b042" },
                new Person { Name = "Tobias", LastName = "Schlachter", Email = "tobias.schlachter@technikum-wien.at", Role = _roles.Single(i => i.Name == "User"), Uid = "if15b032" },
            };

           _deviceTypes = new List<DeviceType>
            {
                new DeviceType { Name = "Notebook", Slug = "notebook", IsActive = true},
                new DeviceType { Name = "PC", Slug = "pc", IsActive = true},
                new DeviceType { Name = "Monitor", Slug = "monitor", IsActive = true},
                new DeviceType { Name = "Festplatte", Slug = "festplatte", IsActive = true},
            };

            _anschluesseFields = new List<Field>
            {
                new Field {Slug = "hdmi", Name = "HDMI"},
                new Field {Slug = "vga", Name = "VGA"},
            };

            _prozessorenFields = new List<Field>
            {
                new Field {Slug = "intel-i7", Name = "Intel i7"},
                new Field {Slug = "intel-i5", Name = "Intel i5"},
                new Field {Slug = "amd-ryzen", Name = "AMD Ryzen"},
                new Field {Slug = "amd-athlon", Name = "AMD Athlon"},
            };


            _aufloesungFields = new List<Field>
            {
                new Field {Slug = "1366x768", Name = "1366x768"},
                new Field {Slug = "1920x1080", Name = "1920x1080"},
                new Field {Slug = "1280x1024", Name = "1280x1024"},
            };


             _fieldGroups = new List<FieldGroup>
            {
                new FieldGroup {Slug = "anschluesse", Name = "Anschlüsse", Fields = _anschluesseFields.ToList(), DeviceTypes = _deviceTypes.Where(i => i.Slug == "pc" || i.Slug == "notebook" || i.Slug == "monitor").ToList()},
                new FieldGroup {Slug = "prozessoren", Name = "Prozessoren", Fields = _prozessorenFields.ToList(), DeviceTypes = _deviceTypes.Where(i => i.Slug == "pc" || i.Slug == "notebook").ToList()},
                new FieldGroup {Slug = "aufloesung", Name = "Auflösung", Fields = _aufloesungFields.ToList(), DeviceTypes = _deviceTypes.Where(i => i.Slug == "monitor" || i.Slug == "notebook").ToList()},
            };


            var meta = new List<DeviceMeta>
            {
                new DeviceMeta
                {
                    MetaValue = "2",
                    FieldName = _fieldGroups.Select(i => i.Fields.Single(x => x.Slug == "hdmi").Name).FirstOrDefault(),
                    FieldSlug = _fieldGroups.Select(i => i.Fields.Single(x => x.Slug == "hdmi").Slug).FirstOrDefault(),
                    FieldGroupName = _fieldGroups.Single(i => i.Slug == "anschluesse").Name,
                    FieldGroupSlug = _fieldGroups.Single(i => i.Slug == "anschluesse").Slug
                },
                 new DeviceMeta
                {
                    MetaValue = "5",
                    FieldName = _fieldGroups.Select(i => i.Fields.Single(x => x.Slug == "vga").Name).FirstOrDefault(),
                    FieldSlug = _fieldGroups.Select(i => i.Fields.Single(x => x.Slug == "vga").Slug).FirstOrDefault(),
                    FieldGroupName = _fieldGroups.Single(i => i.Slug == "anschluesse").Name,
                    FieldGroupSlug = _fieldGroups.Single(i => i.Slug == "anschluesse").Slug
                },
                 new DeviceMeta
                {
                    MetaValue = "5",
                    FieldName = "Intel i5",
                    FieldSlug = "intel-i5",
                    FieldGroupName = "Prozessoren",
                    FieldGroupSlug = "prozessoren"
                }
            };

            var nmeta = new List<DeviceMeta>
            {
                new DeviceMeta
                {
                    MetaValue = "1",
                    FieldName = _fieldGroups.Select(i => i.Fields.Single(x => x.Slug == "hdmi").Name).FirstOrDefault(),
                    FieldSlug = _fieldGroups.Select(i => i.Fields.Single(x => x.Slug == "hdmi").Slug).FirstOrDefault(),
                    FieldGroupName = _fieldGroups.Single(i => i.Slug == "anschluesse").Name,
                    FieldGroupSlug = _fieldGroups.Single(i => i.Slug == "anschluesse").Slug
                },
                 new DeviceMeta
                {
                    MetaValue = "2",
                    FieldName = _fieldGroups.Select(i => i.Fields.Single(x => x.Slug == "vga").Name).FirstOrDefault(),
                    FieldSlug = _fieldGroups.Select(i => i.Fields.Single(x => x.Slug == "vga").Slug).FirstOrDefault(),
                    FieldGroupName = _fieldGroups.Single(i => i.Slug == "anschluesse").Name,
                    FieldGroupSlug = _fieldGroups.Single(i => i.Slug == "anschluesse").Slug
                }
            };

            var nbmeta = new List<DeviceMeta>
            {
                new DeviceMeta
                {
                    MetaValue = "8",
                    FieldName = _fieldGroups.Select(i => i.Fields.Single(x => x.Slug == "hdmi").Name).FirstOrDefault(),
                    FieldSlug = _fieldGroups.Select(i => i.Fields.Single(x => x.Slug == "hdmi").Slug).FirstOrDefault(),
                    FieldGroupName = _fieldGroups.Single(i => i.Slug == "anschluesse").Name,
                    FieldGroupSlug = _fieldGroups.Single(i => i.Slug == "anschluesse").Slug
                },
                new DeviceMeta
                {
                    MetaValue = "4",
                    FieldName = "VGA",
                    FieldSlug = "vga",
                    FieldGroupName = "Anschlüsse",
                    FieldGroupSlug = "anschluesse"
                },
                new DeviceMeta
                {
                    MetaValue = "5",
                    FieldName = "1366x768",
                    FieldSlug = "1366x768",
                    FieldGroupName = "aufloesung",
                    FieldGroupSlug = "Auflösung"
                }
            };

            _devices = new List<Device>
               {
                new Device { Name = "Acer PC", Brand = "Acer", Status = _deviceStatus.Single(i => i.Description == "Verfügbar"), InvNum = "a5123", Type = _deviceTypes.Single(i => i.Slug == "pc"), CreateDate = DateTime.Now, Room = "A0.00", Person = _persons.Single(i => i.LastName == "Calanog"), IsActive = true, DeviceMeta = meta.ToList(), DeviceId = 1},
                new Device { Name = "HP PC", Brand = "HP", Status = _deviceStatus.Single(i => i.Description == "Verfügbar"), InvNum = "b3434", Type = _deviceTypes.Single(i => i.Slug == "pc"), CreateDate = DateTime.Now, Room = "A0.00", Person = _persons.Single(i => i.LastName == "Calanog"), IsActive = true, DeviceMeta = meta.ToList()},
                new Device { Name = "Dell PC", Brand = "Dell", Status = _deviceStatus.Single(i => i.Description == "Verfügbar"), InvNum = "c3434", Type = _deviceTypes.Single(i => i.Slug == "pc"), CreateDate = DateTime.Now, Room = "A0.00", Person = _persons.Single(i => i.LastName == "Schlachter"), IsActive = true, DeviceMeta = meta.ToList()},

                new Device { Name = "Acer Notebook", Brand = "Acer", Status = _deviceStatus.Single(i => i.Description == "Verfügbar"), InvNum = "a51j23", Type = _deviceTypes.Single(i => i.Slug == "notebook"), CreateDate = DateTime.Now, Room = "A0.00", Person = _persons.Single(i => i.LastName == "Schlachter"), IsActive = true, DeviceMeta = nmeta.ToList()},
                new Device { Name = "HP Notebook", Brand = "HP", Status = _deviceStatus.Single(i => i.Description == "Verfügbar"), InvNum = "b343k4", Type = _deviceTypes.Single(i => i.Slug == "notebook"), CreateDate = DateTime.Now, Room = "A0.00", Person = _persons.Single(i => i.LastName == "Calanog"), IsActive = true, DeviceMeta = nmeta.ToList()},
                new Device { Name = "Dell Notebook", Brand = "Dell", Status = _deviceStatus.Single(i => i.Description == "Verfügbar"), InvNum = "c3j434", Type = _deviceTypes.Single(i => i.Slug == "notebook"), CreateDate = DateTime.Now, Room = "A0.00", Person = _persons.Single(i => i.LastName == "Schlachter"), IsActive = true, DeviceMeta = nbmeta.ToList()},

               };

            _fields = _prozessorenFields.Concat(_aufloesungFields).Concat(_anschluesseFields).ToList();
            _deviceMeta = meta.Concat(nmeta).Concat(nbmeta).ToList();
            
        }


        public IQueryable<Device> Devices => _devices.AsQueryable();
        public IQueryable<DeviceMeta> DeviceMeta => _deviceMeta.AsQueryable();
        public IQueryable<DeviceType> DeviceTypes => _deviceTypes.AsQueryable();
        public IQueryable<DeviceStatus> DeviceStatus => _deviceStatus.AsQueryable();
        public IQueryable<OrderStatus> OrderStatus => _orderStatus.AsQueryable();
        public IQueryable<Person> Persons => _persons.AsQueryable();
        public IQueryable<Role> Roles => _roles.AsQueryable();
        public IQueryable<Order> Orders => _orders.AsQueryable();
        public IQueryable<DeviceHistory> DeviceHistory { get; }
        public IQueryable<Field> Fields => _fields.AsQueryable();
        public IQueryable<FieldGroup> FieldGroups => _fieldGroups.AsQueryable();
        public IQueryable<Setting> Settings => _settings.AsQueryable();
        public IQueryable<OrderItem> OrderItems => _orderItems.AsQueryable();


        public Device CreateDevice()
        {
            var dev = new Device();
            _devices.Add(dev);
            return dev;
        }

        public DeviceType CreateDeviceType()
        {
            var dt = new DeviceType();
            _deviceTypes.Add(dt);
            return dt;
        }

        public DeviceStatus CreatDeviceStatus()
        {
            throw new System.NotImplementedException();
        }

        public OrderStatus CreateOrderStatus()
        {
            throw new System.NotImplementedException();
        }

        public Person CreatePerson()
        {
            var obj = new Person();
            _persons.Add(obj);
            return obj;
        }

        public Order CreateOrder()
        {
            var obj = new Order();
            _orders.Add(obj);
            return obj;
        }

        public DeviceHistory CReDeviceHistory()
        {
            throw new System.NotImplementedException();
        }

        public Field CreaField()
        {
            var obj = new Field();
            _fields.Add(obj);

            return obj;
        }

        public FieldGroup CreteFieldGroup()
        {
            var fg = new FieldGroup();
            _fieldGroups.Add(fg);
            return fg;
        }

        void IDAL.SaveChanges()
        {

        }

        public void DeleteDevice()
        {
            throw new System.NotImplementedException();
        }

        public void DeleteDeviceMeta(DeviceMeta dm)
        {
            _deviceMeta.Remove(dm);
        }


        public void UpdateObject(object obj)
        {
        }

        public void DeleteDeviceType(DeviceType dt)
        {
            _deviceTypes.Remove(dt);
        }

        public void DeleteField(Field f)
        {
            _fields.Remove(f);
        }

        public void Dispose()
        {
            throw new NotSupportedException();
        }

        public DeviceStatus CreateDeviceStatus()
        {
            var dt = new DeviceStatus();
            _deviceStatus.Add(dt);
            return dt;
        }

        public DeviceMeta CreateDeviceMeta()
        {
            var dm = new DeviceMeta();
            _deviceMeta.Add(dm);
            return dm;
        }
        public Setting CreateSetting()
        {
            var setting = new Setting();
            _settings.Add(setting);
            return setting;
        }
        public OrderItem CreateOrderItem()
        {
            var obj = new OrderItem();
            _orderItems.Add(obj);
            return obj;
        }

        public void DeleteFieldGroup(FieldGroup fg)
        {
            throw new NotImplementedException();
        }



        public void DeleteSetting(Setting s)
        {
            _settings.Remove(s);
        }


    }
}
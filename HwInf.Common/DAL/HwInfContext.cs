using System.CodeDom;
using HwInf.Common.Migrations;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Linq;
using HwInf.Common.Models;
using log4net;
using System.Collections.Generic;
using System;

namespace HwInf.Common.DAL
{
    public interface IDAL
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
        void Dispose();
    }


    public class HwInfContext : DbContext, IDAL
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(HwInfContext));

        public HwInfContext() : base("HwInfContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<HwInfContext, Configuration>());
        }

        public DbSet<Device> Devices { get; set; }
        public DbSet<DeviceType> DeviceTypes { get; set; }
        public DbSet<DeviceMeta> DeviceMeta { get; set; }
        public DbSet<DeviceStatus> DeviceStatus { get; set; }
        public DbSet<OrderStatus> OrderStatus { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<FieldGroup> FieldGroups { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Damage> Damages { get; set; }
        public DbSet<DamageStatus> DamageStatus { get; set; }
        public DbSet<Accessory> Accessories { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.HasDefaultSchema("public");
        }

        IQueryable<Device> IDAL.Devices => Devices
            .Include(x => x.Type)
            .Include(x => x.DeviceMeta)
            .Include(x => x.Type.FieldGroups.Select(y => y.Fields));
        IQueryable<DeviceMeta> IDAL.DeviceMeta => DeviceMeta;
        IQueryable<DeviceType> IDAL.DeviceTypes => DeviceTypes
            .Include(x => x.FieldGroups.Select(y => y.DeviceTypes))
            .Include(x => x.FieldGroups.Select(y => y.Fields));
        IQueryable<DeviceStatus> IDAL.DeviceStatus => DeviceStatus;
        IQueryable<OrderStatus> IDAL.OrderStatus => OrderStatus;
        IQueryable<Person> IDAL.Persons => Persons;
        IQueryable<Role> IDAL.Roles => Roles;

        IQueryable<Order> IDAL.Orders => Orders
            .Include(i => i.OrderItems)
            .Include(i => i.OrderItems.Select(x => x.Device))
            .Include(i => i.OrderStatus);
        IQueryable<OrderItem> IDAL.OrderItems => OrderItems;
        IQueryable<Field> IDAL.Fields => Fields;
        IQueryable<FieldGroup> IDAL.FieldGroups => FieldGroups
            .Include(x => x.Fields)
            .Include(x => x.DeviceTypes);
        IQueryable<Setting> IDAL.Settings => Settings;
        IQueryable<Damage> IDAL.Damages => Damages
            .Include(i => i.DamageStatus);
        IQueryable<DamageStatus> IDAL.DamageStatus => DamageStatus;
        IQueryable<Accessory> IDAL.Accessories => Accessories;

        public Device CreateDevice()
        {
            var dev = new Device();
            Devices.Add(dev);
            return dev;
        }

        public DeviceType CreateDeviceType()
        {
            var dt = new DeviceType();
            DeviceTypes.Add(dt);
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

        public DamageStatus CreateDamageStatus()
        {
            throw new System.NotImplementedException();
        }

        public Person CreatePerson()
        {
            var obj = new Person();
            Persons.Add(obj);
            return obj;
        }

        public Order CreateOrder()
        {
            var obj = new Order();
            Orders.Add(obj);
            return obj;
        }

        public Field CreateField()
        {
            var obj = new Field();
            Fields.Add(obj);

            return obj;
        }

        public FieldGroup CreateFieldGroup()
        {
            var fg = new FieldGroup();
            FieldGroups.Add(fg);
            return fg;
        }

        void IDAL.SaveChanges()
        {
            SaveChanges();
        }

        public void DeleteDevice()
        {
            throw new System.NotImplementedException();
        }

        public void DeleteDeviceMeta(DeviceMeta dm)
        {
            DeviceMeta.Remove(dm);
        }


        public void UpdateObject(object obj)
        {
            Entry(obj).State = EntityState.Modified;
        }

        public void DeleteDeviceType(DeviceType dt)
        {
            DeviceTypes.Remove(dt);
        }

        public void DeleteField(Field f)
        {
            Fields.Remove(f);
        }
        void IDAL.Dispose()
        {
            Dispose();
        }

        public DeviceStatus CreateDeviceStatus()
        {
            throw new System.NotImplementedException();
        }

        public DeviceMeta CreateDeviceMeta()
        {
            var dm = new DeviceMeta();
            DeviceMeta.Add(dm);
            return dm;
        }

        public void DeleteFieldGroup(FieldGroup fg)
        {
            FieldGroups.Remove(fg);
        }

        public OrderItem CreateOrderItem()
        {
            var obj = new OrderItem();
            OrderItems.Add(obj);
            return obj;
        }

        public Setting CreateSetting()
        {
            var setting = new Setting();
            Settings.Add(setting);
            return setting;
        }

        public Accessory CreateAccessory()
        {
            var accessory = new Accessory();
            Accessories.Add(accessory);
            return accessory;
        }

        public void DeleteSetting(Setting s)
        {
            Settings.Remove(s);
        }

        public Damage CreateDamage()
        {
            var damage = new Damage();
            Damages.Add(damage);
            return damage;
        }

        public void DeleteDamage(Damage d)
        {
            Damages.Remove(d);
        }

        public void DeleteAccessory(Accessory a)
        {
            Accessories.Remove(a);
        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var d in ex.EntityValidationErrors)
                {
                    Log.ErrorFormat("{0}", d.Entry.Entity.GetType().Name);
                    foreach (var msg in d.ValidationErrors)
                    {
                        Log.ErrorFormat("\t{0} : {1}", msg.PropertyName, msg.ErrorMessage);
                    }
                }

                throw;
            }
        }
    }
}
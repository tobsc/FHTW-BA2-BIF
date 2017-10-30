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
using HwInf.Common.Interfaces;

namespace HwInf.Common.DAL
{
    public class HwInfContext : DbContext, IDataAccessLayer
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

        IQueryable<Device> IDataAccessLayer.Devices => Devices
            .Include(x => x.Type)
            .Include(x => x.DeviceMeta)
            .Include(x => x.Type.FieldGroups.Select(y => y.Fields));
        IQueryable<DeviceMeta> IDataAccessLayer.DeviceMeta => DeviceMeta;
        IQueryable<DeviceType> IDataAccessLayer.DeviceTypes => DeviceTypes
            .Include(x => x.FieldGroups.Select(y => y.DeviceTypes))
            .Include(x => x.FieldGroups.Select(y => y.Fields));
        IQueryable<DeviceStatus> IDataAccessLayer.DeviceStatus => DeviceStatus;
        IQueryable<OrderStatus> IDataAccessLayer.OrderStatus => OrderStatus;
        IQueryable<Person> IDataAccessLayer.Persons => Persons;
        IQueryable<Role> IDataAccessLayer.Roles => Roles;

        IQueryable<Order> IDataAccessLayer.Orders => Orders
            .Include(i => i.OrderItems)
            .Include(i => i.OrderItems.Select(x => x.Device))
            .Include(i => i.OrderStatus);
        IQueryable<OrderItem> IDataAccessLayer.OrderItems => OrderItems;
        IQueryable<Field> IDataAccessLayer.Fields => Fields;
        IQueryable<FieldGroup> IDataAccessLayer.FieldGroups => FieldGroups
            .Include(x => x.Fields)
            .Include(x => x.DeviceTypes);
        IQueryable<Setting> IDataAccessLayer.Settings => Settings;
        IQueryable<Damage> IDataAccessLayer.Damages => Damages
            .Include(i => i.DamageStatus);
        IQueryable<DamageStatus> IDataAccessLayer.DamageStatus => DamageStatus;
        IQueryable<Accessory> IDataAccessLayer.Accessories => Accessories;

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

        void IDataAccessLayer.SaveChanges()
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
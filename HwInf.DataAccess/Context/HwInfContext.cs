using System.Linq;
using HwInf.DataAccess.Entities;
using HwInf.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HwInf.DataAccess.Context
{
    public class HwInfContext : DbContext
    {
        public HwInfContext(DbContextOptions<HwInfContext> options) : base(options)
        {
            
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


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Person>()
                .HasIndex(p => p.Uid)
                .IsUnique();

            modelBuilder.Entity<DeviceTypeFieldGroup>().HasOne(dt => dt.DeviceType)
                .WithMany(c => c.DeviceTypesFieldGroups)
                .HasForeignKey(dtfg => dtfg.DeviceTypeId);

            modelBuilder.Entity<DeviceTypeFieldGroup>().HasOne(dt => dt.FieldGroup)
                .WithMany(c => c.DeviceTypeFieldGroups)
                .HasForeignKey(dtfg => dtfg.FieldGroupId);
        }
    }
}
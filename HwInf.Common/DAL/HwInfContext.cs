using HwInf.Common.Migrations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace HwInf.Common.DAL
{

    public class HwInfContext : DbContext
    {
        public HwInfContext() : base("HwInfContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<HwInfContext, Configuration>());
        }

        public DbSet<Device> Devices { get; set; }
        public DbSet<DeviceType> DeviceTypes { get; set; }
        public DbSet<DeviceMeta> DeviceMeta { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Component> Components { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<DeviceHistory> DeviceHistory { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.HasDefaultSchema("public");
        }
    }
}
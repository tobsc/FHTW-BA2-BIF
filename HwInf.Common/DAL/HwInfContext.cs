using HwInf.Common.Migrations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace HwInf.Common.DAL
{

    public class HwInfContext : DbContext
    {
        public HwInfContext() : base("HwInfContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<HwInfContext, Configuration>("HwInfContext"));
        }

        public DbSet<DBDevice> Devices { get; set; }
        public DbSet<DBDeviceType> DeviceTypes { get; set; }
        public DbSet<DBDeviceMeta> DeviceMeta { get; set; }
        public DbSet<DBPerson> Persons { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.HasDefaultSchema("public");
        }
    }
}
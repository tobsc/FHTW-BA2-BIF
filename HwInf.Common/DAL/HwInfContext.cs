using System.CodeDom;
using HwInf.Common.Migrations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using HwInf.Common.Models;
using log4net;

namespace HwInf.Common.DAL
{

    public class HwInfContext : DbContext
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
        public DbSet<DeviceHistory> DeviceHistory { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<FieldGroup> FieldGroups { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.HasDefaultSchema("public");
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
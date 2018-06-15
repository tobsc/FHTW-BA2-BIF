using System.Linq;
using HwInf.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace HwInf.DataAccess.Context
{
    public class HwInfContext : DbContext
    {

        public HwInfContext()
        {
            
        }
        public HwInfContext(DbContextOptions options) : base(options)
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

            modelBuilder.Entity<DeviceTypeFieldGroup>()
                .HasKey(dtfg => new { dtfg.DeviceTypeId, dtfg.FieldGroupId });

            modelBuilder.Entity<DeviceTypeFieldGroup>().HasOne(dt => dt.DeviceType)
                .WithMany(c => c.DeviceTypesFieldGroups)
                .HasForeignKey(dtfg => dtfg.DeviceTypeId);

            modelBuilder.Entity<DeviceTypeFieldGroup>().HasOne(dt => dt.FieldGroup)
                .WithMany(c => c.DeviceTypeFieldGroups)
                .HasForeignKey(dtfg => dtfg.FieldGroupId);

            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {

            var deviceStatus = new[]
                {
                    new DeviceStatus { StatusId = 1, Description = "Verfügbar" },
                    new DeviceStatus { StatusId = 2, Description = "Ausgeliehen" },
                    new DeviceStatus { StatusId = 3, Description = "In Reparatur" },
                    new DeviceStatus { StatusId = 4, Description = "Archiviert" },
                    new DeviceStatus { StatusId = 5, Description = "Nicht verleihbar" },
                    new DeviceStatus { StatusId = 6, Description = "Präsentations-/Bachelorarbeitsgerät" },
                };


            var orderStatus = new[]
                {
                    new OrderStatus { StatusId = 1, Name = "Offen" , Slug = "offen"},
                    new OrderStatus { StatusId = 2, Name = "Akzeptiert" , Slug = "akzeptiert"},
                    new OrderStatus { StatusId = 3, Name = "Abgelehnt", Slug = "abgelehnt"},
                    new OrderStatus { StatusId = 4, Name = "Abgeschlossen", Slug = "abgeschlossen"},
                    new OrderStatus { StatusId = 5, Name = "Ausgeliehen", Slug = "ausgeliehen"},
                    new OrderStatus { StatusId = 6, Name = "Abgebrochen", Slug = "abgebrochen"}
                };


            var roles = new[]
                {
                    new Role { RoleId = 1, Name = "Admin" },
                    new Role { RoleId = 2, Name = "User" },
                    new Role { RoleId = 3, Name = "Verwalter" }
                };


            var persons = new[]
                {
                    new { PersId = 1, Name = "Jan", LastName = "Calanog", Email = "jan.calanog@technikum-wien.at", Uid = "if15b042", RoleId = 1},
                    new { PersId = 2, Name = "Tobias", LastName = "Schlachter", Email = "tobias.schlachter@technikum-wien.at", Uid = "if15b032" , RoleId = 1},
                    new { PersId = 3, Name = "Valentin", LastName = "Sagl", Email = "valentin.sagl@technikum-wien.at", Uid = "if15b030" , RoleId = 1},
                    new { PersId = 4, Name = "Sebastian", LastName = "Slowak", Email = "sebastian.slowak@technikum-wien.at", Uid = "if15b049" , RoleId = 1},
            };


            var settings = new[]
            {
               new Setting { Key = "ss_start", Value = "15.02"},
               new Setting { Key = "ss_end", Value = "30.06"},
               new Setting { Key = "ws_end", Value = "31.01"},
               new Setting { Key = "ws_start", Value = "25.10"},
               new Setting { Key = "reminder_mail", Value = "bitte zurückbringen"},
               new Setting { Key = "new_order_mail", Value = "Neue Anfrage zu einem ihrer Geräte"},
               new Setting { Key = "accept_mail_above", Value = "oben"},
               new Setting { Key = "accept_mail_below", Value = "unten"},
               new Setting { Key = "decline_mail_above", Value = "oben"},
               new Setting { Key = "decline_mail_below", Value = "unten"},
               new Setting { Key = "days_before_reminder", Value = "7" },
            };


            var damageStatus = new[]
            {
                new DamageStatus {StatusId = 1, Name = "Gemeldet" , Slug = "gemeldet"},
                new DamageStatus {StatusId = 2, Name = "Behoben", Slug = "behoben"},
                new DamageStatus {StatusId = 3, Name = "Dauerhaft", Slug = "dauerhaft" }
            };


            modelBuilder.Entity<Setting>().HasData(settings);
            modelBuilder.Entity<DeviceStatus>().HasData(deviceStatus);
            modelBuilder.Entity<OrderStatus>().HasData(orderStatus);
            modelBuilder.Entity<Role>().HasData(roles);
            modelBuilder.Entity<Person>().HasData(persons);
            modelBuilder.Entity<DamageStatus>().HasData(damageStatus);

        }
    }
}
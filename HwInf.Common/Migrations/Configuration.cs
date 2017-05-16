using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using HwInf.Common.Models;

namespace HwInf.Common.Migrations
{

    internal sealed class Configuration : DbMigrationsConfiguration<HwInf.Common.DAL.HwInfContext>
    {
        public Configuration()
        {
            
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(HwInf.Common.DAL.HwInfContext context)
        {

            var deviceStatus = new List<DeviceStatus>
                {
                    new DeviceStatus { Description = "Verfügbar" },
                    new DeviceStatus { Description = "Ausgeliehen" },
                    new DeviceStatus { Description = "In Reparatur" },
                    new DeviceStatus { Description = "Archiviert" },
                };

            var orderStatus = new List<OrderStatus>
                {
                    new OrderStatus { Name = "Offen" , Slug = "offen"},
                    new OrderStatus { Name = "Akzeptiert" , Slug = "akzeptiert"},
                    new OrderStatus { Name = "Abgelehnt", Slug = "abgelehnt"},
                    new OrderStatus { Name = "Abgeschlossen", Slug = "abgeschlossen"},
                    new OrderStatus { Name = "Ausgeliehen", Slug = "ausgeliehen"}
                };

            var roles = new List<Role>
                {
                    new Role { Name = "Admin" },
                    new Role { Name = "User" },
                    new Role { Name = "Verwalter" }
                };

            var persons = new List<Person>
                {
                    new Person { Name = "Jan", LastName = "Calanog", Email = "jan.calanog@technikum-wien.at", Role = roles.Single(i => i.Name == "Admin"), Uid = "if15b042" },
                    new Person { Name = "Tobias", LastName = "Schlachter", Email = "tobias.schlachter@technikum-wien.at", Role = roles.Single(i => i.Name == "Admin"), Uid = "if15b032" },
                    new Person { Name = "Valentin", LastName = "Sagl", Email = "valentin.sagl@technikum-wien.at", Role = roles.Single(i => i.Name == "Admin"), Uid = "if15b030" },
                    new Person { Name = "Sebastian", LastName = "Slowak", Email = "sebastian.slowak@technikum-wien.at", Role = roles.Single(i => i.Name == "Admin"), Uid = "if15b049" },
            };

            var settings = new List<Setting>
            {
               new Setting { Key = "ss_start", Value = "15.02"},
               new Setting { Key = "ss_end", Value = "30.06"},
               new Setting { Key = "ws_end", Value = "31.01"},
               new Setting { Key = "ws_start", Value = "25.10"},
               new Setting { Key = "reminder_mail", Value = "bitte zurückbringen"},
               new Setting { Key = "new_order_mail", Value = "Neue  Anfrage zu einem ihrer Geräte"},
               new Setting { Key = "accept_mail_above", Value = "oben"},
               new Setting { Key = "accept_mail_below", Value = "unten"},
               new Setting { Key = "decline_mail_above", Value = "oben"},
               new Setting { Key = "decline_mail_below", Value = "unten"},
               new Setting { Key = "days_before_reminder", Value = "7" },
            };

            var damageStatus = new List<DamageStatus>
            {
                new DamageStatus { Name = "Gemeldet" , Slug = "gemeldet"},
                new DamageStatus { Name = "In Bearbeitung" , Slug = "in-bearbeitung"},
                new DamageStatus { Name = "Behoben", Slug = "behoben"}
            };

            if (!context.Settings.Any())
            {
                context.Settings.AddRange(settings);
            }


            if (!context.DeviceStatus.Any())
            {
                context.DeviceStatus.AddRange(deviceStatus);
            }

            if (!context.OrderStatus.Any())
            {
                context.OrderStatus.AddRange(orderStatus);
            }

            if (!context.Roles.Any())
            {
                context.Roles.AddRange(roles);
            }

            if (!context.Persons.Any())
            {
                context.Persons.AddRange(persons);
            }

            if (!context.DamageStatus.Any())
            {
                context.DamageStatus.AddRange(damageStatus);
            }

            base.Seed(context);
        }
    }
}

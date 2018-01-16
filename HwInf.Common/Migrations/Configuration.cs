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
            
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(HwInf.Common.DAL.HwInfContext context)
        {

            var deviceStatus = new []
                {
                    new DeviceStatus { Description = "Verfügbar" },
                    new DeviceStatus { Description = "Ausgeliehen" },
                    new DeviceStatus { Description = "In Reparatur" },
                    new DeviceStatus { Description = "Archiviert" },
                    new DeviceStatus { Description = "Nicht verleihbar" },
                    new DeviceStatus { Description = "Präsentations-/Bachelorarbeitsgerät" },
                };


            var orderStatus = new []
                {
                    new OrderStatus { Name = "Offen" , Slug = "offen"},
                    new OrderStatus { Name = "Akzeptiert" , Slug = "akzeptiert"},
                    new OrderStatus { Name = "Abgelehnt", Slug = "abgelehnt"},
                    new OrderStatus { Name = "Abgeschlossen", Slug = "abgeschlossen"},
                    new OrderStatus { Name = "Ausgeliehen", Slug = "ausgeliehen"},
                    new OrderStatus { Name = "Abgebrochen", Slug = "abgebrochen"}
                };


            var roles = new []
                {
                    new Role { Name = "Admin" },
                    new Role { Name = "User" },
                    new Role { Name = "Verwalter" }
                };


            var persons = new []
                {
                    new Person { Name = "Jan", LastName = "Calanog", Email = "jan.calanog@technikum-wien.at", Role = roles.Single(i => i.Name == "Admin"), Uid = "if15b042" },
                    new Person { Name = "Tobias", LastName = "Schlachter", Email = "tobias.schlachter@technikum-wien.at", Role = roles.Single(i => i.Name == "Admin"), Uid = "if15b032" },
                    new Person { Name = "Valentin", LastName = "Sagl", Email = "valentin.sagl@technikum-wien.at", Role = roles.Single(i => i.Name == "Admin"), Uid = "if15b030" },
                    new Person { Name = "Sebastian", LastName = "Slowak", Email = "sebastian.slowak@technikum-wien.at", Role = roles.Single(i => i.Name == "Admin"), Uid = "if15b049" },
            };


            var settings = new []
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


            var damageStatus = new []
            {
                new DamageStatus { Name = "Gemeldet" , Slug = "gemeldet"},
                new DamageStatus { Name = "Behoben", Slug = "behoben"},
                new DamageStatus { Name = "Dauerhaft", Slug = "dauerhaft" }
            };


            context.Settings.AddOrUpdate(a => a.Key, settings);
            context.DeviceStatus.AddOrUpdate(a => a.Description, deviceStatus);
            context.OrderStatus.AddOrUpdate(a => a.Slug, orderStatus);
            context.Roles.AddOrUpdate(a => a.Name, roles);
            context.Persons.AddOrUpdate(a => a.Uid, persons);
            context.DamageStatus.AddOrUpdate(a => a.Slug, damageStatus);



            base.Seed(context);
        }
    }
}

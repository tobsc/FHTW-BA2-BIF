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

           var type = new List<DeviceType>
                {
                    new DeviceType { Name = "Notebook", Slug = "notebook"},
                    new DeviceType { Name = "PC", Slug = "pc" },
                    new DeviceType { Name = "Monitor", Slug = "monitor"}
                };

            var fields = new List<Field>
            {
                new Field {Slug = "hdmi", Name = "HDMI"},
                new Field {Slug = "vga", Name = "VGA"},
            };

            var fieldGroup = new List<FieldGroup>
            {
                new FieldGroup {Slug = "anschluesse", Name = "Anschlüsse", Fields = fields.ToList(), DeviceTypes = type.Where(i => i.Slug == "pc").ToList()}
            };


            var deviceStatus = new List<DeviceStatus>
                {
                    new DeviceStatus { Description = "Verfügbar" },
                    new DeviceStatus { Description = "Ausgeliehen" },
                    new DeviceStatus { Description = "In Reparatur" },
                };

            var orderStatus = new List<OrderStatus>
                {
                    new OrderStatus { Description = "Offen" },
                    new OrderStatus { Description = "Akzeptiert" },
                    new OrderStatus { Description = "Abgelehnt" },
                    new OrderStatus { Description = "Abgeschlossen" }
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

            var meta = new List<DeviceMeta>
            {
                new DeviceMeta
                {
                    MetaValue = "2",
                    FieldName = fieldGroup.Select(i => i.Fields.Single(x => x.Slug == "hdmi").Name).FirstOrDefault(),
                    FieldSlug = fieldGroup.Select(i => i.Fields.Single(x => x.Slug == "hdmi").Slug).FirstOrDefault(),
                    FieldGroupName = fieldGroup.Single(i => i.Slug == "anschluesse").Name,
                    FieldGroupSlug = fieldGroup.Single(i => i.Slug == "anschluesse").Slug
                },
                                new DeviceMeta
                {
                    MetaValue = "5",
                    FieldName = fieldGroup.Select(i => i.Fields.Single(x => x.Slug == "vga").Name).FirstOrDefault(),
                    FieldSlug = fieldGroup.Select(i => i.Fields.Single(x => x.Slug == "vga").Slug).FirstOrDefault(),
                    FieldGroupName = fieldGroup.Single(i => i.Slug == "anschluesse").Name,
                    FieldGroupSlug = fieldGroup.Single(i => i.Slug == "anschluesse").Slug
                }
            };

            var dev = new List<Device>
               {
                new Device { Name = "Acer PC", Brand = "Acer", Status = deviceStatus.Single(i => i.Description == "Verfügbar"), InvNum = "a5123", Type = type.Single(i => i.Slug == "pc"), CreateDate = DateTime.Now, Room = "A0.00", Person = persons.Single(i => i.LastName == "Calanog"), IsActive = true, DeviceMeta = meta.ToList()},
               };


            if (context.DeviceTypes.Count() < 1)
            {
                type.ForEach(s => context.DeviceTypes.Add(s));
            }

            if (context.DeviceStatus.Count() < 1)
            {
                deviceStatus.ForEach(s => context.DeviceStatus.Add(s));
            }

            if (context.OrderStatus.Count() < 1)
            {
                orderStatus.ForEach(s => context.OrderStatus.Add(s));
            }

            if (context.Roles.Count() < 1)
            {
                roles.ForEach(s => context.Roles.Add(s));
            }

            if (context.Persons.Count() < 1)
            {
                persons.ForEach(s => context.Persons.Add(s));
            }

            if (context.Devices.Count() < 1)
            {
                dev.ForEach(s => context.Devices.Add(s));
            }



            base.Seed(context);
        }
    }
}

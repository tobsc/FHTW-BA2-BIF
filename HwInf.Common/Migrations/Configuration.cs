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
                new DeviceType { Name = "Notebook", Slug = "notebook", IsActive = true},
                new DeviceType { Name = "PC", Slug = "pc", IsActive = true },
                new DeviceType { Name = "Monitor", Slug = "monitor", IsActive = true},
                new DeviceType { Name = "Festplatte", Slug = "festplatte", IsActive = true},
            };

            var anschluessefields = new List<Field>
            {
                new Field {Slug = "hdmi", Name = "HDMI"},
                new Field {Slug = "vga", Name = "VGA"},
            };

            var prozessorfields = new List<Field>
            {
                new Field {Slug = "intel-i7", Name = "Intel i7"},
                new Field {Slug = "intel-i5", Name = "Intel i5"},
                new Field {Slug = "amd-ryzen", Name = "AMD Ryzen"},
                new Field {Slug = "amd-athlon", Name = "AMD Athlon"},
            };

            var monitorfields = new List<Field>
            {
                new Field {Slug = "13-zoll", Name = "13 Zoll"},
                new Field {Slug = "15-zoll", Name = "15 Zoll"},
                new Field {Slug = "17-zoll", Name = "17 Zoll"},
            };

            var aufloesungfields = new List<Field>
            {
                new Field {Slug = "1366x768", Name = "1366x768"},
                new Field {Slug = "1920x1080", Name = "1920x1080"},
                new Field {Slug = "1280x1024", Name = "1280x1024"},
            };

            var festplattefields = new List<Field>
            {
                new Field {Slug = "128-gb", Name = "128 GB"},
                new Field {Slug = "256-gb", Name = "256 GB"},
                new Field {Slug = "2-tb", Name = "2 TB"},
            };

            var fieldGroup = new List<FieldGroup>
            {
                new FieldGroup {Slug = "anschluesse", Name = "Anschlüsse", Fields = anschluessefields.ToList(), DeviceTypes = type.Where(i => i.Slug == "pc" || i.Slug == "notebook" || i.Slug == "monitor").ToList()},
                new FieldGroup {Slug = "prozessoren", Name = "Prozessoren", Fields = prozessorfields.ToList(), DeviceTypes = type.Where(i => i.Slug == "pc" || i.Slug == "notebook").ToList()},
                new FieldGroup {Slug = "bilschirmdiagonale", Name = "Bildschirmdiagonale", Fields = monitorfields.ToList(), DeviceTypes = type.Where(i => i.Slug == "monitor" || i.Slug == "notebook").ToList()},
                new FieldGroup {Slug = "aufloesung", Name = "Auflösung", Fields = aufloesungfields.ToList(), DeviceTypes = type.Where(i => i.Slug == "monitor" || i.Slug == "notebook").ToList()},
                new FieldGroup {Slug = "kapazitaet", Name = "Kapazität", Fields = festplattefields.ToList(), DeviceTypes = type.Where(i => i.Slug == "festplatte").ToList()}
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

            if (context.FieldGroups.Count() < 1)
            {
                fieldGroup.ForEach(s => context.FieldGroups.Add(s));
            }

            if (context.Devices.Count() < 1)
            {
                dev.ForEach(s => context.Devices.Add(s));
            }



            base.Seed(context);
        }
    }
}

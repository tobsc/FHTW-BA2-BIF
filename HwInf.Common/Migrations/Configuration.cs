namespace HwInf.Common.Migrations
{
    using DAL;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

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
                    new DeviceType { Description = "Notebook" },
                    new DeviceType { Description = "PC" },
                    new DeviceType { Description = "Monitor" }
                };

            var deviceStatus = new List<Status>
                {
                    new Status { Description = "Verfügbar" },
                    new Status { Description = "Ausgeliehen" },
                    new Status { Description = "In Reparatur" },
                };

            var orderStatus = new List<Status>
                {
                    new Status { Description = "Offen" },
                    new Status { Description = "Akzeptiert" },
                    new Status { Description = "Abgelehnt" },
                    new DAL.Status { Description = "Abgeschlossen" }
                };

            var roles = new List<Role>
                {
                    new Role { Name = "Admin" },
                    new Role { Name = "User" },
                    new Role { Name = "Owner" }
                };

            var persons = new List<Person>
                {
                    new Person { Name = "Jan", LastName = "Calanog", Email = "jan.calanog.technikum-wien.at", Role = roles.Single(i => i.Name == "Admin"), uid = "if15b042" },
                    new Person { Name = "Tobias", LastName = "Schlachter", Email = "tobias.schlachter@technikum-wien.at", Role = roles.Single(i => i.Name == "Admin"), uid = "if15b032" },
                    new Person { Name = "Valentin", LastName = "Sagl", Email = "valentin.sagl@technikum-wien.at", Role = roles.Single(i => i.Name == "Admin"), uid = "if15b030" },
                    new Person { Name = "Sebastian", LastName = "Slowak", Email = "sebastian.slowak@technikum-wien.at", Role = roles.Single(i => i.Name == "Admin"), uid = "if15b049" }
                };

            var dev = new List<Device>
               {
                new Device { Name = "Lenovo Notebook", Brand = "Lenovo", Status = deviceStatus.Single(i => i.Description == "Verfügbar"), InvNum = "a123", Type = type.Single(i => i.Description == "Notebook"), CreateDate = DateTime.Now, Room ="A0.00", Person = persons.Single(i => i.LastName == "Calanog")},
                new Device { Name = "Acer PC", Brand = "Acer", Status = deviceStatus.Single(i => i.Description == "Ausgeliehen"), InvNum = "a5123", Type = type.Single(i => i.Description == "PC"), CreateDate = DateTime.Now, Room = "A0.00", Person = persons.Single(i => i.LastName == "Calanog")},
                new Device { Name = "Benq Monitor", Brand = "Benq", Status = deviceStatus.Single(i => i.Description == "Verfügbar"), InvNum = "a6123", Type = type.Single(i => i.Description == "Monitor"), CreateDate = DateTime.Now, Room = "F0.00", Person = persons.Single(i => i.LastName == "Calanog")},
                new Device { Name = "Medion PC", Brand = "Medion", Status = deviceStatus.Single(i => i.Description == "In Reparatur"), InvNum = "a57123", Type = type.Single(i => i.Description == "PC"), CreateDate = DateTime.Now, Room = "F0.00", Person = persons.Single(i => i.LastName == "Calanog")},
                new Device { Name = "HP PC", Brand = "HP", Status = deviceStatus.Single(i => i.Description == "Verfügbar"), InvNum = "a985123", Type = type.Single(i => i.Description == "PC"), CreateDate = DateTime.Now, Room = "B0.00", Person = persons.Single(i => i.LastName == "Sagl")},
                new Device { Name = "Acer PC", Brand = "Acer", Status = deviceStatus.Single(i => i.Description == "Ausgeliehen"), InvNum = "a512683", Type = type.Single(i => i.Description == "PC"), CreateDate = DateTime.Now, Room = "B0.00", Person = persons.Single(i => i.LastName == "Sagl")}
               };


            var devMeta = new List<DeviceMeta>
            {
                new DeviceMeta { Device = dev.Single(i => i.InvNum == "a123"), DeviceType = type.Single(i => i.Description == "Notebook"), MetaKey = "Prozessor", MetaValue = "Intel Core i7-6500U" },
                new DeviceMeta { Device= dev.Single(i => i.InvNum == "a123"), DeviceType = type.Single(i => i.Description == "Notebook"), MetaKey = "Bildschirmdiagonale", MetaValue = "13 Zoll" },
                new DeviceMeta { Device= dev.Single(i => i.InvNum == "a5123"), DeviceType = type.Single(i => i.Description == "PC"), MetaKey = "Prozessor", MetaValue = "Intel Core i5-3550" },
                new DeviceMeta { Device= dev.Single(i => i.InvNum == "a6123"), DeviceType = type.Single(i => i.Description == "Monitor"), MetaKey = "Farbe", MetaValue = "Schwarz" },

                new DeviceMeta { Device = dev.Single(i => i.InvNum == "a57123"), DeviceType = type.Single(i => i.Description == "PC"), MetaKey = "Prozessor", MetaValue = "Intel Core i7-6500" },
                new DeviceMeta { Device = dev.Single(i => i.InvNum == "a57123"), DeviceType = type.Single(i => i.Description == "PC"), MetaKey = "Arbeitsspeicher", MetaValue = "16GB" },
                new DeviceMeta { Device = dev.Single(i => i.InvNum == "a57123"), DeviceType = type.Single(i => i.Description == "PC"), MetaKey = "Netzteil", MetaValue = "800W" },
                new DeviceMeta { Device = dev.Single(i => i.InvNum == "a985123"), DeviceType = type.Single(i => i.Description == "PC"), MetaKey = "Prozessor", MetaValue = "Intel Core i3-1234" },
                new DeviceMeta { Device = dev.Single(i => i.InvNum == "a985123"), DeviceType = type.Single(i => i.Description == "PC"), MetaKey = "Arbeitsspeicher", MetaValue = "8GB" },
                new DeviceMeta { Device = dev.Single(i => i.InvNum == "a512683"), DeviceType = type.Single(i => i.Description == "PC"), MetaKey = "Prozessor", MetaValue = "AMD FX-1234" },
                new DeviceMeta { Device = dev.Single(i => i.InvNum == "a512683"), DeviceType = type.Single(i => i.Description == "PC"), MetaKey = "Arbeitsspeicher", MetaValue = "32GB" },
                new DeviceMeta { Device = dev.Single(i => i.InvNum == "a512683"), DeviceType = type.Single(i => i.Description == "PC"), MetaKey = "Netzteil", MetaValue = "1400W" },
                new DeviceMeta { Device = dev.Single(i => i.InvNum == "a512683"), DeviceType = type.Single(i => i.Description == "PC"), MetaKey = "Grafikkarte", MetaValue = "Nvidia Geforce GTX-1080" }
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
                orderStatus.ForEach(s => context.DeviceStatus.Add(s));
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

            if (context.OrderStatus.Count() < 1)
            {
                devMeta.ForEach(s => context.DeviceMeta.Add(s));
            }

            context.SaveChanges();

            base.Seed(context);
        }
    }
}

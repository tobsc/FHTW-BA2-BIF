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
           
            var count = context.Devices.Count();

            if(count > 0)
            {
                return;
            }


            var type = new List<DeviceType>
            {
                new DeviceType { Description = "Notebook" },
                new DeviceType { Description = "PC" },
                new DeviceType { Description = "Monitor" }

            };

            type.ForEach(s => context.DeviceTypes.Add(s));
            context.SaveChanges();

            var status = new List<Status>
            {
                new Status { Description = "Verfügbar" },
                new Status { Description = "Ausgeliehen" },
                new Status { Description = "In Reparatur" },
                new Status { Description = "Offen" },
                new Status { Description = "Akzeptiert" },
                new Status { Description = "Abgelehnt" },
                new DAL.Status { Description = "Abgeschlossen" }
            };

            status.ForEach(s => context.Status.Add(s));

            var rooms = new List<Room>
            {
                new Room { Name = "A0.00" },
                new Room { Name = "F0.00" },
                new Room { Name = "B0.00" }
            };

            rooms.ForEach(s => context.Rooms.Add(s));

            var roles = new List<Role>
            {
                new Role { Name = "Admin" },
                new Role { Name = "User" },
                new Role { Name = "Owner" }
            };

            roles.ForEach(s => context.Roles.Add(s));

            var persons = new List<Person>
            {
                new Person { Name = "Jan", LastName = "Calanog", Email = "jan.calanog.technikum-wien.at", Role = roles.Single(i => i.Name == "Admin"), uid = "if15b042" },
                new Person { Name = "Tobias", LastName = "Schlachter", Email = "tobias.schlachter@technikum-wien.at", Role = roles.Single(i => i.Name == "Admin"), uid = "if15b032" },
                new Person { Name = "Valentin", LastName = "Sagl", Email = "valentin.sagl@technikum-wien.at", Role = roles.Single(i => i.Name == "Admin"), uid = "if15b030" },
                new Person { Name = "Sebastian", LastName = "Slowak", Email = "sebastian.slowak@technikum-wien.at", Role = roles.Single(i => i.Name == "Admin"), uid = "if15b049" }
            };


            persons.ForEach(s => context.Persons.Add(s));


            var dev = new List<Device>
           {
            new Device { Name = "Lenovo Notebook", Brand = "Lenovo", Status = status.Single(i => i.Description == "Verfügbar"), InvNum = "a123", Type = type.Single(i => i.Description == "Notebook"), CreateDate = DateTime.Now, Room = rooms.Single(i => i.Name == "A0.00"), Person = persons.Single(i => i.LastName == "Calanog")},
            new Device { Name = "Acer PC", Brand = "Acer", Status = status.Single(i => i.Description == "Ausgeliehen"), InvNum = "a5123", Type = type.Single(i => i.Description == "PC"), CreateDate = DateTime.Now, Room = rooms.Single(i => i.Name == "A0.00"), Person = persons.Single(i => i.LastName == "Calanog")},
            new Device { Name = "Benq Monitor", Brand = "Benq", Status = status.Single(i => i.Description == "Verfügbar"), InvNum = "a6123", Type = type.Single(i => i.Description == "Monitor"), CreateDate = DateTime.Now, Room = rooms.Single(i => i.Name == "F0.00"), Person = persons.Single(i => i.LastName == "Calanog")},
            new Device { Name = "Medion PC", Brand = "Medion", Status = status.Single(i => i.Description == "In Reparatur"), InvNum = "a57123", Type = type.Single(i => i.Description == "PC"), CreateDate = DateTime.Now, Room = rooms.Single(i => i.Name == "F0.00"), Person = persons.Single(i => i.LastName == "Calanog")},
            new Device { Name = "HP PC", Brand = "HP", Status = status.Single(i => i.Description == "Verfügbar"), InvNum = "a985123", Type = type.Single(i => i.Description == "PC"), CreateDate = DateTime.Now, Room = rooms.Single(i => i.Name == "B0.00"), Person = persons.Single(i => i.LastName == "Sagl")},
            new Device { Name = "Acer PC", Brand = "Acer", Status = status.Single(i => i.Description == "Ausgeliehen"), InvNum = "a512683", Type = type.Single(i => i.Description == "PC"), CreateDate = DateTime.Now, Room = rooms.Single(i => i.Name == "B0.00"), Person = persons.Single(i => i.LastName == "Sagl")}
           };

            dev.ForEach(s => context.Devices.Add(s));
            context.SaveChanges();


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

            devMeta.ForEach(s => context.DeviceMeta.Add(s));
            context.SaveChanges();

            base.Seed(context);
        }
    }
}

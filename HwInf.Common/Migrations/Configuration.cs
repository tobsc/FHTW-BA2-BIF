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
            /**
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
                new Status { Description = "In Reparatur" }
            };

            var dev = new List<Device>
           {
            new Device { Description = "Lenovo Notebook", Brand = "Lenovo", Status = status.Single(i => i.Description == "Verfügbar"), InvNum = "a123", Type = type.Single(i => i.Description == "Notebook")},
            new Device { Description = "Acer PC", Brand = "Acer", Status = status.Single(i => i.Description == "Ausgeliehen"), InvNum = "a5123", Type = type.Single(i => i.Description == "PC")},
            new Device { Description = "Benq Monitor", Brand = "Benq", Status = status.Single(i => i.Description == "Verfügbar"), InvNum = "a6123", Type = type.Single(i => i.Description == "Monitor")},
            new Device { Description = "Medion PC", Brand = "Medion", Status = status.Single(i => i.Description == "In Reparatur"), InvNum = "a57123", Type = type.Single(i => i.Description == "PC")},
            new Device { Description = "HP PC", Brand = "HP", Status = status.Single(i => i.Description == "Verfügbar"), InvNum = "a985123", Type = type.Single(i => i.Description == "PC")},
            new Device { Description = "Acer PC", Brand = "Acer", Status = status.Single(i => i.Description == "Ausgeliehen"), InvNum = "a512683", Type = type.Single(i => i.Description == "PC")}
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
    **/
            base.Seed(context);
        }
    }
}

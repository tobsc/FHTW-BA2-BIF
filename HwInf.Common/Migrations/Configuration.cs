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


            var type = new List<DBDeviceType>
            {
                new DBDeviceType { Name = "Notebook" },
                new DBDeviceType { Name = "PC" },
                new DBDeviceType { Name = "Monitor" }

            };

            type.ForEach(s => context.DeviceTypes.Add(s));
            context.SaveChanges();

            var dev = new List<DBDevice>
           {
            new DBDevice { Name = "Lenovo Notebook", Brand = "Lenovo", Status = 0, InvNum = "a123", Type = type.Single(i => i.Name == "Notebook")},
            new DBDevice { Name = "Acer PC", Brand = "Acer", Status = 0 , InvNum = "a5123", Type = type.Single(i => i.Name == "PC")},
            new DBDevice { Name = "Benq Monitor", Brand = "Benq", Status = 0, InvNum = "a6123", Type = type.Single(i => i.Name == "Monitor")},
            new DBDevice { Name = "Medion PC", Brand = "Medion", Status = 0 , InvNum = "a57123", Type = type.Single(i => i.Name == "PC")},
            new DBDevice { Name = "HP PC", Brand = "HP", Status = 0 , InvNum = "a985123", Type = type.Single(i => i.Name == "PC")},
            new DBDevice { Name = "Acer PC", Brand = "Acer", Status = 0 , InvNum = "a512683", Type = type.Single(i => i.Name == "PC")}
           };

            dev.ForEach(s => context.Devices.Add(s));
            context.SaveChanges();


            var devMeta = new List<DBDeviceMeta>
            {
                new DBDeviceMeta { Device = dev.Single(i => i.InvNum == "a123"), DeviceType = type.Single(i => i.Name == "Notebook"), MetaKey = "Prozessor", MetaValue = "Intel Core i7-6500U" },
                new DBDeviceMeta { Device= dev.Single(i => i.InvNum == "a123"), DeviceType = type.Single(i => i.Name == "Notebook"), MetaKey = "Bildschirmdiagonale", MetaValue = "13 Zoll" },
                new DBDeviceMeta { Device= dev.Single(i => i.InvNum == "a5123"), DeviceType = type.Single(i => i.Name == "PC"), MetaKey = "Prozessor", MetaValue = "Intel Core i5-3550" },
                new DBDeviceMeta { Device= dev.Single(i => i.InvNum == "a6123"), DeviceType = type.Single(i => i.Name == "Monitor"), MetaKey = "Farbe", MetaValue = "Schwarz" },

                new DBDeviceMeta { Device = dev.Single(i => i.InvNum == "a57123"), DeviceType = type.Single(i => i.Name == "PC"), MetaKey = "Prozessor", MetaValue = "Intel Core i7-6500" },
                new DBDeviceMeta { Device = dev.Single(i => i.InvNum == "a57123"), DeviceType = type.Single(i => i.Name == "PC"), MetaKey = "Arbeitsspeicher", MetaValue = "16GB" },
                new DBDeviceMeta { Device = dev.Single(i => i.InvNum == "a57123"), DeviceType = type.Single(i => i.Name == "PC"), MetaKey = "Netzteil", MetaValue = "800W" },
                new DBDeviceMeta { Device = dev.Single(i => i.InvNum == "a985123"), DeviceType = type.Single(i => i.Name == "PC"), MetaKey = "Prozessor", MetaValue = "Intel Core i3-1234" },
                new DBDeviceMeta { Device = dev.Single(i => i.InvNum == "a985123"), DeviceType = type.Single(i => i.Name == "PC"), MetaKey = "Arbeitsspeicher", MetaValue = "8GB" },
                new DBDeviceMeta { Device = dev.Single(i => i.InvNum == "a512683"), DeviceType = type.Single(i => i.Name == "PC"), MetaKey = "Prozessor", MetaValue = "AMD FX-1234" },
                new DBDeviceMeta { Device = dev.Single(i => i.InvNum == "a512683"), DeviceType = type.Single(i => i.Name == "PC"), MetaKey = "Arbeitsspeicher", MetaValue = "32GB" },
                new DBDeviceMeta { Device = dev.Single(i => i.InvNum == "a512683"), DeviceType = type.Single(i => i.Name == "PC"), MetaKey = "Netzteil", MetaValue = "1400W" },
                new DBDeviceMeta { Device = dev.Single(i => i.InvNum == "a512683"), DeviceType = type.Single(i => i.Name == "PC"), MetaKey = "Grafikkarte", MetaValue = "Nvidia Geforce GTX-1080" }
            };

            devMeta.ForEach(s => context.DeviceMeta.Add(s));
            context.SaveChanges();
            base.Seed(context);
        }
    }
}

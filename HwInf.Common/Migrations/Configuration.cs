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

            var typeFields = new List<Component>
                {
                    new Component { FieldType = "text", Name = "Prozessor", DeviceType = type.Single(i => i.Description == "PC") },
                    new Component { FieldType = "text", Name = "Arbeitsspeicher", DeviceType = type.Single(i => i.Description == "PC") },
                    new Component { FieldType = "text", Name = "Grafikkarte", DeviceType = type.Single(i => i.Description == "PC") },
                    new Component { FieldType = "text", Name = "Festplatte", DeviceType = type.Single(i => i.Description == "PC") },
                    new Component { FieldType = "text", Name = "DVD-Laufwerk", DeviceType = type.Single(i => i.Description == "PC") },

                    new Component { FieldType = "text", Name = "Prozessor", DeviceType = type.Single(i => i.Description == "Notebook") },
                    new Component { FieldType = "text", Name = "Arbeitsspeicher", DeviceType = type.Single(i => i.Description == "Notebook") },
                    new Component { FieldType = "text", Name = "Grafikkarte", DeviceType = type.Single(i => i.Description == "Notebook") },
                    new Component { FieldType = "text", Name = "Festplatte", DeviceType = type.Single(i => i.Description == "Notebook") },
                    new Component { FieldType = "text", Name = "Display", DeviceType = type.Single(i => i.Description == "Notebook") },
                    new Component { FieldType = "text", Name = "DVD-Laufwerk", DeviceType = type.Single(i => i.Description == "Notebook") },

                    new Component { FieldType = "text", Name = "Bildschirmdiagonale", DeviceType = type.Single(i => i.Description == "Monitor") },
                    new Component { FieldType = "text", Name = "Anschlüsse", DeviceType = type.Single(i => i.Description == "Monitor") }

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
                    new Role { Name = "Owner" }
                };

            var persons = new List<Person>
                {
                    new Person { Name = "Jan", LastName = "Calanog", Email = "jan.calanog@technikum-wien.at", Role = roles.Single(i => i.Name == "Admin"), uid = "if15b042" },
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
                new DeviceMeta { Device = dev.Single(i => i.InvNum == "a123"), Component = typeFields.Single(i => i.DeviceType.Description == "Notebook" && i.Name == "Prozessor"), MetaValue = "Intel Core i7-6500U" },
                new DeviceMeta { Device= dev.Single(i => i.InvNum == "a123"), Component = typeFields.Single(i => i.DeviceType.Description == "Notebook" && i.Name == "Display"), MetaValue = "13 Zoll" },
                new DeviceMeta { Device= dev.Single(i => i.InvNum == "a5123"), Component = typeFields.Single(i => i.DeviceType.Description == "PC" && i.Name == "Prozessor"), MetaValue = "Intel Core i5-3550" },
                new DeviceMeta { Device= dev.Single(i => i.InvNum == "a6123"), Component = typeFields.Single(i => i.DeviceType.Description == "Monitor" && i.Name == "Bildschirmdiagonale"), MetaValue = "17 Zoll" },
                new DeviceMeta { Device= dev.Single(i => i.InvNum == "a6123"), Component = typeFields.Single(i => i.DeviceType.Description == "Monitor" && i.Name == "Anschlüsse"), MetaValue = "HDMI, VGA, DVI" },

                new DeviceMeta { Device = dev.Single(i => i.InvNum == "a57123"), Component = typeFields.Single(i => i.DeviceType.Description == "PC" && i.Name == "Prozessor"),MetaValue = "Intel Core i7-6500" },
                new DeviceMeta { Device = dev.Single(i => i.InvNum == "a57123"), Component = typeFields.Single(i => i.DeviceType.Description == "PC" && i.Name == "Arbeitsspeicher"), MetaValue = "16GB" },
                new DeviceMeta { Device = dev.Single(i => i.InvNum == "a57123"), Component = typeFields.Single(i => i.DeviceType.Description == "PC" && i.Name == "Festplatte"), MetaValue = "120GB SSD, 1TB HDD" },
                new DeviceMeta { Device = dev.Single(i => i.InvNum == "a57123"), Component = typeFields.Single(i => i.DeviceType.Description == "PC" && i.Name == "Grafikkarte"), MetaValue = "Nvidia Geforce GTX-1060" },
                new DeviceMeta { Device = dev.Single(i => i.InvNum == "a57123"), Component = typeFields.Single(i => i.DeviceType.Description == "PC" && i.Name == "DVD-Laufwerk"), MetaValue = "-" },

                new DeviceMeta { Device = dev.Single(i => i.InvNum == "a985123"), Component = typeFields.Single(i => i.DeviceType.Description == "PC" && i.Name == "Prozessor"),MetaValue = "Intel Core i5-6500" },
                new DeviceMeta { Device = dev.Single(i => i.InvNum == "a985123"), Component = typeFields.Single(i => i.DeviceType.Description == "PC" && i.Name == "Arbeitsspeicher"), MetaValue = "8GB" },
                new DeviceMeta { Device = dev.Single(i => i.InvNum == "a985123"), Component = typeFields.Single(i => i.DeviceType.Description == "PC" && i.Name == "Festplatte"), MetaValue = "120GB SSD, 500GB HDD" },
                new DeviceMeta { Device = dev.Single(i => i.InvNum == "a985123"), Component = typeFields.Single(i => i.DeviceType.Description == "PC" && i.Name == "Grafikkarte"), MetaValue = "Nvidia Geforce GTX-1070" },
                new DeviceMeta { Device = dev.Single(i => i.InvNum == "a985123"), Component = typeFields.Single(i => i.DeviceType.Description == "PC" && i.Name == "DVD-Laufwerk"), MetaValue = "-" },

                new DeviceMeta { Device = dev.Single(i => i.InvNum == "a512683"), Component = typeFields.Single(i => i.DeviceType.Description == "PC" && i.Name == "Prozessor"),MetaValue = "Intel Core i3-1234" },
                new DeviceMeta { Device = dev.Single(i => i.InvNum == "a512683"), Component = typeFields.Single(i => i.DeviceType.Description == "PC" && i.Name == "Arbeitsspeicher"), MetaValue = "8GB" },
                new DeviceMeta { Device = dev.Single(i => i.InvNum == "a512683"), Component = typeFields.Single(i => i.DeviceType.Description == "PC" && i.Name == "Festplatte"), MetaValue = "250GB SSD, TBGB HDD" },
                new DeviceMeta { Device = dev.Single(i => i.InvNum == "a512683"), Component = typeFields.Single(i => i.DeviceType.Description == "PC" && i.Name == "Grafikkarte"), MetaValue = "Nvidia Geforce GTX-1080" },
                new DeviceMeta { Device = dev.Single(i => i.InvNum == "a512683"), Component = typeFields.Single(i => i.DeviceType.Description == "PC" && i.Name == "DVD-Laufwerk"), MetaValue = "-" },

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

            if (context.DeviceMeta.Count() < 1)
            {
                devMeta.ForEach(s => context.DeviceMeta.Add(s));
            }

            context.SaveChanges();

            base.Seed(context);
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace HwInf.Common.DAL
{
    public class DBInitializer : System.Data.Entity.CreateDatabaseIfNotExists<HwInfContext>
    {
        protected override void Seed(HwInfContext context)
        {

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
            new DBDevice { Name = "Lenovo Notebook", Hersteller = "Lenovo", Status = 0, InvNum = "a123", Type = type.Single(i => i.Name == "Notebook")},
            new DBDevice { Name = "Acer PC", Hersteller = "Acer", Status = 0 , InvNum = "a5123", Type = type.Single(i => i.Name == "PC")},
            new DBDevice { Name = "Benq Monitor", Hersteller = "Benq", Status = 0, InvNum = "a6123", Type = type.Single(i => i.Name == "Monitor")}
           };

            dev.ForEach(s => context.Devices.Add(s));
            context.SaveChanges();


            var devMeta = new List<DBDeviceMeta>
            {
                new DBDeviceMeta { Device = dev.Single(i => i.InvNum == "a123"), DeviceType = type.Single(i => i.Name == "Notebook"), MetaKey = "Prozessor", MetaValue = "Intel Core i7-6500U" },
                new DBDeviceMeta { Device= dev.Single(i => i.InvNum == "a123"), DeviceType = type.Single(i => i.Name == "Notebook"), MetaKey = "Bildschirmdiagonale", MetaValue = "13 Zoll" },
                new DBDeviceMeta { Device= dev.Single(i => i.InvNum == "a5123"), DeviceType = type.Single(i => i.Name == "PC"), MetaKey = "Prozessor", MetaValue = "Intel Core i5-3550" },
                new DBDeviceMeta { Device= dev.Single(i => i.InvNum == "a6123"), DeviceType = type.Single(i => i.Name == "Monitor"), MetaKey = "Farbe", MetaValue = "Schwarz" }
            };

            devMeta.ForEach(s => context.DeviceMeta.Add(s));
            context.SaveChanges();
            base.Seed(context);
        }
    }
}
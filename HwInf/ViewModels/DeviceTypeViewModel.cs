﻿using HwInf.Common.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HwInf.Models
{
    public class DeviceTypeViewModel
    {
        public int DeviceTypeId { get; set; }
        public string TypeDescription { get; set; }
        public IDictionary<string,string> DeviceTypeFields { get; set; }

        public DeviceTypeViewModel()
        {

        }

        public DeviceTypeViewModel(DeviceType obj)
        {
            Refresh(obj);
        }

        public void Refresh(DeviceType obj)
        {
            var target = this;
            var source = obj;

            target.DeviceTypeId = source.TypeId;
            target.TypeDescription = source.Description;
        }

        public void ApplyChanges(DeviceType obj, HwInfContext db)
        {
            var target = obj;
            var source = this;

            target.Description = source.TypeDescription;
        }

        public void CreateDeviceType(DeviceType obj, HwInfContext db)
        {
            var target = obj;
            var source = this;

            target.Description = source.TypeDescription;



            if (source.DeviceTypeFields.Count != 0)
            {
                for(int i = 0; i<(source.DeviceTypeFields.Count()/2); i++)
                {
                    db.Components.Add(new Component
                    {
                        Name = this.DeviceTypeFields["key"+(i + 1)],
                        FieldType = this.DeviceTypeFields["value" + (i + 1)],
                        DeviceType = target
                    });
                }
            }
        }

        public DeviceTypeViewModel loadComponents(HwInfContext db)
        {
            var typeComponents = db.Components;
            DeviceTypeFields = new Dictionary<string, string>();

            foreach (Component m in typeComponents.Include("DeviceType").Where(i => i.DeviceType.TypeId == DeviceTypeId))
            {
                DeviceTypeFields.Add(m.Name, m.FieldType);
            }

            return this; // fluent interface
        }
    }
}
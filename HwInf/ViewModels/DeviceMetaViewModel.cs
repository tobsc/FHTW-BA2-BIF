﻿using HwInf.Common.BL;
using HwInf.Common.Models;

namespace HwInf.ViewModels
{
    public class DeviceMetaViewModel
    {

        public DeviceMetaViewModel()
        {

        }

        public DeviceMetaViewModel(DeviceMeta dm)
        {
            Refresh(dm);
        }
        public ComponentViewModel Component { get; set; }
        public string Value { get; set; }

        public void Refresh(DeviceMeta dm)
        {
            var target = this;
            var source = dm;

            target.Value = source.MetaValue;
            target.Component = source.Component;
        }

        public void ApplyChanges(DeviceMeta dm, BL bl)
        {
            var target = dm;
            var source = this;

            target.MetaValue = source.Value;
            target.Component = source.Component;
            
        }

        public static implicit operator DeviceMeta(DeviceMetaViewModel vmdl)
        {
            return new DeviceMeta
            {
                Component = vmdl.Component,
                MetaValue = vmdl.Value               
            };
        }

        public static implicit operator DeviceMetaViewModel(DeviceMeta dm)
        {
            return new DeviceMetaViewModel
            {
                Component = dm.Component,
                Value = dm.MetaValue
            };
        }
    }
}
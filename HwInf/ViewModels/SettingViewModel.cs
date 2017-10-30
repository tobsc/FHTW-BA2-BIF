﻿using HwInf.Common.BL;
using HwInf.Common.Interfaces;
using HwInf.Common.Models;

namespace HwInf.ViewModels
{
    public class SettingViewModel
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public SettingViewModel()
        {
            
        }

        public SettingViewModel(Setting obj)
        {
            Refresh(obj);
        }

        public void Refresh(Setting obj)
        {

            if (obj == null) return;

            var target = this;
            var source = obj;

            target.Key = source.Key;
            target.Value = source.Value;
        }

        public void ApplyChanges(Setting obj, IBusinessLayer bl)
        {
            var target = obj;
            var source = this;

            target.Key = source.Key;
            target.Value = source.Value;

        }
    }
}
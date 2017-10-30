using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using HwInf.Common;
using HwInf.Common.BL;
using HwInf.Common.DAL;
using HwInf.Common.Interfaces;
using HwInf.Common.Models;
using IBusinessLayer = HwInf.Common.Interfaces.IBusinessLayer;

namespace HwInf.ViewModels
{
    public class AccessoryViewModel
    {

        public int AccessoryId { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }

        public AccessoryViewModel() { }

        public AccessoryViewModel(Accessory obj)
        {
            Refresh(obj);
        }


        public void Refresh(Accessory obj)
        {

            if(obj == null)
            {
                return;
            }

            var target = this;
            var source = obj;

            target.AccessoryId = source.AccessoryId;
            target.Name = source.Name;
            target.Slug = source.Slug;


        }

        public void ApplyChanges(Accessory obj, IBusinessLayer bl)
        {
            var target = obj;
            var source = this;

            if (target.Slug == null) target.Slug = SlugGenerator.GenerateSlug(bl, source.Name, "accessory");
            target.Name = source.Name;

        }
    }
}
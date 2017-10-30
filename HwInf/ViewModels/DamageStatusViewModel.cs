using HwInf.Common.BL;
using HwInf.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HwInf.ViewModels
{
    public class DamageStatusViewModel
    {
        public DamageStatusViewModel(DamageStatus obj)
        {
            Refresh(obj);
        }

        public string Slug { get; set; }
        public string Name { get; set; }
        public int StatusId { get; set; }


        public void Refresh(DamageStatus obj)
        {
            if (obj == null)
            {
                return;
            }

            var target = this;
            var source = obj;

            target.Name = source.Name;
            target.Slug = source.Slug;
            target.StatusId = source.StatusId;
        }

        public void ApplyChanges(DamageStatus obj, BusinessLayer bl)
        {
            var target = obj;
            var source = this;

            target.Name = source.Name;

            if (target.Slug == null)
            {
                target.Slug = SlugGenerator.GenerateSlug(bl, source.Name, "DamageStatus");
            }
        }
    }
}
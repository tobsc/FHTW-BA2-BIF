﻿using HwInf.BusinessLogic;
using HwInf.BusinessLogic.Interfaces;
using HwInf.Common.Models;

namespace HwInf.Web.ViewModels
{
    public class FieldViewModel
    {

        public string Name { get; set; }
        public string Slug { get; set; }


        public FieldViewModel()
        {
            
        }

        public FieldViewModel(Field fg)
        {
            Refresh(fg);
        }

        public void Refresh(Field fg)
        {
            var target = this;
            var source = fg;

            target.Name = source.Name;
            target.Slug = source.Slug;
        }

        public void ApplyChanges(Field fg, IBusinessLogicFacade bl)
        {
            var target = fg;
            var source = this;

            target.Name = source.Name;
            target.Slug = SlugGenerator.GenerateSlug(bl, source.Name, "field");
        }

        public static implicit operator Field(FieldViewModel vmdl)
        {
            return new Field
            {
                Name = vmdl.Name,
                Slug = SlugGenerator.GenerateSlug(null, vmdl.Name, "field")
            };
        }
    }
}
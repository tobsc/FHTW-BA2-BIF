﻿using System.Collections.Generic;
using System.Linq;
using HwInf.BusinessLogic;
using HwInf.BusinessLogic.Interfaces;
using HwInf.Common.Models;
using MoreLinq;

namespace HwInf.Web.ViewModels
{
    public class FieldGroupViewModel
    {

        public string Name { get; set; }
        public string Slug { get; set; }
        public ICollection<FieldViewModel> Fields { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsCountable { get; set; } = true;


        public FieldGroupViewModel()
        {
            
        }

        public FieldGroupViewModel(FieldGroup fg)
        {
            Refresh(fg);
        }

        public void Refresh(FieldGroup fg)
        {
            var target = this;
            var source = fg;

            target.Name = source.Name;
            target.Slug = source.Slug;
            target.Fields = source.Fields.Select(i => new FieldViewModel(i)).OrderBy(i => i.Name).ToList();
            target.IsCountable = source.IsCountable;
            target.IsActive = source.IsActive;
        }

        public void ApplyChanges(FieldGroup fg, IBusinessLogicFacade bl)
        {
            var target = fg;
            var source = this;


            if(target.Name == null) target.Slug = SlugGenerator.GenerateSlug(bl, source.Name, "fieldGroup");
            if (target.Name != null && !target.Name.Equals(source.Name)) target.Slug = SlugGenerator.GenerateSlug(bl, source.Name, "fieldGroup");
            target.Name = source.Name;

            target.Fields = new List<Field>();
            source.Fields.ForEach(i =>
            {
                var f = bl.CreateField();
                i.ApplyChanges(f, bl);
                target.Fields.Add(f);
            });
            target.IsCountable = source.IsCountable;
            target.IsActive = source.IsActive;
        }

    }
}
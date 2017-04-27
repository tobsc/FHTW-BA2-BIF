using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HwInf.Common.BL;
using HwInf.Common.Models;
using WebGrease.Css.Extensions;

namespace HwInf.ViewModels
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
            target.Fields = source.Fields.Select(i => new FieldViewModel(i)).ToList();
            target.IsCountable = source.IsCountable;
        }

        public void ApplyChanges(FieldGroup fg, BL bl)
        {
            var target = fg;
            var source = this;

            target.Name = source.Name;
            target.Slug = SlugGenerator.GenerateSlug(bl, source.Name, "fieldGroup");
            target.Fields = new List<Field>();
            source.Fields.ForEach(i =>
            {
                var f = bl.CreateField();
                i.ApplyChanges(f, bl);
                target.Fields.Add(f);
            });
            target.IsCountable = source.IsCountable;
        }

    }
}
using System.Collections.Generic;
using System.Linq;
using HwInf.Common.BL;
using HwInf.Common.Models;

namespace HwInf.ViewModels
{
    public class GroupViewModel
    {
        public int GroupId { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public ICollection<string> Fields { get; set; }
        


        public GroupViewModel()
        {

        }

        public GroupViewModel(FieldGroup gr)
        {
            Refresh(gr);
        }

        public void Refresh(FieldGroup gr)
        {
            var target = this;
            var source = gr;

            target.Name = source.Name;
            target.Label = source.Label;
            target.GroupId = source.GroupId;
            target.Fields = source.Fields.Select(i => i.Name).ToList();
        }

        public void ApplyChanges(FieldGroup gr, BL bl)
        {
            var target = gr;
            var source = this;
        }
    }
}
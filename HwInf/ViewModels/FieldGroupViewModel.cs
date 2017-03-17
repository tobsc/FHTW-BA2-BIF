using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HwInf.Common.BL;
using HwInf.Common.Models;

namespace HwInf.ViewModels
{
    public class FieldGroupViewModel
    {

        public int GroupId { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public ICollection<Field> Fields { get; set; }


        public FieldGroupViewModel()
        {
            
        }

        public FieldGroupViewModel(FieldGroup fg)
        {
            Refresh(fg);
        }

        public void Refresh(FieldGroup fg)
        {
            Name = fg.Name;
            Label = fg.Label;
            Fields = fg.Fields;
            GroupId = fg.GroupId;
        }
    }
}
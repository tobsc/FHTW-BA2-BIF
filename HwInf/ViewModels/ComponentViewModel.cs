using System.Diagnostics.SymbolStore;
using HwInf.Common.BL;
using HwInf.Common.Models;

namespace HwInf.ViewModels
{
    public class ComponentViewModel
    {

        public int CompId { get; set; }
        public string Name { get; set; }
        public ComponentType ComponentType { get; set; }



        public ComponentViewModel()
        {

        }

        public ComponentViewModel(Component c)
        {
            Refresh(c);
        }

        public void Refresh(Component c)
        {
            var target = this;
            var source = c;

            target.CompId = source.CompId;
            target.Name = source.Name;
            target.ComponentType = source.ComponentType;
        }

        public void ApplyChanges(Component c, BL bl)
        {
            var target = c;
            var source = this;

            target.Name = source.Name;
            target.ComponentType = bl.GetComponentType(source.ComponentType.Name);

        }

        public static implicit operator Component(ComponentViewModel vmdl)
        {
            return new Component
            {
                CompId = vmdl.CompId,
                Name = vmdl.Name,
                ComponentType = vmdl.ComponentType
            };
        }

    }
}
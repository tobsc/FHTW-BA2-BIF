using HwInf.BusinessLogic;
using HwInf.BusinessLogic.Interfaces;
using HwInf.Common.Models;

namespace HwInf.Web.ViewModels
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

        public void ApplyChanges(Accessory obj, IBusinessLogicFacade bl)
        {
            var target = obj;
            var source = this;

            if (target.Slug == null) target.Slug = SlugGenerator.GenerateSlug(bl, source.Name, "accessory");
            target.Name = source.Name;

        }
    }
}
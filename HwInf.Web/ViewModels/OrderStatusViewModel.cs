using HwInf.BusinessLogic;
using HwInf.BusinessLogic.Interfaces;
using HwInf.Common.Models;

namespace HwInf.Web.ViewModels
{
    public class OrderStatusViewModel
    {

        public OrderStatusViewModel(OrderStatus obj)
        {
            Refresh(obj);
        }

        public string Slug { get; set; }
        public string Name { get; set; }
        public int StatusId { get; set; }


        public void Refresh(OrderStatus obj)
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

        public void ApplyChanges(OrderStatus obj, IBusinessLogicFacade bl)
        {
            var target = obj;
            var source = this;

            target.Name = source.Name;
           
            if (target.Slug == null)
            {
                target.Slug = SlugGenerator.GenerateSlug(bl, source.Name, "orderStatus");
            }
        }
    }
}
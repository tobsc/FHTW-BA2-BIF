using System.Collections.Generic;
using HwInf.BusinessLogic.Interfaces;
using HwInf.Common.Models;

namespace HwInf.Web.ViewModels
{
    public class OrderFilterViewModel
    {
        public string Order { get; set; } = "DESC";
        public string OrderBy { get; set; } = "OrderStatus";
        public string OrderByFallback { get; set; } = "Date";
        public bool IsAdminView { get; set; } = false;
        public ICollection<string> StatusSlugs { get; set; } = new List<string>();
        public int Offset { get; set; } = 0;
        public int Limit { get; set; } = 25;

        public ICollection<Order> FilteredList(IBusinessLogicFacade bl)
        {
            return bl.GetFilteredOrders(StatusSlugs, Order, OrderBy, OrderByFallback, IsAdminView);
        }
    }
}
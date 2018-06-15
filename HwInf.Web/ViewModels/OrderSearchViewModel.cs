using System.Collections.Generic;
using HwInf.BusinessLogic.Interfaces;
using HwInf.Common.Models;

namespace HwInf.Web.ViewModels
{
    public class OrderSearchViewModel
    {
        public string Order { get; set; } = "DESC";
        public string OrderBy { get; set; } = "OrderStatus";
        public string OrderByFallback { get; set; } = "Date";
        public string SearchQuery { get; set; }
        public bool IsAdminView { get; set; } = false;
        public int Offset { get; set; } = 0;
        public int Limit { get; set; } = 25;

        public IEnumerable<Order> Seach(IBusinessLogicFacade bl)
        {
            return bl.SearchOrders(SearchQuery, Order, OrderBy, OrderByFallback, IsAdminView);
        }
    }
}
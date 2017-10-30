using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Compilation;
using HwInf.Common.BL;
using HwInf.Common.Interfaces;
using HwInf.Common.Models;
using MigraDoc.DocumentObjectModel.Shapes.Charts;

namespace HwInf.ViewModels
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

        public IEnumerable<Order> Seach(IBusinessLayer bl)
        {
            return bl.SearchOrders(SearchQuery, Order, OrderBy, OrderByFallback, IsAdminView);
        }
    }
}
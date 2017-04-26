using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Compilation;
using HwInf.Common.BL;
using HwInf.Common.Models;
using MigraDoc.DocumentObjectModel.Shapes.Charts;

namespace HwInf.ViewModels
{
    public class OrderFilterViewModel
    {
        public string Order { get; set; } = "DESC";
        public string OrderBy { get; set; } = "OrderStatus";
        public string OrderByFallback { get; set; } = "Date";
        public bool IsAdminView { get; set; } = false;
        public string StatusSlug { get; set; } = null;
        public int Offset { get; set; } = 0;
        public int Limit { get; set; } = 25;

        public ICollection<Order> FilteredList(BL bl)
        {
            return bl.GetFileteredOrders(StatusSlug, Order, OrderBy, OrderByFallback, IsAdminView);
        }
    }
}
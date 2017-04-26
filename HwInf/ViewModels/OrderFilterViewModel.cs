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
        public ICollection<string> StatusSlugs { get; set; } = new List<string>();
        public int Offset { get; set; } = 0;
        public int Limit { get; set; } = 25;

        public ICollection<Order> FilteredList(BL bl)
        {
            return bl.GetFilteredOrders(StatusSlugs, Order, OrderBy, OrderByFallback, IsAdminView);
        }
    }
}